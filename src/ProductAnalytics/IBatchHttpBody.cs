namespace ProductAnalytics;

/// <summary>
///     Interface that defines Http Body to send events to analytics tool.
/// </summary>
public interface IBatchHttpBody
{
    IEnumerable<IEvent> Events { get; }
}