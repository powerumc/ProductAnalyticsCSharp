namespace ProductAnalytics;

public interface IUserProperties
{
    IDictionary<string, object?> Properties { get; }

    /// <summary>
    ///     User account provided by the platform, ex) user001
    /// </summary>
    string? PlatformAccountId { get; set; }

    /// <summary>
    ///     User name provided by the platform, ex) user001@gmail.com
    /// </summary>
    string? PlatformAccountName { get; set; }

    /// <summary>
    ///     Country/Language information provided by .NET, ex) ko-KR
    /// </summary>
    string? SdkRegionId { get; set; }

    /// <summary>
    ///     Country name, ex) South Korea
    /// </summary>
    string? SdkRegionName { get; set; }

    /// <summary>
    ///     Language name, ex) Korean
    /// </summary>
    string? SdkLanguage { get; set; }

    /// <summary>
    ///     CPU architecture information provided by .NET, ex) Arm64
    /// </summary>
    string? SdkOsArchitecture { get; set; }

    /// <summary>
    ///     TimeZone information provided by .NET, ex) ROK, asia/seoul
    /// </summary>
    string? SdkTimeZone { get; set; }

    /// <summary>
    ///     Product user email
    /// </summary>
    string? ProductUserEmail { get; set; }

    /// <summary>
    ///     Product version
    /// </summary>
    string? ProductVersion { get; set; }

    /// <summary>
    ///     Adds a key/value to the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    IUserProperties SetItem(string key, object? value);

    /// <summary>
    ///     Gets the value of the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns>true if the key exists, false otherwise</returns>
    bool TryGetValue(string key, out object? value);
}