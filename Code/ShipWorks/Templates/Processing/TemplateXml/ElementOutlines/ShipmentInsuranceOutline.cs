using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Insurance' entry of the 'Shipment' node
    /// </summary>
    public class ShipmentInsuranceOutline : ElementOutline
    {
        ShipmentEntity shipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentInsuranceOutline(TemplateTranslationContext context)
            : base(context)
        {
            Lazy<ShipmentType> shipmentType = new Lazy<ShipmentType>(() => ShipmentTypeManager.GetType(shipment));

            AddElement("Package", new ShipmentInsuranceChoiceOutline(context),
                () =>
                    Enumerable.Range(0, shipmentType.Value.GetParcelCount(shipment))
                        .Select(parcelIndex => shipmentType.Value.GetParcelInsuranceChoice(shipment, parcelIndex)));
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipmentInsuranceOutline(Context) { shipment = (ShipmentEntity) data };
        }
    }
}
