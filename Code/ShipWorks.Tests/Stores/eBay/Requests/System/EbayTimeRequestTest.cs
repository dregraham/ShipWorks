using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.System;

namespace ShipWorks.Tests.Stores.eBay.Requests.System
{
    [TestClass]
    public class EbayTimeRequestTest
    {
        // There's not too much to test here from a unit perspective since this is interacting 
        // with eBay directly
        EbayTimeRequest testObject;

        public EbayTimeRequestTest()
        {
            // Check that this token key was valid as of 8/6/2012; check to see that it is still correct
            // when running the integration test
            string tokenKey = "AgAAAA**AQAAAA**aAAAAA**RtwaUA**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCZCCpAWdj6x9nY+seQ**mx0AAA**AAMAAA**/NP/FdHHa5sLL5UImXnWnhT1okn4npvrvkRmwEv9N6/3HYGzF1/CnD90UHw4T1u6wXGdJCW3YRoK7wf9EjFqFFMgHi0p9Ua8WogCQx1PbayLT4KIABEZ4ddnB9XZ2MpJQbqceiZh5BbJ2KkpV4HFE3zprjJCjvYKUMqdzvG+fWXc1sRUttD/ZbImpKn1aRPdl5XqHxmfoTL0K42EvHrw848Ms2z+HcWWhL9xClaPWrS2LHvYlpDj4NXV7NKWGr/PzGdZDkQuXLRNMyMGr+ELryDMZaOOwFCVWytNADOBPfhK0t8N6CkE1PKKMsuOFi4MRdoMFTtsBbYYhBtk4FDnYF7yjjQ9etn5s/zNLE7kXee3VXchlAWEHfzOpvBNk7Apjh4eKS0HY0Qnm7sNZY5xr0e6fz0IZ2OHbbcLalu0fWYCOyIxB4dXUG8WBc7IMVyODdxtuu8PQ/fW6rwgX2KJfTb6XjGemM8IXR8creaKXXnvKe1IG8q1v0fmvWWDDQ87s2Rfh3M3H6V7zO0RaWOGhxVEot71+iSsv4ncsULg/zpxn3FadAGsJa8qIe7vlZGVcaCbDfWTR76Un7hYNWUEmqH6IjXr83GpkifmWqrAYTEmzr26k+5A9VRbGewU+yRrI8xn1mAsFKgRsx+btYxA41yFg1C+BqqumT1tQv8F+9mJKDmMwtHdzDGb/jxW53zvvIWbPMVI6+Y6dBAgrNjHieNn1fZY+a9f8Gq/ICShHI8B77Dd7mHEMb/SSMaH7SFQ";
            testObject = new EbayTimeRequest(new TokenData { Key = tokenKey });
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsGetUser_Test()
        {
            string callName = testObject.GetEbayCallName();
            Assert.AreEqual("GeteBayOfficialTime", callName);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGeteBayOfficialTimeRequestType_Test()
        {
            AbstractRequestType request = testObject.GetEbayRequest();
            Assert.IsInstanceOfType(request, typeof(GeteBayOfficialTimeRequestType));
        }

        ///// <summary>
        ///// THIS CAN ONLY BE RAN IN DEBUG MODE AS IT REQUIRES PUTTING BREAK POINTS AT THE APILOGENTRY.LOGREQUEST(XML)
        ///// AND APILOGENTRY.LOGRESPONSE(XML) METHODS AND STEPPING OVER THAT CODE OTHERWISE THE TEST FAILS BECAUSE OF
        ///// DEPENDENCIES ON SHIPWORKSSESSION.INSTANCEID AND OTHER APPLICATION SETTINGS
        ///// </summary>
        //[TestMethod]
        //public void GetServerTimeInUtc_ReturnsDateTimeInUtc_Test()
        //{
        //    DateTime serverTimeInUtc = testObject.GetServerTimeInUtc();

        //    // This isn't going to be exact, so just check to see that the server 
        //    // time is within an acceptable range - within 2 minutes of system time (1 minute on each side)
        //    TimeSpan acceptableRange = new TimeSpan(0, 1, 0);
        //    Assert.IsTrue(serverTimeInUtc < DateTime.Now.ToUniversalTime().Add(acceptableRange) || serverTimeInUtc > DateTime.Now.ToUniversalTime().Subtract(acceptableRange));
        //}
    }
}
