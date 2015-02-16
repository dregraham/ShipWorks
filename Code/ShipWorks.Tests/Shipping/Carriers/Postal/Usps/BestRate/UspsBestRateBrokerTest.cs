using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.BestRate
{
    [TestClass]
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
        /// This holds a collection of all the shipment objects that were passed into the StampsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<UspsAccountEntity>> genericRepositoryMock;
        private Mock<UspsShipmentType> genericShipmentTypeMock;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            account = new UspsAccountEntity { UspsAccountID = 1, CountryCode = "US"};

            rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            
            rateGroup = new RateGroup(new[] { rate1, rate2, rate3 });

            genericRepositoryMock = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account);

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<UspsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Stamps);
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns(rateGroup)
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(EntityUtility.CloneEntity(e)));

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity { Usps = new UspsShipmentEntity() });

            testObject = new UspsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object)
            {
                GetRatesAction = (shipment, type) => genericShipmentTypeMock.Object.GetRates(shipment)
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

        [TestMethod]
        public void HasAccounts_DelegatesToAccountRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;

            genericRepositoryMock.Verify(r => r.Accounts, Times.Once());
        }

        [TestMethod]
        public void HasAccounts_ReturnsTrue_WhenRepositoryHasMoreThanZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity> { new UspsAccountEntity() });

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsTrue(hasAccounts);
        }

        [TestMethod]
        public void HasAccounts_ReturnsFalse_WhenRepositoryHasZeroAccounts_Test()
        {
            genericRepositoryMock.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            bool hasAccounts = testObject.HasAccounts;

            Assert.IsFalse(hasAccounts);
        }

        [TestMethod]
        public void GetBestRates_CallsConfigureNewShipmentForProfileAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetBestRates_CallsGetRatesForProfileAccount()
        {
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(1));

            foreach (var shipment in getRatesShipments)
            {
                Assert.AreEqual(ShipmentTypeCode.Stamps, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.AreEqual(12.1, shipment.ContentWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsAllRates_WithNoFilter()
        {
            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(3, rates.Rates.Count);
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == rate1.RateID));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == rate2.RateID));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == rate3.RateID));
        }

        [TestMethod]
        public void GetBestRates_ReturnsTwoRate_WhenTwoRatesHaveSameTypeLevelAndPrice()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsTwoRate_WhenTwoRatesHaveSameTypeLevel()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            RateResult result2 = new RateResult("Account 1b", "3", 2, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameTypeAndPriceButDifferentLevels()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBothRates_WhenTwoRatesHaveSameLevelAndPriceButDifferentType()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result1.RateID));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_DoesNotReturnNonSelectableRates()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Account 1a", "4") { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsFalse(rates.Rates.Any(r=>r.RateID == result1.RateID));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.AreEqual(1, rates.Rates.Count);
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\GetBestRates_DoesNotIncludeTypes.csv", "Usps_GetBestRates_DoesNotIncludeTypes#csv", DataAccessMethod.Sequential)]
        [DeploymentItem(@"Shipping\Carriers\Postal\Usps\BestRate\Usps_GetBestRates_DoesNotIncludeTypes.csv")]
        [TestMethod]
        public void GetBestRates_ExcludesVariousTypes()
        {
            PostalServiceType excludedServiceType = (PostalServiceType)Enum.Parse(typeof (PostalServiceType), TestContext.DataRow[0].ToString());

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

            Assert.IsFalse(rates.Rates.Any(r => r.RateID == result1.RateID), "Returned rates should not include {0}", EnumHelper.GetDescription(excludedServiceType));
            Assert.IsTrue(rates.Rates.Any(r => r.RateID == result2.RateID));
            Assert.AreEqual(1, rates.Rates.Count);
        }

        [TestMethod]
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

            Assert.AreEqual("USPS Foo Bar", bestRates.Rates.Single(r => r.RateID == result2.RateID).Description);
            Assert.AreEqual("USPS Baz Other", bestRates.Rates.Single(r => r.RateID == result4.RateID).Description);
            Assert.AreEqual("4", bestRates.Rates.Single(r => r.RateID == result2.RateID).Days);
            Assert.AreEqual("3", bestRates.Rates.Single(r => r.RateID == result4.RateID).Days);
        }

        [TestMethod]
        public void GetBestRates_OriginalStampsShipmentDetailsAreRestoredAfterCall()
        {
            PostalShipmentEntity stampsShipment = new PostalShipmentEntity();
            testShipment.Postal = stampsShipment;
            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(stampsShipment, testShipment.Postal);
        }

        [TestMethod]
        public void GetBestRates_NoRatesAreReturned_WhenShippingExceptionIsThrown()
        {
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>())).Throws<ShippingException>();

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(0, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_CallsHandler_WhenShippingExceptionIsThrown()
        {
            ShippingException exception = new ShippingException();
            List<BrokerException> brokerExceptions = new List<BrokerException>();

            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>())).Throws(exception);

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.IsTrue(brokerExceptions.Any(e=>e.InnerException.Equals(exception)));
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
                Assert.AreEqual(3, shipment.Postal.DimsHeight);
                Assert.AreEqual(5, shipment.Postal.DimsWidth);
                Assert.AreEqual(2, shipment.Postal.DimsLength);
                Assert.AreEqual(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
                Assert.AreEqual(3.4, shipment.Postal.DimsWeight);
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

            testObject.GetBestRates(testShipment, new List<BrokerException>());

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
                                       x.Postal.Usps.UspsAccountID = 999;
                                   });

            testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(getRatesShipments.Any(x => x.Postal.Usps.UspsAccountID == 1));
            Assert.IsFalse(getRatesShipments.Any(x => x.Postal.Usps.UspsAccountID == 999));
        }

        [TestMethod]
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
                Assert.AreEqual(3.4, shipment.Postal.DimsWeight);
            }
        }

        [TestMethod]
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
                Assert.AreEqual(testShipment.BestRate.DimsAddWeight, shipment.Postal.DimsAddWeight);
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
        public void GetBestRates_SetsTagResultKeyToPostalAndServiceType()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("Stamps Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            var resultKeys = rates.Rates.Select(x => x.Tag).Cast<BestRateResultTag>().Select(x => x.ResultKey).ToList();

            Assert.IsTrue(resultKeys.Contains("PostalExpress Mail (Premium)"));
            Assert.IsTrue(resultKeys.Contains("PostalStandard Post"));
        }

       [TestMethod]
        public void GetBestRates_AddsStampsToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup.Rates.Clear();

            RateResult result1 = new RateResult("USPS Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup.Rates.Add(result1);
            rateGroup.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("USPS Ground"));
            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("USPS Some Service"));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity()));
        }

        [TestMethod]
        public void Configure_ShouldNotCallCheckExpress1RatesOnSettings_WithShipmentType()
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();

            testObject.Configure(brokerSettings.Object);

            brokerSettings.Verify(x => x.CheckExpress1Rates(testObject.ShipmentType), Times.Never);
        }

        [TestMethod]
        public void Configure_SetsRetrieveExpress1RatesToFalse_WhenConfigurationIsTrue()
        {
            Configure_ShouldRetrieveExpress1RatesTest(true);
        }

        [TestMethod]
        public void Configure_SetsRetrieveExpress1RatesToFalse_WhenConfigurationIsFalse()
        {
            Configure_ShouldRetrieveExpress1RatesTest(false);
        }

        private void Configure_ShouldRetrieveExpress1RatesTest(bool checkExpress1)
        {
            var brokerSettings = new Mock<IBestRateBrokerSettings>();
            brokerSettings.Setup(x => x.CheckExpress1Rates(It.IsAny<ShipmentType>())).Returns(checkExpress1);

            testObject.Configure(brokerSettings.Object);

            // Best rate should never retrieve Express1 rates
            Assert.AreEqual(false, ((UspsShipmentType)testObject.ShipmentType).ShouldRetrieveExpress1Rates);
        }

        [TestMethod]
        public void GetBestRates_ReturnsNoRates_WhenShipmentTotalWeightTooHeavy()
        {
            testShipment.TotalWeight = 70.1;
            RateGroup rates = testObject.GetBestRates(testShipment, new List<BrokerException>());

            Assert.AreEqual(0, rates.Rates.Count);
        }
    }
}
