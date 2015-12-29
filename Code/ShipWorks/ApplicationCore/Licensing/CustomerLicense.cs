namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class to store customer license information
    /// </summary>
    public class CustomerLicense : ICustomerLicense
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly ICustomerLicenseWriter licenseWriter;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicense(ITangoWebClient tangoWebClient, ICustomerLicenseWriter licenseWriter)
        {
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
        }

        /// <summary>
        /// Activates the customer license
        /// </summary>
        public ICustomerLicense Activate(string email, string password)
        {
            // Activate the license via tango using the given username and password
            ActivationResponse activateionResponse = tangoWebClient.ActivateLicense(email, password);
            Key = activateionResponse.Key;

            // Save license data to the data source
            Save();
            return this;
        }

        /// <summary>
        /// Uses the ILicenseWriter provided in the constructor to save this instance
        /// to a data source.
        /// </summary>
        private void Save()
        {
            licenseWriter.Write(this);
        }

        /// <summary>
        /// The license key
        /// </summary>
        public string Key { get; set; }
    }
}