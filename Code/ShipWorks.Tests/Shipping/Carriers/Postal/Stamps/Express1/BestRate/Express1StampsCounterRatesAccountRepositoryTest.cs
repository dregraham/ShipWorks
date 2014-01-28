using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    [TestClass]
    public class Express1StampsCounterRatesAccountRepositoryTest
    {
        [TestMethod]
        public void GetAccount_GetsCredentialsFromStore()
        {
            var store = new Mock<ICounterRatesCredentialStore>();
            var testObject = new Express1StampsCounterRatesAccountRepository(store.Object);

            testObject.GetAccount(0);

            store.Verify(x => x.Express1StampUsername);
            store.Verify(x => x.Express1StampsPassword);
        }

        [TestMethod]
        public void GetAccount_ReturnsValidCounterAccount()
        {
            var store = new Mock<ICounterRatesCredentialStore>();
            store.Setup(x => x.Express1StampUsername).Returns("Foo");
            store.Setup(x => x.Express1StampsPassword).Returns("Bar");

            var testObject = new Express1StampsCounterRatesAccountRepository(store.Object);

            StampsAccountEntity account = testObject.GetAccount(0);

            Assert.AreEqual("Foo", account.Username);
            Assert.AreEqual("Bar", account.Password);
            Assert.IsTrue(account.IsExpress1);
        }
    }
}
