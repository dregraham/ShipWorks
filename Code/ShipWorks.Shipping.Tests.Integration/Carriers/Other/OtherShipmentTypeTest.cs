using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
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
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock => mock.Provide(mock.Build<ISqlAdapter>()));
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

        [Fact]
        public void UpdateDynamicShipmentData_SetsShipmentInsurance_ToOtherShipmentInsurance()
        {
            var shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.Insurance, false)
                .Set(x => x.OriginOriginID, (int) ShipmentOriginSource.Other)
                .Save();

            shipment.Other.Insurance = true;

            var testObject = context.Mock.Create<OtherShipmentType>();

            // Make sure that the values going in are correct.
            Assert.False(shipment.Insurance);
            Assert.True(shipment.Other.Insurance);

            testObject.UpdateDynamicShipmentData(shipment);

            // Now make sure shipment.Insurance is true and that shipment.Other.Insurance is still true.
            Assert.True(shipment.Insurance);
            Assert.True(shipment.Other.Insurance);

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
                Other = new OtherShipmentEntity
                {
                    Insurance = insured,
                    InsuranceValue = insuranceValue
                }
            };

            var testObject = context.Mock.Create<OtherShipmentType>();
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
                .AsOther()
                .Save();

            shipment.Insurance = !insured;
            shipment.Other.Insurance = insured;

            OtherShipmentType shipmentType = context.Mock.Create<OtherShipmentType>();
            shipmentType.UpdateDynamicShipmentData(shipment);

            Assert.Equal(insured, shipment.Other.Insurance);
            Assert.Equal(insured, shipment.Insurance);
        }

        [Fact]
        public void GetPackageAdapters_ReturnOtherValues()
        {
            ShipmentEntity shipment = Create.Shipment(context.Order)
                .AsOther(x => x.Set(o => o.Insurance, true))
                .Set(o => o.Insurance, false)
                .Save();

            OtherShipmentType shipmentType = context.Mock.Create<OtherShipmentType>();
            IEnumerable<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(shipment);
            Assert.True(packageAdapters.First().InsuranceChoice.Insured);

            shipment.Insurance = true;
            shipment.Other.Insurance = false;
            packageAdapters = shipmentType.GetPackageAdapters(shipment);
            Assert.False(packageAdapters.First().InsuranceChoice.Insured);
        }

        public void Dispose() => context.Dispose();
    }
}