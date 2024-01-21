namespace ProductAnalytics.Amplitude;

public class Config
{
    public Config(string apiKey, uint projectId)
    {
        ApiKey = apiKey;
        ProjectId = projectId;
    }

    /// <summary>
    ///     Posthog server base address
    /// </summary>
    public Uri BaseAddress { get; set; } = new("https://api2.amplitude.com");

    /// <summary>
    ///     Posthog ApiKey
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    ///     Posthog ProjectId
    /// </summary>
    public uint ProjectId { get; }

    /// <summary>
    ///     HttpClient timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    ///     Http Header `User-Agent` value.
    ///     default "ProductAnalytics-Sdk"
    /// </summary>
    public string UserAgent { get; set; } = "ProductAnalytics-Sdk";

    /// <summary>
    ///     If set to true, the Flush operation is performed at the <see cref="FlushTimeSpan" /> time interval.
    /// </summary>
    public bool AutoFlushEnabled { get; set; } = true;

    /// <summary>
    ///     Flush interval time span
    /// </summary>
    public TimeSpan FlushTimeSpan { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     Client IP address
    /// </summary>
    public string? IpAddress { get; set; }

    public int MaxQueueSize { get; set; } = 1000;
}