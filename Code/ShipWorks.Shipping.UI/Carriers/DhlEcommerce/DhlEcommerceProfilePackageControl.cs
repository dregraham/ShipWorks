using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// User control for editing the package properties of a DHL eCommerce profile
    /// </summary>
    public partial class DhlEcommerceProfilePackageControl : ShippingProfileControlCore
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceProfilePackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given profile package into the control
        /// </summary>
        public void LoadProfilePackage(PackageProfileEntity parcelPackageProfileEntity)
        {
            ProfilePackage = parcelPackageProfileEntity;
            groupBox.Text = string.Format(groupBox.Text, Parent.Controls.IndexOf(this) + 1);

            dimensionsControl.Initialize();

            AddValueMapping(ProfilePackage, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(ProfilePackage, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
        }

        /// <summary>
        /// The profile package data loaded into the control
        /// </summary>
        public PackageProfileEntity ProfilePackage { get; private set; }

        /// <summary>
        /// Save the settings from the UI to the entity
        /// </summary>
        public void SaveToEntity()
        {
            SaveMappingsToEntities();
        }
    }
}
