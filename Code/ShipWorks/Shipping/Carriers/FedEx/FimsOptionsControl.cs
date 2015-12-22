using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control for FIMS options
    /// </summary>
    public partial class FimsOptionsControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FimsOptionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads from shipment entity.
        /// </summary>
        internal void LoadFromShipment(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                airWaybill.ApplyMultiText(shipment.FedEx.FimsAirWaybill ?? string.Empty);
                referenceCustomer.ApplyMultiText(shipment.FedEx.ReferenceFIMS ?? string.Empty);
            }
        }

        /// <summary>
        /// Saves to shipment entity.
        /// </summary>
        internal void SaveToShipment(ShipmentEntity shipment)
        {
            airWaybill.ReadMultiText(x => shipment.FedEx.FimsAirWaybill = x);
            referenceCustomer.ReadMultiText(x => shipment.FedEx.ReferenceFIMS = x);
        }
    }
}
