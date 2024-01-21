using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductAnalytics.Amplitude;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProductAnalytics.Tests.Amplitude;

[Collection(nameof(TestAmplitudeCollection))]
public class AmplitudeClientTests
{
    private const string UserId = "product-analytics-1234";
    private readonly ProductAnalyticsFixture _fixture;

    public AmplitudeClientTests(ProductAnalyticsFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.SetOutputHelper(output);
    }

    [Fact]
    public async Task AmplitudeClientTest()
    {
        // given 클라이언트 설정
        using var client = await _fixture.CreateAmplitudeClientAsync();
        var properties1 = client.CreateProperties();
        properties1.IpAddress = "127.0.0.1";
        properties1.EventProperties!.SetItem("event-prop-item", "hello");
        properties1.UserProperties.SetItem("user-prop-item", "world");
        var captureEvent1 = new CaptureEvent(UserId, "login", properties1);

        var properties2 = client.CreateProperties();
        properties2.IpAddress = "127.0.0.1";
        properties2.EventProperties!.SetItem("event-prop-item", "hello");
        properties2.UserProperties.SetItem("user-prop-item", "world-");
        var captureEvent2 = new CaptureEvent(UserId, "logout", properties2);

        var batchEvent = new AmplitudeBatchHttpBody(client.Config.ApiKey, new IEvent[] { captureEvent1, captureEvent2 });

        // when login 이벤트 전송
        var json = JsonConvert.SerializeObject(batchEvent);
        _fixture.Logger.LogInformation(json);
        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.SentCount.Should().Be(batchEvent.Events.Count());
    }

    [Fact]
    public async Task IdentifyEventTest()
    {
        // given 클라이언트 설정
        using var client = await _fixture.CreateAmplitudeClientAsync();

        // given login 기록
        var eventProperties = client.CreateProperties();
        var captureEvent = new CaptureEvent(UserId, "login", eventProperties);

        // given identify 기록
        var properties = client.CreateProperties();
        properties.UserProperties.SetItem("$add", new Dictionary<string, object> { { "friendCount", 1 } });
        var identifyEvent = new IdentifyEvent(UserId, properties);

        // when 배치 전송
        var batchEvent = new AmplitudeBatchHttpBody(client.Config.ApiKey, new IEvent[] { captureEvent, identifyEvent });
        var json = JsonSerializer.Serialize(captureEvent);
        _fixture.Logger.LogInformation(json);

        var batchResult = await client.BatchAsync(batchEvent);

        // then
        batchResult.SentCount.Should().Be(batchEvent.Events.Count());
    }
}