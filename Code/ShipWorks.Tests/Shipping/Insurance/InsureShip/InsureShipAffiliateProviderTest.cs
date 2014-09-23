using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipAffiliateProviderTest
    {
        InsureShipAffiliateProvider testObject;

        [TestMethod]
        public void AddNewInsureShipAffiliate_Succeeds_WithValidParams()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1);

            Assert.AreEqual("storeID", insureShipAffiliate.InsureShipStoreID);
        }

        [TestMethod]
        public void AddExistingInsureShipAffiliate_Succeeds_WhenSameValuesAlreadyExistInProvider()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
        }

        [TestMethod]
        public void AddExistingInsureShipAffiliate_UpdatesPreviousVersionInProvider_WhenKeyUsed()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            testObject.Add(1, new InsureShipAffiliate("differentStoreID", "differentCustomerID"));

            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1);

            Assert.AreEqual("differentStoreID", insureShipAffiliate.InsureShipStoreID);
            Assert.AreEqual("SWdifferentCustomerID", insureShipAffiliate.InsureShipPolicyID);
        }

        [TestMethod]
        public void GetInsureShipAffiliate_ReturnsNull_WhenMissingKeyIsUsed()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1000);

            Assert.IsNull(insureShipAffiliate);
        }
    }
}
