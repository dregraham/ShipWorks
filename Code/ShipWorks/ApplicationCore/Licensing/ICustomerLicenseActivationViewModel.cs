using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interface for the CustomerLicenseActivationViewModel
    /// </summary>
    public interface ICustomerLicenseActivationViewModel
    {
        /// <summary>
        /// The Password 
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// The Username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Called to save the credentials
        /// </summary>
        UserEntity Save();
    }
}