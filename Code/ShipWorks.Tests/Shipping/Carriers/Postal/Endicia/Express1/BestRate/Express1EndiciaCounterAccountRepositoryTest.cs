using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Express1.BestRate
{
    [TestClass]
    public class Express1EndiciaCounterAccountRepositoryTest
    {
        [TestMethod]
        public void GetAccount_GetsCredentialsFromStore()
        {
            var store = new Mock<ICounterRatesCredentialStore>();
            var testObject = new Express1EndiciaCounterAccountRepository(store.Object);

            testObject.GetAccount(0);

            store.Verify(x => x.Express1EndiciaPassPhrase);
            store.Verify(x => x.Express1EndiciaUAccountNumber);
        }

        [TestMethod]
        public void GetAccount_ReturnsAccountWithCredentials()
        {
            var store = new Mock<ICounterRatesCredentialStore>();
            store.Setup(x => x.Express1EndiciaUAccountNumber).Returns("Foo");
            store.Setup(x => x.Express1EndiciaPassPhrase).Returns("Bar");

            var testObject = new Express1EndiciaCounterAccountRepository(store.Object);

            EndiciaAccountEntity account = testObject.GetAccount(0);

            Assert.AreEqual("Foo", account.AccountNumber);
            Assert.AreEqual("Bar", account.ApiUserPassword);
        }
    }
}
