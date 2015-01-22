using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Editions.Freemium;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Detailed information about a ShipWorks trial
    /// </summary>
    public class TrialDetail
    {
        // The store
        StoreEntity store;

        // License
        ShipWorksLicense license;

        // The edition of this trial as configured by Tango
        Edition edition;

        // Trial started
        DateTime started;

        // Trial expires
        DateTime expires;

        // Time on the server
        DateTime serverTime;

        // True if the customer can extend the trial
        bool canExtend = false;

        // True if the trial has been converted to a real license
        bool converted = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public TrialDetail(XmlDocument xmlResponse, StoreEntity store)
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

            if (license.StoreTypeCode != StoreTypeCode.Invalid &&
                license.StoreTypeCode != (StoreTypeCode) store.TypeCode)
            {
                throw new ShipWorksLicenseException("The license does match the store type.");
            }

            // Created
            started = DateTime.Parse(XPathUtility.Evaluate(xpath, "//Created", ""));

            // Expires
            expires = DateTime.Parse(XPathUtility.Evaluate(xpath, "//Expires", ""));

            // Server time 
            serverTime = DateTime.Parse(XPathUtility.Evaluate(xpath, "//ServerTime", ""));

            // Customer can extend the trial
            canExtend = XPathUtility.Evaluate(xpath, "//CanExtend", false);

            // The trial has been converted into a real license
            converted = XPathUtility.Evaluate(xpath, "//Converted", false);

            // Default to standard edition
            edition = new Edition(store);
            
            // Populate the shipment type functionality (restrictions)
            edition.ShipmentTypeFunctionality = ShipmentTypeFunctionality.Deserialize(store.StoreID, xpath);

            // Then see if there is an edition set in tango
            string editionType = XPathUtility.Evaluate(xpath, "//Edition", "");
            if (editionType == EnumHelper.GetDescription(EditionInstalledType.UpsDiscounted))
            {
                // Brown-only trials will always be postal restricted - to get out of it, they'll need to call us and have their trial reverted back to full.
                edition = new BrownDiscountedEdition(store, BrownPostalAvailability.ApoFpoPobox);
            }
            if (editionType == EnumHelper.GetDescription(EditionInstalledType.EndiciaEbay))
            {
                edition = new FreemiumFreeEdition(store, "", FreemiumAccountType.None);
            }
        }

        /// <summary>
        /// The store the trial is for
        /// </summary>
        public StoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// The license created for the trial
        /// </summary>
        public ShipWorksLicense License
        {
            get { return license; }
        }

        /// <summary>
        /// The current edition of the license as it is in Tango
        /// </summary>
        public Edition Edition
        {
            get { return edition; }
        }

        /// <summary>
        /// The day the trial was started.
        /// </summary>
        public DateTime Started
        {
            get { return started.Date; }
        }

        /// <summary>
        /// The date the trial expires
        /// </summary>
        public DateTime Expires
        {
            get { return expires.Date; }
        }

        /// <summary>
        /// The date on the server
        /// </summary>
        public DateTime ServerDate
        {
            get { return serverTime.Date; }
        }

        /// <summary>
        /// True if the customer can extend the trial.
        /// </summary>
        public bool CanExtend
        {
            get { return canExtend; }
        }

        /// <summary>
        /// True if the trial has been converted to a real license
        /// </summary>
        public bool IsConverted
        {
            get { return converted; }
        }

        /// <summary>
        /// Gets the total number of days remaining in the trial.
        /// </summary>
        public int DaysRemaining
        {
            get
            {
                TimeSpan time = Expires  - serverTime.Date;

                return (int) Math.Max(0, time.Days + 1);
            }
        }

        /// <summary>
        /// Indiciates if the trial is already expired.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return Expires < serverTime.Date;
            }
        }

        /// <summary>
        /// User visible description of the status of the trial.
        /// </summary>
        public string Description
        {
            get
            {
                if (IsConverted)
                {
                    return "A license has been purchased and must be entered.";
                }

                if (IsExpired)
                {
                    return "Expired";
                }

                return string.Format("Expires in {0} days.", DaysRemaining);
            }
        }

        /// <summary>
        /// Get the effective edition type to assign to newly created trials
        /// </summary>
        public static EditionInstalledType EffectiveTrialEditionType
        {
            get
            {
                if (StoreManager.GetAllStores().Count == 0)
                {
                    return EditionManager.InstalledEditionType;
                }

                if (StoreManager.GetAllStores().Any(s => EditionSerializer.Restore(s) is BrownDiscountedEdition))
                {
                    return EditionInstalledType.UpsDiscounted;
                }

                return EditionInstalledType.Standard;
            }
        }
    }
}
