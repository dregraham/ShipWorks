using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Editing;
using System.Windows.Forms;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// "Other" (custom) ShipmentType implementation
    /// </summary>
    class OtherShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Other; }
        }

        /// <summary>
        /// Create the Setup Wizard used to setup the "Other" shipment type
        /// </summary>
        public override Form CreateSetupWizard()
        {
            return new OtherSetupWizard();
        }

        /// <summary>
        /// Create the control needed to edit service options for the type
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new OtherServiceControl();
        }

        /// <summary>
        /// Create the control needed to edit the profile settings for the type
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new OtherProfileControl();
        }

        /// <summary>
        /// Ensures that the Other specific data for the shipment is loaded.  If the data already exists nothing is done, it is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Other", typeof(OtherShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "Other", typeof(OtherProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Configure the initial values for a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// ShipWorks typically auto-updates the ShipDate on unprocessed shipments to be Today at 
        /// the earliest.  But the use-case for Other shipments is a bit different, where people
        /// manually enter shipment details, often occurring in the past.
        /// </summary>
        protected override void UpdateShipmentShipDate(ShipmentEntity shipment)
        {
            // nothing
        }

        /// <summary>
        /// For 'Other' we just use return as a marker
        /// </summary>
        public override bool SupportsReturns
        {
            get { return true; }
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long originID = ShippingOriginManager.Origins.Count > 0 ? ShippingOriginManager.Origins[0].ShippingOriginID : (long) ShipmentOriginSource.Store;
            profile.OriginID = originID;

            profile.Other.Carrier = "";
            profile.Other.Service = "";
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            OtherShipmentEntity otherShipment = shipment.Other;
            OtherProfileEntity otherProfile = profile.Other;

            ShippingProfileUtility.ApplyProfileValue(otherProfile.Service, otherShipment, OtherShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(otherProfile.Carrier, otherShipment, OtherShipmentFields.Carrier);

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Update the dyamic data of the shipment
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // Other only has the option to use ShipWorks Insurance
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used.  The carrier specific data must exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return string.Format("{0} {1}", shipment.Other.Carrier, shipment.Other.Service);
        }

        /// <summary>
        /// Get the insurance data for the shipment
        /// </summary>
        public override InsuranceChoice GetParcelInsuranceChoice(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new InsuranceChoice(shipment, shipment, shipment.Other, null);
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            if (shipment.Other.Carrier.Trim().Length == 0)
            {
                throw new ShippingException("No carrier is specified.");
            }

            if (shipment.Other.Service.Trim().Length == 0)
            {
                throw new ShippingException("No service is specified.");
            }
        }

        /// <summary>
        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            ElementOutline outline = container.AddElement("Other");
            outline.AddElement("Carrier", () => loaded().Other.Carrier);
            outline.AddElement("Service", () => loaded().Other.Service);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Other shipment type.
        /// </summary>
        /// <returns>An instance of a NullShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker()
        {
            return new NullShippingBroker();
        }
    }
}
