using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// ShipmentType class for WorldShip shipments
    /// </summary>
    public class WorldShipShipmentType : UpsShipmentType
    {
        /// <summary>
        /// Indicates if the shipment service type supports return shipments
        /// </summary>
        public override bool SupportsReturns
        {
            get { return false; }
        }

        /// <summary>
        /// Type code for WorldShip
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.UpsWorldShip; }
        }

        /// <summary>
        /// Created specifically for WorldShip.  A WorldShip shipment is processed in two phases - first it's processed 
        /// in ShipWorks, then once its processed in WorldShip its completed.  Opted instead of hardcoding WorldShip if statements
        /// to use this instead so its easier to track down all the usgages by doing Find References on this property.
        /// </summary>
        public override bool ProcessingCompletesExternally
        {
            get { return true; }
        }

        /// <summary>
        /// Create settings control for WorldShip
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl()
        {
            WorldShipSettingsControl control = new WorldShipSettingsControl();
            control.Initialize(ShipmentTypeCode);
            return control;
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new WorldShipProfileControl();
        }

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not 
        /// been excluded). The integer values are intended to correspond to the appropriate 
        /// enumeration values of the specific shipment type (i.e. the integer values would 
        /// correspond to PostalServiceType values for a UspsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            List<int> allServiceTypes = Enum.GetValues(typeof(UpsServiceType)).Cast<int>().ToList();
            return allServiceTypes.Except(GetExcludedServiceTypes(repository)).ToList();
        }
        
        /// <summary>
        /// Get the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (shipment.Ups.WorldShipStatus == (int) WorldShipStatusType.Exported)
            {
                return new List<string> { "(Not yet processed in WorldShip)" };
            }

            return base.GetTrackingNumbers(shipment);
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            // USPS tracking details
            container.AddElement("USPSTrackingNumber", () => loaded().Ups.UspsTrackingNumber, ElementOutline.If(() => shipment().Processed));
        }

        /// <summary>
        /// Track the given shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            if (shipment.Ups.WorldShipStatus == (int) WorldShipStatusType.Exported)
            {
                throw new ShippingException("The shipment has not been processed in WorldShip.");
            }

            return base.TrackShipment(shipment);
        }

        /// <summary>
        /// Determines whether [is mail innovations enabled] for WorldShip.
        /// </summary>
        /// <returns></returns>
        public override bool IsMailInnovationsEnabled()
        {
            return ShippingSettings.Fetch().WorldShipMailInnovationsEnabled;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the UPS WorldShip shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a WorldShipBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new WorldShipBestRateBroker();
        }
    }
}
