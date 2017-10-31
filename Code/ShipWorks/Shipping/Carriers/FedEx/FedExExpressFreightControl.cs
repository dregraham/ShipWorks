using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control to manage shipments express freight properties.
    /// </summary>
    public partial class FedExExpressFreightControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExExpressFreightControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        public void LoadShipmentDetails(IEnumerable<IShipmentEntity> shipments)
        {
            SetUiVisibility(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    freightBookingNumber.ApplyMultiText(shipment.FedEx.FreightBookingNumber);
                    freightInsidePickup.ApplyMultiCheck(shipment.FedEx.FreightInsidePickup);
                    freightInsideDelivery.ApplyMultiCheck(shipment.FedEx.FreightInsideDelivery);
                    freightLoadAndCount.ApplyMultiText(shipment.FedEx.FreightLoadAndCount.ToString());
                }
            }
        }

        /// <summary>
        /// Set UI element visibility
        /// </summary>
        private void SetUiVisibility(IEnumerable<IShipmentEntity> shipments)
        {
            bool anyDomestic = false;

            // Determine if all shipments will have the same destination service types
            foreach (ShipmentEntity shipment in shipments)
            {
                // Need to check with the store  to see if anything about the shipment was overridden in case
                // it may have affected the shipping services available (i.e. the eBay GSP program)
                ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);
                if (ShipmentTypeManager.GetType(shipment).IsDomestic(overriddenShipment))
                {
                    anyDomestic = true;
                    break;
                }
            }

            // Show freight if there are all freight
            freightInsidePickup.Visible = anyDomestic;
            freightInsideDelivery.Visible = anyDomestic;
            labelLoadAndCount.Visible = !anyDomestic;
            freightLoadAndCount.Visible = !anyDomestic;
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public void SaveToShipments(IEnumerable<IShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                freightBookingNumber.ReadMultiText(t => shipment.FedEx.FreightBookingNumber = t);
                freightInsideDelivery.ReadMultiCheck(c => shipment.FedEx.FreightInsideDelivery = c);
                freightInsidePickup.ReadMultiCheck(c => shipment.FedEx.FreightInsidePickup = c);
                freightLoadAndCount.ReadMultiText(t => { int count; if (int.TryParse(t, out count)) shipment.FedEx.FreightLoadAndCount = count; });
            }
        }
    }
}
