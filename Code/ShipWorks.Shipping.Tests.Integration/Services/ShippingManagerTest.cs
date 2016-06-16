using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Defaults;
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
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingManagerTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;

        public ShippingManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;

            var customerLicense = mock.Create<CustomerLicense>(new TypedParameter(typeof(string), "someKey"));

            // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
            var repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
            repo.Setup(x => x(It.IsAny<string>()))
                .Returns(customerLicense);
            mock.Provide(repo.Object);

            var writer = mock.MockRepository.Create<ICustomerLicenseWriter>();
            writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Callback(() => { });
            mock.Provide(writer.Object);

            var licenseService = mock.MockRepository.Create<ILicenseService>();
            licenseService.Setup(ls => ls.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<object>()))
                .Returns(EditionRestrictionLevel.None);
            mock.Provide(licenseService.Object);
        }

        /// <summary>
        /// Create a shipment using sql adapter retry to get past deadlocks.
        /// </summary>
        public virtual ShipmentEntity CreateShipment(OrderEntity order, ILifetimeScope lifetimeScope)
        {
            ShipmentEntity shipment = null;
            SqlAdapterRetry<Exception> sqlAdapterRetry = new SqlAdapterRetry<Exception>(5, -6, "Integration Test ShippingManagerTest.CreateShipment");

            sqlAdapterRetry.ExecuteWithRetry(() =>
            {
                shipment = ShippingManager.CreateShipment(order, lifetimeScope);
            });

            return shipment;
        }

        [Fact]
        public void CreateShipment_ThrowsPermissionException_WhenUserDoesNotHavePermission()
        {
            mock.Override<ISecurityContext>()
                .Setup(x => x.DemandPermission(PermissionType.ShipmentsCreateEditProcess, context.Order.OrderID))
                .Throws<PermissionException>();

            Assert.Throws<PermissionException>(() => CreateShipment(context.Order, mock.Container));
        }

        [Fact]
        public void CreateShipment_SetsBasicDefaults()
        {
            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(new DateTime(2015, 12, 28, 12, 00, 00), shipment.ShipDate);
        }

        [Fact]
        public void CreateShipment_SetsWeightToSumOfItems_WhenOrderHasItems()
        {
            Modify.Order(context.Order)
                .WithItem(i => i.Set(x => x.Weight, 2.5).Set(x => x.Quantity, 2))
                .WithItem(i => i.Set(x => x.Weight, 1.25).Set(x => x.Quantity, 1))
                .Save();

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(6.25, shipment.ContentWeight);
            Assert.Equal(6.25, shipment.TotalWeight);
            Assert.Equal(6.25, shipment.BilledWeight);
        }

        [Fact]
        public void CreateShipment_SetsShipAddress_ToOrderShipAddress()
        {
            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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
            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(shipmentTypeCode, shipment.ShipmentTypeCode);
        }

        [Fact]
        public void CreateShipment_DelegatesToValidatedAddressManager_ToCopyValidatedAddresses()
        {
            mock.Override<IValidatedAddressManager>();

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

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

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            shipmentType.Verify(x => x.ConfigureNewShipment(shipment));
        }

        [Fact]
        public void CreateShipment_DelegatesToUpdateDynamicShipmentData_OnShipmentType()
        {
            var shipmentType = mock.CreateMock<FedExShipmentType>();
            shipmentType.CallBase = true;

            mock.Override<IShipmentTypeManager>()
                .Setup(x => x.InitialShipmentType(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
        }

        [Fact]
        public void CreateShipment_SavesCustomsItems_WhenShipmentIsInternational()
        {
            Modify.Order(context.Order)
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Foo");
                    i.Set(x => x.UnitPrice, 1.8M);
                    i.Set(x => x.Quantity, 3);
                    i.Set(x => x.Weight, 2.5);
                })
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Bar");
                    i.Set(x => x.UnitPrice, 2.2M);
                    i.Set(x => x.Quantity, 2);
                    i.Set(x => x.Weight, 0.6);
                })
                .Set(x => x.ShipCountryCode, "UK").Save();

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var predicate = new RelationPredicateBucket(ShipmentCustomsItemFields.ShipmentID == shipment.ShipmentID);
                using (EntityCollection<ShipmentCustomsItemEntity> customs = new EntityCollection<ShipmentCustomsItemEntity>())
                {
                    adapter.FetchEntityCollection(customs, predicate);

                    Assert.Equal(2, customs.Count);

                    Assert.Equal("US", customs[0].CountryOfOrigin);
                    Assert.Equal("Foo", customs[0].Description);
                    Assert.Equal(3, customs[0].Quantity);
                    Assert.Equal(2.5, customs[0].Weight);
                    Assert.Equal(1.8M, customs[0].UnitValue);

                    Assert.Equal("US", customs[1].CountryOfOrigin);
                    Assert.Equal("Bar", customs[1].Description);
                    Assert.Equal(2, customs[1].Quantity);
                    Assert.Equal(0.6, customs[1].Weight);
                    Assert.Equal(2.2M, customs[1].UnitValue);
                }
            }
        }

        [Fact]
        public void CreateShipment_SetsCustomsData_WhenShipmentIsInternational()
        {
            Modify.Order(context.Order)
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Foo");
                    i.Set(x => x.UnitPrice, 1.8M);
                    i.Set(x => x.Quantity, 3);
                    i.Set(x => x.Weight, 2.5);
                })
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Bar");
                    i.Set(x => x.UnitPrice, 2.2M);
                    i.Set(x => x.Quantity, 2);
                    i.Set(x => x.Weight, 0.6);
                })
                .Set(x => x.ShipCountryCode, "UK").Save();

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(9.8M, shipment.CustomsValue);
            Assert.True(shipment.CustomsGenerated);
        }

        [Fact]
        public void CreateShipment_SavesShipmentToDatabase()
        {
            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                adapter.FetchEntity(loadedShipment);

                Assert.Equal(EntityState.Fetched, loadedShipment.Fields.State);
            }
        }

        #region "Carrier specific tests"
        #region "FedEx"
        [Fact]
        public void CreateShipment_LoadsFedExData_WhenShipmentTypeIsFedEx()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.FedEx);
            Assert.Equal(1, shipment.FedEx.Packages.Count);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultFedExProfile_WhenShipmentTypeIsFedEx()
        {
            Create.Profile()
                .AsPrimary()
                .AsFedEx(p => p.Set(x => x.DropoffType, (int) FedExDropoffType.RegularPickup))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) FedExDropoffType.RegularPickup, shipment.FedEx.DropoffType);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WhenFedExAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<FedExAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsFedEx(p => p.Set(x => x.FedExAccountID, account.AccountId + 2000))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForFedExShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.FedEx,
                x => x.AsFedEx(p =>
                {
                    p.Set(s => s.DropoffType, (int) FedExDropoffType.DropBox);
                    p.Set(s => s.SmartPostConfirmation, true);
                }));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.FedEx,
                x => x.AsFedEx(p => p.Set(s => s.DropoffType, (int) FedExDropoffType.RegularPickup)));

            context.SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) FedExDropoffType.RegularPickup, shipment.FedEx.DropoffType);
            Assert.True(shipment.FedEx.SmartPostConfirmation);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForFedExShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.FedEx);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "Ups"
        [Fact]
        public void CreateShipment_LoadsUpsData_WhenShipmentTypeIsUps()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.UpsOnLineTools);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.Ups);
            Assert.Equal(1, shipment.Ups.Packages.Count);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultUpsProfile_WhenShipmentTypeIsUps()
        {
            Create.Profile()
                .AsPrimary()
                .AsUps(p => p.Set(x => x.CostCenter, "Foo bar"))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.UpsOnLineTools);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal("Foo bar", shipment.Ups.CostCenter);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WhenUpsAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<UpsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsUps(p => p.Set(x => x.UpsAccountID, account.AccountId + 2000))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.UpsOnLineTools);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForUpsShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.UpsOnLineTools,
                x => x.AsUps(p =>
                {
                    p.Set(s => s.CostCenter, "Bar");
                    p.Set(s => s.CarbonNeutral, true);
                }));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.UpsOnLineTools,
                x => x.AsUps(p => p.Set(s => s.CostCenter, "Foo")));

            context.SetDefaultShipmentType(ShipmentTypeCode.UpsOnLineTools);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal("Foo", shipment.Ups.CostCenter);
            Assert.True(shipment.Ups.CarbonNeutral);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForUpsShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.UpsOnLineTools);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "iParcel"
        [Fact]
        public void CreateShipment_LoadsiParcelData_WhenShipmentTypeIsiParcel()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.iParcel);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.IParcel);
            Assert.Equal(1, shipment.IParcel.Packages.Count);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultiParcelProfile_WhenShipmentTypeIsiParcel()
        {
            Create.Profile()
                .AsPrimary()
                .AsIParcel(p => p.Set(x => x.Service, (int) iParcelServiceType.Saver))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.iParcel);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) iParcelServiceType.Saver, shipment.IParcel.Service);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WheniParcelAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<IParcelAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsIParcel(p => p.Set(x => x.IParcelAccountID, account.AccountId + 2000))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.iParcel);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForiParcelShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.iParcel,
                x => x.AsIParcel(p =>
                {
                    p.Set(s => s.Service, (int) iParcelServiceType.Saver);
                    p.Set(s => s.IsDeliveryDutyPaid, true);
                }));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.iParcel,
                x => x.AsIParcel(p => p.Set(s => s.Service, (int) iParcelServiceType.SaverDeferred)));

            context.SetDefaultShipmentType(ShipmentTypeCode.iParcel);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) iParcelServiceType.SaverDeferred, shipment.IParcel.Service);
            Assert.True(shipment.IParcel.IsDeliveryDutyPaid);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForiParcelShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.iParcel);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "Usps"
        [Fact]
        public void CreateShipment_LoadsUspsData_WhenShipmentTypeIsUsps()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.Usps);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.Postal);
            Assert.NotNull(shipment.Postal.Usps);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultUspsProfile_WhenShipmentTypeIsUsps()
        {
            Create.Profile()
                .AsPrimary()
                .AsPostal(p =>
                {
                    p.AsUsps(u => u.Set(x => x.HidePostage, true));
                    p.Set(x => x.Service, (int) PostalServiceType.FirstClass);
                })
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.Usps);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) PostalServiceType.FirstClass, shipment.Postal.Service);
            Assert.True(shipment.Postal.Usps.HidePostage);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WhenUspsAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<UspsAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsPostal(p => p.AsUsps(u => u.Set(x => x.UspsAccountID, account.UspsAccountID + 2000)))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.Usps);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForUspsShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Usps,
                x => x.AsPostal(o => o.AsUsps(p =>
                {
                    p.Set(s => s.RateShop, false);
                    p.Set(s => s.HidePostage, true);
                })));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Usps,
                x => x.AsPostal(o => o.AsUsps(p => p.Set(s => s.RateShop, true))));

            context.SetDefaultShipmentType(ShipmentTypeCode.Usps);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.True(shipment.Postal.Usps.RateShop);
            Assert.True(shipment.Postal.Usps.HidePostage);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForUspsShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.Usps);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "Endicia"
        [Fact]
        public void CreateShipment_LoadsEndiciaData_WhenShipmentTypeIsEndicia()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.Endicia);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.Postal);
            Assert.NotNull(shipment.Postal.Endicia);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultEndiciaProfile_WhenShipmentTypeIsEndicia()
        {

            Create.Profile()
            .AsPrimary()
            .AsPostal(p =>
            {
                p.AsEndicia(u => u.Set(x => x.ScanBasedReturn, true));
                p.Set(x => x.Service, (int) PostalServiceType.FirstClass);
            })
            .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.Endicia);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) PostalServiceType.FirstClass, shipment.Postal.Service);
            Assert.True(shipment.Postal.Endicia.ScanBasedReturn);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WhenEndiciaAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<EndiciaAccountEntity>()
                .Set(x => x.AccountNumber, "abc123").Save();

            Create.Profile().AsPrimary()
                .AsPostal(p => p.AsEndicia(u => u.Set(x => x.EndiciaAccountID, account.EndiciaAccountID + 2000)))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.Endicia);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForEndiciaShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Endicia,
                x => x.AsPostal(o => o.AsEndicia(p =>
                {
                    p.Set(s => s.ScanBasedReturn, false);
                    p.Set(s => s.StealthPostage, true);
                })));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Endicia,
                x => x.AsPostal(o => o.AsEndicia(p => p.Set(s => s.ScanBasedReturn, true))));

            context.SetDefaultShipmentType(ShipmentTypeCode.Endicia);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.True(shipment.Postal.Endicia.ScanBasedReturn);
            Assert.True(shipment.Postal.Endicia.StealthPostage);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForEndiciaShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.Endicia);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "OnTrac"
        [Fact]
        public void CreateShipment_LoadsOnTracData_WhenShipmentTypeIsOnTrac()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.OnTrac);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.OnTrac);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultOnTracProfile_WhenShipmentTypeIsOnTrac()
        {
            Create.Profile()
                .AsPrimary()
                .AsOnTrac(u => u.Set(x => x.Service, (int) OnTracServiceType.Sunrise))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.OnTrac);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) OnTracServiceType.Sunrise, shipment.OnTrac.Service);
        }

        [Fact]
        public void CreateShipment_ResetsAccountData_WhenOnTracAccountDoesNotExist()
        {
            var account = Create.CarrierAccount<OnTracAccountEntity>().Save();

            Create.Profile().AsPrimary()
                .AsOnTrac(u => u.Set(x => x.OnTracAccountID, account.OnTracAccountID + 2000))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.OnTrac);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal(account.AccountId, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForOnTracShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.OnTrac,
                x => x.AsOnTrac(p =>
                {
                    p.Set(s => s.Service, (int) OnTracServiceType.Sunrise);
                    p.Set(s => s.DimsAddWeight, true);
                }));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.OnTrac,
                x => x.AsOnTrac(p => p.Set(s => s.Service, (int) OnTracServiceType.SunriseGold)));

            context.SetDefaultShipmentType(ShipmentTypeCode.OnTrac);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal((int) OnTracServiceType.SunriseGold, shipment.OnTrac.Service);
            Assert.True(shipment.OnTrac.DimsAddWeight);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOnTracTYpe_ForOnTracShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.OnTrac);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #region "Other"
        [Fact]
        public void CreateShipment_LoadsOtherData_WhenShipmentTypeIsOther()
        {
            context.SetDefaultShipmentType(ShipmentTypeCode.Other);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.NotNull(shipment.Other);
        }

        [Fact]
        public void CreateShipment_AppliesDefaultOtherProfile_WhenShipmentTypeIsOther()
        {
            Create.Profile()
                .AsPrimary()
                .AsOther(u => u.Set(x => x.Service, "Foo"))
                .Save();

            context.SetDefaultShipmentType(ShipmentTypeCode.Other);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal("Foo", shipment.Other.Service);
        }

        [Fact]
        public void CreateShipment_AppliesProfilesInOrder_ForOtherShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther(p =>
                {
                    p.Set(s => s.Service, "Bar");
                    p.Set(s => s.Carrier, "Baz");
                }));

            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.Other,
                x => x.AsOther(p => p.Set(s => s.Service, "Foo")));

            context.SetDefaultShipmentType(ShipmentTypeCode.Other);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.Equal("Foo", shipment.Other.Service);
            Assert.Equal("Baz", shipment.Other.Carrier);
        }

        [Fact]
        public void CreateShipment_DoesNotApplyProfileForOtherTYpe_ForOtherShipment()
        {
            CreateProfileRule(context.Order.OrderID, ShipmentTypeCode.OnTrac,
                x => x.AsOnTrac().Set(p => p.ReturnShipment, true));

            context.SetDefaultShipmentType(ShipmentTypeCode.Other);

            ShipmentEntity shipment = CreateShipment(context.Order, mock.Container);

            Assert.False(shipment.ReturnShipment);
        }
        #endregion

        #endregion

        /// <summary>
        /// Create a filter node
        /// </summary>
        private static FilterNodeEntity CreateFilterNode(long objectID)
        {
            var filter = Create.Entity<FilterEntity>()
                .Save();

            var sequence = Create.Entity<FilterSequenceEntity>()
                .Set(x => x.Filter, filter)
                .Save();

            var content = Create.Entity<FilterNodeContentEntity>()
                .Save();

            Create.Entity<FilterNodeContentDetailEntity>()
                .Set(x => x.FilterNodeContentID, content.FilterNodeContentID)
                .Set(x => x.ObjectID, objectID)
                .Save();

            return Create.Entity<FilterNodeEntity>()
                .Set(x => x.FilterSequence, sequence)
                .Set(x => x.FilterNodeContent, content)
                .Save();
        }

        /// <summary>
        /// Create a profile that will be associated with a rule
        /// </summary>
        private static void CreateProfileRule(long objectId, ShipmentTypeCode shipmentType,
            Func<ProfileEntityBuilder, EntityBuilder<ShippingProfileEntity>> configureProfile)
        {
            var profile = configureProfile(Create.Profile())
                //.Set(x => x.Name, Path.GetRandomFileName())
                //.DoNotSetDefaults()
                .Save();

            var node = CreateFilterNode(objectId);

            Create.Entity<ShippingDefaultsRuleEntity>()
                .Set(x => x.ShippingProfileID, profile.ShippingProfileID)
                .Set(x => x.ShipmentTypeCode, shipmentType)
                .Set(x => x.FilterNodeID, node.FilterNodeID)
                .Save();

            ShippingDefaultsRuleManager.CheckForChangesNeeded();
        }

        public void Dispose() => context.Dispose();
    }
}
