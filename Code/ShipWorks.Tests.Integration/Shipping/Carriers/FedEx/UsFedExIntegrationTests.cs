﻿using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx
{
    [CollectionDefinition("US Fedex Tests")]
    public class UsFedExIntegrationTests : ICollectionFixture<FedExDatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
