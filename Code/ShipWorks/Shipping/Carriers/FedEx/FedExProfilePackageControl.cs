﻿using Interapptive.Shared;
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
        public void FedExValueMapping(FedExProfilePackageEntity profilePackage)
        {
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
            AddValueMapping(profilePackage, FedExProfilePackageFields.ContainerType, containerTypeState, containerType);
            AddValueMapping(profilePackage, FedExProfilePackageFields.NumberOfContainers, numberOfContainersState, numberOfContainers);
            AddValueMapping(profilePackage, FedExProfilePackageFields.SignatoryContactName, signatoryNameState, signatoryName);
            AddValueMapping(profilePackage, FedExProfilePackageFields.SignatoryTitle, signatoryTitleState, signatoryTitle);
            AddValueMapping(profilePackage, FedExProfilePackageFields.SignatoryPlace, signatoryPlaceState, signatoryPlace);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialNumber, hazardousMaterialIdState, hazardousMaterialId);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialClass, hazardClassState, hazardClass);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialProperName, properNameState, properName);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialPackingGroup, packingGroupState, packingGroup);
            AddValueMapping(profilePackage, FedExProfilePackageFields.PackingDetailsCargoAircraftOnly, packingCargoAircraftOnlyState, packingCargoAircraftOnly);
            AddValueMapping(profilePackage, FedExProfilePackageFields.PackingDetailsPackingInstructions, packingInstructionsState, packingInstructions);
            AddValueMapping(profilePackage, FedExProfilePackageFields.DangerousGoodsPackagingCount, packagingCountState, packagingCount);
            AddValueMapping(profilePackage, FedExProfilePackageFields.HazardousMaterialQuantityValue, quantityState, quantity);
            AddValueMapping(profilePackage, FedExProfilePackageFields.BatteryMaterial, batteryMaterialState, batteryMaterial);
            AddValueMapping(profilePackage, FedExProfilePackageFields.BatteryPacking, batteryPackingState, batteryPacking);
            AddValueMapping(profilePackage, FedExProfilePackageFields.BatteryRegulatorySubtype, batteryRegulatorySubtypeState, batteryRegulatorySubtype);
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
