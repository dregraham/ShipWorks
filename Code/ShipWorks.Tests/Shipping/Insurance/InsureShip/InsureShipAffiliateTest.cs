using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipAffiliateTest
    {
        InsureShipAffiliate testObject;

        [Fact]
        public void InsureShipStoreID_IsCorrect_FromConstructorValue()
        {
            testObject = new InsureShipAffiliate("tangoStoreID", "xx");

            Assert.AreEqual("tangoStoreID", testObject.InsureShipStoreID);
        }

        [Fact]
        public void InsureShipPolicyID_IsCorrect_FromConstructorValue()
        {
            testObject = new InsureShipAffiliate("tangoStoreID", "tangoCustomerID");

            Assert.AreEqual("SWtangoCustomerID", testObject.InsureShipPolicyID);
        }

        [Fact]
        public void ArgumentNullException_IsNotThrown_WhenTangoStoreIDIsEmpty()
        {
            testObject = new InsureShipAffiliate("", "tangoCustomerID");
        }

        [Fact]
        public void ArgumentNullException_IsNotThrown_WhenTangoCustomerIDIsEmpty()
        {
            testObject = new InsureShipAffiliate("xxx", "");
        }
    }
}
