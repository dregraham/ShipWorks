using Interapptive.Shared.Net;
using Xunit;
using ShipWorks.Actions.Tasks.Common;
using System.Collections.Generic;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class WebRequestTaskTests
    {
        WebRequestTask testObject;

        [TestInitialize]
        public void TestInitialize()
        {
            testObject = new WebRequestTask()
            {
                HttpHeaders = new[]
                {
                    new KeyValuePair<string, string>("first", "1st"),
                    new KeyValuePair<string, string>("second", "2nd"),
                    new KeyValuePair<string, string>("third", "3rd")
                },
                UseBasicAuthentication = true,
                Username = "user",
                Url = "http://www.shipworks.com",
                Verb = HttpVerb.Get
            };

            testObject.SetPassword("password");
        }

        [Fact]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            string serializedObject = testObject.SerializeSettings();

            var deserializedObject = new WebRequestTask();
            deserializedObject.Initialize(serializedObject);

            Assert.AreEqual(true, deserializedObject.UseBasicAuthentication);
            Assert.AreEqual("user", deserializedObject.Username);
            Assert.AreEqual("2nd", deserializedObject.HttpHeaders[1].Value);
            Assert.AreEqual("second", deserializedObject.HttpHeaders[1].Key);

            Assert.AreEqual(HttpVerb.Get, deserializedObject.Verb);
        }
    }
}
