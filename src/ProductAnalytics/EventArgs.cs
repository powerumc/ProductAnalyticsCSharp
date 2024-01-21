namespace ProductAnalytics;

public readonly struct FlushedEventArgs(int count)
{
    /// <summary>
    ///     Flushed event count
    /// </summary>
    public int Count { get; } = count;
}

/// <summary>
///     Structure definition to pass to the constructor of the Capture structure.
/// </summary>
public readonly struct CaptureEventArgs(
    string eventName,
    string? distinctId,
    IProperties? properties = null,
    DateTime? createdAt = null)
{
    public string EventName { get; } = eventName;
    public string? DistinctId { get; } = distinctId;
    public IProperties? Properties { get; } = properties;
    public DateTime? CreatedAt { get; } = createdAt ?? DateTime.UtcNow;
}

/// <summary>
///     Event that identifies the identity of a user
/// </summary>
public readonly struct IdentifyEventArgs(
    string? distinctId,
    IProperties? properties = null,
    DateTime? createdAt = null)
{
    public string? DistinctId { get; } = distinctId;
    public IProperties? Properties { get; } = properties;
    public DateTime? CreatedAt { get; } = createdAt ?? DateTime.Now;
}