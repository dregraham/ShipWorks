using System;
using System.Text;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon.SFP
{
    public class GetAmazonCarrierCredentialsViewModelTest : IDisposable
    {
        private AutoMock mock;
        bool onCompleteCalled = false;

        public GetAmazonCarrierCredentialsViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Save_SavesStoreCredentials_WhenAddingACarrier()
        {
            var testObject = mock.Create<GetAmazonCarrierCredentialsViewModel>();
            testObject.OnComplete = HandleOnComplete;

            var store = new ChannelAdvisorStoreEntity();
            testObject.Load(store);
            testObject.CredentialsToken = CreateToken("sellerId", "marketplaceId", "carrierid");
            testObject.SelectedRegion = "US";

            testObject.Save();

            Assert.Equal(store.MerchantID, "sellerId");
            Assert.Equal(store.Region, "US");
            Assert.Equal(store.PlatformAmazonCarrierID, "carrierid");
            Assert.True(onCompleteCalled);
        }

        [Fact]
        public void Save_DoesNotCloseWindow_WhenCredentialstokenIsInvalid()
        {
            var testObject = mock.Create<GetAmazonCarrierCredentialsViewModel>();
            testObject.OnComplete = HandleOnComplete;

            var store = new ChannelAdvisorStoreEntity();
            testObject.Load(store);
            testObject.CredentialsToken = "Invalid token";
            testObject.SelectedRegion = "US";

            testObject.Save();

            Assert.Null(store.MerchantID);
            Assert.Null(store.Region);
            Assert.Null(store.PlatformAmazonCarrierID);
            Assert.False(onCompleteCalled);
        }

        private void HandleOnComplete()
        {
            onCompleteCalled = true;
        }

        private string CreateToken(string merchantId, string marketplaceId, string carrierId)
        {
            var plainText = $"{merchantId}_{marketplaceId}_{carrierId}";
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
