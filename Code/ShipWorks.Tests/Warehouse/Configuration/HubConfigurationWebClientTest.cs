using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration;
using Xunit;

namespace ShipWorks.Tests.Warehouse.Configuration
{
    public class HubConfigurationWebClientTest
    {
        private readonly AutoMock mock;

        public HubConfigurationWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public async Task GetSmsVerificationNumber_ReturnsVerificationNumber()
        {
            var response = mock.CreateMock<IRestResponse>();
            response.SetupGet(r => r.Content)
                .Returns("{\"smsVerifiedPhoneNumber\":\"5882300\"}");
   
            var warehouseRequestClient = mock.Mock<IWarehouseRequestClient>();
            warehouseRequestClient.Setup(c => c.MakeRequest(It.IsAny<IRestRequest>(), "Get SMS Verification Number"))
                .ReturnsAsync(GenericResult.FromSuccess(response.Object));
            
            var testObject = mock.Create<HubConfigurationWebClient>();
            var phoneNumber = await testObject.GetSmsVerificationNumber();
            
            Assert.Equal("5882300", phoneNumber.SmsVerifiedPhoneNumber);
        }
    }
}