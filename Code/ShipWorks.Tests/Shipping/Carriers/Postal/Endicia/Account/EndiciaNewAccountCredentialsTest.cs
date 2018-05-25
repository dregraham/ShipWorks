using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Account
{
    public class EndiciaNewAccountCredentialsTest
    {

        [Theory]
        [InlineData(null, "passPhrase", "question", "answer", "internet password")]
        [InlineData("1234", "passPhrase", "question", "answer", "internet password")]

        [InlineData("webPassword", null, "question", "answer", "software password")]
        [InlineData("webPassword", "1234", "question", "answer", "software password")]

        [InlineData("webPassword", "passPhrase", null, "answer", "challenge question")]
        [InlineData("webPassword", "passPhrase", "1234", "answer", "challenge question")]

        [InlineData("webPassword", "passPhrase", "question", null, "challenge answer")]
        [InlineData("webPassword", "passPhrase", "question", "1234", "challenge answer")]
        public void Constructor_ThrowsException_WhenInputDataInvalid(string webPassword,
            string passPhrase,
            string challengeQuestion,
            string challengeAnswer,
            string errorPhrase)
        {
            var exception = Assert.Throws<EndiciaException>(()=>new EndiciaNewAccountCredentials(webPassword, passPhrase, challengeQuestion, challengeAnswer));
            Assert.Equal($"Your {errorPhrase} must be at least 5 characters.", exception.Message);
        }

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