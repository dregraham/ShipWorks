using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using ShipWorks.Core.Shipping;
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
using ShipWorks.Shipping.Carriers.Ups;
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
        public override SettingsControlBase CreateSettingsControl()
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
        /// Process the given WorldShip shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            try
            {
                base.ProcessShipment(shipment);

                WorldShipUtility.ExportToWorldShip(shipment);

                // Mark shipment as exported
                shipment.Ups.WorldShipStatus = (int) WorldShipStatusType.Exported;
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
            catch (TemplateTokenException templateTokenException)
            {
                throw new ShippingException("ShipWorks encountered an error attempting to process the shipment.", templateTokenException);
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            // If it's been exported but not yet processed in WorldShip, then don't actually void the underlying shipment, but
            // we do have to remove the entry from the table
            if (shipment.Ups.WorldShipStatus == (int) WorldShipStatusType.Exported)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntity(new WorldShipShipmentEntity(shipment.ShipmentID));
                }
            }
            else
            {
                UpsServiceType serviceType = (UpsServiceType)shipment.Ups.Service;

                // If we are WolrdShip AND the WorldShipStatus is already set to Voided
                // then this void request MUST have come from WorldShipImportMonitor, so just return
                if (shipment.Ups.WorldShipStatus == (int)WorldShipStatusType.Voided)
                {
                    return;
                }

                if (UpsUtility.IsUpsMiOrSurePostService(serviceType))
                {
                    // If we got here, the request must have come from the user clicking void in ShipWorks,
                    // which is not supported...they need to start the void in WorldShip
                    throw new ShippingException("UPS Mail Innovations or UPS SurePost shipments must be voided using WorldShip.");
                }

                base.VoidShipment(shipment);
            }
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

        /// <summary>
        /// Return a shipment adapter
        /// </summary>
        public override IShipmentAdapter GetShipmentAdapter(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            IShipmentAdapter shipmentAdapter = new UpsShipmentAdapter(shipment);

            return shipmentAdapter;
        }
    }
}
