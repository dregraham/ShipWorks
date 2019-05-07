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
        private readonly AutoMock mock;

        public WalmartStoreSetupControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Save_ThrowsWalmartException_WhenClientIDIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientSecret = "12345";

            Assert.Throws<WalmartException>(() => testObject.Save(new WalmartStoreEntity()));
        }

        [Fact]
        public void Save_ThrowsWalmartException_WhenClientSecretIsEmptyAndStoreIsNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.IsNewStore = true;

            Assert.Throws<WalmartException>(() => testObject.Save(new WalmartStoreEntity()));
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenClientSecretIsEmptyAndStoreIsNotNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.IsNewStore = false;

            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenAllFieldsHaveValue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "12345";

            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Save_SavesTrimmedClientID()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "   ClientID     ";
            testObject.ClientSecret = "12345";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            Assert.Equal("ClientID", store.ClientID);
        }

        [Fact]
        public void Save_DoesNotSaveTrimmedClientSecret_WhenClientSecretIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "";
            testObject.IsNewStore = false;

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientSecret = "KEY";

            testObject.Save(store);

            Assert.Equal("KEY", store.ClientSecret);
        }

        [Fact]
        public void Save_SavesTrimmedClientSecret_WhenClientSecretHasValue()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            mock.Mock<IEncryptionProviderFactory>().Setup(e => e.CreateWalmartEncryptionProvider()).Returns(encryptionProvider);

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientSecret = "KEY";

            encryptionProvider.Setup(e => e.Encrypt(testObject.ClientSecret)).Returns(testObject.ClientSecret);

            testObject.Save(store);

            Assert.Equal("NEW KEY", store.ClientSecret);
        }

        [Fact]
        public void Save_DelegatesToEncryptionProvider_WhenClientSecretHasValue()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            mock.Mock<IEncryptionProviderFactory>().Setup(e => e.CreateWalmartEncryptionProvider()).Returns(encryptionProvider);

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            encryptionProvider.Verify(e => e.Encrypt("NEW KEY"));
        }

        [Fact]
        public void Save_DelegatesToWebClient()
        {
            var webClient = mock.Mock<IWalmartWebClient>();

            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "NEW KEY";

            WalmartStoreEntity store = new WalmartStoreEntity();

            testObject.Save(store);

            webClient.Verify(w => w.TestConnection(store));
        }

        [Fact]
        public void Save_SetsUpdatingClientSecretToFalse_WhenStoreIsNotNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "NEW KEY";
            testObject.IsNewStore = false;
            testObject.UpdatingClientSecret = true;
            testObject.Save(new WalmartStoreEntity());

            Assert.False(testObject.UpdatingClientSecret);
        }

        [Fact]
        public void Save_DoesNotThrowWalmartException_WhenCredentialsAreValid()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.ClientID = "ClientID";
            testObject.ClientSecret = "NEW KEY";
            testObject.IsNewStore = false;
            testObject.UpdatingClientSecret = true;
            testObject.Save(new WalmartStoreEntity());
        }

        [Fact]
        public void Load_SetsClientIDToStoresClientID()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientID = "ClientID";

            testObject.Load(store);

            Assert.Equal("ClientID", testObject.ClientID);
        }

        [Fact]
        public void Load_SetsIsNewStoreToTrue_WhenStoresClientIDIsEmpty()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientID = "";

            testObject.Load(store);

            Assert.True(testObject.IsNewStore);
        }

        [Fact]
        public void Load_SetsIsNewStoreToFalse_WhenStoresClientIDHasValue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientID = "ClientID";

            testObject.Load(store);

            Assert.False(testObject.IsNewStore);
        }

        [Fact]
        public void Load_SetsUpdatingClientSecretToTrue_WhenStoreIsNew()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.ClientID = "";

            testObject.Load(store);

            Assert.True(testObject.UpdatingClientSecret);
        }

        [Fact]
        public void UpdateClientSecretCommand_SetsUpdatingClientSecretToTrue()
        {
            var testObject = mock.Create<WalmartStoreSetupControlViewModel>();
            testObject.UpdatingClientSecret = false;

            testObject.UpdateClientSecretCommand.Execute(null);

            Assert.True(testObject.UpdatingClientSecret);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}