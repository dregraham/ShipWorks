using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using System.Data;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.BestRate
{
    public class UspsBestRateBrokerTest
    {
        private UspsAccountEntity account;
        private RateGroup rateGroup;
        private RateResult rate1;
        private RateResult rate2;
        private RateResult rate3;
        private UspsBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the UspsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<UspsAccountEntity>> genericRepositoryMock;
        private Mock<UspsShipmentType> genericShipmentTypeMock;
        private int timesGetRatesCalled = 0;

        private UspsAccountEntity account1;
        private UspsAccountEntity account2;
        private UspsAccountEntity account3;


        public UspsBestRateBrokerTest()
        {
            account = new UspsAccountEntity { UspsAccountID = 1, CountryCode = "US"};

            account1 = new UspsAccountEntity { UspsAccountID = 1, CountryCode = "US" };
            account2 = new UspsAccountEntity { UspsAccountID = 2, CountryCode = "US" };
            account3 = new UspsAccountEntity { UspsAccountID = 3, CountryCode = "US" };


            rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            
            rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });

            genericRepositoryMock = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();
            genericRepositoryMock.Setup(r => r.Accounts).Returns(() =>
            {
                return new List<UspsAccountEntity> { account1, account2, account3 };
            });

            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<UspsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity { Usps = new UspsShipmentEntity() });

            testObject = new UspsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) =>
                {
                    getRatesShipments.Add(EntityUtility.CloneEntity(shipment));
                    timesGetRatesCalled++;
                    return rateGroup;
                }
            };

            testShipment = new ShipmentEntity
            {
                ShipmentType = (int)ShipmentTypeCode.BestRate, 
                ContentWeight = 12.1, 
                BestRate = new BestRateShipmentEntity
                {
                    DimsWeight = 3.4,
                    DimsAddWeight = true
                },
                OriginCountryCode = "US"
            };
        }

        [Fact]
        public void HasAccounts_DelegatesToAccountRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.Accounts, Times.Once());
        }

        [Fact]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity> { new UspsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.True(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.False(hasAccounts);
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForProfileAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(1));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForProfileAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(1, timesGetRatesCalled);
            
            foreach (var shipment in getRatesShipments)
            {
                Assert.Equal(ShipmentTypeCode.Usps, (ShipmentTypeCode) shipment.ShipmentType);
                Assert.Equal(12.1, shipment.ContentWeight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsAllRates_WithNoFilter()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(3, rates.Rates.Count);
            Assert.True(rates.Rates.Any(r => r.RateID == rate1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == rate2.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == rate3.RateID));
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRate_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRate_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_DoesNotReturnNonSelectableRates()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.False(rates.Rates.Any(r=>r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(1, rates.Rates.Count);
        }

        //[Fact]
        //public void GetBestRates_DoesNotIncludeMediaMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.MediaMail);
        //}

        //[Fact]
        //public void GetBestRates_DoesNotIncludeLibrayMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.LibraryMail);
        //}

        //[Fact]
        //public void GetBestRates_DoesNotIncludeBPM()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.BoundPrintedMatter);
        //}

        //[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\GetBestRates_DoesNotIncludeTypes.csv", "Usps_GetBestRates_DoesNotIncludeTypes#csv", DataAccessMethod.Sequential)]
        //[DeploymentItem(@"Shipping\Carriers\Postal\Usps\BestRate\Usps_GetBestRates_DoesNotIncludeTypes.csv")]
        [CsvData(@"Shipping\Carriers\Postal\Usps\BestRate", "Usps_GetBestRates_DoesNotIncludeTypes")]
        [Theory]
        public void GetBestRates_ExcludesVariousTypes(DataRow row)
        {
            PostalServiceType excludedServiceType = (PostalServiceType)Enum.Parse(typeof (PostalServiceType), row[0].ToString());

            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(excludedServiceType, PostalConfirmationType.None))
                {
                    ServiceLevel = ServiceLevelType.OneDay
                };

            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None))
                {
                    ServiceLevel = ServiceLevelType.OneDay
                };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.False(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.True(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.Equal(1, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_UpdatesDescriptionWhenPartOfRateGroup()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Foo", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("       Bar", string.Empty, 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            RateResult result3 = new RateResult("Baz", "3") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result4 = new RateResult("   Other", string.Empty, 4, new PostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);
            rateGroup.Rates.Add(result3);
            rateGroup.Rates.Add(result4);

            RateGroup bestRates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal("USPS Foo Bar", bestRates.Rates.Single(r => r.RateID == result2.RateID).Description);
            Assert.Equal("USPS Baz Other", bestRates.Rates.Single(r => r.RateID == result4.RateID).Description);
            Assert.Equal("4", bestRates.Rates.Single(r => r.RateID == result2.RateID).Days);
            Assert.Equal("3", bestRates.Rates.Single(r => r.RateID == result4.RateID).Days);
        }

        [Fact]
        public void GetBestRates_OriginalUspsShipmentDetailsAreRestoredAfterCall()
        {
            PostalShipmentEntity UspsShipment = new PostalShipmentEntity();
            testShipment.Postal = UspsShipment;
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(UspsShipment, testShipment.Postal);
        }

        [Fact]
        public void GetBestRates_NoRatesAreReturned_WhenShippingExceptionIsThrown()
        {
            testObject.GetRatesAction = (shipment, type) =>
            {
                throw new ShippingException();
            };

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(0, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown()
        {
            ShippingException exception = new ShippingException();
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            testObject.GetRatesAction = (shipment, type) =>
            {
                throw exception;
            };

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.True(brokerExceptions.Any(e => e.InnerException.Equals(exception)));
        }

        [Fact]
        public void GetBestRates_SetsPackageDetails()
        {
            testShipment.BestRate.DimsHeight = 3;
            testShipment.BestRate.DimsWidth = 5;
            testShipment.BestRate.DimsLength = 2;

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(3, shipment.Postal.DimsHeight);
                Assert.Equal(5, shipment.Postal.DimsWidth);
                Assert.Equal(2, shipment.Postal.DimsLength);
                Assert.Equal(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
                Assert.Equal(3.4, shipment.Postal.DimsWeight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileServiceAndPackagingType()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Service = (int)PostalServiceType.ConsolidatorDomestic;
                                       x.Postal.PackagingType = (int) PostalPackagingType.FlatRateSmallBox;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(PostalServiceType.PriorityMail, (PostalServiceType)shipment.Postal.Service);
                Assert.Equal(PostalPackagingType.Package, (PostalPackagingType)shipment.Postal.PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Usps.UspsAccountID = 999;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(getRatesShipments.Any(x => x.Postal.Usps.UspsAccountID == 1));
            Assert.False(getRatesShipments.Any(x => x.Postal.Usps.UspsAccountID == 999));
        }

        [Fact]
        public void GetBestRates_OverridesProfileWeight()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.ContentWeight = 123;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(3.4, shipment.Postal.DimsWeight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesDimsAddWeight_Test()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.ContentWeight = 123;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.Equal(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagToBestRateResultTag()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                BestRateResultTag tag = rate.Tag as BestRateResultTag;

                if (tag == null)
                {
                    Assert.False(true, $"Tag is {rate.Tag.GetType()} instead of BestRateResultTag");
                }

                Assert.IsAssignableFrom<Action<ShipmentEntity>>(tag.RateSelectionDelegate);
                Assert.NotNull(tag.OriginalTag);
                Assert.NotNull(tag.ResultKey);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagResultKeyToPostalAndServiceType()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Usps Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.True(resultKeys.Contains("PostalExpress Mail (Premium)"));
            Assert.True(resultKeys.Contains("PostalStandard Post"));
        }

       [Fact]
        public void GetBestRates_AddsUspsToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("USPS Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Select(x => x.Description).Contains("USPS Ground"));
            Assert.True(rates.Rates.Select(x => x.Description).Contains("USPS Some Service"));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_WhenUsingShipWorksInsurance_Test()
        {
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { UspsInsuranceProvider = (int)InsuranceProvider.ShipWorks };

            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(shippingSettings));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_WhenUsingCarrierInsurance_Test()
        {
            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity { UspsInsuranceProvider = (int) InsuranceProvider.Carrier };

            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(shippingSettings));
        }

        [Fact]
        public void Configure_ShouldNotCallCheckExpress1RatesOnSettings_WithShipmentType()
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();

            testObject.Configure(brokerSettings.Object);

            brokerSettings.Verify(x => x.CheckExpress1Rates(testObject.ShipmentType), Times.Never);
        }

        [Fact]
        public void GetBestRates_ReturnsNoRates_WhenShipmentTotalWeightTooHeavy()
        {
            testShipment.TotalWeight = 70.1;
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(0, rates.Rates.Count);
        }
    }
}
