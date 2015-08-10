using System;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    [TestClass]
    public class AmazonAccountEditorViewModelTest
    {
        [TestMethod]
        public void Load_WithNullAccount_ThrowsNullArgumentException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                try
                {
                    testObject.Load(null);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Success
                }
            }
        }

        [TestMethod]
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

                Assert.AreEqual("Foo", testObject.Credentials.MerchantId);
                Assert.AreEqual("Bar", testObject.Credentials.AuthToken);
            }
        }

        [TestMethod]
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

                Assert.AreEqual("Foo", testObject.Person.FirstName);
                Assert.AreEqual("Bar", testObject.Person.LastName);
            }
        }

        [TestMethod]
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

                Assert.AreEqual("Foo", testObject.Description);
            }
        }

        [TestMethod]
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

                Assert.IsNull(testObject.Description);
            }
        }

        [TestMethod]
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

                Assert.AreEqual("Foo description", testObject.DescriptionPrompt);
            }
        }

        [TestMethod]
        public void Save_WithNullAccount_ThrowsArgumentNullException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                try
                {
                    testObject.Save(null);
                    Assert.Fail();
                }
                catch (ArgumentNullException)
                {
                    // Success
                }
            }
        }

        [TestMethod]
        public void Save_DelegatesToAmazonCredentialsValidate_WithValidAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Save(new AmazonAccountEntity());

                mock.Mock<IAmazonCredentials>().Verify(x => x.Validate());

                Assert.IsFalse(testObject.Success);
            }
        }

        [TestMethod]
        public void Save_SuccessIsFalse_WhenCredentialsFailValidation()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, false)
                    .SetupProperty(x => x.Message, "Foo");

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();

                testObject.Save(new AmazonAccountEntity());

                Assert.IsFalse(testObject.Success);
                Assert.AreEqual("Foo", testObject.Message);
            }
        }

        [TestMethod]
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

        [TestMethod]
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

                Assert.AreEqual("NewFoo", account.FirstName);
                Assert.AreEqual("NewBar", account.City);
            }
        }

        [TestMethod]
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

                Assert.AreEqual("NewFoo", account.Description);
            }
        }

        [TestMethod]
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

                Assert.AreEqual("New Description", account.Description);
            }
        }

        [TestMethod]
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

        [TestMethod]
        public void Save_SuccessIsTrue_WhenSaveIsSuccessful()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IAmazonCredentials>()
                    .SetupProperty(x => x.Success, true);

                AmazonAccountEditorViewModel testObject = mock.Create<AmazonAccountEditorViewModel>();
                AmazonAccountEntity account = new AmazonAccountEntity();

                testObject.Save(account);

                Assert.IsTrue(testObject.Success);
                Assert.AreEqual(testObject.Message, string.Empty);
            }
        }

        [TestMethod]
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

                Assert.IsFalse(testObject.Success);
                Assert.AreEqual(testObject.Message, "Your changes cannot be saved because another use has deleted the account.");
            }
        }
    }
}
