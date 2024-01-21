using Newtonsoft.Json;
using ProductAnalytics.Extensions;

namespace ProductAnalytics.Amplitude;

public class AmplitudeProperties : IProperties
{
    public AmplitudeProperties(IProperties? defaultProperties = null)
    {
        if (defaultProperties is null)
        {
            Properties = new Dictionary<string, object?>();
            EventProperties = new AmplitudeEventProperties();
            UserProperties = new AmplitudeUserProperties();
        }
        else
        {
            Properties = new Dictionary<string, object?>(defaultProperties.Properties!);
        }

        Init();
    }

    public AmplitudeProperties(IProperties properties, IProperties? defaultProperties = null)
    {
        if (defaultProperties == null)
        {
            Properties = new Dictionary<string, object?>();
            EventProperties = new AmplitudeEventProperties();
            UserProperties = new AmplitudeUserProperties();
        }
        else
        {
            Properties = new Dictionary<string, object?>(properties.Properties.Merge(defaultProperties.Properties)!);
        }

        Init();
    }

    // ReSharper disable once UnusedMember.Local
    [JsonExtensionData]
    private IDictionary<string, object?> Serialization => Properties;

    [JsonIgnore]
    public IDictionary<string, object?> Properties { get; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? UserId
    {
        get => Properties.TryGetValue("user_id", out var value) ? value?.ToString() : null;
        set => Properties["user_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public object? SessionId
    {
        get => Properties.TryGetValue("session_id", out var value) ? value : null;
        set => Properties["session_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? DeviceId
    {
        get => Properties.TryGetValue("device_id", out var value) ? value?.ToString() : null;
        set => Properties["device_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? IpAddress
    {
        get => Properties.TryGetValue("ip_address", out var value) ? value?.ToString() : null;
        set => Properties["ip_address"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CityName
    {
        get => Properties.TryGetValue("city", out var value) ? value?.ToString() : null;
        set => Properties["city"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? ContinentCode { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? ContinentName { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CountryCode { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CountryName
    {
        get => Properties.TryGetValue("country", out var value) ? value?.ToString() : null;
        set => Properties["country"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public float? Latitude
    {
        get => (float?)(Properties.TryGetValue("location_lat", out var value) ? value : null);
        set => Properties["location_lat"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public float? Longitude
    {
        get => (float?)(Properties.TryGetValue("location_lng", out var value) ? value : null);
        set => Properties["location_lng"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? SubdivisionCode { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? SubdivisionName
    {
        get => Properties.TryGetValue("region", out var value) ? value?.ToString() : null;
        set => Properties["region"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? TimeZone { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? Os
    {
        get => Properties.TryGetValue("os", out var value) ? value?.ToString() : null;
        set => Properties["os"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? OsName
    {
        get => Properties.TryGetValue("os_name", out var value) ? value?.ToString() : null;
        set => Properties["os_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public IEventProperties? EventProperties
    {
        get => Properties["event_properties"] as IEventProperties;
        private set => Properties["event_properties"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public IUserProperties? UserProperties
    {
        get => Properties["user_properties"] as IUserProperties;
        private set => Properties["user_properties"] = value;
    }

    /// <inheritdoc />
    public IProperties SetItem(string key, object? value)
    {
        Properties[key] = value;
        return this;
    }

    /// <inheritdoc />
    public bool TryGetValue(string key, out object? value) =>
        Properties.TryGetValue(key, out value);

    private void Init()
    {
        // Set default values, but now it's empty.
    }

    public class AmplitudeEventProperties : IEventProperties
    {
        // ReSharper disable once UnusedMember.Local
        [JsonExtensionData]
        private IDictionary<string, object?> Serialization => Properties;

        [JsonIgnore]
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

        /// <inheritdoc />
        [JsonIgnore]
        public string? PlatformName
        {
            get => Properties.TryGetValue("platform_name", out var value) ? value?.ToString() : null;
            set => Properties["platform_name"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? PlatformVersion
        {
            get => Properties.TryGetValue("platform_version", out var value) ? value?.ToString() : null;
            set => Properties["platform_version"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? PlatformArchitecture
        {
            get => Properties.TryGetValue("platform_architecture", out var value) ? value?.ToString() : null;
            set => Properties["platform_architecture"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? ProductName
        {
            get => Properties.TryGetValue("product_name", out var value) ? value?.ToString() : null;
            set => Properties["product_name"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? ProductVersion
        {
            get => Properties.TryGetValue("product_version", out var value) ? value?.ToString() : null;
            set => Properties["product_version"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public int? ScreenWidth
        {
            get => (int?)(Properties.TryGetValue("screen_width", out var value) ? value : null);
            set => Properties["screen_width"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public int? ScreenHeight
        {
            get => (int?)(Properties.TryGetValue("screen_height", out var value) ? value : null);
            set => Properties["screen_height"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public int? ViewportWidth
        {
            get => (int?)(Properties.TryGetValue("viewport_width", out var value) ? value : null);
            set => Properties["viewport_width"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public int? ViewportHeight
        {
            get => (int?)(Properties.TryGetValue("viewport_height", out var value) ? value : null);
            set => Properties["viewport_height"] = value;
        }

        /// <inheritdoc />
        public IEventProperties SetItem(string key, object? value)
        {
            Properties[key] = value;
            return this;
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out object? value) =>
            Properties.TryGetValue(key, out value);
    }

    public class AmplitudeUserProperties : IUserProperties
    {
        // ReSharper disable once EmptyConstructor
        public AmplitudeUserProperties()
        {
            SdkRegionId = SystemInfoUtility.GetRegionId();
            SdkRegionName = SystemInfoUtility.GetRegionName();
            SdkLanguage = SystemInfoUtility.GetLanguage();
            SdkOsArchitecture = SystemInfoUtility.GetOsArchitecture();
            SdkTimeZone = SystemInfoUtility.GetTimeZone();
        }

        // ReSharper disable once UnusedMember.Local
        [JsonExtensionData]
        private IDictionary<string, object?> Serialization => Properties;

        [JsonIgnore]
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

        /// <inheritdoc />
        [JsonIgnore]
        public string? PlatformAccountId
        {
            get => Properties.TryGetValue("platform_account_id", out var value) ? value?.ToString() : null;
            set => Properties["platform_account_id"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? PlatformAccountName
        {
            get => Properties.TryGetValue("platform_account_name", out var value) ? value?.ToString() : null;
            set => Properties["platform_account_name"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? SdkRegionId
        {
            get => Properties.TryGetValue("sdk_region_id", out var value) ? value?.ToString() : null;
            set => Properties["sdk_region_id"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? SdkRegionName
        {
            get => Properties.TryGetValue("sdk_region_name", out var value) ? value?.ToString() : null;
            set => Properties["sdk_region_name"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? SdkLanguage
        {
            get => Properties.TryGetValue("sdk_language", out var value) ? value?.ToString() : null;
            set => Properties["sdk_language"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? SdkOsArchitecture
        {
            get => Properties.TryGetValue("sdk_os_architecture", out var value) ? value?.ToString() : null;
            set => Properties["sdk_os_architecture"] = value;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string? SdkTimeZone
        {
            get => Properties.TryGetValue("sdk_time_zone", out var value) ? value?.ToString() : null;
            set => Properties["sdk_time_zone"] = value;
        }

        [JsonIgnore]
        public string? ProductUserEmail
        {
            get => Properties.TryGetValue("product_user_email", out var value) ? value?.ToString() : null;
            set => Properties["product_user_email"] = value;
        }

        [JsonIgnore]
        public string? ProductVersion
        {
            get => Properties.TryGetValue("product_version", out var value) ? value?.ToString() : null;
            set => Properties["product_version"] = value;
        }

        /// <inheritdoc />
        public IUserProperties SetItem(string key, object? value)
        {
            Properties[key] = value;
            return this;
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out object? value) =>
            Properties.TryGetValue(key, out value);
    }
}