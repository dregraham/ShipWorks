using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.UI.Platforms.Walmart.WizardPages;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartStoreSetupControlViewModelTest : IDisposable
    {
        private AutoMock mock;

        public WalmartStoreSetupControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Save_ThrowsWalmartException_WhenConsumerIDIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "12345";

            Assert.Throws<WalmartException>(() => testObject.Save(new WalmartStoreEntity()));
        }

        [Fact]
        public void Save_ThrowsWalmartException_WhenChannelTypeIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.PrivateKey = "12345";

            Assert.Throws<WalmartException>(() => testObject.Save(new WalmartStoreEntity()));
        }

        [Fact]
        public void Save_ThrowsWalmartException_WhenPrivateKeyIsEmptyAndStoreIsNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.IsNewStore = true;

            Assert.Throws<WalmartException>(() => testObject.Save(new WalmartStoreEntity()));
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenPrivateKeyIsEmptyAndStoreIsNotNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.IsNewStore = false;

            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenAllFieldsHaveValue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "12345";

            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Save_SavesTrimmedConsumerID()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "   ConsumerID     ";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "12345";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            Assert.Equal("ConsumerID", store.ConsumerID);
        }

        [Fact]
        public void Save_SavesTrimmedChannelType()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "  channel     ";
            testObject.PrivateKey = "12345";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            Assert.Equal("channel", store.ChannelType);
        }

        [Fact]
        public void Save_DoesNotSaveTrimmedPrivateKey_WhenPrivateKeyIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "";
            testObject.IsNewStore = false;

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.PrivateKey = "KEY";

            testObject.Save(store);

            Assert.Equal("KEY", store.PrivateKey);
        }

        [Fact]
        public void Save_SavesTrimmedPrivateKey_WhenPrivateKeyHasValue()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            mock.Mock<IEncryptionProviderFactory>().Setup(e => e.CreateWalmartEncryptionProvider()).Returns(encryptionProvider);

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.PrivateKey = "KEY";

            encryptionProvider.Setup(e => e.Encrypt(testObject.PrivateKey)).Returns(testObject.PrivateKey);

            testObject.Save(store);

            Assert.Equal("NEW KEY", store.PrivateKey);
        }

        [Fact]
        public void Save_DelegatesToEncryptionProvider_WhenPrivateKeyHasValue()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            mock.Mock<IEncryptionProviderFactory>().Setup(e => e.CreateWalmartEncryptionProvider()).Returns(encryptionProvider);

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            encryptionProvider.Verify(e => e.Encrypt("NEW KEY"));
        }

        [Fact]
        public void Save_DelegatesToWebClient()
        {
            var webClient = mock.Mock<IWalmartWebClient>();

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            webClient.Verify(w => w.TestConnection(store));
        }

        [Fact]
        public void Save_SetsUpdatingPrivateKeyToFalse_WhenStoreIsNotNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "NEW KEY";
            testObject.IsNewStore = false;
            testObject.UpdatingPrivateKey = true;
            testObject.Save(new WalmartStoreEntity());

            Assert.False(testObject.UpdatingPrivateKey);
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenCredentialsAreValid()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ConsumerID = "ConsumerID";
            testObject.ChannelType = "channel";
            testObject.PrivateKey = "NEW KEY";
            testObject.IsNewStore = false;
            testObject.UpdatingPrivateKey = true;
            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Load_SetsConsumerIDToStoresConsumerID()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "ConsumerID";

            testObject.Load(store);

            Assert.Equal("ConsumerID", testObject.ConsumerID);
        }

        [Fact]
        public void Load_SetsChannelTypeToStoresChannelType()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ChannelType = "channel";

            testObject.Load(store);

            Assert.Equal("channel", testObject.ChannelType);
        }

        [Fact]
        public void Load_SetsIsNewStoreToTrue_WhenStoresConsumerIDIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "";

            testObject.Load(store);

            Assert.True(testObject.IsNewStore);
        }

        [Fact]
        public void Load_SetsIsNewStoreToFalse_WhenStoresConsumerIDHasValue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "ConsumerID";

            testObject.Load(store);

            Assert.False(testObject.IsNewStore);
        }

        [Fact]
        public void Load_SetsUpdatingPrivateKeyToTrue_WhenStoreIsNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ConsumerID = "";

            testObject.Load(store);

            Assert.True(testObject.UpdatingPrivateKey);
        }

        [Fact]
        public void UpdatePrivateKeyCommand_SetsUpdatingPrivateKeyToTrue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.UpdatingPrivateKey = false;

            testObject.UpdatePrivateKeyCommand.Execute(null);

            Assert.True(testObject.UpdatingPrivateKey);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}