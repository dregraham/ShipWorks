using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the package properties of a fedex shipment
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class FedExProfilePackageControl : ShippingProfileControlCore
    {
        private FedExProfilePackageEntity package;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExProfilePackageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The profile package data loaded into the control
        /// </summary>
        public FedExProfilePackageEntity ProfilePackage
        {
            get
            {
                return package;
            }
        }

        /// <summary>
        /// Load the data from the given profile package into the control
        /// </summary>
        public void LoadProfilePackage(FedExProfilePackageEntity profilePackage)
        {
            package = profilePackage;
            groupBox.Text = string.Format(groupBox.Text, Parent.Controls.IndexOf(this) + 1);

            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<FedExDangerousGoodsMaterialType>(dangerousGoodsMaterialType);
            EnumHelper.BindComboBox<FedExDangerousGoodsAccessibilityType>(dangerousGoodsAccessibility);
            EnumHelper.BindComboBox<FedExHazardousMaterialsPackingGroup>(packingGroup);
            

            AddValueMapping(profilePackage, FedExProfilePackageFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);

            AddValueMapping(profilePackage, FedExProfilePackageFields.DryIceWeight, dryIceState, dryIceWeight, labelDryIceWeight);

            AddValueMapping(profilePackage, FedExProfilePackageFields.ContainsAlcohol, alcoholState, containsAlcohol, labelAlcohol);

            AddValueMapping(profilePackage, FedExProfilePackageFields.PriorityAlert, priorityAlertState, priorityAlertControl);

            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsEnabled, dangerousGoodsEnabledState, dangerousGoodsEnabled);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsType, dangerousGoodsMaterialTypeState, dangerousGoodsMaterialType);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsAccessibilityType, dangerousGoodsAccessibilityState, dangerousGoodsAccessibility);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsCargoAircraftOnly, aircraftState, aircarft);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsEmergencyContactPhone, emergencyContactPhoneState, emergencyContactPhone);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsOfferor, offerorState, offeror);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialNumber, hazardousMaterialIdState, hazardousMaterialId);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialClass, hazardClassState, hazardClass);

            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialProperName, properNameState, properName);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialPackingGroup, packingGroupState, packingGroup);

            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsPackagingCount, packagingCountState, packagingCount);

            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialQuantityValue, quantityState, quantity);
        }

        /// <summary>
        /// Save the settings from the UI to the entity
        /// </summary>
        public void SaveToEntity()
        {
            if (string.IsNullOrWhiteSpace(packagingCount.Text))
            {
                packagingCount.Text = "0";
            }

            SaveMappingsToEntities();
        }
    }
}
