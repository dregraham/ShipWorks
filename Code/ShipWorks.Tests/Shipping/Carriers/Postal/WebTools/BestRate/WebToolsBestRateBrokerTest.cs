using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.WebTools.BestRate
{
    [TestClass]
    public class WebToolsBestRateBrokerTest
    {
        private NullEntity account1;
        private RateGroup rateGroup1;
        private RateResult account1Rate1;
        private RateResult account1Rate2;
        private RateResult account1Rate3;
        private WebToolsBestRateBroker testObject;
        private ShipmentEntity testShipment;

        /// <summary>
        /// This holds a collection of all the shipment objects that were passed into the StampsShipmentType.GetRates method
        /// </summary>
        private List<ShipmentEntity> getRatesShipments;

        private Mock<ICarrierAccountRepository<NullEntity>> genericRepositoryMock;
        private Mock<PostalWebShipmentType> genericShipmentTypeMock;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            account1 = new NullEntity(); { };

            account1Rate1 = new RateResult("Account 1a", "4", 12, new PostalRateSelection(PostalServiceType.PriorityMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.TwoDays };
            account1Rate2 = new RateResult("Account 1b", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.FourToSevenDays };
            account1Rate3 = new RateResult("Account 1c", "1", 15, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            
            rateGroup1 = new RateGroup(new[] { account1Rate1, account1Rate2, account1Rate3 });

            genericRepositoryMock = new Mock<ICarrierAccountRepository<NullEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<NullEntity> { account1 });

            getRatesShipments = new List<ShipmentEntity>();

            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            genericShipmentTypeMock = new Mock<PostalWebShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.PostalWebTools);
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns(rateGroup1)
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(EntityUtility.CloneEntity(e)));

            // Mimic the bare minimum of what the configure method is doing
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                            .Callback<ShipmentEntity>(x => x.Postal = new PostalShipmentEntity());

            testObject = new WebToolsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object);
            testObject.GetRatesAction = (shipment, type) =>
                {
                    getRatesShipments.Add(EntityUtility.CloneEntity(shipment));
                    return rateGroup1;
                };

            testShipment = new ShipmentEntity { ShipmentType = (int)ShipmentTypeCode.BestRate, ContentWeight = 12.1, BestRate = new BestRateShipmentEntity() };
        }

        [TestMethod]
        public void GetRates_ThrowsException_WhenAccountAddressIsSelected()
        {
            testShipment.OriginOriginID = (int) ShipmentOriginSource.Account;

            BrokerException calledException = null;
            testObject.GetBestRates(testShipment, (ex) => calledException = ex);

            Assert.IsNotNull(calledException);
        }

        [TestMethod]
        public void GetRates_DoesNotThrow_WhenAccountAddressIsNotSelected()
        {
            testShipment.OriginOriginID = (int)ShipmentOriginSource.Store;

            BrokerException calledException = null;
            testObject.GetBestRates(testShipment, (ex) => calledException = ex);

            Assert.IsNull(calledException);
        }

        [TestMethod]
        public void GetInsuranceProvider_ReturnsShipWorks_Test()
        {
            Assert.AreEqual(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity()));
        }

        [TestMethod]
        public void GetBestRates_AddsPostalToDescription_WhenItDoesNotAlreadyExist()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("USPS (w/o Postage) Ground", "4", 4, new PostalRateSelection(PostalServiceType.ExpressMailPremium, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("USPS (w/o Postage) Ground"));
            Assert.IsTrue(rates.Rates.Select(x => x.Description).Contains("USPS (w/o Postage) Some Service"));
            Assert.AreEqual(2, rates.Rates.Count);
        }

        [TestMethod]
        public void GetBestRates_DoesNotReturnMediaMail()
        {
            rateGroup1.Rates.Clear();

            RateResult result1 = new RateResult("Ground", "4", 4, new PostalRateSelection(PostalServiceType.MediaMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result2 = new RateResult("Ground", "4", 4, new PostalRateSelection(PostalServiceType.BoundPrintedMatter, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result3 = new RateResult("Ground", "4", 4, new PostalRateSelection(PostalServiceType.LibraryMail, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };
            RateResult result4 = new RateResult("Some Service", "3", 4, new PostalRateSelection(PostalServiceType.StandardPost, PostalConfirmationType.None)) { ServiceLevel = ServiceLevelType.OneDay };

            rateGroup1.Rates.Add(result1);
            rateGroup1.Rates.Add(result2);
            rateGroup1.Rates.Add(result3);
            rateGroup1.Rates.Add(result4);

            var rates = testObject.GetBestRates(testShipment, ex => { });

            Assert.IsTrue(rates.Rates.Contains(result4));
            Assert.AreEqual(1, rates.Rates.Count);
        }
    }
}
