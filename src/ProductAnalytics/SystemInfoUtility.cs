using System.Globalization;
using System.Runtime.InteropServices;

namespace ProductAnalytics;

internal static class SystemInfoUtility
{
    private static readonly CultureInfo CultureInfo = CultureInfo.CurrentUICulture;
    private static readonly RegionInfo RegionInfo = RegionInfo.CurrentRegion;

    public static string? GetRegionId()
    {
        return CultureInfo.Name;
    }

    public static string? GetRegionName()
    {
        return RegionInfo.EnglishName;
    }

    public static string? GetLanguage()
    {
        return CultureInfo.GetCultureInfo(CultureInfo.TwoLetterISOLanguageName).EnglishName;
    }

    public static string? GetOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows.ToString();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSPlatform.OSX.ToString();
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OSPlatform.Linux.ToString();
        }

        return "Unknown";
    }

    public static string GetOsName()
    {
        //osx.12-arm64
        return AppContext.GetData("RUNTIME_IDENTIFIER").ToString();
    }

    public static string? GetOsArchitecture()
    {
        // Arm64
        return RuntimeInformation.OSArchitecture.ToString();
    }

    public static string? GetTimeZone()
    {
        // ROK, asia/seoul
        return TimeZoneInfo.Local.Id;
    }
}