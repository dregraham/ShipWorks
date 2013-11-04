using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.BestRate; }
        }
        
        public override Editing.ServiceControlBase CreateServiceControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Apply the configured defaults and profile rule settings to the given shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);

            if (shipment.BestRateShipment != null)
            {
                shipment.BestRateShipment.DimsAddWeight = false;
                shipment.BestRateShipment.DimsHeight = 0;
                shipment.BestRateShipment.DimsLength = 0;
                shipment.BestRateShipment.DimsWeight = 0;
                shipment.BestRateShipment.DimsWidth = 0;
            }
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "BestRate", typeof(UpsShipmentEntity), refreshIfPresent);
        }

        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        public override Insurance.InsuranceChoice GetParcelInsuranceChoice(ShipmentEntity shipment, int parcelIndex)
        {
            throw new NotImplementedException();
        }

        public override void ProcessShipment(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether the specified settings tab should be hidden in the UI
        /// </summary>
        public override bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
        {
            return tab == ShipmentTypeSettingsControl.Page.Actions || tab == ShipmentTypeSettingsControl.Page.Printing;
        }
    }
}
