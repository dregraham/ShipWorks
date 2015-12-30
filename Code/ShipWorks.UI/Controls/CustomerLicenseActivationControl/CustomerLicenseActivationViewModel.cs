using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.UI;
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
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(ICustomerLicense customerLicense, IUserManagerWrapper userManager, IMessageHelper messageHelper)
        {
            this.customerLicense = customerLicense;
            this.userManager = userManager;
            this.messageHelper = messageHelper;
            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The username
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Username
        {
            get { return username; }
            set { Handler.Set(nameof(Username), ref username, value); }
        }

        /// <summary>
        /// The password
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Password
        {
            get { return password; }
            set { Handler.Set(nameof(Password), ref password, value); }
        }

        /// <summary>
        /// Saves the user to the database
        /// </summary>
        public UserEntity Save()
        {
            UserEntity user = null;
            if (ValidateUser())
            {
                // Activate the software using the given username/password
                customerLicense.Activate(Username, Password);
                
                try
                {
                    user = userManager.CreateUser(Username, Password, true);
                }
                catch (Exception ex)
                {
                    messageHelper.ShowError(ex.Message);
                }
            }

            return user;
        }

        private bool ValidateUser()
        {
            // Validate the username
            if (!EmailUtility.IsValidEmailAddress(Username))
            {
                messageHelper.ShowError("Please enter a valid username.");
                return false;
            }

            // Validate the password
            if (string.IsNullOrWhiteSpace(Password))
            {
                messageHelper.ShowError("Please enter a password.");
                return false;
            }

            return true;
        }
    }
}