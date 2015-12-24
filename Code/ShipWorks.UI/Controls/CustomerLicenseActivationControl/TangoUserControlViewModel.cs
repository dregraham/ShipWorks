using System.ComponentModel;
using System.Reflection;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// View model for the TangoUserControl
    /// </summary>
    public class CustomerLicenseActivationViewModel
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(ITangoWebClient tangoWebClient)
        {
            this.tangoWebClient = tangoWebClient;
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
        /// <returns></returns>
        public string Save()
        {
            return $"yup that works {Username} {Password}";
        }
    }
}