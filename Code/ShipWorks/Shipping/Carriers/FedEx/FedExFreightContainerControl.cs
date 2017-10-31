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
            this.AutoSize = false;
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        public void LoadShipmentDetails(IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments.All(s => FedExUtility.IsFreightExpressService((FedExServiceType) s.FedEx.Service)))
            {
                panelLtlFreight.Visible = false;

                fedExExpressFreightControl.LoadShipmentDetails(shipments);
                fedExExpressFreightControl.Location = new Point(2, 2);
                fedExExpressFreightControl.Visible = true;

                Height = fedExExpressFreightControl.Bottom;
            }
            else
            {
                fedExExpressFreightControl.Visible = false;

                panelLtlFreight.Location = new Point(2, 2);
                fedExPackageFreightDetailControl.LoadShipments(shipments, true);
                fedExLtlFreightControl.LoadShipmentDetails(shipments);

                UpdateLayout();

                panelLtlFreight.Visible = true;
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public void SaveToShipments(IEnumerable<ShipmentEntity> shipments)
        {
            if (shipments.All(s => FedExUtility.IsFreightExpressService((FedExServiceType) s.FedEx.Service)))
            {
                fedExExpressFreightControl.SaveToShipments(shipments);
            }

            if (shipments.All(s => FedExUtility.IsFreightLtlService((FedExServiceType) s.FedEx.Service)))
            {
                fedExPackageFreightDetailControl.SaveToEntities();
                fedExLtlFreightControl.SaveToShipments(shipments);
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

                UpdateLayout();
            }
        }

        /// <summary>
        /// Update the UI layout (for sizing)
        /// </summary>
        private void UpdateLayout()
        {
            groupFreightPackages.Height = fedExPackageFreightDetailControl.Bottom + 5;
            panelLtlFreight.Height = groupFreightPackages.Bottom + 5;
            Height = panelLtlFreight.Bottom;
        }
    }
}
