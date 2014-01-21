using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Tests.Shipping.Carriers
{
    [TestClass]
    public class TangoCounterRatesCredentialStoreTests
    {
        private Mock<ICounterRatesCredentialStore> testObject = new Mock<ICounterRatesCredentialStore>();

        [TestInitialize]
        public void Initialize()
        {
            testObject.Setup(t => t.FedExMeterNumber).Returns("FedEx Meter Number");
            testObject.Setup(t => t.FedExAccountNumber).Returns("FedEx Account Number");
            testObject.Setup(t => t.FedExPassword).Returns("FedEx Password");
            testObject.Setup(t => t.FedExUsername).Returns("FedEx Username");
        }

        [TestMethod]
        public void TangoCounterRatesCredentialStore_ReturnsInstance_Test()
        {
            Assert.IsNotNull(TangoCounterRatesCredentialStore.Instance);
        }

        [TestMethod]
        public void TangoCounterRatesCredentialStore_ReturnsFedExValues_Test()
        {
            Assert.IsNotNull(testObject.Object.FedExAccountNumber);
            Assert.IsNotNull(testObject.Object.FedExMeterNumber);
            Assert.IsNotNull(testObject.Object.FedExPassword);
            Assert.IsNotNull(testObject.Object.FedExUsername);

            Assert.AreEqual(testObject.Object.FedExMeterNumber, "FedEx Meter Number");
            Assert.AreEqual(testObject.Object.FedExAccountNumber, "FedEx Account Number");
            Assert.AreEqual(testObject.Object.FedExPassword, "FedEx Password");
            Assert.AreEqual(testObject.Object.FedExUsername, "FedEx Username");
        }
    }
}
