namespace ProductAnalytics;

public interface IAnalyticsApi : IDisposable
{
    TimeSpan FlushTimeSpan { get; }

    event EventHandler<FlushedEventArgs>? Flushed;

    /// <summary>
    ///     Performs SDK initialization work.
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();

    /// <summary>
    ///     Sets whether to Flush to the Posthog server periodically.
    ///     If you change the Flush time, you must call this method again to reflect it immediately.
    /// </summary>
    void SetAutoFlush(bool enabled);

    /// <summary>
    ///     Create an event and put it in the queue.
    /// </summary>
    /// <param name="eventArgs">Event object</param>
    /// <typeparam name="TEventArgs">Event object type</typeparam>
    void EnqueueWith<TEventArgs>(TEventArgs eventArgs);

    /// <summary>
    ///     Creates and returns a property object that contains event information.
    /// </summary>
    /// <returns></returns>
    IProperties CreateProperties();

    /// <summary>
    ///     Creates and returns a property object that contains event information.
    /// </summary>
    /// <param name="properties">Event properties</param>
    /// <returns>Object copied from <paramref name="properties" /> information</returns>
    IProperties CreateProperties(IProperties properties);

    /// <summary>
    ///     Adds an event to the queue.
    /// </summary>
    void Enqueue(params IEvent[] events);

    /// <summary>
    ///     Flushes the events in the queue.
    ///     Sends all events in the queue to Posthog and empties the queue.
    /// </summary>
    Task FlushAsync();

    /// <summary>
    ///     Sends all events passed to <paramref name="body" /> to the Posthog server.
    ///     The passed event is sent to analytics service immediately without being put in the queue.
    ///     <remarks>
    ///         To put it in the event queue and send it periodically, <see cref="SetAutoFlush" /> must be set to true
    ///         (default)
    ///         or AutoFlushEnabled must be set to true.
    ///     </remarks>
    /// </summary>
    /// <param name="body"></param>
    Task<BatchResult> BatchAsync(IBatchHttpBody body);

    /// <summary>
    ///     Sets the default properties.
    /// </summary>
    /// <param name="defaultProperties">Properties object</param>
    void SetDefaultProperties(IProperties defaultProperties);
}