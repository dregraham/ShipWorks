using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipAffiliateProviderTest
    {
        InsureShipAffiliateProvider testObject;

        [Fact]
        public void AddNewInsureShipAffiliate_Succeeds_WithValidParams()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1);

            Assert.AreEqual("storeID", insureShipAffiliate.InsureShipStoreID);
        }

        [Fact]
        public void AddExistingInsureShipAffiliate_Succeeds_WhenSameValuesAlreadyExistInProvider()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
        }

        [Fact]
        public void AddExistingInsureShipAffiliate_UpdatesPreviousVersionInProvider_WhenKeyUsed()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            testObject.Add(1, new InsureShipAffiliate("differentStoreID", "differentCustomerID"));

            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1);

            Assert.AreEqual("differentStoreID", insureShipAffiliate.InsureShipStoreID);
            Assert.AreEqual("SWdifferentCustomerID", insureShipAffiliate.InsureShipPolicyID);
        }

        [Fact]
        public void GetInsureShipAffiliate_ReturnsNull_WhenMissingKeyIsUsed()
        {
            testObject = new InsureShipAffiliateProvider();
            testObject.Add(1, new InsureShipAffiliate("storeID", "customerID"));
            InsureShipAffiliate insureShipAffiliate = testObject.GetInsureShipAffiliate(1000);

            Assert.IsNull(insureShipAffiliate);
        }
    }
}
