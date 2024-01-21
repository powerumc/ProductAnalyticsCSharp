using Newtonsoft.Json;
using ProductAnalytics.Extensions;

namespace ProductAnalytics.Posthog;

public class PosthogProperties : IProperties
{
    public PosthogProperties(IProperties? defaultProperties = null)
    {
        if (defaultProperties == null)
        {
            Properties = new Dictionary<string, object?>();
            EventProperties = new PosthogEventProperties();
            UserProperties = new PosthogUserProperties();
        }
        else
        {
            Properties = new Dictionary<string, object?>(((PosthogProperties)defaultProperties).Properties);
            EventProperties = new PosthogEventProperties(defaultProperties.EventProperties!.Properties);
            UserProperties = new PosthogUserProperties(defaultProperties.UserProperties.Properties);
        }

        Init();
    }

    public PosthogProperties(IProperties properties, IProperties? defaultProperties)
    {
        if (defaultProperties == null)
        {
            Properties = new Dictionary<string, object?>();
            EventProperties = new PosthogEventProperties();
            UserProperties = new PosthogUserProperties();
        }
        else
        {
            Properties = new Dictionary<string, object?>(properties.Properties.Merge(defaultProperties.Properties));
            EventProperties = new PosthogEventProperties(properties.EventProperties!.Properties
                .Merge(defaultProperties.EventProperties!.Properties));
            UserProperties = new PosthogUserProperties(properties.UserProperties.Properties
                .Merge(defaultProperties.UserProperties.Properties));
        }

        Init();
    }

    // ReSharper disable once UnusedMember.Local
    [JsonExtensionData]
    private IDictionary<string, object?> Serialization =>
        Properties
            .Concat(((PosthogEventProperties)EventProperties!).Properties)
            .ToDictionary(o => o.Key, o => o.Value);

    [JsonIgnore]
    public IDictionary<string, object?> Properties { get; }

    /// <inheritdoc />
    [JsonIgnore]
    public string? UserId
    {
        get => Properties.TryGetValue("distinct_id", out var value) ? value?.ToString() : null;
        set => Properties["distinct_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public object? SessionId
    {
        get => Properties.TryGetValue("$session_id", out var value) ? value?.ToString() : null;
        set => Properties["$session_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? DeviceId
    {
        get => Properties.TryGetValue("$device_id", out var value) ? value?.ToString() : null;
        set => Properties["$device_id"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? IpAddress
    {
        get => Properties.TryGetValue("$ip", out var value) ? value?.ToString() : null;
        set => Properties["$ip"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CityName
    {
        get => Properties.TryGetValue("$geoip_city_name", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_city_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? ContinentCode
    {
        get => Properties.TryGetValue("$geoip_continent_code", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_continent_code"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? ContinentName
    {
        get => Properties.TryGetValue("$geoip_continent_name", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_continent_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CountryCode
    {
        get => Properties.TryGetValue("$geoip_country_code", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_country_code"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? CountryName
    {
        get => Properties.TryGetValue("$geoip_country_name", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_country_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public float? Latitude
    {
        get => (float?)(Properties.TryGetValue("$geoip_latitude", out var value) ? value : null);
        set => Properties["$geoip_latitude"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public float? Longitude
    {
        get => (float?)(Properties.TryGetValue("$geoip_longitude", out var value) ? value : null);
        set => Properties["$geoip_longitude"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? SubdivisionCode
    {
        get => Properties.TryGetValue("$geoip_subdivision_1_code", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_subdivision_1_code"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? SubdivisionName
    {
        get => Properties.TryGetValue("$geoip_subdivision_1_name", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_subdivision_1_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? TimeZone
    {
        get => Properties.TryGetValue("$geoip_time_zone", out var value) ? value?.ToString() : null;
        set => Properties["$geoip_time_zone"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? Os
    {
        get => Properties.TryGetValue("$os", out var value) ? value?.ToString() : null;
        set => Properties["$os"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public string? OsName
    {
        get => Properties.TryGetValue("$os_name", out var value) ? value?.ToString() : null;
        set => Properties["$os_name"] = value;
    }

    /// <inheritdoc />
    [JsonIgnore]
    public IEventProperties? EventProperties { get; }

    /// <inheritdoc />
    [JsonIgnore]
    public IUserProperties? UserProperties
    {
        get => Properties["$set"] as IUserProperties;
        set => Properties["$set"] = value;
    }

    /// <inheritdoc />
    public IProperties SetItem(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    /// <inheritdoc />
    public bool TryGetValue(string key, out object? value) =>
        Properties.TryGetValue(key, out value);

    private void Init()
    {
        Os = SystemInfoUtility.GetOs();
        OsName = SystemInfoUtility.GetOsName();
    }

    public class PosthogEventProperties : IEventProperties
    {
        public PosthogEventProperties()
        {
            Properties = new Dictionary<string, object?>();
        }

        public PosthogEventProperties(IDictionary<string, object?> properties)
        {
            Properties = new Dictionary<string, object?>(properties);
        }

        // ReSharper disable once UnusedMember.Local
        [JsonExtensionData]
        private IDictionary<string, object?> Serialization => Properties;

        [JsonIgnore]
        public IDictionary<string, object?> Properties { get; }

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

    public class PosthogUserProperties : IUserProperties
    {
        public PosthogUserProperties()
        {
            SdkRegionId = SystemInfoUtility.GetRegionId();
            SdkRegionName = SystemInfoUtility.GetRegionName();
            SdkLanguage = SystemInfoUtility.GetLanguage();
            SdkOsArchitecture = SystemInfoUtility.GetOsArchitecture();
            SdkTimeZone = SystemInfoUtility.GetTimeZone();
        }

        public PosthogUserProperties(IDictionary<string, object?> properties) : this()
        {
            Properties = new Dictionary<string, object?>(properties);
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