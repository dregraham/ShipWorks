using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipAffiliateTest
    {
        InsureShipAffiliate testObject;

        [TestMethod]
        public void InsureShipStoreID_IsCorrect_FromConstructorValue()
        {
            testObject = new InsureShipAffiliate("tangoStoreID", "xx");

            Assert.AreEqual("tangoStoreID", testObject.InsureShipStoreID);
        }

        [TestMethod]
        public void InsureShipPolicyID_IsCorrect_FromConstructorValue()
        {
            testObject = new InsureShipAffiliate("tangoStoreID", "tangoCustomerID");

            Assert.AreEqual("SWtangoCustomerID", testObject.InsureShipPolicyID);
        }

        [TestMethod]
        public void ArgumentNullException_IsNotThrown_WhenTangoStoreIDIsEmpty()
        {
            testObject = new InsureShipAffiliate("", "tangoCustomerID");
        }

        [TestMethod]
        public void ArgumentNullException_IsNotThrown_WhenTangoCustomerIDIsEmpty()
        {
            testObject = new InsureShipAffiliate("xxx", "");
        }
    }
}
