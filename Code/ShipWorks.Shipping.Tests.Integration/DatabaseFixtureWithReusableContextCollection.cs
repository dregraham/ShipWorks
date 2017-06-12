using Xunit;

namespace ShipWorks.Tests.Shared.Database
{
    [CollectionDefinition("DatabaseFixtureWithReusableContext")]
    public class DatabaseFixtureWithReusableContextCollection : ICollectionFixture<DatabaseFixtureWithReusableContext>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
