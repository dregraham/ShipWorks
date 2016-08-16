using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExPackagePriorityAlertsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackagePriorityAlertsControl" /> class.
        /// </summary>
        public FedExPackagePriorityAlertsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExPriorityAlertEnhancementType>(this.priorityAlertEnhancementType);
        }

        /// <summary>
        /// Called when [priority alert enhancement type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPriorityAlertEnhancementTypeChanged(object sender, EventArgs e)
        {
            // Only allow the content of the text box to be edited if a value other than
            // none is selected.
            priorityAlertContentDetail.Enabled = priorityAlertEnhancementType.SelectedValue != null && EnhancementType != FedExPriorityAlertEnhancementType.None;
        }

        /// <summary>
        /// Gets the content detail.
        /// </summary>
        /// <value>The content detail.</value>
        public string ContentDetail
        {
            get { return priorityAlertContentDetail.Text; }
        }

        /// <summary>
        /// Gets the type of the enhancement.
        /// </summary>
        /// <value>The type of the enhancement.</value>
        public FedExPriorityAlertEnhancementType EnhancementType
        {
            get { return (FedExPriorityAlertEnhancementType) priorityAlertEnhancementType.SelectedValue; }
        }

        /// <summary>
        /// Loads the priority alert data from the package.
        /// </summary>
        /// <param name="package">The package.</param>
        public void LoadPriorityAlertData(FedExPackageEntity package)
        {
            priorityAlertContentDetail.ApplyMultiText(package.PriorityAlertDetailContent);
            priorityAlertEnhancementType.ApplyMultiValue((FedExPriorityAlertEnhancementType)package.PriorityAlertEnhancementType);
        }

        /// <summary>
        /// Saves the priority alert to the packages.
        /// </summary>
        /// <param name="packages">The packages.</param>
        public void SavePriorityAlertToPackage(IEnumerable<FedExPackageEntity> packages)
        {
            foreach (FedExPackageEntity package in packages)
            {
                priorityAlertContentDetail.ReadMultiText(t => package.PriorityAlertDetailContent = t);
                priorityAlertEnhancementType.ReadMultiValue(v => package.PriorityAlertEnhancementType = (int) v);

                // Slightly backwards logic, but will still work if a new enhancement type is added in a newer version of the API
                priorityAlertEnhancementType.ReadMultiValue(v => package.PriorityAlert = ((FedExPriorityAlertEnhancementType)v != FedExPriorityAlertEnhancementType.None));
            }
        }
    }
}
