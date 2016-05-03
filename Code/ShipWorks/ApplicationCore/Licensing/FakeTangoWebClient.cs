using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
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
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The '{0}' contract type was logged to Tango.  Not really, but just play along.", EnumHelper.GetDescription((UspsResellerType)account.UspsReseller));
        }


        string UpsRestriction = @"        
            <Feature>
				<Type>BestRateUpsRestriction</Type>
				<Config>False</Config>
			</Feature>";
        private static string QtyRestriction = @"
            <Feature>
				<Type>RateResultCount</Type>
				<Config>5</Config>
			</Feature>";

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public override LicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store)
        {
            ShipWorksLicense license = new ShipWorksLicense(licenseKey);
            string rawXml = GetLicenseXmlFromFile("C:\\Temp\\License.xml", GenerateDummyLicenseXml());

            XmlDocument licenseXml = new XmlDocument();
            licenseXml.LoadXml(string.Format(rawXml, license.Key, StoreTypeManager.GetType(store).LicenseIdentifier));

            return new LicenseAccountDetail(licenseXml, store);
        }

        /// <summary>
        /// Request a trial for use with the specified store. If a trial already exists, a new one will not be created.
        /// </summary>
        public override TrialDetail GetTrial(StoreEntity store)
        {
            string rawXml = GetLicenseXmlFromFile("C:\\Temp\\Trial.xml", GenerateDummyTrialLicenseXml());

            XmlDocument trialXml = new XmlDocument();
            trialXml.LoadXml(string.Format(rawXml, store.License));

            return new TrialDetail(trialXml, store);
        }

        /// <summary>
        /// Reads a file from the the given file path for a license information.
        /// </summary>
        /// <param name="path">The file path to read from.</param>
        /// <param name="defaultXml">The XML to use if there is a problem reading from the given file path.</param>
        /// <returns>The contents of the file.</returns>
        private static string GetLicenseXmlFromFile(string path, string defaultXml)
        {
            string rawXml = string.Empty;

            try
            {
                using (StreamReader licenseFile = new StreamReader(path))
                {
                    rawXml = licenseFile.ReadToEnd();
                    licenseFile.Close();
                }
            }
            catch (IOException)
            {
                // Fall back to the hard-coded values if there is a problem reading from the 
                // license.xml file
                rawXml = defaultXml;
            }

            return rawXml;
        }

        /// <summary>
        /// Generates dummy license XML that can be used in the event that the 
        /// License.xml file cannot be read from.
        /// </summary>
        /// <returns>Fake license information.</returns>
        private string GenerateDummyLicenseXml()
        {
            return @"<License>
	<Key>{0}</Key>
	<Machine>{1}</Machine>
	<Active>true</Active>
	<Cancelled>false</Cancelled>
	<DisabledReason/>
	<Valid>true</Valid>
	<StoreID>12024</StoreID>
	<CustomerID>54</CustomerID>
	<Version>Checked</Version>					
	<AlphaBeta>true</AlphaBeta>
	<EndiciaDhlEnabled status='1'/>
	<EndiciaInsuranceEnabled status='1'/>
	<UpsSurePostEnabled status='1'/>
	<EndiciaConsolidator status='1'>APC</EndiciaConsolidator>
	<EndiciaScanBasedReturns status='1'/>
    <ShipmentTypeFunctionality>
        <ShipmentType TypeCode='14'>
            <Restriction>Disabled</Restriction>
            " + UpsRestriction + @"
	        " + QtyRestriction + @"
		</ShipmentType>
        <!-- This is the USPS shipment type. Testing to confirm that the feature settings are ignored. -->
		<ShipmentType TypeCode='15'>
			<Feature>
				<Type>BestRateUpsRestriction</Type>
				<Config>False</Config>
			</Feature>
			<Feature>
				<Type>RateResultCount</Type>
				<Config>5</Config>
			</Feature>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>";
        }

        /// <summary>
        /// Generates the dummy trial license XML in the event that the Trial.xml file cannot be read from.
        /// </summary>
        /// <returns>Fake license info for a trial store.</returns>
        private static string GenerateDummyTrialLicenseXml()
        {
            return @"<License>
	<Key>{0}</Key>
	<Created>2012-11-02 14:53:09</Created>
	<Expires>2014-03-27 12:43:44</Expires>
	<Converted>true</Converted>
	<CanExtend>false</CanExtend>
	<ServerTime>2015-01-22 20:36:15</ServerTime>
	<Edition/>
	<ShipmentTypeFunctionality>
		<ShipmentType TypeCode='2'>
			<Restriction>AccountRegistration</Restriction>
		</ShipmentType>
		<ShipmentType TypeCode='14'>
            <Feature>
				<Type>BestRateUpsRestriction</Type>
				<Config>False</Config>
			</Feature>
			<Feature>
				<Type>RateResultCount</Type>
				<Config>5</Config>
			</Feature>
		</ShipmentType>
	</ShipmentTypeFunctionality>
</License>";
        }
    }
}
