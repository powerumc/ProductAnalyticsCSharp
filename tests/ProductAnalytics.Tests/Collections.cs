namespace ProductAnalytics.Tests;

[CollectionDefinition(nameof(TestCollection), DisableParallelization = false)]
public class TestCollection : ICollectionFixture<ProductAnalyticsFixture>
{
}

[CollectionDefinition(nameof(TestPosthogCollection), DisableParallelization = false)]
public class TestPosthogCollection : ICollectionFixture<ProductAnalyticsFixture>
{
}

[CollectionDefinition(nameof(TestAmplitudeCollection), DisableParallelization = false)]
public class TestAmplitudeCollection : ICollectionFixture<ProductAnalyticsFixture>
{
}

[CollectionDefinition(nameof(CryptoCollection), DisableParallelization = false)]
public class CryptoCollection : ICollectionFixture<ProductAnalyticsFixture>
{
}