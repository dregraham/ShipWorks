﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.BestRate
{
    [TestClass]
    public class StampsBestRateBrokerTest
    {
        private StampsAccountEntity account1;
        private StampsAccountEntity account2;
        private StampsAccountEntity account3;
        private RateGroup rateGroup1;
        private RateGroup rateGroup2;
        private RateGroup rateGroup3;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private RateResult account2Rate1;
        private RateResult account3Rate1;
        private RateResult account3Rate2;
        private StampsBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the StampsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<StampsAccountEntity>> genericRepositoryMock;
        private Mock<StampsShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            account1 = new StampsAccountEntity { StampsAccountID = 1 };
            account2 = new StampsAccountEntity { StampsAccountID = 2 };
            account3 = new StampsAccountEntity { StampsAccountID = 3 };

            account1Rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            account1Rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, new PostalRateSelection(PostalServiceType.ParcelSelect, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.Anytime };
            account3Rate2 = new RateResult("Account 3b", "3", 10, new PostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };

            rateGroup1 = new RateGroup(new[] { account1Rate1, account1Rate2, account1Rate3 });
            rateGroup2 = new RateGroup(new[] { account2Rate1 });
            rateGroup3 = new RateGroup(new[] { account3Rate1, account3Rate2 });

            rateResults = new Dictionary<long, RateGroup>
                {
                    {1, rateGroup1},
                    {2, rateGroup2},
                    {3, rateGroup3},
                };

            genericRepositoryMock = new Mock<ICarrierAccountRepository<StampsAccountEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<StampsAccountEntity> { account1, account2, account3 });

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<StampsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Stamps);
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns((ShipmentEntity s) => rateResults[s.Postal.Stamps.StampsAccountID])
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(EntityUtility.CloneEntity(e)));

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity { Stamps = new StampsShipmentEntity() });

            testObject = new StampsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = shipment => genericShipmentTypeMock.Object.GetRates(shipment)
            };

            testShipment = new ShipmentEntity { ShipmentType = (int)ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity() };
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
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity> { new StampsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsTrue(hasAccounts);
        }

        [TestMethod]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsFalse(hasAccounts);
        }

        [TestMethod]
        public void GetBestRates_RetrievesAllAccounts()
        {
            testObject.GetBestRates(testShipment, ex => { });

            genericRepositoryMock.Verify(x => x.Accounts);
        }

        [TestMethod]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount()
        {
            testObject.GetBestRates(testShipment, ex => { });

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetBestRates_CallsGetRatesForEachAccount()
        {
            testObject.GetBestRates(testShipment, ex => { });

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(3));

            foreach (var shipment in getRatesShipments)
            {
                Assert.AreEqual(ShipmentTypeCode.Stamps, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.AreEqual(12.1, shipment.ContentWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsAllRates_WithNoFilter()
        {
            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.AreEqual(5, rates.Count);
            Assert.IsTrue(rates.Contains(account1Rate1));
            Assert.IsTrue(rates.Contains(account1Rate2));
            Assert.IsTrue(rates.Contains(account1Rate3));
            Assert.IsTrue(rates.Contains(account3Rate1));
            Assert.IsTrue(rates.Contains(account3Rate2));
        }

        [TestMethod]
        public void GetBestRates_ReturnsFirstRate_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Contains(result1));
            Assert.AreEqual(1, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsCheapestRate_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Contains(result2));
            Assert.AreEqual(1, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Contains(result1));
            Assert.IsTrue(rates.Contains(result2));
            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Contains(result1));
            Assert.IsTrue(rates.Contains(result2));
            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_DoesNotReturnNonSelectableRates()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsFalse(rates.Contains(result1));
            Assert.IsTrue(rates.Contains(result2));
            Assert.AreEqual(1, rates.Count);
        }

        //[TestMethod]
        //public void GetBestRates_DoesNotIncludeMediaMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.MediaMail);
        //}

        //[TestMethod]
        //public void GetBestRates_DoesNotIncludeLibrayMail()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.LibraryMail);
        //}

        //[TestMethod]
        //public void GetBestRates_DoesNotIncludeBPM()
        //{
        //    TestServiceTypeIsExcluded(PostalServiceType.BoundPrintedMatter);
        //}

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\GetBestRates_DoesNotIncludeTypes.csv", "Stamps_GetBestRates_DoesNotIncludeTypes#csv", DataAccessMethod.Sequential)]
        [DeploymentItem(@"Shipping\Carriers\Postal\Stamps\BestRate\Stamps_GetBestRates_DoesNotIncludeTypes.csv")]
        [TestMethod]
        public void GetBestRates_ExcludesVariousTypes()
        {
            PostalServiceType excludedServiceType = (PostalServiceType)Enum.Parse(typeof (PostalServiceType), TestContext.DataRow[0].ToString());

            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(excludedServiceType, PostalConfirmationType.None))
                {
                    ServiceLevel = ServiceLevelType.OneDay
                };

            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None))
                {
                    ServiceLevel = ServiceLevelType.OneDay
                };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsFalse(rates.Contains(result1), "Returned rates should not include {0}", EnumHelper.GetDescription(excludedServiceType));
            Assert.IsTrue(rates.Contains(result2));
            Assert.AreEqual(1, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_UpdatesDescriptionWhenPartOfRateGroup()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Foo", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("       Bar", string.Empty, 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            RateResult result3 = new RateResult("Baz", "3") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result4 = new RateResult("   Other", string.Empty, 4, new PostalRateSelection(PostalServiceType.ExpressMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);
            rateGroup3.Rates.Add(result3);
            rateGroup3.Rates.Add(result4);

            testObject.GetBestRates(testShipment, ex => { });

            Assert.AreEqual("Stamps Foo Bar", result2.Description);
            Assert.AreEqual("Stamps Baz Other", result4.Description);
            Assert.AreEqual("4", result2.Days);
            Assert.AreEqual("3", result4.Days);
        }

        [TestMethod]
        public void GetBestRates_OriginalStampsShipmentDetailsAreRestoredAfterCall()
        {
            PostalShipmentEntity StampsShipment = new PostalShipmentEntity();
            testShipment.Postal = StampsShipment;
            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.AreEqual(StampsShipment, testShipment.Postal);
        }

        [TestMethod]
        public void GetBestRates_ReturnedRatesAreNotAffected_WhenShippingExceptionIsThrown()
        {
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                                   .Returns((ShipmentEntity s) => rateResults[s.Postal.Stamps.StampsAccountID])
                                   .Callback((ShipmentEntity s) =>
                                   {
                                       if (s.Postal.Stamps.StampsAccountID == 2) throw new ShippingException();
                                   });

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.AreEqual(5, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown()
        {
            ShippingException exception = new ShippingException();
            ShippingException calledException = null;

            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                                   .Returns((ShipmentEntity s) => rateResults[s.Postal.Stamps.StampsAccountID])
                                   .Callback((ShipmentEntity s) =>
                                   {
                                       if (s.Postal.Stamps.StampsAccountID == 2) throw exception;
                                   });

            testObject.GetBestRates(testShipment, ex => calledException = ex);

            Assert.AreEqual(exception, calledException.InnerException);
        }

        [TestMethod]
        public void GetBestRates_SetsPackageDetails()
        {
            testShipment.BestRate.DimsHeight = 3;
            testShipment.BestRate.DimsWidth = 5;
            testShipment.BestRate.DimsLength = 2;

            testObject.GetBestRates(testShipment, ex => { });

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(3, shipment.Postal.DimsHeight);
                Assert.AreEqual(5, shipment.Postal.DimsWidth);
                Assert.AreEqual(2, shipment.Postal.DimsLength);
                Assert.IsFalse(shipment.Postal.DimsAddWeight);
                Assert.AreEqual(12.1, shipment.Postal.DimsWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_OverridesProfileServiceAndPackagingType()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Service = (int)PostalServiceType.ConsolidatorDomestic;
                                       x.Postal.PackagingType = (int) PostalPackagingType.FlatRateSmallBox;
                                   });

            testObject.GetBestRates(testShipment, ex => { });

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(PostalServiceType.PriorityMail, (PostalServiceType)shipment.Postal.Service);
                Assert.AreEqual(PostalPackagingType.Package, (PostalPackagingType)shipment.Postal.PackagingType);
            }
        }

        [TestMethod]
        public void GetBestRates_OverridesProfileAccount()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.Postal.Stamps.StampsAccountID = 999;
                                   });

            testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(getRatesShipments.Any(x => x.Postal.Stamps.StampsAccountID == 1));
            Assert.IsTrue(getRatesShipments.Any(x => x.Postal.Stamps.StampsAccountID == 2));
            Assert.IsTrue(getRatesShipments.Any(x => x.Postal.Stamps.StampsAccountID == 3));
            Assert.IsFalse(getRatesShipments.Any(x => x.Postal.Stamps.StampsAccountID == 999));
        }

        [TestMethod]
        public void GetBestRates_OverridesProfileWeight()
        {
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                                   .Callback<ShipmentEntity>(x =>
                                   {
                                       x.ContentWeight = 123;
                                   });

            testObject.GetBestRates(testShipment, ex => { });

            foreach (ShipmentEntity shipment in getRatesShipments)
            {
                Assert.AreEqual(12.1, shipment.Postal.DimsWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsRatesAsRegularRateResults()
        {
            var rates = testObject.GetBestRates(testShipment, ex => { });

            foreach (var rate in rates)
            {
                Assert.IsNotInstanceOfType(rate, typeof(NoncompetitiveRateResult));
            }
        }

        [TestMethod]
        public void GetBestRates_SetsTagToAction()
        {
            var rates = testObject.GetBestRates(testShipment, ex => { });

            foreach (var rate in rates)
            {
                Assert.IsInstanceOfType(rate.Tag, typeof(Action<ShipmentEntity>));
            }
        }

        //[TestMethod]
        //public void GetBestRates_ConvertsShipmentToStamps_WhenRateIsSelected()
        //{
        //    rateGroup1.Rates.Clear();
        //    rateGroup3.Rates.Clear();

        //    RateResult result1 = new RateResult("Account 1a", "4", 4, PostalServiceType.StampsExpressEarlyAm) { ServiceLevel = ServiceLevelType.FourToSevenDays };

        //    rateGroup1.Rates.Add(result1);

        //    var rates = testObject.GetBestRates(testShipment);
        //    ((Action<ShipmentEntity>) rates[0].Tag)(testShipment);

        //    Assert.AreEqual((int)ShipmentTypeCode.Stamps, testShipment.ShipmentType);
        //    Assert.AreEqual((int)PostalServiceType.StampsExpressEarlyAm, testShipment.Stamps.Service);
        //    Assert.AreEqual(account1.StampsAccountID, testShipment.Stamps.StampsAccountID);
        //}

        //[TestMethod]
        //public void GetBestRates_DoesNotAlterOtherShipmentTypeData_WhenRateIsSelected()
        //{
        //    rateGroup1.Rates.Clear();
        //    rateGroup3.Rates.Clear();

        //    FedExShipmentEntity fedExEntity = new FedExShipmentEntity();
        //    testShipment.FedEx = fedExEntity;

        //    RateResult result1 = new RateResult("Account 1a", "4", 4, PostalServiceType.StampsExpressEarlyAm) { ServiceLevel = ServiceLevelType.FourToSevenDays };

        //    rateGroup1.Rates.Add(result1);

        //    var rates = testObject.GetBestRates(testShipment);
        //    ((Action<ShipmentEntity>)rates[0].Tag)(testShipment);

        //    Assert.AreEqual(fedExEntity, testShipment.FedEx);
        //}

        [TestMethod]
        public void GetBestRates_AddsStampsToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup1.Rates.Clear();
            rateGroup3.Rates.Clear();

            RateResult result1 = new RateResult("Stamps Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup3.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Select(x => x.Description).Contains("Stamps Ground"));
            Assert.IsTrue(rates.Select(x => x.Description).Contains("Stamps Some Service"));
            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity()));
        }
    }
}
