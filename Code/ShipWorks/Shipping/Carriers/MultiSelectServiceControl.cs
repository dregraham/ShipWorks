using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Service control to use when multiple different services are selected
    /// </summary>
    public partial class MultiSelectServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public MultiSelectServiceControl(RateControl rateControl) : 
            base(ShipmentTypeCode.None, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// This control has no weight to refresh.
        /// </summary>
        public override void RefreshContentWeight()
        {

        }

        /// <summary>
        /// Load the state from the given shipments
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            UpdateInsuranceDisplay();
        }

        /// <summary>
        /// Save the state to the shipments
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            insuranceControl.SaveToInsuranceChoices();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Update the display of insurance rates
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            var allInsurnace =
                LoadedShipments.Where(shipment => shipment.ShipmentType != (int) ShipmentTypeCode.None && ShippingManager.IsShipmentTypeActivated((ShipmentTypeCode) shipment.ShipmentType))
                    .SelectMany(shipment =>
                        Enumerable.Range(0, ShipmentTypeManager.GetType(shipment).GetParcelCount(shipment))
                            .Select(parcelIndex => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, parcelIndex).Insurance));

            insuranceControl.LoadInsuranceChoices(allInsurnace);
        }
    }
}
