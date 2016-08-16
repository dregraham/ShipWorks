using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelCredentials
    {
        private readonly IiParcelServiceGateway serviceGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelCredentials" /> class.
        /// </summary>
        /// <param name="serviceGateway">The service gateway.</param>
        public iParcelCredentials(IiParcelServiceGateway serviceGateway)
            : this(string.Empty, string.Empty, false, serviceGateway)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelCredentials" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isPasswordEncrypted">if set to <c>true</c> [password is encrypted].</param>
        /// <param name="serviceGateway">The service gateway used to validate credentials with i-parcel.</param>
        public iParcelCredentials(string username, string password, bool isPasswordEncrypted, IiParcelServiceGateway serviceGateway)
        {
            Username = username;
            Password = password;
            IsPasswordEncrypted = isPasswordEncrypted;

            this.serviceGateway = serviceGateway;
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; private set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        private string Password { get; set; }


        /// <summary>
        /// Gets a value indicating whether this instance's password is encrypted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance's password is encrypted; otherwise, <c>false</c>.
        /// </value>
        private bool IsPasswordEncrypted { get; set; }

        /// <summary>
        /// Gets the encrypted password.
        /// </summary>
        /// <value>The encrypted password.</value>
        public string EncryptedPassword
        {
            get
            {
                string encryptedPassword = Password;

                if (!IsPasswordEncrypted)
                {
                    encryptedPassword = SecureText.Encrypt(Password, Username);
                }

                return encryptedPassword;
            }
        }

        /// <summary>
        /// Gets the decrypted password.
        /// </summary>
        /// <value>The decrypted password.</value>
        public string DecryptedPassword
        {
            get
            {
                string decryptedPassword = Password;

                if (IsPasswordEncrypted)
                {
                    decryptedPassword = SecureText.Decrypt(Password, Username);
                }

                return decryptedPassword;
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="iParcelException">Thrown when the username or password is null/empty or when
        /// the credentials do not validate against the i-parcel service.</exception>
        public void Validate()
        {
            if (string.IsNullOrEmpty(Username))
            {
                throw new iParcelException("Please enter your i-parcel username.");
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new iParcelException("Please enter your i-parcel password.");
            }

            if (serviceGateway != null)
            {
                // Validate the username and password against the i-parcel service
                if (!serviceGateway.IsValidUser(this))
                {
                    throw new iParcelException("An invalid i-parcel username or password was provided.");
                }
            }
        }

        /// <summary>
        /// Saves to entity. An iParcelException is thrown if the credentials do not pass validation.
        /// </summary>
        /// <param name="account">The account.</param>
        public void SaveToEntity(IParcelAccountEntity account)
        {
            // Ensures that validation occurs even if consumers don't validate
            Validate();

            account.Username = Username;
            account.Password = EncryptedPassword;
        }
    }
}
