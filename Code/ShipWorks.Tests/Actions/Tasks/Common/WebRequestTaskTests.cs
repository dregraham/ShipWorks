using Interapptive.Shared.Net;
using Xunit;
using ShipWorks.Actions.Tasks.Common;
using System.Collections.Generic;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class WebRequestTaskTests
    {
        WebRequestTask testObject;

        public WebRequestTaskTests()
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

            Assert.Equal(true, deserializedObject.UseBasicAuthentication);
            Assert.Equal("user", deserializedObject.Username);
            Assert.Equal("2nd", deserializedObject.HttpHeaders[1].Value);
            Assert.Equal("second", deserializedObject.HttpHeaders[1].Key);

            Assert.Equal(HttpVerb.Get, deserializedObject.Verb);
        }
    }
}
