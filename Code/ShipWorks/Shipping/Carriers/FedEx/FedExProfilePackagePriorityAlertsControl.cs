using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control for managing FedEx profile package priority alert info
    /// </summary>
    public partial class FedExProfilePackagePriorityAlertsControl : UserControl, IShippingProfileControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExProfilePackagePriorityAlertsControl" /> class.
        /// </summary>
        public FedExProfilePackagePriorityAlertsControl()
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
            priorityAlertContentDetail.Enabled = EnhancementType != FedExPriorityAlertEnhancementType.None;
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
        /// Loads the priority alert data from the profile package.
        /// </summary>
        /// <param name="package">The profile package.</param>
        public void LoadPriorityAlertData(FedExProfilePackageEntity package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            if (string.IsNullOrWhiteSpace(package.PriorityAlertDetailContent))
            {
                package.PriorityAlertDetailContent = string.Empty;
            }
            else
            {
                priorityAlertContentDetail.Text = package.PriorityAlertDetailContent;
            }

            if (!package.PriorityAlertEnhancementType.HasValue)
            {
                package.PriorityAlertEnhancementType = (int) FedExPriorityAlertEnhancementType.None;
            }
            else
            {
                priorityAlertEnhancementType.SelectedValue = (FedExPriorityAlertEnhancementType) package.PriorityAlertEnhancementType;
            }

            State = package.PriorityAlert.HasValue && package.PriorityAlert.Value;
        }

        /// <summary>
        /// Saves the priority alert to the packages.
        /// </summary>
        /// <param name="package">The packages.</param>
        public void SavePriorityAlertToProfilePackage(FedExProfilePackageEntity package)
        {
            package.PriorityAlertDetailContent = priorityAlertContentDetail.Text;
            package.PriorityAlertEnhancementType = (int)priorityAlertEnhancementType.SelectedValue;
            package.PriorityAlert = State;
        }

        /// <summary>
        /// Gets/Sets the Enabled value of this control
        /// </summary>
        public bool State
        {
            get
            {
                return this.Enabled;
            }
            set
            {
                this.Enabled = value;
            }
        }

        /// <summary>
        /// Save the priority alert info to the profile package
        /// </summary>
        public void SaveToEntity(EntityBase2 entity)
        {
            SavePriorityAlertToProfilePackage(entity as FedExProfilePackageEntity);
        }

        /// <summary>
        /// Load the profile package priority alert data to this control
        /// </summary>
        public void LoadFromEntity(EntityBase2 entity)
        {
            LoadPriorityAlertData(entity as FedExProfilePackageEntity);
        }
    }
}
