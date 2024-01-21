using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductAnalytics.Amplitude.Extensions;

namespace ProductAnalytics.Amplitude;

/// <summary>
///     <remarks>
///         Upload limit: For Starter plan customers:
///         Limit your upload to 100 batches per second and 1000 events per second. You can batch events into an upload,
///         but don't send more than 10 events per batch. Amplitude expects fewer than 100 batches per second, and the 1000
///         events per second limit still applies.
///         <seealso href="https://www.docs.developers.amplitude.com/analytics/apis/http-v2-api/#upload-limit" />
///     </remarks>
/// </summary>
public class AmplitudeClient : AnalyticsApi
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public AmplitudeClient(Config config, ILogger logger, HttpClient httpClient) : base(logger)
    {
        Config = config;
        _httpClient = httpClient;
        _httpClient.BaseAddress = config.BaseAddress;
        _httpClient.Timeout = config.Timeout;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);

        _jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new JsonConverter[] { new UnixTimestampDateTimeConverter() },
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

        if (DefaultProperties is not null)
        {
            DefaultProperties.SessionId = new Random((int)DateTime.Now.Ticks).Next(int.MaxValue);
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
        var amplitudeProperties = new AmplitudeProperties(DefaultProperties);
        if (!string.IsNullOrEmpty(Config.IpAddress))
        {
            amplitudeProperties.IpAddress = Config.IpAddress;
        }

        return amplitudeProperties;
    }

    public override IProperties CreateProperties(IProperties properties)
    {
        var amplitudeProperties = new AmplitudeProperties(DefaultProperties!, properties);
        if (!string.IsNullOrEmpty(Config.IpAddress))
        {
            amplitudeProperties.IpAddress = Config.IpAddress;
        }

        return amplitudeProperties;
    }

    protected override Task<BatchResult> FlushInternalAsync(IList<IEvent> events) =>
        BatchAsync(new AmplitudeBatchHttpBody(Config.ApiKey, events));

    /// <inheritdoc />
    public override async Task<BatchResult> BatchAsync(IBatchHttpBody body)
    {
        var queueItemCount = body.Events.Count();

        if (!body.Events.Any())
        {
            Log(LogLevel.Debug, "But queue empty.");
            return new BatchResult(queueItemCount, 0);
        }

        // TODO: If the maximum Queue size (Body Size to be processed by the API server at once) is exceeded, it is necessary to implement to send Junk several times.
        // Amplitude recommends the following limits (Starter plan).
        // - Recommended not to exceed 10 events per batch.
        // - 100 batches per second (considering concurrent execution of clients)
        // - 1000 events per second (considering concurrent execution of clients)
        // REF: https://www.docs.developers.amplitude.com/analytics/apis/http-v2-api/#considerations

        var json = JsonConvert.SerializeObject(body, _jsonSerializerSettings);
        Log(LogLevel.Debug, $"Sending Queue Items={json}");

        try
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/2/httpapi");
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8);
            using var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                Log(LogLevel.Error, $"StatusCode={httpResponseMessage.StatusCode}, Message {responseMessage}");
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
            // TODO: Depending on the status of the HTTP Response response code, it will be implemented later whether to put the failed item back into the Queue or to treat it as a failure.
            Log(LogLevel.Error, e.ToString());

            return new BatchResult(0, 0);
        }

        Log(LogLevel.Debug, $"Sent {queueItemCount} queue item(s).");

        return new BatchResult(queueItemCount, queueItemCount);
    }

    /// <summary>
    ///     Amplitude uses millisecond unit timestamp
    /// </summary>
    private sealed class UnixTimestampDateTimeConverter : JsonConverter<DateTime>
    {
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToAmplitudeEpoch());

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) =>
            throw new NotImplementedException();
    }
}