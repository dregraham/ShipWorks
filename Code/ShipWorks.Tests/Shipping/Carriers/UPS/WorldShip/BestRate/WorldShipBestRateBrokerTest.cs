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
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.WorldShip.BestRate
{
    public class WorldShipBestRateBrokerTest
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
        private WorldShipBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the UpsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments; 

        private Mock<ICarrierAccountRepository<UpsAccountEntity>> genericRepositoryMock;
        private Mock<WorldShipShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;

        private int expectedNumberOfAccountsReturned = 1;
       
        public WorldShipBestRateBrokerTest()
        {
            account1 = new UpsAccountEntity { UpsAccountID = 1, CountryCode = "US" };
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
            genericShipmentTypeMock = new Mock<WorldShipShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsWorldShip);
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns((ShipmentEntity s) => rateResults[s.Ups.UpsAccountID])
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(EntityUtility.CloneEntity(e)));

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x => x.Ups.Packages.Add(new UpsPackageEntity()));

            testObject = new WorldShipBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) => genericShipmentTypeMock.Object.GetRates(shipment)
            };


            testShipment = new ShipmentEntity { ShipmentType = (int)ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity(), OriginCountryCode = "US" };
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

            Assert.True(hasAccounts);
        }

        [Fact]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.False(hasAccounts);
        }

        [Fact]
        public void GetBestRates_RetrievesAllAccounts()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericRepositoryMock.Verify(x => x.DefaultProfileAccount);
        }

        [Fact]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(expectedNumberOfAccountsReturned));
        }

        [Fact]
        public void GetBestRates_CallsGetRatesForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(expectedNumberOfAccountsReturned));

            foreach (var shipment in getRatesShipments)
            {
                Assert.Equal(ShipmentTypeCode.UpsWorldShip, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.Equal(12.1, shipment.ContentWeight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result2));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
            Assert.True(rates.Rates.Contains(result2));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup2.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Contains(result1));
            Assert.True(rates.Rates.Contains(result2));
            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_OriginalUpsShipmentDetailsAreRestoredAfterCall()
        {
            UpsShipmentEntity upsShipment = new UpsShipmentEntity();
            testShipment.Ups = upsShipment;
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(upsShipment, testShipment.Ups);
        }
        public void GetBestRates_NoRatesAreReturned_WhenShippingExceptionIsThrown_Test()
        {
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>())).Throws<ShippingException>();

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.Equal(0, rates.Rates.Count);
        }

        [Fact]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown()
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
            
            Assert.Equal(exception, brokerExceptions.First().InnerException);
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
                Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);
                Assert.Equal(5, shipment.Ups.Packages[0].DimsWidth);
                Assert.Equal(2, shipment.Ups.Packages[0].DimsLength);
                Assert.False(shipment.Ups.Packages[0].DimsAddWeight);
                Assert.Equal(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileServiceAndPackagingType()
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
                Assert.Equal(UpsServiceType.UpsGround, (UpsServiceType)shipment.Ups.Service);
                Assert.Equal(UpsPackagingType.Custom, (UpsPackagingType)shipment.Ups.Packages[0].PackagingType);
            }
        }

        [Fact]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Ups.UpsAccountID = 999;
                                       x.Ups.Packages.Add(new UpsPackageEntity());
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.False(getRatesShipments.Any(x => x.Ups.UpsAccountID == 1));
            Assert.True(getRatesShipments.Any(x => x.Ups.UpsAccountID == 2));
            Assert.False(getRatesShipments.Any(x => x.Ups.UpsAccountID == 3));
            Assert.False(getRatesShipments.Any(x => x.Ups.UpsAccountID == 999));
        }

        [Fact]
        public void GetBestRates_OverridesProfileWeight()
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
                Assert.Equal(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [Fact]
        public void GetBestRates_ReturnsRatesAsNonCompetitiveRateResults()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                Assert.IsAssignableFrom<RateResult>(rate);
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
        public void GetBestRates_SetsTagResultKeyToCarrierAndServiceType()
        {
            rateGroup2.Rates.Clear();

            rateGroup2.Rates.Add(new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays });
            rateGroup2.Rates.Add(new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay });

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.True(resultKeys.Contains("UPSUpsGround"));
            Assert.True(resultKeys.Contains("UPSUpsNextDayAir"));
        }

        [Fact]
        public void GetBestRates_AddsUPSToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup1.Rates.Clear();
            rateGroup2.Rates.Clear();

            RateResult result1 = new RateResult("UPS Ground", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup2.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.True(rates.Rates.Select(x => x.Description).Contains("UPS Some Service"));
            Assert.Equal(1, rates.Rates.Count);
        }
       
    }
}
