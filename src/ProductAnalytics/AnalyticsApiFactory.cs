namespace ProductAnalytics;

public sealed class AnalyticsApiFactory : IAsyncDisposable
{
    private AnalyticsApiFactory(IEnumerable<IAnalyticsApi> apis)
    {
        Apis = apis.ToDictionary(o => o.GetType(), o => o);
    }

    private IDictionary<Type, IAnalyticsApi> Apis { get; }

    public async ValueTask DisposeAsync()
    {
        foreach (var api in Apis.Values)
        {
            api.SetAutoFlush(false);
        }

        foreach (var api in Apis.Values)
        {
            await api.FlushAsync();
            api.Dispose();
        }
    }

    public static AnalyticsApiFactory Create(params IAnalyticsApi[] apis)
    {
        return new AnalyticsApiFactory(apis);
    }

    /// <summary>
    ///     Create an event and put it in the queue.
    /// </summary>
    /// <param name="args">Event object</param>
    public void Capture(CaptureEventArgs args)
    {
        foreach (var api in Apis)
        {
            var props = (args.Properties as FactoryProperties)?.GetProperties(api.Key)
                        ?? api.Value.CreateProperties();
            var captureEventArgs = new CaptureEventArgs(args.EventName, args.DistinctId,
                api.Value.CreateProperties(props), args.CreatedAt);
            api.Value.EnqueueWith(captureEventArgs);
        }
    }

    /// <summary>
    ///     Create an event and put it in the queue.
    /// </summary>
    /// <param name="args">Event argument</param>
    public void Identify(IdentifyEventArgs args)
    {
        foreach (var api in Apis)
        {
            var props = (args.Properties as FactoryProperties)?.GetProperties(api.Key)
                        ?? api.Value.CreateProperties();
            var captureEventArgs =
                new IdentifyEventArgs(args.DistinctId, api.Value.CreateProperties(props), args.CreatedAt);
            api.Value.EnqueueWith(captureEventArgs);
        }
    }

    public void SetDefaultProperties(FactoryProperties factoryProperties)
    {
        foreach (var property in factoryProperties)
        {
            Apis[property.Key].SetDefaultProperties(property.Value);
        }
    }

    public FactoryProperties CreateProperties()
    {
        return new FactoryProperties(Apis);
    }
}