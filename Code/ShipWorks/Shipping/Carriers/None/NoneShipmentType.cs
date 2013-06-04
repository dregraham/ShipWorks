using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// "None" shipment type implementation
    /// </summary>
    class NoneShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentType code of this type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.None; }
        }

        /// <summary>
        /// The UserControl for editing the settings of this type
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new NoneServiceControl();
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {

        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return "";
        }

        /// <summary>
        /// No parcels for 'None' shipments
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            return 0;
        }

        /// <summary>
        /// Get the insurance data for the shipment
        /// </summary>
        public override InsuranceChoice GetParcelInsuranceChoice(ShipmentEntity shipment, int parcelIndex)
        {
            throw new NotSupportedException("GetParcelInsuranceChoice not supported for none.");
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            throw new ShippingException("No carrier is selected for the shipment.");
        }
    }
}
