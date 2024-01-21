using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ProductAnalytics.Posthog;

public class PosthogClient : AnalyticsApi
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public PosthogClient(Config config, ILogger logger, HttpClient httpClient) : base(logger)
    {
        Config = config;
        _httpClient = httpClient;
        _httpClient.BaseAddress = config.BaseAddress;
        _httpClient.Timeout = config.Timeout;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);

        _jsonSerializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore
        };

        SetAutoFlush(config.AutoFlushEnabled);
    }

    public Config Config { get; }

    public override TimeSpan FlushTimeSpan => Config.FlushTimeSpan;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);

        if (DefaultProperties != null)
        {
            DefaultProperties.SessionId = $"{Guid.NewGuid()}_{Guid.NewGuid()}";
        }
    }

    public override void EnqueueWith<TEventArgs>(TEventArgs eventArgs)
    {
        switch (eventArgs)
        {
            case CaptureEventArgs captureEventArgs:
                Enqueue(new CaptureEvent(captureEventArgs));
                break;

            case IdentifyEventArgs identifyEventArgs:
                Enqueue(new IdentifyEvent(identifyEventArgs));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(eventArgs));
        }
    }

    public override IProperties CreateProperties()
    {
        var posthogProperties = new PosthogProperties(DefaultProperties);
        if (!string.IsNullOrEmpty(Config.IpAddress))
        {
            posthogProperties.IpAddress = Config.IpAddress;
        }

        return posthogProperties;
    }

    public override IProperties CreateProperties(IProperties properties)
    {
        var posthogProperties = new PosthogProperties(DefaultProperties!, properties);
        if (!string.IsNullOrEmpty(Config.IpAddress))
        {
            posthogProperties.IpAddress = Config.IpAddress;
        }

        return posthogProperties;
    }

    protected override Task<BatchResult> FlushInternalAsync(IList<IEvent> events) =>
        BatchAsync(new PosthogBatchHttpBody(Config.ApiKey, events));

    /// <inheritdoc />
    public override async Task<BatchResult> BatchAsync(IBatchHttpBody body)
    {
        var queueItemCount = body.Events.Count();
        if (!body.Events.Any())
        {
            Log(LogLevel.Debug, "But queue empty.");
            return new BatchResult(queueItemCount, 0);
        }

        // TODO: If the maximum Queue size (Body Size to be processed by the API server at once) is exceeded, it is necessary to implement Junk to send multiple times.
        // Posthog requires that the RequestBody size be less than 20MB when sending in batches
        // REF: https://posthog.com/docs/api/post-only-endpoints#batch-events

        var json = JsonConvert.SerializeObject(body, _jsonSerializerSettings);

        Log(LogLevel.Debug, $"Sending Queue Items={json}");

        try
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/capture/");
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8);
            using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                Log(LogLevel.Error, $"StatusCode={httpResponseMessage.StatusCode}, Message={responseMessage}");
            }
        }
        catch (HttpRequestException hrex)
        {
            Log(LogLevel.Error, $"HTTP Error. e={hrex}");

            // If the network is disconnected and the number of queue items is more than MaxQueueSize, just empty the queue

            if (queueItemCount + Queue.Count >= Config.MaxQueueSize)
            {
                Log(LogLevel.Warning, "Drop queue all items, because unsent item has so many items.");

                ClearQueue();
            }

            return new BatchResult(0, 0, false);
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, e.ToString());

            return new BatchResult(0, 0);
        }

        Log(LogLevel.Debug, $"Sent {queueItemCount} queue item(s).");

        return new BatchResult(queueItemCount, queueItemCount);
    }
}