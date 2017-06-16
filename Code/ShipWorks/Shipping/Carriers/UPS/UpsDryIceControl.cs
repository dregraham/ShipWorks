using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Gathers and displays data related to shipping dry ice
    /// </summary>
    public partial class UpsDryIceControl : UserControl, IShippingProfileControl
    {
        /// <summary>
        /// A change occurred that impacts the rate of a shipment.
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsDryIceControl()
        {
            InitializeComponent();
            EnumHelper.BindComboBox<UpsDryIceRegulationSet>(regulationSet);
            panelDryIceDetails.Enabled = containsDryIce.Checked;
        }

        /// <summary>
        /// The regulation set has changed
        /// </summary>
        private void OnRegulationSetChanged(object sender, System.EventArgs e)
        {
            medicalUse.Enabled = (regulationSet.SelectedItem != null &&
                ((EnumEntry<UpsDryIceRegulationSet>) regulationSet.SelectedItem).Value == UpsDryIceRegulationSet.Cfr);
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Whether the package contains dry ice has changed
        /// </summary>
        private void OnContainsDryIceChanged(object sender, System.EventArgs e)
        {
            panelDryIceDetails.Enabled = containsDryIce.Checked;
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Called when [rate criteria changed].
        /// </summary>
        private void OnRateCriteriaChanged(object sender, System.EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Reads values from the control into the package
        /// </summary>
        /// <param name="package">Package into which values will be read</param>
        public void ReadInto(UpsPackageEntity package)
        {
            regulationSet.ReadMultiValue(x => package.DryIceRegulationSet = (int)x);
            medicalUse.ReadMultiCheck(x => package.DryIceIsForMedicalUse = x);
            weight.ReadMultiWeight(x => package.DryIceWeight = x);
            containsDryIce.ReadMultiCheck(x => package.DryIceEnabled = x);
        }

        /// <summary>
        /// Applies package values into the controls
        /// </summary>
        /// <param name="package">Package from which values will be applied</param>
        public void ApplyFrom(UpsPackageEntity package)
        {
            regulationSet.ApplyMultiValue((UpsDryIceRegulationSet) package.DryIceRegulationSet);
            medicalUse.ApplyMultiCheck(package.DryIceIsForMedicalUse);
            weight.ApplyMultiWeight(package.DryIceWeight);
            containsDryIce.ApplyMultiCheck(package.DryIceEnabled);
        }

        /// <summary>
        /// Gets and sets the state in the profile
        /// </summary>
        public bool State
        {
            get { return containsDryIce.Checked; } 
            set { containsDryIce.Checked = value; }
        }

        /// <summary>
        /// Saves the values to the specified profile
        /// </summary>
        /// <param name="entity">Profile to which the dry ice information should be saved</param>
        public void SaveToEntity(EntityBase2 entity)
        {
            UpsProfilePackageEntity packageEntity = entity as UpsProfilePackageEntity;
            if (packageEntity == null)
            {
                return;
            }

            packageEntity.DryIceEnabled = containsDryIce.Checked;
            packageEntity.DryIceIsForMedicalUse = medicalUse.Checked;
            packageEntity.DryIceWeight = weight.Weight;
            packageEntity.DryIceRegulationSet = (int)regulationSet.SelectedValue;
        }

        /// <summary>
        /// Loads the values from the specified profile
        /// </summary>
        /// <param name="entity">Profile from which the dry ice information should be loaded</param>
        public void LoadFromEntity(EntityBase2 entity)
        {
            UpsProfilePackageEntity packageEntity = entity as UpsProfilePackageEntity;
            if (packageEntity == null)
            {
                return;
            }

            containsDryIce.Checked = packageEntity.DryIceEnabled.GetValueOrDefault();
            medicalUse.Checked = packageEntity.DryIceIsForMedicalUse.GetValueOrDefault();
            weight.Weight = packageEntity.DryIceWeight.GetValueOrDefault();
            regulationSet.SelectedValue = (UpsDryIceRegulationSet)packageEntity.DryIceRegulationSet.GetValueOrDefault();
        }

        /// <summary>
        /// Raises the rate criteria changed event.
        /// </summary>
        private void RaiseRateCriteriaChanged()
        {
            RateCriteriaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
