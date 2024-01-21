using Newtonsoft.Json;
using ProductAnalytics.Posthog;

namespace ProductAnalytics.Tests.Posthog;

[Collection(nameof(TestPosthogCollection))]
public class PosthogClientTests
{
    private const string DistinctId = "product-analytics-1234";
    private readonly ProductAnalyticsFixture _fixture;
    private readonly ITestOutputHelper _output;

    public PosthogClientTests(ITestOutputHelper output, ProductAnalyticsFixture fixture)
    {
        _fixture = fixture;
        _output = output;

        fixture.SetOutputHelper(output);
    }

    [Fact]
    public async Task IdentifyEventTest()
    {
        // given 사용자를 정의
        using var client = await _fixture.CreatePosthogClientAsync();
        var properties = client.CreateProperties();
        properties.EventProperties!.SetItem("A", "B").SetItem("B", "C");
        properties.IpAddress = "127.0.0.1";

        var identityEvent = new IdentifyEvent(DistinctId, properties);
        var batchEvent = new PosthogBatchHttpBody(client.Config.ApiKey, [identityEvent]);

        // when HTTP 전송
        var json = JsonConvert.SerializeObject(batchEvent, Formatting.Indented);
        _output.WriteLine(json);
        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.QueueItemCount.Should().Be(batchEvent.Events.Count());
        batchResult.SentCount.Should().Be(batchEvent.Events.Count());
    }

    [Fact]
    public async Task CaptureEventTest()
    {
        // given 사용자 행동 캡춰
        using var client = await _fixture.CreatePosthogClientAsync();
        var properties = client.CreateProperties();
        properties.EventProperties!.SetItem("A", "B").SetItem("B", "C");
        var captureEvent = new CaptureEvent(DistinctId, "hello", properties);
        var batchEvent = new PosthogBatchHttpBody(client.Config.ApiKey, new IEvent[] { captureEvent });

        // when HTTP 전송
        var json = JsonConvert.SerializeObject(batchEvent, Formatting.Indented);
        _output.WriteLine(json);
        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.QueueItemCount.Should().Be(batchEvent.Events.Count());
        batchResult.SentCount.Should().Be(batchEvent.Events.Count());
    }

    [Fact]
    public async Task AliasEventTest()
    {
        // given 사용자에게 alias(별칭) 부여
        using var client = await _fixture.CreatePosthogClientAsync();
        var aliasEvent = new AliasEvent(DistinctId, "tripolygon-1234-alias", client.CreateProperties());
        var batchEvent = new PosthogBatchHttpBody(client.Config.ApiKey, new IEvent[] { aliasEvent });

        // when HTTP 전송
        var json = JsonConvert.SerializeObject(batchEvent, Formatting.Indented);
        _output.WriteLine(json);
        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.QueueItemCount.Should().Be(batchEvent.Events.Count());
        batchResult.SentCount.Should().Be(batchEvent.Events.Count());
    }

    [Fact]
    public async Task ScenarioTest()
    {
        // given 지난 시간 순으로 login, action..., logout 행동을 기록
        using var client = await _fixture.CreatePosthogClientAsync();
        var now = DateTime.UtcNow.AddMinutes(-5);
        var events = new List<IEvent>();
        var random = new Random();
        var actions = new[] { "draw-circle", "draw-line", "draw-rectangle", "draw-triangle", "draw-ellipse" };

        // given login
        events.Add(new CaptureEvent(DistinctId, "login", client.CreateProperties(), now));

        // given 랜덤 횟수로 랜덤 액션을 기록
        for (var i = 0; i < random.Next(5) + 1; i++)
        {
            now = now.AddSeconds(1);
            var action = actions[random.Next(5)];
            events.Add(new CaptureEvent(DistinctId, action, client.CreateProperties(), now));
        }

        // given logout
        now = now.AddMinutes(1);
        events.Add(new CaptureEvent(DistinctId, "logout", client.CreateProperties(), now));

        // when 배치 전송
        var batchEvent = new PosthogBatchHttpBody(client.Config.ApiKey, events);
        var json = JsonConvert.SerializeObject(batchEvent, Formatting.Indented);
        _output.WriteLine(json);
        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.SentCount.Should().Be(events.Count);
    }

    [Fact]
    public async Task PosthogClientTimerTest()
    {
        // given
        var config = _fixture.CreatePosthogConfig();
        config.FlushTimeSpan = TimeSpan.FromSeconds(1);
        using var httpClient = new HttpClient();
        using var client = new PosthogClient(config, _fixture.Logger, httpClient);
        using var monitor = client.Monitor();
        var flushedCount = 0;
        client.Flushed += (sender, args) => { flushedCount++; };

        // when
        await Task.Delay(TimeSpan.FromSeconds(5));

        // then
        monitor.Should().Raise(nameof(client.Flushed));
        flushedCount.Should().BeGreaterThan(3);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task FlushedCountTest()
    {
        // given
        var config = _fixture.CreatePosthogConfig();
        config.FlushTimeSpan = TimeSpan.FromSeconds(1);
        using var httpClient = new HttpClient();
        using var client = new PosthogClient(config, _fixture.Logger, httpClient);
        using var monitor = client.Monitor();
        _output.WriteLine($"{nameof(DistinctId)}: {DistinctId}");

        var flushTask = Task.Run(async () =>
        {
            // when 큐에 2개 이벤트만 있으면 2개만 Flushed 되어야 함
            _output.WriteLine("Enqueued login.");
            _output.WriteLine("Enqueued draw.");
            client.Enqueue(new CaptureEvent(DistinctId, "login", client.CreateProperties()));
            client.Enqueue(new CaptureEvent(DistinctId, "draw", client.CreateProperties()));
            await Task.Delay(TimeSpan.FromSeconds(2));

            // when 큐에 1개 이벤트만 있으면 1개만 Flushed 되어야 함
            _output.WriteLine("Enqueued logout.");
            client.Enqueue(new CaptureEvent(DistinctId, "logout", client.CreateProperties()));
            await Task.Delay(TimeSpan.FromSeconds(2));
        });

        var delayTask = Task.Delay(TimeSpan.FromSeconds(5));

        // when
        await Task.WhenAll(flushTask, delayTask);

        // then
        monitor.Should().Raise(nameof(client.Flushed));
    }
}