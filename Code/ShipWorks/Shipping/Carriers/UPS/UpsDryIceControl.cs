using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Gathers and displays data related to shipping dry ice
    /// </summary>
    public partial class UpsDryIceControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsDryIceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The form has loaded
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            EnumHelper.BindComboBox<UpsDryIceRegulationSet>(regulationSet);
            panelDryIceDetails.Enabled = containsDryIce.Checked;
        }

        /// <summary>
        /// The regulation set has changed
        /// </summary>
        private void OnRegulationSetChanged(object sender, System.EventArgs e)
        {
            medicalUse.Enabled = (regulationSet.SelectedItem != null &&
                ((EnumEntry<UpsDryIceRegulationSet>)regulationSet.SelectedItem).Value == UpsDryIceRegulationSet.Cfr);
        }

        /// <summary>
        /// Whether the package contains dry ice has changed
        /// </summary>
        private void OnContainsDryIceChanged(object sender, System.EventArgs e)
        {
            panelDryIceDetails.Enabled = containsDryIce.Checked;
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
    }
}
