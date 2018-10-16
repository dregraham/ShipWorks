using System;
using BoDi;
using ShipWorks.Tests.Shared.Database;
using TechTalk.SpecFlow;

namespace ShipWorks.Shipping.Specs
{
    [Binding]
    public class DatabaseBinding
    {
        private static readonly Lazy<DatabaseFixture> dbFixture = new Lazy<DatabaseFixture>(() => new DatabaseFixture());
        private readonly IObjectContainer objectContainer;

        public DatabaseBinding(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InjectDatabase()
        {
            objectContainer.RegisterInstanceAs(dbFixture.Value);
        }
    }
}
