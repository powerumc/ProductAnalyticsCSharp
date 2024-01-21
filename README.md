# Product Analytics

Product Analytics is a C# client for business analysis of products.

It supports the SaaS services below:
- [PostHog](https://posthog.com/)
- [Amplitude](https://amplitude.com/)

## Installation

The binary has not been distributed to nuget yet.

## Usage

---

#### PostHog
```csharp
// Config for PostHog
var apiKey = "YOUR_API_KEY"; // your api key
var projectId = 12345;  // your project id
var config = new Config(apiKey, projectId);
config.BaseAddress = new Uri("https://us.i.posthog.com");
config.FlushTimeSpan = TimeSpan.FromSeconds(5); // flush interval

// Create client
var httpClient = new HttpClient();
var client = new PosthogClient(clientConfig, logger, httpClient);
await client.InitializeAsync();

var distinctId = "user1"; // user distinct id
var properties = client.CreateProperties(); // create properties of event
properties.EventProperties!.SetItem("A", "B").SetItem("B", "C");
var captureEvent = new CaptureEvent(distinctId, "signin", properties);
client.Enqueue(captureEvent);

// Send all event after 5 seconds
```

If you want to send all events immediately, you can call `FlushAsync` method.

```csharp
await client.FlushAsync();
```

If you want multiple events to be sent immediately, you can call `BatchAsync` method

```csharp
var captureEvent = new CaptureEvent(distinctId, "signin", properties);
var identityEvent = new IdentifyEvent(DistinctId, properties);
await client.BatchAsync(batchEvent);
```

---
#### Amplitude

Amplitude also uses an API that implements the same interface.

```csharp
var apiKey = "YOUR_API_KEY"; // your api key
var projectId = 12345;  // your project id
var config = new Config(apiKey, projectId);
var httpClient = new HttpClient();
var client = new AmplitudeClient(config, logger, httpClient);
await client.InitializeAsync();

var distinctId = "user1"; // user distinct id
var properties = client.CreateProperties(); // create properties of event
properties.EventProperties!.SetItem("Device", "Web");
var captureEvent = new CaptureEvent(distinctId, "signin", properties);
client.Enqueue(captureEvent);
```

#### Combine Multiple Clients

Send events to multiple SaaS services.

```csharp
var posthogClient = new PosthogClient(posthogConfig, logger, postHogHttpClient);
var amplitudeClient = new AmplitudeClient(AmplitudeConfig, logger, amplitudeHttpClient);
var clients = AnalyticsApiFactory.Create(new[] { postHogHttpClient, amplitudeHttpClient });

var properties = clients.CreateProperties();
clients.Capture(new CaptureEventArgs("signin", "user1", properties));
```