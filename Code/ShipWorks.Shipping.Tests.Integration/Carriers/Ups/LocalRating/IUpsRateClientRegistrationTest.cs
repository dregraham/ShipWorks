using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.ReadOnlyEntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class IUpsRateClientRegistrationTest : IDisposable
    {
        private readonly IContainer container;
        private readonly Func<UpsAccountEntity, IUpsRateClient> upsRateClientFactory;

        public IUpsRateClientRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
            upsRateClientFactory = container.Resolve<Func<UpsAccountEntity, IUpsRateClient>>();
        }

        [Fact]
        public void EnsureApiRateClientIsReturned_WhenAccountHasDisabledLocalRates()
        {
            var account = new UpsAccountEntity()
            {
                LocalRatingEnabled = false
            };

            Assert.IsType<UpsApiRateClient>(upsRateClientFactory(account));
        }

        [Fact]
        public void EnsureLocalRateClientIsReturned_WhenAccountHasDisabledLocalRates()
        {
            var account = new UpsAccountEntity()
            {
                LocalRatingEnabled = true
            };

            Assert.IsType<UpsLocalRateClient>(upsRateClientFactory(account));
        }

        public void Dispose() => container.Dispose();
    }
}
