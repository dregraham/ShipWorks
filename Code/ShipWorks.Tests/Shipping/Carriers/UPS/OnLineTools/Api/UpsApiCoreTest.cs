using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.Api
{
    [TestClass]
    public class UpsApiCoreTest
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GetUspsEndorsementTypeCode_ReturnsCorrectValue_Test()
        {
            Dictionary<UspsEndorsementType, string> testList = new Dictionary<UspsEndorsementType, string>();
            testList.Add(UspsEndorsementType.ReturnServiceRequested, "1");
            testList.Add(UspsEndorsementType.ForwardingServiceRequested, "2");
            testList.Add(UspsEndorsementType.AddressServiceRequested, "3");
            testList.Add(UspsEndorsementType.ChangeServiceRequested, "4");

            foreach (var entry in testList)
            {
                string testCode = UpsApiCore.GetUspsEndorsementTypeCode(entry.Key);
                Assert.AreEqual(entry.Value, testCode, string.Format("Assert.AreEqual failed. Expected:{0}. Actual:{1}. ", entry.Value, testCode));
            }
        }
    }
}
