using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.BestRate
{
    [TestClass]
    public class StampsCounterRateAccountRepositoryTest
    {
        private StampsCounterRateAccountRepository testObject;

        private readonly Mock<ICounterRatesCredentialStore> credentialStore;

        public StampsCounterRateAccountRepositoryTest()
        {
            credentialStore = new Mock<ICounterRatesCredentialStore>();
            credentialStore.Setup(s => s.StampsUsername).Returns("stampsUser");
            credentialStore.Setup(s => s.StampsPassword).Returns("sampsPassword");

            testObject = new StampsCounterRateAccountRepository(credentialStore.Object);
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

            credentialStore.Verify(s => s.StampsUsername, Times.Once());
            Assert.AreEqual(credentialStore.Object.StampsUsername, account.Username);
        }

        [TestMethod]
        public void Accounts_DelegatesToCredentialStore_WhenAssigningPassword_Test()
        {
            UspsAccountEntity account = testObject.Accounts.First();

            credentialStore.Verify(s => s.StampsPassword, Times.Once());
            Assert.AreEqual(credentialStore.Object.StampsPassword, account.Password);
        }
    }
}
