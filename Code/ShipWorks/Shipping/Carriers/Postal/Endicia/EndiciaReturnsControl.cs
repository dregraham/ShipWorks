using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    /// <summary>
    /// Returns control for Endicia
    /// </summary>
    public partial class EndiciaReturnsControl : ReturnsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaReturnsControl" /> class.
        /// </summary>
        public EndiciaReturnsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the UI from the provided shipments
        /// </summary>
        public override void LoadShipments(List<ShipmentEntity> shipments)
        {
            base.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    EndiciaShipmentEntity endicia = shipment.Postal.Endicia;
                    scanBasedReturn.ApplyMultiCheck(endicia.ScanBasedReturn);
                }
            }
        }

        /// <summary>
        /// Save UI values back to the shipments
        /// </summary>
        public override void SaveToShipments()
        {
            base.SaveToShipments();

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    ShippingManager.EnsureShipmentLoaded(shipment);

                    EndiciaShipmentEntity endicia = shipment.Postal.Endicia;
                    scanBasedReturn.ReadMultiCheck(v => endicia.ScanBasedReturn = v);
                }
            }
        }
    }
}