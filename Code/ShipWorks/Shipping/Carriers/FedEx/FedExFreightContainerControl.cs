using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Container for FedEx freight controls
    /// </summary>
    public partial class FedExFreightContainerControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExFreightContainerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        public void LoadShipmentDetails(IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments.All(s => FedExUtility.IsFreightExpressService((FedExServiceType) s.FedEx.Service)))
            {
                fedExPackageFreightDetailControl.Visible = false;

                fedExExpressFreightControl.LoadShipmentDetails(shipments);
                fedExExpressFreightControl.Location = new Point(2, 2);
                fedExExpressFreightControl.Visible = true;

                Height = fedExExpressFreightControl.Bottom;
            }
            else
            {
                fedExExpressFreightControl.Visible = false;

                fedExPackageFreightDetailControl.Location = new Point(2, 2);
                fedExPackageFreightDetailControl.LoadShipments(shipments, true);
                fedExPackageFreightDetailControl.Visible = true;

                Height = fedExPackageFreightDetailControl.Bottom;
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public void SaveToShipments(IEnumerable<IShipmentEntity> shipments)
        {
            if (shipments.All(s => FedExUtility.IsFreightExpressService((FedExServiceType) s.FedEx.Service)))
            {
                fedExExpressFreightControl.SaveToShipments(shipments);
            }

            if (shipments.All(s => FedExUtility.IsFreightLtlService((FedExServiceType) s.FedEx.Service)))
            {
                fedExPackageFreightDetailControl.SaveToEntities();
            }
        }

        /// <summary>
        /// The package count changed so reload.
        /// </summary>
        public void PackageCountChanged(int packageCount)
        {
            if (fedExPackageFreightDetailControl.Visible)
            {
                fedExPackageFreightDetailControl.PackageCountChanged(packageCount);
                Height = fedExPackageFreightDetailControl.Bottom;
            }
        }
    }
}
