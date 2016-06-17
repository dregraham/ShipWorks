using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class CheckCredentialsResponseSerializerTest
    {
        private string successfulResponseXml;
        private string invalidSellerIdXml;
        private string invalidSecretKeyXml;

        private CheckCredentialsResponseSerializer serializer;

        public CheckCredentialsResponseSerializerTest()
        {
            serializer = new CheckCredentialsResponseSerializer();

            successfulResponseXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<NeweggAPIResponse>
  <IsSuccess>true</IsSuccess>
  <OperationType>GetServiceStatus</OperationType>
  <SellerID>A09V</SellerID>
  <ResponseBody>
    <Status>1</Status>
    <Timestamp>06/21/2012 06:09:33</Timestamp>
  </ResponseBody>
</NeweggAPIResponse>";

            invalidSellerIdXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>AccessDenied</Code>
    <Message>The specified seller id is invalid or you have not yet got the authorization from this seller.</Message>
  </Error>
</Errors>";

            invalidSecretKeyXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>InvalidToken</Code>
    <Message>Illegal secret key.</Message>
  </Error>
</Errors>";
        }


        [Fact]
        public void Deserialize_ReturnsCheckCredentialResult_WhenSucessfulResponse()
        {
            object result = serializer.Deserialize(successfulResponseXml);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<CheckCredentialsResult>(result);
        }

        [Fact]
        public void Deserialize_CheckCredentialResultIsSuccessful_WhenSucessfulResponse()
        {
            CheckCredentialsResult result = serializer.Deserialize(successfulResponseXml) as CheckCredentialsResult;
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Deserialize_ThrowsInvalidOperationException_WhenInvalidSellerId()
        {
            Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(invalidSellerIdXml));
        }

        [Fact]
        public void Deserialize_ThrowsInvalidOperationException_WhenInvalidSecretKey()
        {
            Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(invalidSecretKeyXml));
        }

    }
}
