using FluentAssertions;
using Moq;
using Moq.Protected;
using ProductAnalytics.Posthog;
using Xunit.Abstractions;

namespace ProductAnalytics.Tests.Posthog;

[Collection(nameof(TestPosthogCollection))]
public class PosthogPolicyTests
{
    private const string DistinctId = "product-analytics-1234";
    private readonly ProductAnalyticsFixture _fixture;

    public PosthogPolicyTests(ProductAnalyticsFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.SetOutputHelper(output);
    }

    [Fact]
    public async Task WhenNetworkOff_ThenClearQueue()
    {
        // given 네트워크 단절 효과를 내는 HttpMessageHandler Mocking
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Throws<HttpRequestException>();

        // given PosthogClient 의 MaxQueueSize=10 으로 설정
        var config = _fixture.CreatePosthogConfig();
        config.MaxQueueSize = 10;
        using var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        using var client = new PosthogClient(config, _fixture.Logger, httpClient);
        client.SetAutoFlush(false);
        var properties = client.CreateProperties();

        var captures = new List<IEvent>();
        for (var i = 0; i < 10; i++)
        {
            var captureEvent = new CaptureEvent(DistinctId, "test", properties);
            client.Enqueue(captureEvent);

            captures.Add(captureEvent);
        }

        await client.BatchAsync(new PosthogBatchHttpBody("", captures));

        // then 큐를 모두 비운다 (네트워크 단절 상태에서 계속 큐가 쌓이지 않도록)
        client.QueueCount.Should().Be(0);
    }
}