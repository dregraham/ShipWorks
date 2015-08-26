using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    [TestClass]
    public class AmazonCredentialsTest
    {
        [TestMethod]
        public void Initialize_WithNoStores_DoesNotPopulateCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual(string.Empty, testObject.MerchantId);
                Assert.AreEqual(string.Empty, testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithNonAmazonStores_DoesNotPopulateCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new GenericModuleStoreEntity {Enabled = true}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual(string.Empty, testObject.MerchantId);
                Assert.AreEqual(string.Empty, testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithOnlyDisabledAmazonStores_DoesNotPopulateCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = false}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual(string.Empty, testObject.MerchantId);
                Assert.AreEqual(string.Empty, testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithSingleEnabledAmazonStore_PopulatesCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = true}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual("Foo", testObject.MerchantId);
                Assert.AreEqual("Bar", testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithSingleIdenticalEnabledAmazonStore_PopulatesCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = true},
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = true}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual("Foo", testObject.MerchantId);
                Assert.AreEqual("Bar", testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithEnabledAndDisabledAmazonStore_PopulatesCredentialsFromEnabled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new AmazonStoreEntity {MerchantID = "Baz", AuthToken = "Quux", Enabled = false},
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = true}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual("Foo", testObject.MerchantId);
                Assert.AreEqual("Bar", testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Initialize_WithMultipleDifferentEnabledAmazonStores_DoesNotPopulateCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(x => x.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new AmazonStoreEntity {MerchantID = "Baz", AuthToken = "Quux", Enabled = true},
                        new AmazonStoreEntity {MerchantID = "Foo", AuthToken = "Bar", Enabled = true}
                    });

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();

                Assert.AreEqual(string.Empty, testObject.MerchantId);
                Assert.AreEqual(string.Empty, testObject.AuthToken);
            }
        }

        [TestMethod]
        public void Validate_DelegatesValidationToWebClient()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IAmazonShippingWebClient> webClient = mock.Mock<IAmazonShippingWebClient>();
                Mock<IAmazonMwsWebClientSettingsFactory> settingsFactory = mock.Mock<IAmazonMwsWebClientSettingsFactory>();

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Validate();

                AmazonMwsWebClientSettings settings = settingsFactory.Object.Create("Foo", "Bar", "US");

                webClient.Verify(x => x.ValidateCredentials(settings));
            }
        }

        [TestMethod]
        public void Validate_DoesNotCallWebClient_WhenMerchantIdIsNotSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IAmazonShippingWebClient> webClient = mock.Mock<IAmazonShippingWebClient>();
                Mock<IAmazonMwsWebClientSettingsFactory> settingsFactory = mock.Mock<IAmazonMwsWebClientSettingsFactory>();

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();


                testObject.MerchantId = string.Empty;
                testObject.AuthToken = "Bar";
                testObject.Validate();

                webClient.Verify(x => x.ValidateCredentials(It.IsAny<AmazonMwsWebClientSettings>()), Times.Never);
                Assert.AreEqual("MerchantId and AuthToken are required", testObject.Message);
                Assert.AreEqual(false, testObject.Success);
            }
        }

        [TestMethod]
        public void Validate_DoesNotCallWebClient_WhenAuthTokenIsNotSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IAmazonShippingWebClient> webClient = mock.Mock<IAmazonShippingWebClient>();

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = string.Empty;
                testObject.Validate();

                webClient.Verify(x => x.ValidateCredentials(It.IsAny<AmazonMwsWebClientSettings>()), Times.Never);
                Assert.AreEqual("MerchantId and AuthToken are required", testObject.Message);
                Assert.AreEqual(false, testObject.Success);
            }
        }

        [TestMethod]
        public void Validate_WithValidCredentials_ClearsMessageAndSetsSuccessToTrue()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(x => x.ValidateCredentials(It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(AmazonValidateCredentialsResponse.Succeeded);

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Validate();

                Assert.AreEqual(string.Empty, testObject.Message);
                Assert.AreEqual(true, testObject.Success);
            }
        }

        [TestMethod]
        public void Validate_WithInvalidCredentials_SetsMessageAndSetsSuccessToFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonShippingWebClient>()
                    .Setup(x => x.ValidateCredentials(It.IsAny<AmazonMwsWebClientSettings>()))
                    .Returns(AmazonValidateCredentialsResponse.Failed("Error message"));

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Validate();

                Assert.AreEqual("Error message", testObject.Message);
                Assert.AreEqual(false, testObject.Success);
            }
        }

        [TestMethod]
        public void PopulateAccount_SetsCredentials_WhenValidated()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity();

                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Success = true;

                testObject.PopulateAccount(account);

                Assert.AreEqual("Foo", account.MerchantID);
                Assert.AreEqual("Bar", account.AuthToken);
            }
        }

        [TestMethod]
        public void PopulateAccount_ThrowsInvalidOperationException_WhenNotValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Success = false;

                try
                {
                    testObject.PopulateAccount(new AmazonAccountEntity());
                    Assert.Fail();
                }
                catch (InvalidOperationException)
                {
                    // Pass
                }
            }
        }

        [TestMethod]
        public void PopulateAccount_ThrowsArgumentNullException_WhenAccountIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                try
                {
                    testObject.PopulateAccount(null);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Pass
                }
            }
        }
    }
}
