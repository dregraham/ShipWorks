using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Email;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls.CustomerLicenseActivation
{
    /// <summary>
    /// View model for the TangoUserControl
    /// </summary>
    public class CustomerLicenseActivationViewModel : ICustomerLicenseActivationViewModel
    {
        private readonly ICustomerLicense customerLicense;
        private readonly IUserService userManager;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string email;
        private SecureString password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(Func<string, ICustomerLicense> customerLicenseFactory, IUserService userManager)
        {
            this.customerLicense = customerLicenseFactory(string.Empty);
            this.userManager = userManager;
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
        /// Saves the user to the database
        /// </summary>
        public GenericResult<ICustomerLicense> Save()
        {
            // Create an empty result
            GenericResult<ICustomerLicense> result = ValidateCredentials();
            
            // if the username and password passed our data validation 
            // call activate and create the user
            if (result.Success)
            {
                try
                {
                    // Activate the software using the given username/password
                    customerLicense.Activate(Email, DecryptedPassword);
                    userManager.CreateUser(Email, DecryptedPassword, true);
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.Success = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Validates that the credentials
        /// </summary>
        private GenericResult<ICustomerLicense> ValidateCredentials()
        {
            GenericResult<ICustomerLicense> result = new GenericResult<ICustomerLicense>(customerLicense)
            {
                Success = true,
                Message = string.Empty
            };

            // Validate the username
            if (!EmailUtility.IsValidEmailAddress(Email))
            {
                result.Message = "Please enter a valid email for the username.";
                result.Success = false;
                return result;
            }

            // Validate the password
            if (string.IsNullOrWhiteSpace(DecryptedPassword))
            {
                result.Message = "Please enter a password.";
                result.Success = false;
            }

            return result;
        }
    }
}