﻿using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.FedEx
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class FedExShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public FedExShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            context.UpdateShippingSetting(x => x.FedExInsuranceProvider = (int) InsuranceProvider.Carrier);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage()).Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChangedWithMultiplePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage().WithPackage())
                .Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToCarrier_WhenShipmentHasMultiplePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage()
                    .WithPackage()
                    .Set(f => f.CodOriginID, (int) ShipmentOriginSource.Other))
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.ShipWorks)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<FedExShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);

            Assert.Equal((int) InsuranceProvider.Carrier, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToShippingSetting_WhenShipmentHasSinglePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.CodOriginID, (int) ShipmentOriginSource.Other))
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<FedExShipmentType>();

            // Check both values so that we catch incorrect carrier setting 
            //(i.e. the shipment type is using UPSInsuranceProvider instead of iParcel.  Yes, this happened.)
            context.UpdateShippingSetting(x => x.FedExInsuranceProvider = (int)InsuranceProvider.Carrier);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int) InsuranceProvider.Carrier, shipment.InsuranceProvider);

            context.UpdateShippingSetting(x => x.FedExInsuranceProvider = (int)InsuranceProvider.ShipWorks);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int)InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_DoesNotChangeShipment_WhenInsuranceProviderHasNotChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsFedEx(x => x.WithPackage()
                    .WithPackage()
                    .Set(f => f.HomeDeliveryDate, new DateTime(2016, 3, 26))
                    .Set(f => f.CodOriginID, (int) ShipmentOriginSource.Other))
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Set(x => x.ShipDate, new DateTime(2016, 3, 26))
                .Save();

            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now).Returns(new DateTime(2016, 3, 26));

            var testObject = context.Mock.Create<FedExShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);
            var dirtyEntities = shipment.GetDirtyGraph();

            Assert.Empty(dirtyEntities);
        }

        public void Dispose() => context.Dispose();
    }
}