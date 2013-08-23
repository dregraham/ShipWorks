using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;

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
        }

        /// <summary>
        /// The regulation set has changed
        /// </summary>
        private void OnRegulationSetChanged(object sender, System.EventArgs e)
        {
            UpsDryIceRegulationSet selectedValue = (UpsDryIceRegulationSet)regulationSet.SelectedItem;

            medicalUse.Enabled = selectedValue == UpsDryIceRegulationSet.Cfr;
        }
    }
}
