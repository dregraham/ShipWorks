using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Account
{
    /// <summary>
    /// Data structure for new endicia accounts credentials
    /// </summary>
    public class EndiciaNewAccountCredentials
    {
        private readonly string webPassword;
        private readonly string passPhrase;
        private readonly string challengeQuestion;
        private readonly string challengeAnswer;
        private readonly string temporaryPassPhrase;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaNewAccountCredentials(string webPassword, string passPhrase, string challengeQuestion, string challengeAnswer)
        {
            this.webPassword = webPassword;
            this.passPhrase = passPhrase;
            this.challengeQuestion = challengeQuestion;
            this.challengeAnswer = challengeAnswer;
            temporaryPassPhrase = $"{passPhrase}_Initial";
        }

        /// <summary>
        /// Populates account entity with encrypted passwords
        /// </summary>
        public void PopulateAccountEntity(EndiciaAccountEntity account)
        {
            account.WebPassword = SecureText.Encrypt(webPassword, "Endicia");
            account.ApiInitialPassword = SecureText.Encrypt(temporaryPassPhrase, "Endicia");
            account.ApiUserPassword = SecureText.Encrypt(passPhrase, "Endicia");
        }

        /// <summary>
        /// Returns account credentials to use when creating an Endicia account
        /// </summary>
        public AccountCredentials GetApiAccountCredentials()
        {
            return new AccountCredentials()
            {
                SecurityQuestion = challengeQuestion,
                SecurityAnswer = challengeAnswer,
                TemporaryPassPhrase = temporaryPassPhrase,
                WebPassword = webPassword
            };
        }
    }
}
