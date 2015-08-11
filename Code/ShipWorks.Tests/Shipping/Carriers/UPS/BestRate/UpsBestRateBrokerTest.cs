using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.BestRate
{
    public class UpsBestRateBrokerTest
    {
        private UpsAccountEntity account1;
        private UpsAccountEntity account2;
        private UpsAccountEntity account3;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;
        private RateGroup rateGroup3;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private RateResult account2Rate1;
        private RateResult account3Rate1;
        private RateResult account3Rate2;
        private UpsBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the UpsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments; 

        private Mock<ICarrierAccountRepository<UpsAccountEntity>> genericRepositoryMock;
        private Mock<UpsShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;

        [TestInitialize]
        public void Initialize()
        {
            account1 = new UpsAccountEntity { UpsAccountID = 1, CountryCode = "US"};
            account2 = new UpsAccountEntity { UpsAccountID = 2, CountryCode = "US" };
            account3 = new UpsAccountEntity { UpsAccountID = 3, CountryCode = "US" };

            account1Rate1 = new RateResult("Account 1a", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays};
            account1Rate2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, UpsServiceType.UpsExpressSaver) { ServiceLevel = ServiceLevelType.Anytime };
            account3Rate2 = new RateResult("Account 3b", "3", 10, UpsServiceType.Ups2DayAirAM) { ServiceLevel = ServiceLevelType.TwoDays };

            rateGroup1 = new RateGroup(new [] { account1Rate1, account1Rate2, account1Rate3 });
            rateGroup2 = new RateGroup(new [] { account2Rate1 });
            rateGroup3 = new RateGroup(new [] { account3Rate1, account3Rate2 });

            rateResults = new Dictionary<long, RateGroup>
                {
                    {1, rateGroup1},
                    {2, rateGroup2},
                    {3, rateGroup3},
                };

            genericRepositoryMock = new Mock<ICarrierAccountRepository<UpsAccountEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<UpsAccountEntity> { account1, account2, account3 });
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account2);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<UpsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns((ShipmentEntity s) => rateResults[s.Ups.UpsAccountID])
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(EntityUtility.CloneEntity(e)));

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x => x.Ups.Packages.Add(new UpsPackageEntity()));

            testObject = new UpsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object, new UpsSettingsRepository())
            {
                GetRatesAction = (shipment, type) => genericShipmentTypeMock.Object.GetRates(shipment)
            };

            testShipment = new ShipmentEntity {ShipmentType = (int)ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity(), OriginCountryCode = "US"};
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
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { new UpsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsTrue(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHaSZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsFalse(hasAccounts);
        }

        [Fact]
        public void GetBestRates_RetrievesAllAccounts_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericRepositoryMock.Verify(x => x.DefaultProfileAccount);
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(1));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForEachAccount_Test()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(1));

            foreach (var shipment in getRatesShipments)
            {
                Assert.AreEqual(ShipmentTypeCode.UpsOnLineTools, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.AreEqual(12.1, shipment.ContentWeight);
            }
        }

        //[Fact]
        //public void GetBestRates_ReturnsAllRatesOrdered_WithCheapestFirst()
        //{
        //    var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

        //    Assert.AreEqual(5, rates.Count);
        //    Assert.AreEqual(account1Rate1, OriginalRates(rates).ElementAt(3));
        //    Assert.AreEqual(account1Rate2, OriginalRates(rates).ElementAt(1));
        //    Assert.AreEqual(account1Rate3, OriginalRates(rates).ElementAt(4));
        //    Assert.AreEqual(account3Rate1, OriginalRates(rates).ElementAt(0));
        //    Assert.AreEqual(account3Rate2, OriginalRates(rates).ElementAt(2));
        //}

        //[Fact]
        //public void GetBestRates_ReturnsBestRateForEachAccount_WhenServiceTypeIsFiltered()
        //{
        //    testShipment.BestRate.ServiceLevel = (int) ServiceLevelType.ThreeDays;

        //    var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

        //    Assert.IsTrue(OriginalRates(rates).Contains(account1Rate1));
        //    Assert.IsTrue(OriginalRates(rates).Contains(account1Rate3));
        //    Assert.IsTrue(OriginalRates(rates).Contains(account3Rate2));
        //    Assert.AreEqual(3, rates.Count);
        //}

        //[Fact]
        //public void GetBestRates_ReturnsBestRateForSingleAccount_WhenServiceTypeFilterExcludesAllRatesInSecondAccount()
        //{
        //    testShipment.BestRate.ServiceLevel = (int)ServiceLevelType.OneDay;

        //    var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

        //    Assert.IsTrue(OriginalRates(rates).Contains(account1Rate3));
        //    Assert.AreEqual(1, rates.Count);
        //}

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice_Test()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Contains(result1));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel_Test()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels_Test()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Contains(result1));
            Assert.IsTrue(rates.Rates.Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType_Test()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Contains(result1));
            Assert.IsTrue(rates.Rates.Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_OriginalUpsShipmentDetailsAreRestoredAfterCall_Test()
        {
            UpsShipmentEntity upsShipment = new UpsShipmentEntity();
            testShipment.Ups = upsShipment;
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(upsShipment, testShipment.Ups);
        }

        [Fact]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown_Test()
        {
            ShippingException exception = new ShippingException();
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                                   .Returns((ShipmentEntity s) => rateResults[s.Ups.UpsAccountID])
                                   .Callback((ShipmentEntity s) =>
                                   {
                                       if (s.Ups.UpsAccountID == 2) throw exception;
                                   });

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual(exception, brokerExceptions.First().InnerException);
        }

        [Fact]
        public void GetBestRates_SetsPackageDetails_Test()
        {
            testShipment.BestRate.DimsHeight = 3;
            testShipment.BestRate.DimsWidth = 5;
            testShipment.BestRate.DimsLength = 2;

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(3, shipment.Ups.Packages[0].DimsHeight);
                Assert.AreEqual(5, shipment.Ups.Packages[0].DimsWidth);
                Assert.AreEqual(2, shipment.Ups.Packages[0].DimsLength);
                Assert.IsFalse(shipment.Ups.Packages[0].DimsAddWeight);
                Assert.AreEqual(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileServiceAndPackagingType_Test()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                       {
                                           x.Ups.Service = (int) UpsServiceType.UpsMailInnovationsExpedited;
                                           x.Ups.Packages.Add(new UpsPackageEntity { PackagingType = (int)UpsPackagingType.Tube });
                                       });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(UpsServiceType.UpsGround, (UpsServiceType)shipment.Ups.Service);
                Assert.AreEqual(UpsPackagingType.Custom, (UpsPackagingType)shipment.Ups.Packages[0].PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount_Test()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Ups.UpsAccountID = 999;
                                       x.Ups.Packages.Add(new UpsPackageEntity());
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsFalse(getRatesShipments.Any(x => x.Ups.UpsAccountID == 1));
            Assert.IsTrue(getRatesShipments.Any(x => x.Ups.UpsAccountID == 2));
            Assert.IsFalse(getRatesShipments.Any(x => x.Ups.UpsAccountID == 3));
            Assert.IsFalse(getRatesShipments.Any(x => x.Ups.UpsAccountID == 999));
        }

        [Fact]
        public void GetBestRates_OverridesProfileWeight_Test()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                       {
                                           x.ContentWeight = 123;
                                           x.Ups.Packages.Add(new UpsPackageEntity());
                                       });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsRatesAsRateResults_Test()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                Assert.IsInstanceOfType(rate, typeof(RateResult));
            }
        }

        [Fact]
        public void GetBestRates_SetsTagToBestRateResultTag_Test()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                BestRateResultTag tag = rate.Tag as BestRateResultTag;

                if (tag == null)
                {
                    Assert.Fail("Tag is {0} instead of BestRateResultTag", rate.Tag.GetType());
                }

                Assert.IsInstanceOfType(tag.RateSelectionDelegate, typeof(Action<ShipmentEntity>));
                Assert.IsNotNull(tag.OriginalTag);
                Assert.IsNotNull(tag.ResultKey);
            }
        }

        [Fact]
        public void GetBestRates_SetsTagResultKeyToCarrierAndServiceType_Test()
        {
            rateGroup2.Rates.Clear();

            rateGroup2.Rates.Add(new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays });
            rateGroup2.Rates.Add(new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay });

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();
            
            Assert.IsTrue(resultKeys.Contains("UPSUpsGround"));
            Assert.IsTrue(resultKeys.Contains("UPSUpsNextDayAir"));
        }

        //[Fact]
        //public void GetBestRates_ConvertsShipmentToUps_WhenRateIsSelected()
        //{
        //    rateGroup1.Rates.Clear();
        //    rateGroup3.Rates.Clear();

        //    RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsExpressEarlyAm) { ServiceLevel = ServiceLevelType.FourToSevenDays };

        //    rateGroup1.Rates.Add(result1);

        //    var rates = testObject.GetBestRates(testShipment);
        //    ((Action<ShipmentEntity>) rates[0].Tag)(testShipment);

        //    Assert.AreEqual((int)ShipmentTypeCode.UpsOnLineTools, testShipment.ShipmentType);
        //    Assert.AreEqual((int)UpsServiceType.UpsExpressEarlyAm, testShipment.Ups.Service);
        //    Assert.AreEqual(account1.UpsAccountID, testShipment.Ups.UpsAccountID);
        //}

        //[Fact]
        //public void GetBestRates_DoesNotAlterOtherShipmentTypeData_WhenRateIsSelected()
        //{
        //    rateGroup1.Rates.Clear();
        //    rateGroup3.Rates.Clear();

        //    FedExShipmentEntity fedExEntity = new FedExShipmentEntity();
        //    testShipment.FedEx = fedExEntity;

        //    RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsExpressEarlyAm) { ServiceLevel = ServiceLevelType.FourToSevenDays };

        //    rateGroup1.Rates.Add(result1);

        //    var rates = testObject.GetBestRates(testShipment);
        //    ((Action<ShipmentEntity>)rates[0].Tag)(testShipment);

        //    Assert.AreEqual(fedExEntity, testShipment.FedEx);
        //}

        [Fact]
        public void GetBestRates_AddsUPSToDescription_WhenItDoesNotAlreadyExist_Test()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("UPS Ground", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("UPS Ground"));
            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("UPS Some Service"));
            Assert.AreEqual(2, rates.Rates.Count);
        }
       
        [Fact]
        public void GetBestRates_AddsBrokerException_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays};
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            
            RateGroup rateGroup = new RateGroup(new [] { rate1, rate2, rate3 });
            genericShipmentTypeMock.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(rateGroup);

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual(1, brokerExceptions.Count);
        }

        [Fact]
        public void GetBestRates_BrokerExceptionMessageIndicatesSurePostRatesWereNotReceived_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();
            

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            genericShipmentTypeMock.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(rateGroup);

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual("UPS did not provide SurePost rates.", brokerExceptions[0].Message);
        }

        [Fact]
        public void GetBestRates_BrokerExceptionSeverityLevelIsWarning_WhenSurePostCanBeUsed_AndSurePostRatesAreNotIncluded_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();
            
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            genericShipmentTypeMock.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(rateGroup);

            // Setup the broker settings to indicate we can use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(true);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual(BrokerExceptionSeverityLevel.Warning, brokerExceptions[0].SeverityLevel);
        }

        [Fact]
        public void GetBestRates_DoesNotIncludeBrokerException_WhenSurePostCannotBeUsed_AndSurePostRatesAreNotIncluded_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });
            genericShipmentTypeMock.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(rateGroup);

            // Setup the broker settings to indicate we cannot use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(false);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual(0, brokerExceptions.Count);
        }

        [Fact]
        public void GetBestRates_DoesNotIncludeBrokerException_WhenSurePostCanBeUsed_AndSurePostRatesAreIncluded_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { account1 });

            // Include a rate with a sure post service for this test
            RateResult rate1 = new RateResult("Account 1", "4", 12, UpsServiceType.Ups2DayAir) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult rate2 = new RateResult("Account 1", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult rate3 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult rate4 = new RateResult("Account 1", "1", 15, UpsServiceType.UpsSurePost1LbOrGreater) { ServiceLevel = ServiceLevelType.OneDay };

            RateGroup rateGroup = new RateGroup(new[] { rate1, rate2, rate3, rate4 });
            genericShipmentTypeMock.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(rateGroup);

            // Setup the broker settings to indicate we cannot use SurePost (and we don't have an exception added for MI)
            Mock<IBestRateBrokerSettings> settings = new Mock<IBestRateBrokerSettings>();
            settings.Setup(s => s.IsMailInnovationsAvailable(It.IsAny<ShipmentType>())).Returns(false);
            settings.Setup(s => s.CanUseSurePost()).Returns(false);

            testObject.Configure(settings.Object);
            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.AreEqual(0, brokerExceptions.Count);
        }
        
        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_UpsSettingSpecfiesShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { UpsInsuranceProvider = (int)InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_UpsSettingSpecfiesCarrier_Test()
        {
            Assert.AreEqual(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { UpsInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
