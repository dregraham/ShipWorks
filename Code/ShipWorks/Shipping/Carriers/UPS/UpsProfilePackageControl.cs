using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// User control for editing ups package profile settings
    /// </summary>
    public partial class UpsProfilePackageControl : ShippingProfileControlCore
    {
        UpsProfilePackageEntity package;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsProfilePackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given profile package into the control
        /// </summary>
        public void LoadProfilePackage(UpsProfilePackageEntity package)
        {
            this.package = package;
            groupBox.Text = string.Format(groupBox.Text, Parent.Controls.IndexOf(this) + 1);

            EnumHelper.BindComboBox<UpsPackagingType>(packagingType);
            dimensionsControl.Initialize();

            AddValueMapping(package, UpsProfilePackageFields.PackagingType, packagingState, packagingType, labelPackaging);
            AddValueMapping(package.PackageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(package.PackageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
            AddValueMapping(package, UpsProfilePackageFields.AdditionalHandlingEnabled, additionalHandlingState, additionalHandling, labelAdditionalHandling);
            AddValueMapping(package, UpsProfilePackageFields.DryIceEnabled, dryIceState, dryIceControl);
            AddValueMapping(package, UpsProfilePackageFields.VerbalConfirmationEnabled, verbalConfirmationState, verbalConfirmationDetails);
        }

        /// <summary>
        /// The profile package data loaded into the control
        /// </summary>
        public UpsProfilePackageEntity ProfilePackage
        {
            get { return package; }
        }

        /// <summary>
        /// Save the state of the ui to the loaded package
        /// </summary>
        public void SaveToEntity()
        {
            SaveMappingsToEntities();
        }
    }
}
