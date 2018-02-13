using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the package properties of a fedex shipment
    /// </summary>
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

            FedExProfileBindComboBox();

            FedExValueMapping(profilePackage);
        }

        /// <summary>
        /// Initialize combobox binding for the profile
        /// </summary>
        public void FedExProfileBindComboBox()
        {
            EnumHelper.BindComboBox<FedExDangerousGoodsMaterialType>(dangerousGoodsMaterialType);
            EnumHelper.BindComboBox<FedExDangerousGoodsAccessibilityType>(dangerousGoodsAccessibility);
            EnumHelper.BindComboBox<FedExHazardousMaterialsPackingGroup>(packingGroup);
            EnumHelper.BindComboBox<FedExBatteryMaterialType>(batteryMaterial);
            EnumHelper.BindComboBox<FedExBatteryPackingType>(batteryPacking);
            EnumHelper.BindComboBox<FedExBatteryRegulatorySubType>(batteryRegulatorySubtype);
        }

        /// <summary>
        /// Initialize value mappings for the profile
        /// </summary>
        public void FedExValueMapping(FedExProfilePackageEntity fedexPackageProfile)
        {
            AddValueMapping(fedexPackageProfile.PackageProfile, PackageProfileFields.Weight, weightState, weight, labelWeight);
            AddValueMapping(fedexPackageProfile.PackageProfile, PackageProfileFields.DimsProfileID, dimensionsState, dimensionsControl, labelDimensions);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DryIceWeight, dryIceState, dryIceWeight, labelDryIceWeight);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.ContainsAlcohol, alcoholState, containsAlcohol, labelAlcohol);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.PriorityAlert, priorityAlertState, priorityAlertControl);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsEnabled, dangerousGoodsEnabledState, dangerousGoodsEnabled);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsType, dangerousGoodsMaterialTypeState, dangerousGoodsMaterialType);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsAccessibilityType, dangerousGoodsAccessibilityState, dangerousGoodsAccessibility);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsCargoAircraftOnly, aircraftState, aircarft);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsEmergencyContactPhone, emergencyContactPhoneState, emergencyContactPhone);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsOfferor, offerorState, offeror);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.ContainerType, containerTypeState, containerType);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.NumberOfContainers, numberOfContainersState, numberOfContainers);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.SignatoryContactName, signatoryNameState, signatoryName);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.SignatoryTitle, signatoryTitleState, signatoryTitle);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.SignatoryPlace, signatoryPlaceState, signatoryPlace);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.HazardousMaterialNumber, hazardousMaterialIdState, hazardousMaterialId);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.HazardousMaterialClass, hazardClassState, hazardClass);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.HazardousMaterialProperName, properNameState, properName);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.HazardousMaterialPackingGroup, packingGroupState, packingGroup);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.PackingDetailsCargoAircraftOnly, packingCargoAircraftOnlyState, packingCargoAircraftOnly);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.PackingDetailsPackingInstructions, packingInstructionsState, packingInstructions);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.DangerousGoodsPackagingCount, packagingCountState, packagingCount);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.HazardousMaterialQuantityValue, quantityState, quantity);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.BatteryMaterial, batteryMaterialState, batteryMaterial);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.BatteryPacking, batteryPackingState, batteryPacking);
            AddValueMapping(fedexPackageProfile, FedExProfilePackageFields.BatteryRegulatorySubtype, batteryRegulatorySubtypeState, batteryRegulatorySubtype);
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
