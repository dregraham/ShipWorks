using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.BestRate
{
    public class UspsCounterRateAccountRepositoryTest
    {
        private UspsCounterRateAccountRepository testObject;

        private readonly Mock<ICredentialStore> credentialStore;

        public UspsCounterRateAccountRepositoryTest()
        {
            credentialStore = new Mock<ICredentialStore>();
            credentialStore.Setup(s => s.UspsUsername).Returns("UspsUser");
            credentialStore.Setup(s => s.UspsPassword).Returns("sampsPassword");

            testObject = new UspsCounterRateAccountRepository(credentialStore.Object);
        }

        [Fact]
        public void Accounts_ReturnsCollectionWithOneItem()
        {
            Assert.Equal(1, testObject.Accounts.Count());
        }

        [Fact]
        public void Accounts_DelegatesToCredentialStore_WhenAssigningUsername()
        {
            UspsAccountEntity account = testObject.Accounts.First();

            credentialStore.Verify(s => s.UspsUsername, Times.Once());
            Assert.Equal(credentialStore.Object.UspsUsername, account.Username);
        }

        [Fact]
        public void Accounts_DelegatesToCredentialStore_WhenAssigningPassword()
        {
            UspsAccountEntity account = testObject.Accounts.First();

            credentialStore.Verify(s => s.UspsPassword, Times.Once());
            Assert.Equal(credentialStore.Object.UspsPassword, account.Password);
        }
    }
}
