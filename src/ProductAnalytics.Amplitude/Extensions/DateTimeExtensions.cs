namespace ProductAnalytics.Amplitude.Extensions;

/// <summary>
///     Get Unix Timestamp in milliseconds.
///     <remarks>
///         Amplitude uses millisecond timestamps instead of second timestamps.
///         <seealso href="https://www.docs.developers.amplitude.com/analytics/apis/http-v2-api/#set-time-values" />
///     </remarks>
/// </summary>
internal static class DateTimeExtensions
{
    public static ulong ToAmplitudeEpoch(this DateTime dateTime)
    {
        return (ulong)dateTime.ToUniversalTime()
            .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .TotalMilliseconds;
    }
}