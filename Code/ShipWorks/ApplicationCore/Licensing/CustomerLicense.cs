using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class to store customer license information
    /// </summary>
    public class CustomerLicense : ICustomerLicense, ILicense
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly ICustomerLicenseWriter licenseWriter;
        private readonly ICustomerLicenseReader licenseReader;
        private readonly ILog log;

        public CustomerLicense(ITangoWebClient tangoWebClient, ICustomerLicenseWriter licenseWriter,
            ICustomerLicenseReader licenseReader) :
                this(tangoWebClient, licenseWriter, licenseReader, LogManager.GetLogger(typeof(CustomerLicense)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicense(ITangoWebClient tangoWebClient, ICustomerLicenseWriter licenseWriter, ICustomerLicenseReader licenseReader, ILog log)
        {
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
            this.licenseReader = licenseReader;
        }

        /// <summary>
        /// Activates the customer license
        /// </summary>
        public void Activate(string email, string password)
        {
            // Activate the license via tango using the given username and password
            GenericResult<ActivationResponse> activationResponse = tangoWebClient.ActivateLicense(email, password);

            // Check to see if something went wrong and if so we throw
            if (!activationResponse.Success)
            {
                throw new ShipWorksLicenseException(activationResponse.Message);
            }

            Key = activationResponse.Context.Key;

            // Save license data to the data source
            Save();
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

        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        public bool IsDisabled => (DisabledReason != string.Empty);
    }
}