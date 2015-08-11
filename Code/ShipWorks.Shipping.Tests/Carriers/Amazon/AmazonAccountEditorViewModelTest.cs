using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonAccountEditorViewModelTest
    {
        [Fact]
        public void Load_WithNullAccount_ThrowsNullArgumentException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        [Fact]
        public void Load_WithValidAccount_SetsCredentials()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupAllProperties();

                AmazonAccountEntity account = new AmazonAccountEntity
                {
                    MerchantID = "Foo",
                    AuthToken = "Bar"
                };

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Load(account);

                Assert.Equal("Foo", testObject.Credentials.MerchantId);
                Assert.Equal("Bar", testObject.Credentials.AuthToken);
            }
        }

        [Fact]
        public void Load_WithValidAccount_SetsPersonDetails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity
                {
                    FirstName = "Foo",
                    LastName = "Bar"
                };

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Load(account);

                Assert.Equal("Foo", testObject.Person.FirstName);
                Assert.Equal("Bar", testObject.Person.LastName);
            }
        }

        [Fact]
        public void Load_WithValidAccount_SetsDescription()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity
                {
                    Description = "Foo"
                };

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Load(account);

                Assert.Equal("Foo", testObject.Description);
            }
        }

        [Fact]
        public void Load_SetsDescriptionToNull_WhenDescriptionMatchesDefault()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity
                {
                    Description = "Foo"
                };

                mock.Mock<IAmazonAccountManager>()
                    .Setup(x => x.GetDefaultDescription(account))
                    .Returns("Foo");

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Load(account);

                Assert.Null(testObject.Description);
            }
        }

        [Fact]
        public void Load_WithValidAccount_SetsDescriptionPromptFromAccountManager()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity();
                mock.Mock<IAmazonAccountManager>()
                    .Setup(x => x.GetDefaultDescription(account))
                    .Returns("Foo description");

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Load(account);

                Assert.Equal("Foo description", testObject.DescriptionPrompt);
            }
        }

        [Fact]
        public void Save_WithNullAccount_ThrowsArgumentNullException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                Assert.Throws<ArgumentNullException>(() => testObject.Save(null));
            }
        }

        [Fact]
        public void Save_DelegatesToAmazonCredentialsValidate_WithValidAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Save(new AmazonAccountEntity());

                mock.Mock<IAmazonCredentials>().Verify(x => x.Validate());

                Assert.False(testObject.Success);
            }
        }

        [Fact]
        public void Save_SuccessIsFalse_WhenCredentialsFailValidation()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, false)
                    .SetupProperty(x => x.Message, "Foo");

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Save(new AmazonAccountEntity());

                Assert.False(testObject.Success);
                Assert.Equal("Foo", testObject.Message);
            }
        }

        [Fact]
        public void Save_DelegatesAccountPopulationToCredentials_WhenCredentialsAreValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                mock.Mock<IAmazonCredentials>()
                    .Verify(x => x.PopulateAccount(account));
            }
        }

        [Fact]
        public void Save_CopiesContactInfoToAccount_WhenCredentialsAreValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                testObject.Person.FirstName = "NewFoo";
                testObject.Person.City = "NewBar";

                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                Assert.Equal("NewFoo", account.FirstName);
                Assert.Equal("NewBar", account.City);
            }
        }

        [Fact]
        public void Save_CopiesDescriptionToAccount_WhenDescriptionIsSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                testObject.Description = "NewFoo";

                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                Assert.Equal("NewFoo", account.Description);
            }
        }

        [Fact]
        public void Save_CopiesDefaultDescriptionToAccount_WhenDescriptionIsNotSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity();

                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);
                mock.Mock<IAmazonAccountManager>()
                    .Setup(x => x.GetDefaultDescription(account))
                    .Returns("New Description");

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                testObject.Description = string.Empty;

                testObject.Save(account);

                Assert.Equal("New Description", account.Description);
            }
        }

        [Fact]
        public void Save_SavesToAccountManager_WhenCredentialsAreValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                mock.Mock<IAmazonAccountManager>()
                    .Verify(x => x.SaveAccount(account));
            }
        }

        [Fact]
        public void Save_SuccessIsTrue_WhenSaveIsSuccessful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                Assert.True(testObject.Success);
                Assert.Equal(testObject.Message, string.Empty);
            }
        }

        [Fact]
        public void Save_SuccessIsFalseWithMessage_WhenSaveHasORMConcurrencyException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEntity account = new AmazonAccountEntity();

                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                mock.Mock<IAmazonAccountManager>()
                    .Setup(x => x.SaveAccount(It.IsAny<AmazonAccountEntity>()))
                    .Throws(new ORMConcurrencyException("Foo", account));

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Save(account);

                Assert.False(testObject.Success);
                Assert.Equal(testObject.Message, "Your changes cannot be saved because another use has deleted the account.");
            }
        }
    }
}
