using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Stores;
using Xunit;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    [Trait("Carrier", "Amazon")]
    public class AmazonCredentialsTest
    {
        [Fact]
        public void Initialize_WithNoStores_DoesNotPopulateCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.PopulateFromStore();
                
                Assert.Equal(string.Empty, testObject.MerchantId);
                Assert.Equal(string.Empty, testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal(string.Empty, testObject.MerchantId);
                Assert.Equal(string.Empty, testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal(string.Empty, testObject.MerchantId);
                Assert.Equal(string.Empty, testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal("Foo", testObject.MerchantId);
                Assert.Equal("Bar", testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal("Foo", testObject.MerchantId);
                Assert.Equal("Bar", testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal("Foo", testObject.MerchantId);
                Assert.Equal("Bar", testObject.AuthToken);
            }
        }

        [Fact]
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

                Assert.Equal(string.Empty, testObject.MerchantId);
                Assert.Equal(string.Empty, testObject.AuthToken);
            }
        }

        [Fact]
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

        [Fact]
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
                Assert.Equal("MerchantId and AuthToken are required", testObject.Message);
                Assert.Equal(false, testObject.Success);
            }
        }

        [Fact]
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
                Assert.Equal("MerchantId and AuthToken are required", testObject.Message);
                Assert.Equal(false, testObject.Success);
            }
        }

        [Fact]
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

                Assert.Equal(string.Empty, testObject.Message);
                Assert.Equal(true, testObject.Success);
            }
        }

        [Fact]
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

                Assert.Equal("Error message", testObject.Message);
                Assert.Equal(false, testObject.Success);
            }
        }

        [Fact]
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

                Assert.Equal("Foo", account.MerchantID);
                Assert.Equal("Bar", account.AuthToken);
            }
        }

        [Fact]
        public void PopulateAccount_ThrowsInvalidOperationException_WhenNotValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                testObject.MerchantId = "Foo";
                testObject.AuthToken = "Bar";
                testObject.Success = false;

                Assert.Throws<InvalidOperationException>(() => testObject.PopulateAccount(new AmazonAccountEntity()));
            }
        }

        [Fact]
        public void PopulateAccount_ThrowsArgumentNullException_WhenAccountIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonCredentials testObject = mock.Create<AmazonCredentials>();

                Assert.Throws<ArgumentNullException>(() => testObject.PopulateAccount(null));
            }
        }
    }
}
