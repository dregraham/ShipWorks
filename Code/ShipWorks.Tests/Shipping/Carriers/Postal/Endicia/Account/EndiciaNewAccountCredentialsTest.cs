using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Account
{
    public class EndiciaNewAccountCredentialsTest
    {
        [Fact]
        public void PopulateAccountEntity_PasswordsArePopulated()
        {
            var testObject = new EndiciaNewAccountCredentials("webPassword", "passPhrase", "question", "answer");

            EndiciaAccountEntity endiciaAccountEntity = new EndiciaAccountEntity();
            testObject.PopulateAccountEntity(endiciaAccountEntity);

            Assert.Equal(SecureText.Encrypt("webPassword", "Endicia"), endiciaAccountEntity.WebPassword);
            Assert.Equal(SecureText.Encrypt("passPhrase_Initial", "Endicia"), endiciaAccountEntity.ApiInitialPassword);
            Assert.Equal(SecureText.Encrypt("passPhrase", "Endicia"), endiciaAccountEntity.ApiUserPassword);
        }

        [Fact]
        public void GetApiAccountCredentials_CredentialsAreCorrect()
        {
            var testObject = new EndiciaNewAccountCredentials("webPassword", "passPhrase", "question", "answer");
            var apiCredentials = testObject.GetApiAccountCredentials();

            Assert.Equal("question", apiCredentials.SecurityQuestion);
            Assert.Equal("answer", apiCredentials.SecurityAnswer);
            Assert.Equal("passPhrase_Initial", apiCredentials.TemporaryPassPhrase);
            Assert.Equal("webPassword", apiCredentials.WebPassword);
        }
    }
}