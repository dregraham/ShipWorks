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

            EnumHelper.BindComboBox<FedExReturnType>(returnService);
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

                    returnService.ApplyMultiValue((FedExReturnType) fedEx.ReturnType);
                    rmaNumber.ApplyMultiText(fedEx.RmaNumber);
                    rmaReason.ApplyMultiText(fedEx.RmaReason);
                    saturdayPickup.ApplyMultiCheck(fedEx.ReturnSaturdayPickup);
                    returnsClearance.ApplyMultiCheck(fedEx.ReturnsClearance);
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

                    returnService.ReadMultiValue(v => fedEx.ReturnType = (int) v);
                    rmaNumber.ReadMultiText(t => fedEx.RmaNumber = t);
                    rmaReason.ReadMultiText(t => fedEx.RmaReason = t);
                    saturdayPickup.ReadMultiCheck(t => fedEx.ReturnSaturdayPickup = t);
                    returnsClearance.ReadMultiCheck(t=> fedEx.ReturnsClearance = t);
                }
            }
        }
    }
}