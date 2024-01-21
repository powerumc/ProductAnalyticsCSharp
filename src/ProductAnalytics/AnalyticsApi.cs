using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ProductAnalytics;

public abstract class AnalyticsApi : IAnalyticsApi
{
    private readonly ILogger _logger;
    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    protected readonly ConcurrentQueue<IEvent> Queue = new();
    private Timer? _timer;

    protected AnalyticsApi(ILogger logger)
    {
        _logger = logger;
    }

    public IProperties? DefaultProperties { get; private set; }

    public int QueueCount => Queue.Count;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public abstract TimeSpan FlushTimeSpan { get; }
    public event EventHandler<FlushedEventArgs>? Flushed;

    public virtual Task InitializeAsync()
    {
        DefaultProperties ??= CreateProperties();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public void SetAutoFlush(bool enabled)
    {
        Log(LogLevel.Debug, $"Value Changed: {enabled}");

        if (!enabled)
        {
            _timer?.Dispose();
            return;
        }

        _timer ??= new Timer(OnTimerCallback);
        _timer.Change(FlushTimeSpan, Timeout.InfiniteTimeSpan);
    }

    public async Task FlushAsync()
    {
        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

        var count = Queue.Count;
        var events = new List<IEvent>();
        for (var i = 0; i < count; i++)
        {
            Queue.TryDequeue(out var @event);
            events.Add(@event);
        }

        try
        {
            var batchResult = await FlushInternalAsync(events).ConfigureAwait(false);
            if (batchResult.IsSuccess)
            {
                OnFlushed(new FlushedEventArgs(count));
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public abstract Task<BatchResult> BatchAsync(IBatchHttpBody body);

    /// <inheritdoc />
    public void SetDefaultProperties(IProperties defaultProperties) =>
        DefaultProperties = defaultProperties;

    /// <inheritdoc />
    public abstract void EnqueueWith<TEventArgs>(TEventArgs eventArgs);

    /// <summary>
    ///     Adds an event to the queue.
    /// </summary>
    public void Enqueue(params IEvent[] events)
    {
        foreach (var @event in events)
        {
            Queue.Enqueue(@event);
        }
    }

    public abstract IProperties CreateProperties();

    public abstract IProperties CreateProperties(IProperties properties);

    protected virtual void ClearQueue()
    {
        while (Queue.TryDequeue(out _))
        {
            // nothing...
        }
    }

    protected abstract Task<BatchResult> FlushInternalAsync(IList<IEvent> events);

    protected virtual void OnFlushed(FlushedEventArgs args) =>
        Flushed?.Invoke(this, args);

    private async void OnTimerCallback(object state)
    {
        try
        {
            await FlushAsync().ConfigureAwait(false);
        }
        finally
        {
            _timer?.Change(FlushTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer?.Dispose();
            _timer = null;
        }
    }

    protected void Log(LogLevel level, string message, [CallerMemberName] string? memberName = default) =>
        _logger.Log(level, "{Name}.{MemberName}: {Message}", GetType().Name, memberName, message);
}