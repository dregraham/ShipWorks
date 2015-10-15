using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Package' node
    /// </summary>
    public class PackageOutline : ElementOutline
    {
        ShipmentEntity shipment;

        int parcelIndex;

            /// <summary>
        /// Constructor
        /// </summary>
        public PackageOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => Parcel.PackageID, If(() => Parcel.PackageID != null));
            
            AddAttribute("TrackingNumber", () => Parcel.TrackingNumber, If(() => !string.IsNullOrEmpty(Parcel.TrackingNumber)));

            // Insurance selection
            AddElement("Insurance", new InsuranceOutline(context), () => Parcel.Insurance);

            // Dimensinos
            AddElement("Dimensions", new DimensionsOutline(context), () => Parcel.Dimensions);

            AddElement("Weight", () => Parcel.Weight);
        }

        /// <summary>
        /// Return the parcel data for this package node
        /// </summary>
        private ShipmentParcel Parcel
        {
            get
            {
                return ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, parcelIndex);
            }
        }

        /// <summary>
        /// Create a clone of the outline, bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            Tuple<ShipmentEntity, int> tuple = (Tuple<ShipmentEntity, int>) data;

            return new PackageOutline(Context)
                {
                    shipment = tuple.Item1,
                    parcelIndex = tuple.Item2
                };
        }
    }
}
