namespace ProductAnalytics.Tests;

public class AmplitudeConfig
{
    public const string SectionName = "Amplitude";

    public string ApiKey { get; set; }
    public uint ProjectId { get; set; }
}

public class PosthogConfig
{
    public const string SectionName = "Posthog";

    public string ApiKey { get; set; }
    public uint ProjectId { get; set; }
}