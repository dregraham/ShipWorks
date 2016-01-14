using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShippingManagerTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;

        public ShippingManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
        }

        [Fact]
        public void CreateShipment_ThrowsPermissionException_WhenUserDoesNotHavePermission()
        {
            mock.Override<ISecurityContext>()
                .Setup(x => x.DemandPermission(PermissionType.ShipmentsCreateEditProcess, context.Order.OrderID))
                .Throws<PermissionException>();

            Assert.Throws<PermissionException>(() => ShippingManager.CreateShipment(context.Order, mock.Container));
        }

        [Fact]
        public void CreateShipment_SetsBasicDefaults()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal(ShipmentTypeCode.None, shipment.ShipmentTypeCode);
            Assert.False(shipment.Processed);
            Assert.False(shipment.Voided);
            Assert.Equal(0, shipment.ShipmentCost);
            Assert.Equal(string.Empty, shipment.TrackingNumber);
            Assert.Equal((int) ResidentialDeterminationType.CommercialIfCompany, shipment.ResidentialDetermination);
            Assert.True(shipment.ResidentialResult);
            Assert.False(shipment.ReturnShipment);
            Assert.False(shipment.Insurance);
            Assert.Equal((int) InsuranceProvider.ShipWorks, shipment.InsuranceProvider);
            Assert.Equal((int) BestRateEventTypes.None, shipment.BestRateEvents);
            Assert.Equal((int) ShipSenseStatus.NotApplied, shipment.ShipSenseStatus);
            Assert.Equal("<ChangeSets />", shipment.ShipSenseChangeSets);
            Assert.NotEmpty(shipment.ShipSenseEntry);
            Assert.Equal(string.Empty, shipment.OnlineShipmentID);
            Assert.Equal((int) ThermalLanguage.None, shipment.RequestedLabelFormat);
            Assert.Equal((int) BilledType.Unknown, shipment.BilledType);
            Assert.False(shipment.CustomsGenerated);
            Assert.Equal(0, shipment.CustomsValue);
        }

        [Fact]
        public void CreateShipment_SetsShipDateToNoonToday()
        {
            mock.Override<IDateTimeProvider>()
                .Setup(x => x.Now)
                .Returns(new DateTime(2015, 12, 28, 15, 30, 12));

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal(new DateTime(2015, 12, 28, 12, 00, 00), shipment.ShipDate);
        }

        [Fact]
        public void CreateShipment_SetsWeightToSumOfItems_WhenOrderHasItems()
        {
            Modify.Order(context.Order)
                .WithItem(i => i.Set(x => x.Weight, 2.5).Set(x => x.Quantity, 2))
                .WithItem(i => i.Set(x => x.Weight, 1.25).Set(x => x.Quantity, 1))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal(6.25, shipment.ContentWeight);
            Assert.Equal(6.25, shipment.TotalWeight);
            Assert.Equal(6.25, shipment.BilledWeight);
        }

        [Fact]
        public void CreateShipment_SetsShipAddress_ToOrderShipAddress()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal("1 Memorial Dr.", shipment.ShipStreet1);
            Assert.Equal("Suite 2000", shipment.ShipStreet2);
            Assert.Equal("St. Louis", shipment.ShipCity);
            Assert.Equal("MO", shipment.ShipStateProvCode);
            Assert.Equal("63102", shipment.ShipPostalCode);
            Assert.Equal("US", shipment.ShipCountryCode);
        }

        [Fact]
        public void CreateShipment_CopiesShipAddressValidation_FromOrder()
        {
            Modify.Order(context.Order)
                .Set(x => x.ShipAddressValidationError, "Foo")
                .Set(x => x.ShipAddressValidationStatus, (int) AddressValidationStatusType.SuggestionIgnored)
                .Set(x => x.ShipAddressValidationSuggestionCount, 6)
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal("Foo", shipment.ShipAddressValidationError);
            Assert.Equal((int) AddressValidationStatusType.SuggestionIgnored, shipment.ShipAddressValidationStatus);
            Assert.Equal(6, shipment.ShipAddressValidationSuggestionCount);
            Assert.Equal((int) ValidationDetailStatusType.Unknown, shipment.ShipResidentialStatus);
            Assert.Equal((int) ValidationDetailStatusType.Unknown, shipment.ShipPOBox);
            Assert.Equal((int) ValidationDetailStatusType.Unknown, shipment.ShipUSTerritory);
            Assert.Equal((int) ValidationDetailStatusType.Unknown, shipment.ShipMilitaryAddress);
        }

        [Fact]
        public void CreateShipment_SetsOriginAddress_ToStoreAddress()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal("A Test Store", shipment.OriginUnparsedName);
            Assert.Equal("123 Main St.", shipment.OriginStreet1);
            Assert.Equal("Suite 456", shipment.OriginStreet2);
            Assert.Equal("St. Louis", shipment.OriginCity);
            Assert.Equal("MO", shipment.OriginStateProvCode);
            Assert.Equal("63123", shipment.OriginPostalCode);
            Assert.Equal("US", shipment.OriginCountryCode);
            Assert.Equal((int) ShipmentOriginSource.Store, shipment.OriginOriginID);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        public void CreateShipment_SetsShipmentType_BasedOnShipmentTypeManager(ShipmentTypeCode shipmentTypeCode)
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(x => x.ShipmentTypeCode).Returns(shipmentTypeCode);

            mock.Override<IShipmentTypeManager>()
                .Setup(x => x.InitialShipmentType(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal(shipmentTypeCode, shipment.ShipmentTypeCode);
        }

        [Fact]
        public void CreateShipment_DelegatesToValidatedAddressManager_ToCopyValidatedAddresses()
        {
            mock.Override<IValidatedAddressManager>();

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            mock.Mock<IValidatedAddressManager>()
                .Verify(x => x.CopyValidatedAddresses(It.IsAny<SqlAdapter>(), context.Order.OrderID, "Ship", shipment.ShipmentID, "Ship"));
        }

        [Fact]
        public void CreateShipment_CreatesCustomsItems_WhenShipmentIsInternational()
        {
            Modify.Order(context.Order)
                .WithShipAddress("1 Memorial Dr.", "Suite 2000", "London", string.Empty, "63102", "UK")
                .WithItem(i => i.Set(x => x.Weight, 2).Set(x => x.Quantity, 1).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Weight, 3).Set(x => x.Quantity, 4).Set(x => x.Name, "Bar"))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Equal(2, shipment.CustomsItems.Count);

            Assert.Equal(1, shipment.CustomsItems[0].Quantity);
            Assert.Equal("Foo", shipment.CustomsItems[0].Description);
            Assert.Equal(2, shipment.CustomsItems[0].Weight);

            Assert.Equal(4, shipment.CustomsItems[1].Quantity);
            Assert.Equal("Bar", shipment.CustomsItems[1].Description);
            Assert.Equal(3, shipment.CustomsItems[1].Weight);
        }

        [Fact]
        public void CreateShipment_CreatesCustomsItems_WhenShipmentIsDomestic()
        {
            Modify.Order(context.Order)
                .WithItem(i => i.Set(x => x.Weight, 2).Set(x => x.Quantity, 1).Set(x => x.Name, "Foo"))
                .WithItem(i => i.Set(x => x.Weight, 3).Set(x => x.Quantity, 4).Set(x => x.Name, "Bar"))
                .Save();

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.Empty(shipment.CustomsItems);
        }

        [Fact]
        public void CreateShipment_DelegatesToConfigureNewShipment_OnShipmentType()
        {
            var shipmentType = mock.CreateMock<FedExShipmentType>();
            shipmentType.CallBase = true;

            mock.Override<IShipmentTypeManager>()
                .Setup(x => x.InitialShipmentType(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            shipmentType.Verify(x => x.ConfigureNewShipment(shipment));
        }

        [Fact]
        public void CreateShipment_SavesShipmentToDatabase()
        {
            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                adapter.FetchEntity(loadedShipment);

                Assert.Equal(EntityState.Fetched, loadedShipment.Fields.State);
            }
        }

        #region "Carrier specific tests"

        [Fact]
        public void CreateShipment_LoadsFedExData_WhenShipmentTypeIsFedEx()
        {
            SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.FedEx);
            Assert.Equal(1, shipment.FedEx.Packages.Count);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultFedExProfile_WhenShipmentTypeIsFedEx()
        {
            var profile = Create.Entity<ShippingProfileEntity>()
                .Set(x => x.ShipmentTypeCode, ShipmentTypeCode.FedEx)
                .Set(x => x.ShipmentTypePrimary, true)
                .Save();

            Create.Entity<FedExProfileEntity>()
                .Set(x => x.ShippingProfile, profile)
                .Set(x => x.DropoffType, (int) FedExDropoffType.RegularPickup)
                .Save();

            SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = ShippingManager.CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.FedEx);

            Assert.Equal((int) FedExDropoffType.RegularPickup, shipment.FedEx.DropoffType);
        }

        /// <summary>
        /// Set the default shipment type in the shipping settings
        /// </summary>
        private static void SetDefaultShipmentType(ShipmentTypeCode defaultType)
        {
            var settings = ShippingSettings.Fetch();
            settings.DefaultShipmentTypeCode = defaultType;
            ShippingSettings.Save(settings);
        }

        #endregion

        public void Dispose() => context.Dispose();
    }
}
