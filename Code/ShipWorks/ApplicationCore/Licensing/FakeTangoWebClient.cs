using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// A fake web client for internal testing purposes to simulate calls to Tango that may not yet be
    /// implemented on the Tango side. This is just to streamline the development side of things on the
    /// ShipWorks side without having to mess with Fiddler and all of the certificate inspection that
    /// goes along with it when trying to setup specific test cases.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class FakeTangoWebClient : TangoWebClientWrapper, ITangoWebClient
    {
        private const string CustomizedTangoFilesKeyName = "TangoWebClientDataPath";
        ILog log = LogManager.GetLogger(typeof(FakeTangoWebClient));

        /// <summary>
        /// Log the given processed shipment to Tango.  isRetry is only for internal interapptive purposes to handle rare cases where shipments a customer
        /// insured did not make it up into tango, but the shipment did actually process.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string LogShipment(StoreEntity store, ShipmentEntity shipment, bool isRetry = false)
        {
            log.Fatal($"Shipment logged to Tango for shipment id: {shipment.ShipmentID}");
            return Guid.NewGuid().ToString("D");
        }

        /// <summary>
        /// Void the given processed shipment to Tango
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override void VoidShipment(StoreEntity store, ShipmentEntity shipment)
        {
            log.Fatal($"Shipment voided in Tango for shipment id: {shipment.ShipmentID}");
        }

        /// <summary>
        /// Gets the nudges.
        /// </summary>
        /// <returns>A couple of fake nudges for testing purposes.</returns>
        [Obfuscation(Exclude = true)]
        public override IEnumerable<Nudge> GetNudges(IEnumerable<StoreEntity> stores)
        {
            // Build up a couple of dummy nudges for testing purposes. Null is being configured as the INudgeAction 
            // until the actual implementations are ready. Null is a good test to ensure that this is accounted 
            return Enumerable.Empty<Nudge>();
        }

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override void LogNudgeOption(NudgeOption option)
        {
            // Just log the option that was selected to disk to simulate a call to Tango
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The '{0}' option result was selected for nudge ID {1}", option.Result, option.Owner.NudgeID);
        }

        /// <summary>
        /// Sends USPS account info to Tango.
        /// </summary>
        /// <param name="account">The account.</param>
        [Obfuscation(Exclude = true)]
        public override void LogUspsAccount(UspsAccountEntity account)
        {
            // Just log the account contract type to disk to simulate a call to Tango
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The '{0}' contract type was logged to Tango.  Not really, but just play along.", EnumHelper.GetDescription((UspsResellerType) account.UspsReseller));
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public override ILicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store)
        {
            XmlDocument licenseXml = GetXmlDocumentFromFile("License.xml",
                new ShipWorksLicense(licenseKey).Key,
                StoreTypeManager.GetType(store).LicenseIdentifier);

            return new LicenseAccountDetail(licenseXml, store);
        }

        /// <summary>
        /// Request a trial for use with the specified store. If a trial already exists, a new one will not be created.
        /// </summary>
        public override TrialDetail GetTrial(StoreEntity store)
        {
            XmlDocument trialXml = GetXmlDocumentFromFile("Trial.xml", store.License);

            return new TrialDetail(trialXml, store);
        }

        /// <summary>
        /// Gets the license capabilities.
        /// </summary>
        public override ILicenseCapabilities GetLicenseCapabilities(ICustomerLicense license)
        {
            XmlDocument xmlResponse = GetXmlDocumentFromFile("LicenseCapabilities.xml", string.Empty);

            try
            {
                return new LicenseCapabilities(xmlResponse);
            }
            catch (ShipWorksLicenseException ex)
            {
                throw new TangoException(ex);
            }
        }

        /// <summary>
        /// Gets the active stores from Tango.
        /// </summary>
        public override IEnumerable<ActiveStore> GetActiveStores(ICustomerLicense license)
        {
            XmlDocument xmlResponse = GetXmlDocumentFromFile("ActiveStores.xml", string.Empty);

            return new GetActiveStoresResponse(xmlResponse).ActiveStores;
        }

        /// <summary>
        /// Get an xml document from the given file
        /// </summary>
        private static XmlDocument GetXmlDocumentFromFile(string fileName, params object[] args)
        {
            string rawXml = GetXmlStringFromFile(fileName);

            XmlDocument document = new XmlDocument();
            document.LoadXml(string.Format(rawXml, args));
            return document;
        }

        /// <summary>
        /// Get an xml string from the given file
        /// </summary>
        private static string GetXmlStringFromFile(string fileName)
        {
            string filePath = InterapptiveOnly.Registry.GetValue(CustomizedTangoFilesKeyName, @"C:\Temp");
            string fullFileName = Path.Combine(filePath, fileName);

            try
            {
                using (StreamReader licenseFile = new StreamReader(fullFileName))
                {
                    return licenseFile.ReadToEnd();
                }
            }
            catch (IOException)
            {
                // Fall back to the hard-coded values if there is a problem reading from the
                // license.xml file
                return null;
            }
        }
    }
}
