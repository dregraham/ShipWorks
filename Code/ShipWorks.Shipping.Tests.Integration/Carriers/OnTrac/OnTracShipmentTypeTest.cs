using System;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.OnTrac
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnTracShipmentTypeTest : IDisposable
    {
        private readonly DataContext context;

        public OnTracShipmentTypeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Provide(mock.Build<ISqlAdapter>());
                    mock.Override<IMainForm>();
                });
            context.Mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
            context.UpdateShippingSetting(x => x.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks);
        }

        [Fact]
        public void SavingThroughShippingManager_ShouldNotUpdateRowVersion_WhenNoDataHasChanged()
        {
            var shipment = Create.Shipment(context.Order).AsOnTrac().Save();

            CarrierTestUtilities.VerifyRowVersionDoesNotChangeAfterSecondSave(this, shipment);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsInsuranceProviderToShippingSetting_WhenShipmentHasSinglePackages()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOnTrac()
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.Carrier)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            var testObject = context.Mock.Create<OnTracShipmentType>();

            // Check both values so that we catch incorrect carrier setting
            //(i.e. the shipment type is using UPSInsuranceProvider instead of iParcel.  Yes, this happened.)
            context.UpdateShippingSetting(x => x.OnTracInsuranceProvider = (int) InsuranceProvider.Carrier);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int) InsuranceProvider.Carrier, shipment.InsuranceProvider);

            context.UpdateShippingSetting(x => x.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks);
            testObject.UpdateDynamicShipmentData(shipment);
            Assert.Equal((int) InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
        }

        [Fact]
        public void UpdateDynamicShipmentData_DoesNotChangeShipment_WhenInsuranceProviderHasNotChanged()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOnTrac()
                .Set(x => x.InsuranceProvider, (int) InsuranceProvider.ShipWorks)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Set(x => x.ShipDate, new DateTime(2016, 3, 26))
                .Save();

            context.Mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now).Returns(new DateTime(2016, 3, 26));

            var testObject = context.Mock.Create<OnTracShipmentType>();

            testObject.UpdateDynamicShipmentData(shipment);
            var dirtyEntities = shipment.GetDirtyGraph();

            Assert.Empty(dirtyEntities);
        }

        [Fact]
        public void UpdateDynamicShipmentData_SetsShipmentInsurance_ToOnTracShipmentInsurance()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOnTrac()
                .Set(x => x.Insurance, false)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            shipment.OnTrac.Insurance = true;

            var testObject = context.Mock.Create<OnTracShipmentType>();

            // Make sure that the values going in are correct.
            Assert.False(shipment.Insurance);
            Assert.True(shipment.OnTrac.Insurance);

            testObject.UpdateDynamicShipmentData(shipment);

            // Now make sure shipment.Insurance is true and that shipment.OnTrac.Insurance is still true.
            Assert.True(shipment.Insurance);
            Assert.True(shipment.OnTrac.Insurance);

            testObject.UpdateDynamicShipmentData(shipment);
        }

        [Theory]
        [InlineData(true, 9.99)]
        [InlineData(false, 6.66)]
        public void Insured_ReturnsInsuranceFromShipment(bool insured, decimal insuranceValue)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Insurance = !insured,
                OnTrac = new OnTracShipmentEntity
                {
                    Insurance = insured,
                    InsuranceValue = insuranceValue
                }
            };

            var testObject = context.Mock.Create<OnTracShipmentType>();

            ShipmentParcel parcel = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(insured, parcel.Insurance.Insured);
            Assert.Equal(insuranceValue, parcel.Insurance.InsuranceValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpdateDynamicShipmentData_SetsShipmentInsuranceFromCarrierShipment(bool insured)
        {
            var shipment = Create.Shipment(context.Order)
                .AsOnTrac()
                .Save();

            shipment.Insurance = !insured;
            shipment.OnTrac.Insurance = insured;

            OnTracShipmentType shipmentType = context.Mock.Create<OnTracShipmentType>();
            shipmentType.UpdateDynamicShipmentData(shipment);

            Assert.Equal(insured, shipment.OnTrac.Insurance);
            Assert.Equal(insured, shipment.Insurance);
        }

        public void Dispose() => context.Dispose();
    }
}