using Xunit;

namespace ShipWorks.Shiping.Specs
{
    [CollectionDefinition("database")]
    public class DatabaseCollection : ICollectionFixture<object>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
