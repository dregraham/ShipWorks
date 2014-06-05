using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Express1.BestRate
{
    [TestClass]
    public class Express1EndiciaCounterAccountRepositoryTest
    {
        [TestMethod]
        public void GetAccount_GetsCredentialsFromStore()
        {
            Mock<ICredentialStore> store = new Mock<ICredentialStore>();
            Express1EndiciaCounterAccountRepository testObject = new Express1EndiciaCounterAccountRepository(store.Object);

            testObject.GetAccount(0);

            store.Verify(x => x.Express1EndiciaPassPhrase);
            store.Verify(x => x.Express1EndiciaAccountNumber);
        }

        [TestMethod]
        public void GetAccount_ReturnsValidCounterAccount()
        {
            Mock<ICredentialStore> store = new Mock<ICredentialStore>();
            store.Setup(x => x.Express1EndiciaAccountNumber).Returns("Foo");
            store.Setup(x => x.Express1EndiciaPassPhrase).Returns("Bar");

            Express1EndiciaCounterAccountRepository testObject = new Express1EndiciaCounterAccountRepository(store.Object);

            EndiciaAccountEntity account = testObject.GetAccount(0);

            Assert.AreEqual("Foo", account.AccountNumber);
            Assert.AreEqual("Bar", account.ApiUserPassword);
            Assert.AreEqual((int)EndiciaReseller.Express1, account.EndiciaReseller);
        }

        [TestMethod]
        public void DefaultProfileAccount_ReturnsFirstItemInAccountsCollection_Test()
        {
            Mock<ICredentialStore> store = new Mock<ICredentialStore>();
            store.Setup(x => x.Express1EndiciaAccountNumber).Returns("Foo");
            store.Setup(x => x.Express1EndiciaPassPhrase).Returns("Bar");

            Express1EndiciaCounterAccountRepository testObject = new Express1EndiciaCounterAccountRepository(store.Object);

            EndiciaAccountEntity account = testObject.DefaultProfileAccount;

            Assert.AreEqual(testObject.Accounts.First(), account);
        }
    }
}
