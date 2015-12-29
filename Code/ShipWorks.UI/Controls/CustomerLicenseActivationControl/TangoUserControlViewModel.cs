using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Users;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// View model for the TangoUserControl
    /// </summary>
    public class CustomerLicenseActivationViewModel
    {
        private readonly ICustomerLicense customerLicense;
        private readonly PropertyChangedHandler Handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string username;
        private string password;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseActivationViewModel(ICustomerLicense customerLicense)
        {
            this.customerLicense = customerLicense;
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
        public string Save(SqlSession sqlSession)
        {
            Cursor.Current = Cursors.WaitCursor;
            
            // Activate the software using the given username/password
            try
            {
                customerLicense.Activate(Username, Password);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            try
            {
                using (new SqlSessionScope(sqlSession))
                {
                    UserUtility.CreateUser(Username, Password, Password, true);
                }
            }
            catch (DuplicateNameException ex)
            {
                return ex.Message;
            }

            // nothing went wrong so we return an empty string
            return string.Empty;
        }
    }
}