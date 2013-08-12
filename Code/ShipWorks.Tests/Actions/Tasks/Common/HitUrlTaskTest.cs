using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    [TestClass]
    public class HitUrlTaskTest
    {

        HitUrlTask testObject;

        [TestInitialize]
        public void TestInitialize()
        {
            testObject = new HitUrlTask()
            {
                HttpHeaders = new[]
                {
                    new KeyValuePair<string, string>("first", "1st"),
                    new KeyValuePair<string, string>("second", "2nd"),
                    new KeyValuePair<string, string>("third", "3rd")
                },
                UseBasicAuthentication = true,
                Username = "user",
                Password = "pw",
                UrlToHit = "http://www.shipworks.com",
                Verb = HttpVerb.Get
            };
        }

        [TestMethod]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            string serializedObject = testObject.SerializeSettings();

            HitUrlTask deserailzedObject = new HitUrlTask();
            deserailzedObject.Initialize(serializedObject);

            Assert.AreEqual(true,deserailzedObject.UseBasicAuthentication);
            Assert.AreEqual("user",deserailzedObject.Username);
            Assert.AreEqual("2nd",deserailzedObject.HttpHeaders[1].Value);
            Assert.AreEqual("second", deserailzedObject.HttpHeaders[1].Key);

            Assert.AreEqual(HttpVerb.Get,deserailzedObject.Verb);

        }
    }
}
