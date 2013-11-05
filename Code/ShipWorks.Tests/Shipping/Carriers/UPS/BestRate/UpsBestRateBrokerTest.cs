using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.BestRate
{
    [TestClass]
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

        private List<ShipmentEntity> getRatesShipments; 

        private Mock<ICarrierAccountRepository<UpsAccountEntity>> genericRepositoryMock;
        private Mock<UpsShipmentType> genericShipmentTypeMock;
        private Dictionary<long, RateGroup> rateResults;
       

        [TestInitialize]
        public void Initialize()
        {
            account1 = new UpsAccountEntity { UpsAccountID = 1 };
            account2 = new UpsAccountEntity { UpsAccountID = 2 };
            account3 = new UpsAccountEntity { UpsAccountID = 3 };

            account1Rate1 = new RateResult("Account 1a", "4", 12, null);
            account1Rate2 = new RateResult("Account 1b", "3", 4, null);
            account1Rate3 = new RateResult("Account 1c", "1", 15, null);
            account2Rate1 = new RateResult("* No rates were returned for the selected Service.", "");
            account3Rate1 = new RateResult("Account 3a", "4", 3, null);
            account3Rate2 = new RateResult("Account 3b", "3", 10, null);

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

            genericShipmentTypeMock = new Mock<UpsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.GetRates(It.IsAny<ShipmentEntity>()))
                            .Returns((ShipmentEntity s) => rateResults[s.Ups.UpsAccountID])
                            .Callback<ShipmentEntity>(e => getRatesShipments.Add(e));

            testObject = new UpsBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object);

            testShipment = new ShipmentEntity {ShipmentType = (int)ShipmentTypeCode.BestRate, ContentWeight = 12.1 };
        }

        [TestMethod]
        public void GetBestRates_RetrievesAllAccounts()
        {
            testObject.GetBestRates(new ShipmentEntity());

            genericRepositoryMock.Verify(x => x.Accounts);
        }

        [TestMethod]
        public void GetBestRates_CallsConfigureNewShipmentForEachAccount()
        {
            testObject.GetBestRates(testShipment);

            genericShipmentTypeMock.Verify(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()), Times.Exactly(3));
        }

        [TestMethod]
        public void GetBestRates_CallsGetRatesForEachAccount()
        {
            testObject.GetBestRates(testShipment);

            genericShipmentTypeMock.Verify(x => x.GetRates(It.IsAny<ShipmentEntity>()), Times.Exactly(3));

            foreach (var shipment in getRatesShipments)
            {
                Assert.AreEqual(ShipmentTypeCode.UpsOnLineTools, (ShipmentTypeCode)shipment.ShipmentType);
                Assert.AreEqual(12.1, shipment.ContentWeight);
            }
        }

        [TestMethod]
        public void GetBestRates_ReturnsTwoRates_SinceSecondAccountHasNoRates()
        {
            var rates = testObject.GetBestRates(testShipment);

            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_ReturnsBestRateForEachAccount()
        {
            var rates = testObject.GetBestRates(testShipment);

            Assert.IsTrue(rates.Contains(account1Rate2));
            Assert.IsTrue(rates.Contains(account3Rate1));
        }

        [TestMethod]
        public void GetBestRates_OriginalUpsShipmentDetailsAreRestoredAfterCall()
        {
            UpsShipmentEntity upsShipment = new UpsShipmentEntity();
            testShipment = new ShipmentEntity { Ups = upsShipment };
            var rates = testObject.GetBestRates(testShipment);

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

            var rates = testObject.GetBestRates(testShipment);

            Assert.AreEqual(2, rates.Count);
        }

        [TestMethod]
        public void GetBestRates_SetsHoverText()
        {
            var rates = testObject.GetBestRates(testShipment);

            Assert.AreEqual("UPS - Account 1b", account1Rate2.HoverText);
            Assert.AreEqual("UPS - Account 3a", account3Rate1.HoverText);
        }
    }
}
