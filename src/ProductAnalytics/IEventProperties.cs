namespace ProductAnalytics;

public interface IEventProperties
{
    IDictionary<string, object?> Properties { get; }

    /// <summary>
    ///     Platform name, ex) Unity
    /// </summary>
    string? PlatformName { get; set; }

    /// <summary>
    ///     Platform version, ex) 1.2.0
    /// </summary>
    string? PlatformVersion { get; set; }

    /// <summary>
    ///     Platform build atchitecture, ex) x64
    /// </summary>
    string? PlatformArchitecture { get; set; }

    /// <summary>
    ///     Product name
    /// </summary>
    string? ProductName { get; set; }

    /// <summary>
    ///     Product version, ex) 1.0.0
    /// </summary>
    string? ProductVersion { get; set; }

    /// <summary>
    ///     The width of the monitor
    /// </summary>
    int? ScreenWidth { get; set; }

    /// <summary>
    ///     The height of the monitor
    /// </summary>
    int? ScreenHeight { get; set; }

    /// <summary>
    ///     The width of the visible area or window
    /// </summary>
    int? ViewportWidth { get; set; }

    /// <summary>
    ///     The height of the visible area or window
    /// </summary>
    int? ViewportHeight { get; set; }

    /// <summary>
    ///     Adds a key/value to the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    IEventProperties SetItem(string key, object? value);

    /// <summary>
    ///     Gets the value of the property.
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns>true if the key exists, false otherwise</returns>
    bool TryGetValue(string key, out object? value);
}