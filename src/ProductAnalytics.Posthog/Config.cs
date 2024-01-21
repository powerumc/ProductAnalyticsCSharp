namespace ProductAnalytics.Posthog;

public class Config
{
    public Config(string apiKey, uint projectId)
    {
        if (apiKey == null || string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentNullException(nameof(apiKey));
        }

        if (projectId == default)
        {
            throw new ArgumentOutOfRangeException(nameof(projectId));
        }

        ApiKey = apiKey;
        ProjectId = projectId;
    }

    /// <summary>
    ///     Posthog server address
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://app.posthog.com");

    /// <summary>
    ///     ApiKey issued by Posthog
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    ///     ProjectId issued by Posthog
    /// </summary>
    public uint ProjectId { get; }

    /// <summary>
    ///     The time-out value, in seconds. The default value is 5 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    ///     The value to send as the User-Agent header. The default value is "ProductAnalytics-SDK".
    /// </summary>
    public string UserAgent { get; set; } = "ProductAnalytics-SDK";

    /// <summary>
    ///     If set to true, the Flush operation is performed at the <see cref="FlushTimeSpan" /> time interval.
    /// </summary>
    public bool AutoFlushEnabled { get; set; } = true;

    /// <summary>
    ///     The time interval for batch processing
    /// </summary>
    public TimeSpan FlushTimeSpan { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     client ip address
    /// </summary>
    public string? IpAddress { get; set; }

    public int MaxQueueSize { get; set; } = 1000;
}