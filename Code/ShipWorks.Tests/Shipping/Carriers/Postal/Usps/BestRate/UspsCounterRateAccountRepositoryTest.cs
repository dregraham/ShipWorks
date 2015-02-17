using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.BestRate
{
    [TestClass]
    public class UspsCounterRateAccountRepositoryTest
    {
        private UspsCounterRateAccountRepository testObject;

        private readonly Mock<ICounterRatesCredentialStore> credentialStore;

        public UspsCounterRateAccountRepositoryTest()
        {
            credentialStore = new Mock<ICounterRatesCredentialStore>();
            credentialStore.Setup(s => s.UspsUsername).Returns("UspsUser");
            credentialStore.Setup(s => s.UspsPassword).Returns("sampsPassword");

            testObject = new UspsCounterRateAccountRepository(credentialStore.Object);
        }

        [TestMethod]
        public void Accounts_ReturnsCollectionWithOneItem_Test()
        {
            Assert.AreEqual(1, testObject.Accounts.Count());
        }

        [TestMethod]
        public void Accounts_DelegatesToCredentialStore_WhenAssigningUsername_Test()
        {
            UspsAccountEntity account = testObject.Accounts.First();

            credentialStore.Verify(s => s.UspsUsername, Times.Once());
            Assert.AreEqual(credentialStore.Object.UspsUsername, account.Username);
        }

        [TestMethod]
        public void Accounts_DelegatesToCredentialStore_WhenAssigningPassword_Test()
        {
            UspsAccountEntity account = testObject.Accounts.First();

            credentialStore.Verify(s => s.UspsPassword, Times.Once());
            Assert.AreEqual(credentialStore.Object.UspsPassword, account.Password);
        }
    }
}
