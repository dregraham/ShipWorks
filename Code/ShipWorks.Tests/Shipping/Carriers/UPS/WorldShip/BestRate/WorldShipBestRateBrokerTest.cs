﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
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
       
        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void HasAccounts_DelegatesToAccountRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.Accounts, Times.Once());
        }

        [TestMethod]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity> { new UpsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsTrue(hasAccounts);
        }

        [TestMethod]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsFalse(hasAccounts);
        }

        [TestMethod]
        public void GetBestRates_RetrievesAllAccounts()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericRepositoryMock.Verify(x => x.Accounts);
        }

        [TestMethod]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetBestRates_CallsGetRatesForEachAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(3));

            foreach (var shipment in getRatesShipments)
            {
                Assert.AreEqual(ShipmentTypeCode.UpsWorldShip, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.AreEqual(12.1, shipment.ContentWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result1));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsTwoRates_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result1));
            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result1));
            Assert.IsTrue(OriginalRates(rates.Rates).Contains(result2));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_OriginalUpsShipmentDetailsAreRestoredAfterCall()
        {
            UpsShipmentEntity upsShipment = new UpsShipmentEntity();
            testShipment.Ups = upsShipment;
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(upsShipment, testShipment.Ups);
        }

        [TestMethod]
        public void GetBestRates_ReturnedRatesAreNotAffected_WhenShippingExceptionIsThrown()
        {
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                                   .Returns((ShipmentEntity s) => rateResults[s.Ups.UpsAccountID])
                                   .Callback((ShipmentEntity s) =>
                                       {
                                           if (s.Ups.UpsAccountID == 2) throw new ShippingException();
                                       });

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(5, rates.Rates.Count);
        }

        [TestMethod]
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
            
            Assert.AreEqual(exception, brokerExceptions.First().InnerException);
        }

        [TestMethod]
        public void GetBestRates_SetsPackageDetails()
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

        [TestMethod]
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
                Assert.AreEqual(UpsServiceType.UpsGround, (UpsServiceType)shipment.Ups.Service);
                Assert.AreEqual(UpsPackagingType.Custom, (UpsPackagingType)shipment.Ups.Packages[0].PackagingType);
            }
        }

        [TestMethod]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Ups.UpsAccountID = 999;
                                       x.Ups.Packages.Add(new UpsPackageEntity());
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(getRatesShipments.Any(x => x.Ups.UpsAccountID == 1));
            Assert.IsTrue(getRatesShipments.Any(x => x.Ups.UpsAccountID == 2));
            Assert.IsTrue(getRatesShipments.Any(x => x.Ups.UpsAccountID == 3));
            Assert.IsFalse(getRatesShipments.Any(x => x.Ups.UpsAccountID == 999));
        }

        [TestMethod]
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
                Assert.AreEqual(12.1, shipment.Ups.Packages[0].Weight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsRatesAsNonCompetitiveRateResults()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            foreach (var rate in rates.Rates)
            {
                Assert.IsInstanceOfType(rate, typeof(NoncompetitiveRateResult));
            }
        }

        [TestMethod]
        public void GetBestRates_SetsTagToBestRateResultTag()
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

        [TestMethod]
        public void GetBestRates_SetsTagResultKeyToCarrierAndServiceType()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            rateGroup1.Rates.Add(new RateResult("Account 1b", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.FourToSevenDays });
            rateGroup1.Rates.Add(new RateResult("Account 1c", "1", 15, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay });

            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.IsTrue(resultKeys.Contains("UPSUpsGround"));
            Assert.IsTrue(resultKeys.Contains("UPSUpsNextDayAir"));
        }

        [TestMethod]
        public void GetBestRates_AddsUPSToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("UPS Ground", "4", 4, UpsServiceType.UpsNextDayAir) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, UpsServiceType.UpsGround) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(OriginalRates(rates.Rates).Select(x => x.Description).Contains("UPS Ground"));
            Assert.IsTrue(OriginalRates(rates.Rates).Select(x => x.Description).Contains("UPS Some Service"));
            Assert.AreEqual(2, rates.Rates.Count);
        }
       
        /// <summary>
        /// Gets a list of original rates from a list of NonCompetitiveRateResults
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        private static IEnumerable<RateResult> OriginalRates(IEnumerable<RateResult> rates)
        {
            return rates.OfType<NoncompetitiveRateResult>().Select(x => x.OriginalRate);
        }
    }
}
