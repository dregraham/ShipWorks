using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// View model for the TangoUserControl
    /// </summary>
    public class CustomerLicenseActivationViewModel : ICustomerLicenseActivationViewModel
    {
        private readonly ICustomerLicense customerLicense;
        private readonly IUserManagerWrapper userManager;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private SecureString password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(ICustomerLicense customerLicense, IUserManagerWrapper userManager)
        {
            this.customerLicense = customerLicense;
            this.userManager = userManager;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The username
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Username
        {
            get { return username; }
            set { handler.Set(nameof(Username), ref username, value); }
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
        /// Saves the user to the database
        /// </summary>
        public GenericResult<UserEntity> Save()
        {
            GenericResult<UserEntity> result = ValidateUser();

            if (result.Success)
            {
                // Activate the software using the given username/password
                customerLicense.Activate(Username, DecryptedPassword);

                try
                {
                    result.Context = userManager.CreateUser(Username, DecryptedPassword, true);
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }
            }

            return result;
        }

        /// <summary>
        /// Validate the username is an email and password is not blank
        /// </summary>
        /// <returns></returns>
        private GenericResult<UserEntity> ValidateUser()
        {
            GenericResult<UserEntity> result = new GenericResult<UserEntity>(null)
            {
                Message = string.Empty,
                Success = true
            };

            // Validate the username
            if (!EmailUtility.IsValidEmailAddress(Username))
            {
                result.Message = "Please enter a valid username.";
                result.Success = false;
            }

            // Validate the password
            if (string.IsNullOrWhiteSpace(DecryptedPassword))
            {
                result.Message = "Please enter a password.";
                result.Success = false;
            }

            return result;
        }

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
    }
}