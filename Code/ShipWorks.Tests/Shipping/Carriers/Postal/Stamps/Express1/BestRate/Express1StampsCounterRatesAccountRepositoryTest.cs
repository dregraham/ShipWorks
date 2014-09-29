using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    [TestClass]
    public class Express1StampsCounterRatesAccountRepositoryTest
    {
        [TestMethod]
        public void GetAccount_GetsCredentialsFromStore()
        {
            Mock<ICounterRatesCredentialStore> store = new Mock<ICounterRatesCredentialStore>();
            Express1StampsCounterRatesAccountRepository testObject = new Express1StampsCounterRatesAccountRepository(store.Object);

            testObject.GetAccount(0);

            store.Verify(x => x.Express1StampsUsername);
            store.Verify(x => x.Express1StampsPassword);
        }

        [TestMethod]
        public void GetAccount_ReturnsValidCounterAccount()
        {
            Mock<ICounterRatesCredentialStore> store = new Mock<ICounterRatesCredentialStore>();
            store.Setup(x => x.Express1StampsUsername).Returns("Foo");
            store.Setup(x => x.Express1StampsPassword).Returns("Bar");

            Express1StampsCounterRatesAccountRepository testObject = new Express1StampsCounterRatesAccountRepository(store.Object);

            StampsAccountEntity account = testObject.GetAccount(0);

            Assert.AreEqual("Foo", account.Username);
            Assert.AreEqual("Bar", account.Password);
            Assert.IsTrue((StampsResellerType)account.StampsReseller == StampsResellerType.Express1);
        }
    }
}
