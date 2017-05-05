using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class UpsSurchargeFactoryTest : IDisposable
    {
        private readonly ITestOutputHelper output;
        readonly AutoMock mock;

        public UpsSurchargeFactoryTest(ITestOutputHelper output)
        {
            this.output = output;
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Get_AllIUpsSurchargesAreReturned()
        {
            Assembly shippingAssembly = AssemblyProvider.GetAssemblies().SingleOrDefault(a => a.FullName.StartsWith("ShipWorks.Shipping,"));
            Assert.NotNull(shippingAssembly);

            var iSurchargeType = typeof(IUpsSurcharge);
            IEnumerable<Type> surchargeTypes =
                shippingAssembly.GetTypes().Where(p => iSurchargeType.IsAssignableFrom(p) && iSurchargeType != p).ToList();

            var surcharges = new Dictionary<UpsSurchargeType, double>();
            var zoneFiles = new UpsLocalRatingZoneFileEntity();

            var testObject = mock.Create<UpsSurchargeFactory>();
            var retreivedSurcharges = testObject.Get(surcharges, zoneFiles).ToList();

            var missingSurchargeMessage = new StringBuilder();
            foreach (Type surchargeType in surchargeTypes)
            {

                if (retreivedSurcharges.Where(retrievedSurcharge => retrievedSurcharge.GetType() == surchargeType).None())
                {
                    missingSurchargeMessage.AppendLine($"Missing {surchargeType.FullName}");
                }
            }

            string message = missingSurchargeMessage.ToString();
            output.WriteLine(message);
            Assert.Empty(message);
            Assert.Equal(surchargeTypes.Count(), retreivedSurcharges.Count);
            Assert.Equal(surchargeTypes.Count(), retreivedSurcharges.Distinct().Count());
        }

        [Fact]
        public void Get_SurchargesAreInCorrectOrder()
        {
            var expectedTypes = new[]
            {
                typeof(DeliveryAreaSurcharge),
                typeof(LargePackageUpsSurcharge),
                typeof(FuelGroundSurcharge),
                typeof(SaturdayDeliverySurcharge),
                typeof(FuelAirSurcharge),
                typeof(ReturnsSurcharge),
                typeof(AdditionalHandlingSurcharge),
                typeof(CarbonNeutralSurcharge),
                typeof(CODSurcharge),
                typeof(DryIceSurcharge),
                typeof(ShipperReleaseSurcharge),
                typeof(SignatureSurcharge),
                typeof(VerbalConfirmationSurcharge),
                typeof(ThirdPartyBillingSurcharge)
            };

            var testObject = mock.Create<UpsSurchargeFactory>();
            var surcharges = new Dictionary<UpsSurchargeType, double>();
            var zoneFiles = new UpsLocalRatingZoneFileEntity();
            var retreivedSurcharges = testObject.Get(surcharges, zoneFiles).Select(s=>s.GetType()).ToArray();

            Assert.Equal(expectedTypes, retreivedSurcharges);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}