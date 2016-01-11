using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Control for choosing i-parcel label options.
    /// </summary>
    public partial class iParcelOptionsControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelOptionsControl" /> class.
        /// </summary>
        public iParcelOptionsControl()
        {
            InitializeComponent();

            requestedLabelFormat.ExcludeFormats(ThermalLanguage.ZPL);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            requestedLabelFormat.LoadDefaultProfile(ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel));
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            requestedLabelFormat.SaveDefaultProfile();
        }
    }
}
