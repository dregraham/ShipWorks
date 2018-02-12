using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// User control for editing the package properties of a iParcel shipment
    /// </summary>
    public partial class iParcelProfilePackageControl : ShippingProfileControlCore
    {
        PackageProfileEntity package;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelProfilePackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given profile package into the control
        /// </summary>
        public void LoadProfilePackage(PackageProfileEntity parcelPackageProfileEntity)
        {
            this.package = parcelPackageProfileEntity;
            groupBox.Text = string.Format(groupBox.Text, Parent.Controls.IndexOf(this) + 1);

            dimensionsControl.Initialize();

            AddValueMapping(package, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(package, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
        }

        /// <summary>
        /// The profile package data loaded into the control
        /// </summary>
        public PackageProfileEntity ProfilePackage
        {
            get { return package; }
        }

        /// <summary>
        /// Save the settings from the UI to the entity
        /// </summary>
        public void SaveToEntity()
        {
            SaveMappingsToEntities();
        }
    }
}
