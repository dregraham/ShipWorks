using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Returns control for FedEx
    /// </summary>
    public partial class FedExReturnsControl : ReturnsControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExReturnsControl" /> class.
        /// </summary>
        public FedExReturnsControl()
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
                    FedExShipmentEntity fedEx = shipment.FedEx;

                    rmaNumber.ApplyMultiText(fedEx.RmaNumber);
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
                    FedExShipmentEntity fedEx = shipment.FedEx;

                    rmaNumber.ReadMultiText(t => fedEx.RmaNumber = t);
                }
            }
        }
    }
}