﻿using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.WebTools
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class WebToolsShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public WebToolsShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Provide(mock.Build<ISqlAdapter>());
                    mock.Override<IMainForm>();
                });
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order).AsPostal().Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToShippingSetting_WhenShipmentHasSinglePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal()
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<PostalWebShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);

            Assert.Equal((int) InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_DoesNotChangeShipment_WhenInsuranceProviderHasNotChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal(x => x.Set(f => f.Confirmation = (int) PostalConfirmationType.Delivery))
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.ShipWorks)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Set(x => x.ShipDate, new DateTime(2016, 3, 26))
                .Set(x => x.ShipCountryCode, "US")
                .Set(x => x.OriginCountryCode, "US")
                .Save();

            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now).Returns(new DateTime(2016, 3, 26));

            var testObject = context.Mock.Create<PostalWebShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);
            var dirtyEntities = shipment.GetDirtyGraph();

            Assert.Empty(dirtyEntities);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsShipmentInsurance_ToPostalShipmentInsurance()
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal()
                .Set(x => x.Insurance, false)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            shipment.Postal.Insurance = true;

            var testObject = context.Mock.Create<PostalWebShipmentType>();

            // Make sure that the values going in are correct.
            Assert.False(shipment.Insurance);
            Assert.True(shipment.Postal.Insurance);

            testObject.UpdateDynamicShipmentData(shipment);

            // Now make sure shipment.Insurance is true and that shipment.Postal.Insurance is still true.
            Assert.True(shipment.Insurance);
            Assert.True(shipment.Postal.Insurance);

            testObject.UpdateDynamicShipmentData(shipment);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateDynamicShipmentData_SetsShipmentInsuranceFromCarrierShipment(bool insured)
        {
            var shipment = Create.Shipment(context.Order)
                .AsPostal()
                .Save();

            shipment.Insurance = !insured;
            shipment.Postal.Insurance = insured;

            PostalWebShipmentType shipmentType = context.Mock.Create<PostalWebShipmentType>();
            shipmentType.UpdateDynamicShipmentData(shipment);

            Assert.Equal(insured, shipment.Postal.Insurance);
            Assert.Equal(insured, shipment.Insurance);
        }

        public void Dispose() => context.Dispose();
    }
}