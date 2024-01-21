using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductAnalytics.Amplitude;
using ProductAnalytics.Posthog;
using Serilog;
using Xunit.Abstractions;
using Config = ProductAnalytics.Posthog.Config;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ProductAnalytics.Tests;

public class ProductAnalyticsFixture
{
    public ProductAnalyticsFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<AmplitudeConfig>(configuration.GetSection(AmplitudeConfig.SectionName))
            .Configure<PosthogConfig>(configuration.GetSection(PosthogConfig.SectionName));

        ServiceProvider = services.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }
    public ILogger Logger { get; private set; }

    public Config CreatePosthogConfig()
    {
        var config = ServiceProvider.GetRequiredService<IOptions<PosthogConfig>>().Value;
        var clientConfig = new Config(config.ApiKey, config.ProjectId);
        clientConfig.BaseAddress = new Uri("https://us.i.posthog.com");
        return clientConfig;
    }

    public async Task<PosthogClient> CreatePosthogClientAsync()
    {
        var clientConfig = CreatePosthogConfig();
        var httpClient = new HttpClient();
        var client = new PosthogClient(clientConfig, Logger, httpClient);
        await client.InitializeAsync();

        return client;
    }

    public async Task<AmplitudeClient> CreateAmplitudeClientAsync()
    {
        var clientConfig = CreateAmplitudeConfig();
        var httpClient = new HttpClient();
        var client = new AmplitudeClient(clientConfig, Logger, httpClient);
        await client.InitializeAsync();

        return client;
    }

    public ProductAnalytics.Amplitude.Config CreateAmplitudeConfig()
    {
        var config = ServiceProvider.GetRequiredService<IOptions<AmplitudeConfig>>().Value;
        var clientConfig = new ProductAnalytics.Amplitude.Config(config.ApiKey, config.ProjectId);
        return clientConfig;
    }

    public void SetOutputHelper(ITestOutputHelper output)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        Logger = ServiceProvider.GetRequiredService<ILogger<ProductAnalyticsFixture>>();
    }
}