using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Email;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// View model for the TangoUserControl
    /// </summary>
    public class CustomerLicenseActivationViewModel : ICustomerLicenseActivationViewModel
    {
        private readonly ICustomerLicenseActivationService activationService;
        private readonly IUserService userManager;
        private readonly IUspsAccountManager uspsAccountManager;
        private readonly IShippingSettings shippingSettings;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string email;
        private SecureString password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(ICustomerLicenseActivationService activationService, IUserService userManager, IUspsAccountManager uspsAccountManager, IShippingSettings shippingSettings)
        {
            this.activationService = activationService;
            this.userManager = userManager;
            this.uspsAccountManager = uspsAccountManager;
            this.shippingSettings = shippingSettings;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The username
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Email
        {
            get { return email; }
            set { handler.Set(nameof(Email), ref email, value); }
        }

        /// <summary>
        /// The password
        /// </summary>
        [Obfuscation(Exclude = true)]
        public SecureString Password
        {
            get { return password; }
            set { handler.Set(nameof(Password), ref password, value); }
        }

        /// <summary>
        /// The decrypted password
        /// </summary>
        /// <remarks>
        /// We have to do this because the PasswordBox control in XAML
        /// does not give us access to the plain text password
        /// </remarks>
        public string DecryptedPassword
        {
            get
            {
                string insecurePassword;

                try
                {
                    IntPtr passwordBSTR = Marshal.SecureStringToBSTR(password);
                    insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);
                }
                catch
                {
                    insecurePassword = string.Empty;
                }

                return insecurePassword;
            }
        }

        /// <summary>
        /// The password reset link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string PasswordResetLink =>
            "https://www.interapptive.com/account/forgotpassword.php";

        /// <summary>
        /// The account creation link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CreateAccountLink =>
            "http://www.shipworks.com/registration/?source=si10883693";

        /// <summary>
        /// Saves the user to the database
        /// </summary>
        public GenericResult<ICustomerLicense> Save(bool createCustomer)
        {
            GenericResult<ICustomerLicense> result = ValidateCredentials();

            // if the username and password passed our data validation
            // call activate and create the user
            if (result.Success)
            {
                try
                {
                    uspsAccountManager.InitializeForCurrentSession();
                    shippingSettings.InitializeForCurrentDatabase();

                    ICustomerLicense customerLicense = activationService.Activate(email, DecryptedPassword);

                    if (createCustomer)
                    {
                        userManager.CreateUser(Email, DecryptedPassword, true);
                    }

                    result = GenericResult.FromSuccess(customerLicense);
                }
                catch (Exception ex)
                {
                    result = GenericResult.FromError<ICustomerLicense>(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Validates that the credentials
        /// </summary>
        /// <remarks>
        /// The context of the returning Generic Result will be null
        /// as we don't have a context yet.  If the result is successful,
        /// we are ready to download
        /// </remarks>
        private GenericResult<ICustomerLicense> ValidateCredentials()
        {
            // Validate the username
            if (!EmailUtility.IsValidEmailAddress(Email))
            {
                return GenericResult.FromError<ICustomerLicense>("Please enter a valid email for the username.");
            }

            // Validate the password
            if (string.IsNullOrWhiteSpace(DecryptedPassword))
            {
                return GenericResult.FromError<ICustomerLicense>("Please enter a password.");
            }

            // we don't have a license yet...
            return GenericResult.FromSuccess<ICustomerLicense>(null);
        }
    }
}