using FluentAssertions;
using Moq;
using Moq.Protected;
using ProductAnalytics.Amplitude;
using Xunit.Abstractions;

namespace ProductAnalytics.Tests.Amplitude;

[Collection(nameof(TestAmplitudeCollection))]
public class AmplitudePolicyTests
{
    private const string DistinctId = "producy-analytics-1234";
    private readonly ProductAnalyticsFixture _fixture;

    public AmplitudePolicyTests(ProductAnalyticsFixture fixture, ITestOutputHelper output)
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
        var config = _fixture.CreateAmplitudeConfig();
        config.MaxQueueSize = 10;
        using var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        using var client = new AmplitudeClient(config, _fixture.Logger, httpClient);
        client.SetAutoFlush(false);
        var properties = client.CreateProperties();

        var captures = new List<IEvent>();
        for (var i = 0; i < 10; i++)
        {
            var captureEvent = new CaptureEvent(DistinctId, "test", properties);
            client.Enqueue(captureEvent);

            captures.Add(captureEvent);
        }

        await client.BatchAsync(new AmplitudeBatchHttpBody("", captures));

        // then 큐를 모두 비운다 (네트워크 단절 상태에서 계속 큐가 쌓이지 않도록)
        client.QueueCount.Should().Be(0);
    }
}