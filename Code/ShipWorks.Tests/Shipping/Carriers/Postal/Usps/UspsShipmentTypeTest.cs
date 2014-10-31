﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    [TestClass]
    public class UspsShipmentTypeTest
    {
        private UspsShipmentType testObject;
        private Mock<ICarrierAccountRepository<StampsAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<StampsAccountEntity>>();

            testObject = new UspsShipmentType();
        }

        [TestMethod]
        public void ShouldRetrieveExpress1Rates_ReturnsFalse_Test()
        {
            // Never get Express1 rates for stamps expedited
            Assert.IsFalse(testObject.ShouldRetrieveExpress1Rates);
        }

        [TestMethod]
        public void GetShippingBroker_ReturnsUspsCounterRateBroker_WhenNoUspsAccountsExist_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity>());
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsInstanceOfType(broker, typeof(UspsCounterRatesBroker));
        }

        [TestMethod]
        public void GetShippingBroker_ReturnsUspsRateBroker_WhenUspsAccountExists_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity> { new StampsAccountEntity() });
            testObject.AccountRepository = accountRepository.Object;

            IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity());

            Assert.IsInstanceOfType(broker, typeof(UspsBestRateBroker));
        }
    }
}
