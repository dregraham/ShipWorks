﻿using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Other
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OtherShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public OtherShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order).AsOther().Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToShippingSetting_WhenShipmentHasSinglePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<OtherShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);

            Assert.Equal((int) InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_DoesNotChangeShipment_WhenInsuranceProviderHasNotChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.ShipWorks)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Set(x => x.ShipDate, new DateTime(2016, 3, 26))
                .Save();

            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now).Returns(new DateTime(2016, 3, 26));

            var testObject = context.Mock.Create<OtherShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);
            var dirtyEntities = shipment.GetDirtyGraph();

            Assert.Empty(dirtyEntities);
        }

        public void Dispose() => context.Dispose();
    }
}