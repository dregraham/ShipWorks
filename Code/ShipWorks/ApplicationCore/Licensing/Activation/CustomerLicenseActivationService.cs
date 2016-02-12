namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// This acts as a facade/entry point for activating a customer license. The various activities in the
    /// workflow required to activate a license are managed/coordinated here.
    /// </summary>
    public class CustomerLicenseActivationService : ICustomerLicenseActivationService
    {   
        private readonly ICustomerLicenseActivationActivity licenseActivationActivity;
        private readonly IUspsAccountSetupActivity uspsAccountSetupActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerLicenseActivationService"/> class.
        /// </summary>
        /// <param name="uspsAccountSetupActivity"></param>
        /// <param name="licenseActivationActivity"></param>
        public CustomerLicenseActivationService(ICustomerLicenseActivationActivity licenseActivationActivity, 
            IUspsAccountSetupActivity uspsAccountSetupActivity)
        {
            this.licenseActivationActivity = licenseActivationActivity;
            this.uspsAccountSetupActivity = uspsAccountSetupActivity;
        }

        /// <summary>
        /// Attempts to use the email address and password provided to activate a customer license with Tango.
        /// </summary>
        public ICustomerLicense Activate(string email, string password)
        {
            ICustomerLicense license = licenseActivationActivity.Execute(email, password);
            uspsAccountSetupActivity.Execute(license.AssociatedStampsUsername, password);
            
            return license;
        }
    }
}