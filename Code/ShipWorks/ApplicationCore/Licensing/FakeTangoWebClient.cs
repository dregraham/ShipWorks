using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// A fake web client for internal testing purposes to simulate calls to Tango that may not yet be 
    /// implemented on the Tango side. This is just to streamline the development side of things on the
    /// ShipWorks side without having to mess with Fiddler and all of the certificate inspection that
    /// goes along with it when trying to setup specific test cases.
    /// </summary>
    public class FakeTangoWebClient : TangoWebClientWrapper, ITangoWebClient
    {
        /// <summary>
        /// Gets the nudges.
        /// </summary>
        /// <returns>A couple of fake nudges for testing purposes.</returns>
        public override IEnumerable<Nudge> GetNudges(IEnumerable<StoreEntity> stores)
        {
            // Build up a couple of dummy nudges for testing purposes. Null is being configured as the INudgeAction 
            // until the actual implementations are ready. Null is a good test to ensure that this is accounted 
            // for, however.
            List<Nudge> nudges = new List<Nudge>
            {
                new Nudge(1, "Nudge 1", NudgeType.ShipWorksUpgrade, new Uri("http://www.shipworks.com"), new Size(625, 575)),
                new Nudge(2, "Nudge 2", NudgeType.ShipWorksUpgrade, new Uri("http://www.google.com"), new Size(300, 500)),
                new Nudge(3, "Nudge 3", NudgeType.RegisterStampsAccount, new Uri("http://www.bing.com"), new Size(400, 600)),
                new Nudge(4, "Nudge 4", NudgeType.ProcessEndicia, new Uri("http://www.endicia.com"), new Size(400, 600)),
                new Nudge(5, "Nudge 5", NudgeType.PurchaseEndicia, new Uri("http://www.endicia.com"), new Size(400, 600)),
            };

            // Add a couple of options to the first nudge
            nudges[0].AddNudgeOption(new NudgeOption(3, 0, "OK", nudges[0], NudgeOptionActionType.None));
            nudges[0].AddNudgeOption(new NudgeOption(2, 1, "Close ShipWorks", nudges[0], NudgeOptionActionType.Shutdown));

            // Add one option to the second nudge in the list
            nudges[1].AddNudgeOption(new NudgeOption(3, 0, "Close", nudges[1], NudgeOptionActionType.None));

            // Add one option to the third nudge in the list
            nudges[2].AddNudgeOption(new NudgeOption(4, 0, "Close", nudges[1], NudgeOptionActionType.None));
            nudges[2].AddNudgeOption(new NudgeOption(5, 1, "Register Stamps Account", nudges[1], NudgeOptionActionType.RegisterStampsAccount));

            nudges[3].AddNudgeOption(new NudgeOption(4, 0, "OK", nudges[3], NudgeOptionActionType.None));
            nudges[4].AddNudgeOption(new NudgeOption(5, 1, "OK", nudges[4], NudgeOptionActionType.None));

            return nudges;
        }

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        public override void LogNudgeOption(NudgeOption option)
        {
            // Just log the option that was selected to disk to simulate a call to Tango
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The '{0}' option result was selected for nudge ID {1}", option.Result, option.Owner.NudgeID);
        }

        /// <summary>
        /// Sends Stamps.com account info to Tango.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void LogStampsAccount(UspsAccountEntity account)
        {
            // Just log the account contract type to disk to simulate a call to Tango
            LogManager.GetLogger(typeof(FakeTangoWebClient)).InfoFormat("The '{0}' contract type was logged to Tango.  Not really, but just play along.", EnumHelper.GetDescription((StampsResellerType)account.UspsReseller));
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
