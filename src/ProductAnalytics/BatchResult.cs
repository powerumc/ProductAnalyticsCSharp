namespace ProductAnalytics;

/// <summary>
///     A structure that returns the result of batch processing.
/// </summary>
public readonly struct BatchResult(
    int queueItemCount,
    int sentCount,
    bool isSuccess = true)
{
    /// <summary>
    ///     The number of messages waiting to be batch processed
    /// </summary>
    public int QueueItemCount { get; } = queueItemCount;

    /// <summary>
    ///     The number of messages batch processed
    /// </summary>
    public int SentCount { get; } = sentCount;

    /// <summary>
    ///     Whether the log transmission was successful
    ///     <remarks>
    ///         Returns true even if there is no data in the queue.
    ///         Returns false only if there is an HTTP communication error.
    ///     </remarks>
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;
}