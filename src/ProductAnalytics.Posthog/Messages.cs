using Newtonsoft.Json;

namespace ProductAnalytics.Posthog;

public interface IPosthogEvent : IEvent
{
}

public readonly struct PosthogBatchHttpBody : IBatchHttpBody
{
    public PosthogBatchHttpBody(string apiKey, IEnumerable<IEvent> events)
    {
        _ = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        ApiKey = apiKey;
        Events = events;
    }

    [JsonProperty("api_key")]
    public string ApiKey { get; }

    [JsonProperty("batch")]
    public IEnumerable<IEvent> Events { get; }
}

public readonly struct AliasEvent : IPosthogEvent
{
    public AliasEvent(string? distinctId, string aliasId, IProperties? properties, DateTime? createdAt = null)
    {
        _ = distinctId ?? throw new ArgumentNullException(nameof(distinctId));
        _ = aliasId ?? throw new ArgumentNullException(nameof(aliasId));

        Properties = properties;
        CreatedAt = createdAt ?? DateTime.UtcNow;

        if (properties != null)
        {
            DistinctId = distinctId;
            AliasId = aliasId;
        }
    }

    [JsonIgnore]
    public string? DistinctId
    {
        get => Properties?.UserId;
        set => Properties!.UserId = value;
    }

    [JsonIgnore]
    public string AliasId
    {
        get
        {
            Properties!.TryGetValue("alias", out var value);
            return value!.ToString();
        }
        set => Properties!.SetItem("alias", value);
    }

    [JsonProperty("event")]
    public string EventName => "$create_alias";

    [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
    public IProperties? Properties { get; }

    [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime CreatedAt { get; }
}

public readonly struct IdentifyEvent : IPosthogEvent
{
    public IdentifyEvent(string? distinctId, IProperties? properties, DateTime? createdAt = null)
    {
        _ = distinctId ?? throw new ArgumentNullException(nameof(distinctId));

        Properties = properties;
        CreatedAt = createdAt ?? DateTime.UtcNow;

        if (Properties != null)
        {
            DistinctId = distinctId;
        }
    }

    public IdentifyEvent(IdentifyEventArgs args) : this(args.DistinctId, args.Properties, args.CreatedAt)
    {
    }

    [JsonProperty("event")]
    public string EventName => "$identify";

    [JsonIgnore]
    public string? DistinctId
    {
        get => Properties?.UserId;
        set => Properties!.UserId = value;
    }

    [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
    public IProperties? Properties { get; }

    [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime CreatedAt { get; }
}

public readonly struct CaptureEvent : IPosthogEvent
{
    public CaptureEvent(CaptureEventArgs args)
        : this(args.DistinctId, args.EventName, args.Properties, args.CreatedAt)
    {
    }

    public CaptureEvent(string? distinctId, string eventName, IProperties? properties = null,
        DateTime? createdAt = null)
    {
        _ = distinctId ?? throw new ArgumentNullException(nameof(distinctId));
        _ = eventName ?? throw new ArgumentNullException(nameof(eventName));

        EventName = eventName;
        Properties = properties;
        CreatedAt = createdAt ?? DateTime.UtcNow;

        if (Properties != null)
        {
            Properties.UserId = distinctId;
            DistinctId = distinctId;
        }
    }

    [JsonProperty("event")]
    public string EventName { get; }

    [JsonIgnore]
    public string? DistinctId
    {
        get => Properties?.UserId;
        set => Properties!.UserId = value;
    }

    [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
    public IProperties? Properties { get; }

    [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime CreatedAt { get; }
}