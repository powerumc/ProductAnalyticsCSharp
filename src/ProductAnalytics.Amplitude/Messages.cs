using Newtonsoft.Json;

namespace ProductAnalytics.Amplitude;

public interface IAmplitudeEvent : IEvent
{
}

public readonly struct AmplitudeBatchHttpBody : IBatchHttpBody
{
    public AmplitudeBatchHttpBody(string apiKey, IEnumerable<IEvent> events)
    {
        _ = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        ApiKey = apiKey;
        Events = events;
    }

    [JsonProperty("api_key")]
    public string ApiKey { get; }

    [JsonProperty("events")]
    public IEnumerable<IEvent> Events { get; }
}

public readonly struct CaptureEvent : IAmplitudeEvent
{
    public CaptureEvent(CaptureEventArgs args)
        : this(args.DistinctId!, args.EventName, args.Properties, args.CreatedAt)
    {
    }

    public CaptureEvent(string userId, string eventName, IProperties? eventProperties, DateTime? createdAt = null)
    {
        _ = userId ?? throw new ArgumentNullException(nameof(userId));
        _ = eventName ?? throw new ArgumentNullException(nameof(eventName));

        EventName = eventName;
        UserId = userId;
        Properties = eventProperties;
        CreatedAt = createdAt ?? DateTime.UtcNow;
    }

    [JsonProperty("event_type")]
    public string EventName { get; }

    [JsonProperty("user_id")]
    public string UserId { get; }

    [JsonIgnore]
    public IProperties? Properties { get; }

    // ReSharper disable once UnusedMember.Local
    [JsonExtensionData]
    private IDictionary<string, object?> Serialization => ((AmplitudeProperties)Properties!).Properties;

    [JsonProperty("time")]
    public DateTime CreatedAt { get; }
}

public readonly struct IdentifyEvent : IAmplitudeEvent
{
    public IdentifyEvent(string userId, IProperties? properties)
    {
        _ = userId ?? throw new ArgumentNullException(nameof(userId));

        UserId = userId;
        Properties = properties;
    }

    public IdentifyEvent(IdentifyEventArgs args) : this(args.DistinctId!, args.Properties)
    {
    }

    [JsonProperty("user_id")]
    public string UserId { get; }

    [JsonProperty("event_type")]
    public string EventName => "$identify";

    [JsonIgnore]
    public IProperties? Properties { get; }

    // ReSharper disable once UnusedMember.Local
    [JsonExtensionData]
    private IDictionary<string, object?> Serialization => ((AmplitudeProperties)Properties!).Properties;

    [JsonIgnore]
    public DateTime CreatedAt => throw new NotSupportedException();
}