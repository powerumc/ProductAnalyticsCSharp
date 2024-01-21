using System.Collections;

namespace ProductAnalytics;

public class FactoryProperties : IProperties, IEnumerable<KeyValuePair<Type, IProperties>>
{
    private readonly Dictionary<Type, IProperties> _properties;

    public FactoryProperties(IDictionary<Type, IAnalyticsApi> apis)
    {
        _properties = apis.ToDictionary(o => o.Key, o => o.Value.CreateProperties());
        EventProperties = new FactoryEventProperties(_properties);
        UserProperties = new FactoryUserProperties(_properties);
    }

    public IEnumerator<KeyValuePair<Type, IProperties>> GetEnumerator()
    {
        return _properties.Select(o => o).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IDictionary<string, object?> Properties => throw new NotSupportedException();

    public string? UserId
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.UserId = value;
            }
        }
    }

    public object? SessionId
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.SessionId = value;
            }
        }
    }

    public string? DeviceId
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.DeviceId = value;
            }
        }
    }

    public string? IpAddress
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.IpAddress = value;
            }
        }
    }

    public string? CityName
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.CityName = value;
            }
        }
    }

    public string? ContinentCode
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.ContinentCode = value;
            }
        }
    }

    public string? ContinentName
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.ContinentName = value;
            }
        }
    }

    public string? CountryCode
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.CountryCode = value;
            }
        }
    }

    public string? CountryName
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.CountryName = value;
            }
        }
    }

    public float? Latitude
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.Latitude = value;
            }
        }
    }

    public float? Longitude
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.Longitude = value;
            }
        }
    }

    public string? SubdivisionCode
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.SubdivisionCode = value;
            }
        }
    }

    public string? SubdivisionName
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.SubdivisionName = value;
            }
        }
    }

    public string? TimeZone
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.TimeZone = value;
            }
        }
    }

    public string? Os
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.Os = value;
            }
        }
    }

    public string? OsName
    {
        get => throw new NotSupportedException();
        set
        {
            foreach (var properties in _properties.Values)
            {
                properties.OsName = value;
            }
        }
    }

    public IEventProperties? EventProperties { get; }
    public IUserProperties UserProperties { get; }

    public IProperties SetItem(string key, object? value)
    {
        foreach (var properties in _properties.Values)
        {
            properties.SetItem(key, value);
        }

        return this;
    }

    public bool TryGetValue(string key, out object? value)
    {
        throw new NotSupportedException();
    }

    public IProperties GetProperties(Type type)
    {
        return _properties[type];
    }

    private sealed class FactoryEventProperties(Dictionary<Type, IProperties> eventProperties) : IEventProperties
    {
        public IDictionary<string, object?> Properties => throw new NotSupportedException();

        public string? PlatformName
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.PlatformName = value;
                }
            }
        }

        public string? PlatformVersion
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.PlatformVersion = value;
                }
            }
        }

        public string? PlatformArchitecture
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.PlatformArchitecture = value;
                }
            }
        }

        public string? ProductName
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ProductName = value;
                }
            }
        }

        public string? ProductVersion
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ProductVersion = value;
                }
            }
        }

        public int? ScreenWidth
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ScreenWidth = value;
                }
            }
        }

        public int? ScreenHeight
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ScreenHeight = value;
                }
            }
        }

        public int? ViewportWidth
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ViewportWidth = value;
                }
            }
        }

        public int? ViewportHeight
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in eventProperties.Values)
                {
                    properties.EventProperties!.ViewportHeight = value;
                }
            }
        }

        public IEventProperties SetItem(string key, object? value)
        {
            foreach (var properties in eventProperties.Values)
            {
                properties.EventProperties!.SetItem(key, value);
            }

            return this;
        }

        public bool TryGetValue(string key, out object? value)
        {
            throw new NotSupportedException();
        }
    }


    private sealed class FactoryUserProperties(Dictionary<Type, IProperties> userProperties) : IUserProperties
    {
        public IDictionary<string, object?> Properties => throw new NotSupportedException();

        public string? PlatformAccountId
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.PlatformAccountId = value;
                }
            }
        }

        public string? PlatformAccountName
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.PlatformAccountName = value;
                }
            }
        }

        public string? SdkRegionId
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.SdkRegionId = value;
                }
            }
        }

        public string? SdkRegionName
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.SdkRegionName = value;
                }
            }
        }

        public string? SdkLanguage
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.SdkLanguage = value;
                }
            }
        }

        public string? SdkOsArchitecture
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.SdkOsArchitecture = value;
                }
            }
        }

        public string? SdkTimeZone
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.SdkTimeZone = value;
                }
            }
        }

        public string? ProductUserEmail
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.ProductUserEmail = value;
                }
            }
        }

        public string? ProductVersion
        {
            get => throw new NotSupportedException();
            set
            {
                foreach (var properties in userProperties.Values)
                {
                    properties.UserProperties.ProductVersion = value;
                }
            }
        }

        public IUserProperties SetItem(string key, object? value)
        {
            foreach (var properties in userProperties.Values)
            {
                properties.UserProperties.SetItem(key, value);
            }

            return this;
        }

        public bool TryGetValue(string key, out object? value)
        {
            throw new NotSupportedException();
        }
    }
}