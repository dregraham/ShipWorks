using System;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Editions.Freemium;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Detailed information about a license retrieved from a customer's interapptive account
    /// </summary>
    public class LicenseAccountDetail : ILicenseAccountDetail
    {
        // The store
        StoreEntity store;

        // License key
        ShipWorksLicense license;

        // The edition of this license as configured by Tango
        Edition edition;

        // Identifier license is activated to
        string identifier;

        // Is the license active or not
        bool active = false;

        // If the license is canceled
        bool canceled = false;

        // Is the license even in the db
        bool valid = false;

        // If its deactivated (disabled), this is the reason why (for metered only)
        string disabledReason = "";

        // State of the license
        LicenseActivationState licenseState = LicenseActivationState.Invalid;

        /// <summary>
        /// Instantiate based on the interapptive XML response
        /// </summary>
        public LicenseAccountDetail(XmlDocument xmlResponse, StoreEntity store)
        {
            if (xmlResponse == null)
            {
                throw new ArgumentNullException("xmlResponse");
            }

            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;

            // Determine if an error occurred
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            // See if an error occurred
            string error = (string) xpath.Evaluate("string(//Error/Description)");

            // Throw the error if there was one
            if (error.Length > 0)
            {
                throw new ShipWorksLicenseException(error);
            }

            // Key
            license = new ShipWorksLicense(XPathUtility.Evaluate(xpath, "//Key", ""));

            // Machine
            identifier = XPathUtility.Evaluate(xpath, "//Machine", "");

            // Active
            active = XPathUtility.Evaluate(xpath, "//Active", false);

            // Cancelled (metered only)
            canceled = XPathUtility.Evaluate(xpath, "//Cancelled", false);

            // Disabled Reason (metered only)
            disabledReason = XPathUtility.Evaluate(xpath, "//DisabledReason", "");

            // Valid
            valid = XPathUtility.Evaluate(xpath, "//Valid", false);

            // State
            DetermineState();

            // Edition
            edition = DetermineEdition(xpath);

            // Get the Tango store ID
            TangoStoreID = XPathUtility.Evaluate(xpath, "//StoreID", "");

            // Get the Tango CustomerID
            TangoCustomerID = XPathUtility.Evaluate(xpath, "//CustomerID", "");

            // Get the trial status
            InTrial = XPathUtility.Evaluate(xpath, "//InTrial", false);

            // Get the trial end date
            RecurlyTrialEndDate = DateTime.TryParse(XPathUtility.Evaluate(xpath, "//RecurlyTrialEndDate", ""), out var trialEndDate)
                ? trialEndDate
                : DateTime.MinValue;

            UpsStatus = (UpsStatus) XPathUtility.Evaluate(xpath, "//UpsOnly/@status", (int) UpsStatus.None);
        }

        /// <summary>
        /// Determine the edition this license represents
        /// </summary>
        private Edition DetermineEdition(XPathNavigator xpath)
        {
            Edition edition = InstantiateEdition(xpath);

            bool dhlEcommerceMax = XPathUtility.Evaluate(xpath, "//DhlEcommerceSmParcelExpeditedMaxEnabled/@status", 0) == 1;
            edition.SharedOptions.DhlEcommerceSmParcelExpeditedMaxEnabled = dhlEcommerceMax;

            // Now see if there are Stamps special stuff set. We do this at the end, basically ignoring them if any other editions are active.
            bool stampsDhl = XPathUtility.Evaluate(xpath, "//StampsDhlEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsDhlEnabled = stampsDhl;

            // Now see if there are endicia special stuff set. We do this at the end, basically ignoring them if any other editions are active.
            bool endiciaDhl = XPathUtility.Evaluate(xpath, "//EndiciaDhlEnabled/@status", 0) == 1;
            edition.SharedOptions.EndiciaDhlEnabled = endiciaDhl;

            bool endiciaInsurance = XPathUtility.Evaluate(xpath, "//EndiciaInsuranceEnabled/@status", 0) == 1;
            edition.SharedOptions.EndiciaInsuranceEnabled = endiciaInsurance;

            // Check to see if SurePost is allowed
            bool upsSurePost = XPathUtility.Evaluate(xpath, "//UpsSurePostEnabled/@status", 0) == 1;
            edition.SharedOptions.UpsSurePostEnabled = upsSurePost;

            // Check if Endicia consolidation is allowed
            bool endiciaConsolidator = XPathUtility.Evaluate(xpath, "//EndiciaConsolidator/@status", 0) == 1;
            edition.SharedOptions.EndiciaConsolidatorEnabled = endiciaConsolidator;

            // Check if Endicia scan based payment returns is allowed
            bool endiciaScanBasedReturns = XPathUtility.Evaluate(xpath, "//EndiciaScanBasedReturns/@status", 0) == 1;
            edition.SharedOptions.EndiciaScanBasedReturnEnabled = endiciaScanBasedReturns;

            // Check if Stamps insurance is allowed
            bool stampsInsurance = XPathUtility.Evaluate(xpath, "//StampsInsuranceEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsInsuranceEnabled = stampsInsurance;

            // Add Stamps consolidators if Stamps consolidation is allowed
            AddStampsConsolidatorSharedOptions(xpath, edition);

            edition.ShipmentTypeFunctionality = ShipmentTypeFunctionality.Deserialize(store.StoreID, xpath);

            // Check if Warehouse is allowed
            bool warehouse = XPathUtility.Evaluate(xpath, "//WarehouseEnabled/@status", 0) == 1;
            edition.SharedOptions.WarehouseEnabled = warehouse;

            return edition;
        }

        /// <summary>
        /// Check if Stamps Ascendia consolidation is allowed
        /// </summary>
        private static void AddStampsConsolidatorSharedOptions(XPathNavigator xpath, Edition edition)
        {
            edition.SharedOptions.StampsAscendiaEnabled = XPathUtility.Evaluate(xpath, "//StampsAscendiaEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsDhlConsolidatorEnabled = XPathUtility.Evaluate(xpath, "//StampsDhlConsolidatorEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsGlobegisticsEnabled = XPathUtility.Evaluate(xpath, "//StampsGlobegisticsEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsIbcEnabled = XPathUtility.Evaluate(xpath, "//StampsIbcEnabled/@status", 0) == 1;
            edition.SharedOptions.StampsRrDonnelleyEnabled = XPathUtility.Evaluate(xpath, "//StampsRrDonnelleyEnabled/@status", 0) == 1;
        }

        /// <summary>
        /// Determine the edition this license represents
        /// </summary>
        private Edition InstantiateEdition(XPathNavigator xpath)
        {
            int freemiumStatus = XPathUtility.Evaluate(xpath, "//FreemiumAccount/@status", 0);
            if (freemiumStatus > 0)
            {
                // In Tango, 1 is Free, 2 is Upgraded (Paid)
                if (freemiumStatus == 1)
                {
                    string account = XPathUtility.Evaluate(xpath, "//FreemiumAccount", "");
                    FreemiumAccountType accountType = (FreemiumAccountType) XPathUtility.Evaluate(xpath, "//FreemiumAccount/@type", 0);

                    return new FreemiumFreeEdition(store, account, accountType);
                }
                else
                {
                    return new FreemiumPaidEdition(store);
                }
            }

            int upsStatus = XPathUtility.Evaluate(xpath, "//UpsOnly/@status", 0);
            if (upsStatus > 0)
            {
                BrownPostalAvailability postalAvailability = (BrownPostalAvailability) XPathUtility.Evaluate(xpath, "//UpsOnly/@postal", (int) BrownPostalAvailability.ApoFpoPobox);

                // In Tango, 1 is Discount, 2 is Subsidized, and 3 is tier1, 4 is tier2, 5 is tier3
                if (upsStatus == 1)
                {
                    return new BrownDiscountedEdition(store, postalAvailability);
                }
                else if (upsStatus == 2)
                {
                    string accounts = XPathUtility.Evaluate(xpath, "//UpsOnly", "");

                    return new BrownSubsidizedEdition(store, accounts.Split(';'), postalAvailability);
                }
                else if (upsStatus == 3 || upsStatus == 4 || upsStatus == 5)
                {
                    return new BrownCtp2014Edition(store, postalAvailability);
                }
            }

            int srEndiciaStatus = XPathUtility.Evaluate(xpath, "//SrEndicia/@status", 0);
            if (srEndiciaStatus > 0)
            {
                return new ShipRushEndiciaEdition(store);
            }

            return new Edition(store);
        }

        /// <summary>
        /// Determine the state of the license
        /// </summary>
        private void DetermineState()
        {
            // Invalid
            if (!valid)
            {
                licenseState = LicenseActivationState.Invalid;
            }
            else if (!license.IsMetered)
            {
                licenseState = LicenseActivationState.Invalid;
            }
            else
            {
                // Deactivated
                if (!active)
                {
                    // If canceled is true, then its canceled.
                    if (canceled)
                    {
                        licenseState = LicenseActivationState.Canceled;
                    }
                    // Otherwise its disabled\deactivated
                    else
                    {
                        licenseState = LicenseActivationState.Deactivated;
                    }
                }
                // Active
                else
                {
                    licenseState = LicenseActivationState.Active;
                }
            }
        }

        /// <summary>
        /// Readable description of the license status
        /// </summary>
        public string Description
        {
            get
            {
                if (!license.IsValid)
                {
                    return "Invalid license key.";
                }

                if (!license.IsMetered)
                {
                    return "Not valid for this version of ShipWorks.";
                }

                switch (ActivationState)
                {
                    case LicenseActivationState.Active:
                        return "Active";

                    case LicenseActivationState.ActiveNowhere:
                        return "Not Activated";

                    case LicenseActivationState.ActiveElsewhere:
                        return "Activated to another store.";

                    case LicenseActivationState.Deactivated:
                        return "Disabled: " + DisabledReason;

                    case LicenseActivationState.Canceled:
                        return "Cancelled";

                    case LicenseActivationState.Invalid:
                        return "Invalid License";

                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Get the current state of the license
        /// </summary>
        public LicenseActivationState ActivationState
        {
            get
            {
                return licenseState;
            }
        }

        /// <summary>
        /// The license
        /// </summary>
        public ShipWorksLicense License
        {
            get { return license; }
        }

        /// <summary>
        /// The current edition of the license as it is in Tango
        /// </summary>
        public IEdition Edition
        {
            get { return edition; }
        }

        /// <summary>
        /// License key
        /// </summary>
        public string Key
        {
            get
            {
                return license.Key;
            }
        }

        /// <summary>
        /// Identifier the license is registered to
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// If the license is active
        /// </summary>
        public bool Active
        {
            get
            {
                return active;
            }
        }

        /// <summary>
        /// If the license is in the interapptive db
        /// </summary>
        public bool Valid
        {
            get
            {
                return valid;
            }
        }

        /// <summary>
        /// If its deactivated (disabled), this is the reason why (for metered only)
        /// </summary>
        public string DisabledReason
        {
            get
            {
                return disabledReason;
            }
        }

        /// <summary>
        /// The Tango StoreID associated with this license
        /// </summary>
        public string TangoStoreID
        {
            get;
            private set;
        }

        /// <summary>
        /// The Tango CustomerID associated with this license
        /// </summary>
        public string TangoCustomerID
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether or not this license is in trial
        /// </summary>
        public bool InTrial { get; }

        /// <summary>
        /// The date that the recurly trial ends
        /// </summary>
        public DateTime RecurlyTrialEndDate { get; }

        /// <summary>
        /// How many days are left in the trial
        /// </summary>
        public int DaysLeftInTrial => (RecurlyTrialEndDate - DateTime.UtcNow).Days;

        /// <summary>
        /// Whether or not the trial is expired
        /// </summary>
        public bool TrialIsExpired => DaysLeftInTrial < 0;

        /// <summary>
        /// The UPS Status
        /// </summary>
        public UpsStatus UpsStatus { get; private set; }
    }
}
