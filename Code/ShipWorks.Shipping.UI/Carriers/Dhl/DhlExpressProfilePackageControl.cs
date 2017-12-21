using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// User control for editing the package properties of a DhlExpress profile
    /// </summary>
    public partial class DhlExpressProfilePackageControl : ShippingProfileControlCore
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressProfilePackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given profile package into the control
        /// </summary>
        public void LoadProfilePackage(DhlExpressProfilePackageEntity parcelPackageProfileEntity)
        {
            ProfilePackage = parcelPackageProfileEntity;
            groupBox.Text = string.Format(groupBox.Text, Parent.Controls.IndexOf(this) + 1);

            dimensionsControl.Initialize();

            AddValueMapping(ProfilePackage, DhlExpressProfilePackageFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(ProfilePackage, DhlExpressProfilePackageFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
        }

        /// <summary>
        /// The profile package data loaded into the control
        /// </summary>
        public DhlExpressProfilePackageEntity ProfilePackage { get; private set; }

        /// <summary>
        /// Save the settings from the UI to the entity
        /// </summary>
        public void SaveToEntity()
        {
            SaveMappingsToEntities();
        }
    }
}
