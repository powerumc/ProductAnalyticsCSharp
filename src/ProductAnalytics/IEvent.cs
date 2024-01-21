namespace ProductAnalytics;

/// <summary>
///     Interface that defines event to send to analytics tool.
/// </summary>
public interface IEvent
{
    /// <summary>
    ///     Event name
    /// </summary>
    string EventName { get; }

    /// <summary>
    ///     Event properties
    /// </summary>
    IProperties? Properties { get; }

    /// <summary>
    ///     Event created date/time
    /// </summary>
    DateTime CreatedAt { get; }
}