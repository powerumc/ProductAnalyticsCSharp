namespace ProductAnalytics;

public interface IProperties
{
    IDictionary<string, object?> Properties { get; }

    string? UserId { get; set; }

    /// <summary>
    ///     User's SessionId, managed by SDK and does not need to be managed separately.
    ///     SessionId is initialized with an arbitrary value when each API Client is created.
    ///     <remarks>
    ///         Posthog is a string, Amplitude is a long type number
    ///         <seealso href="https://www.docs.developers.amplitude.com/analytics/apis/http-v2-api/#keys-for-the-event-argument">
    ///             Amplitude
    ///             session_id
    ///         </seealso>
    ///     </remarks>
    /// </summary>
    object? SessionId { get; set; }

    /// <summary>
    ///     Device unique Id
    /// </summary>
    string? DeviceId { get; set; }

    /// <summary>
    ///     IP, ex) 127.0.0.1
    /// </summary>
    string? IpAddress { get; set; }

    /// <summary>
    ///     City name, ex) Goyang-si
    /// </summary>
    string? CityName { get; set; }

    /// <summary>
    ///     Continent code, ex) AS
    /// </summary>
    string? ContinentCode { get; set; }

    /// <summary>
    ///     Continent name, ex) Asia
    /// </summary>
    string? ContinentName { get; set; }

    /// <summary>
    ///     Country code, ex) KR
    /// </summary>
    string? CountryCode { get; set; }

    /// <summary>
    ///     Country name, ex) South Korea
    /// </summary>
    string? CountryName { get; set; }

    /// <summary>
    ///     Latitude, ex) 37.661
    /// </summary>
    float? Latitude { get; set; }

    /// <summary>
    ///     Longitude, ex) 126.8324
    /// </summary>
    float? Longitude { get; set; }

    /// <summary>
    ///     Subdivision code, ex) 41
    /// </summary>
    string? SubdivisionCode { get; set; }

    /// <summary>
    ///     Subdivision name, ex) Gyeonggi-do
    /// </summary>
    string? SubdivisionName { get; set; }

    /// <summary>
    ///     Timezone, ex) Asia/Seoul
    /// </summary>
    string? TimeZone { get; set; }

    /// <summary>
    ///     OS name, ex) OSX, Windows, Linux
    /// </summary>
    string? Os { get; set; }

    /// <summary>
    ///     Operating system generalized name, ex) osx.12-arm64
    /// </summary>
    string? OsName { get; set; }

    /// <summary>
    ///     Event properties
    /// </summary>
    IEventProperties? EventProperties { get; }

    /// <summary>
    ///     User properties
    /// </summary>
    IUserProperties UserProperties { get; }

    /// <summary>
    ///     Adds a key/value to the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    IProperties SetItem(string key, object? value);

    /// <summary>
    ///     Gets the value of the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns>true if the key exists, false otherwise</returns>
    bool TryGetValue(string key, out object? value);
}