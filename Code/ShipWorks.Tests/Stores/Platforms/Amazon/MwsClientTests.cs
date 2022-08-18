using System;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class MwsClientTests : IDisposable
    {
        private readonly AutoMock mock;

        public MwsClientTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ValidateSigning()
        {
            string stringToSign = "POST\nmws.amazonservices.com\n/Orders/2011-01-01\nAWSAccessKeyId=AKIAJU465C2O2U6WNQTA&Action=ListOrderItems&AmazonOrderId=SHIPWORKS_CONNECT_ATTEMPT&SellerId=A2OIXR4TA6XKOD&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp=2011-08-19T15%3A55%3A39Z&Version=2011-01-01";
            string signature = "XKab7VXEklL8EYE0HcqRF97fgjqTNsM7NFe/gJ7eWLE=";

            string signed = RequestSignature.CreateRequestSignature(stringToSign, "0+NcTFU11/qooQriFFO7k0JxY64t/P38DIAAAgeW", SigningAlgorithm.SHA256);

            Assert.Equal(signature, signed);
        }

        public void Dispose() => mock.Dispose();
    }
}
