using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Usps
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class UspsShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public UspsShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            context.UpdateShippingSetting(x => x.UspsInsuranceProvider = (int) InsuranceProvider.Carrier);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order).AsPostal(x => x.AsUsps()).Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToShippingSetting_WhenShipmentHasSinglePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal(x => x.AsUsps())
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.ShipWorks)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<UspsShipmentType>();

            // Check both values so that we catch incorrect carrier setting 
            //(i.e. the shipment type is using UPSInsuranceProvider instead of iParcel.  Yes, this happened.)
            context.UpdateShippingSetting(x => x.UspsInsuranceProvider = (int)InsuranceProvider.Carrier);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int)InsuranceProvider.Carrier, shipment.InsuranceProvider);

            context.UpdateShippingSetting(x => x.UspsInsuranceProvider = (int)InsuranceProvider.ShipWorks);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int)InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_DoesNotChangeShipment_WhenInsuranceProviderHasNotChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal(x => x.AsUsps().Set(f => f.Confirmation = (int) PostalConfirmationType.Delivery))
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Set(x => x.ShipDate, new DateTime(2016, 3, 26))
                .Set(x => x.ShipCountryCode, "US")
                .Set(x => x.OriginCountryCode, "US")
                .Save();

            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now).Returns(new DateTime(2016, 3, 26));

            var testObject = context.Mock.Create<UspsShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);
            var dirtyEntities = shipment.GetDirtyGraph();

            Assert.Empty(dirtyEntities);
        }

        public void Dispose() => context.Dispose();
    }
}