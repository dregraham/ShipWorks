using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXPath;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Base class for all postal service shipment types
    /// </summary>
    abstract class PostalShipmentTypeBase : ShipmentType
    {
        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void EnsureCarrierSpecificData(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Refreshes any existing carrier specific data for the shipemnt.  If the data does not exist, it is not creatd.
        /// </summary>
        public override void RefreshCarrierSpecificData(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Delete any USPS specific data for the shipment
        /// </summary>
        public override void DeleteCarrierSpecificData(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            throw new ShippingException("Not Implemented");
        }

        /// <summary>
        /// Generate the USPS specific template output
        /// </summary>
        public override void GenerateTemplateXml(TemplateXPathElement parent, ShipmentEntity shipment)
        {

        }
    }
}
