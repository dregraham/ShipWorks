using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.UI.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExDangerousGoodsControl : UserControl
    {
        public event EventHandler DangerousGoodsChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExDangerousGoodsControl" /> class.
        /// </summary>
        public FedExDangerousGoodsControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExDangerousGoodsMaterialType>(dangerousGoodsMaterialType);
            EnumHelper.BindComboBox<FedExDangerousGoodsAccessibilityType>(dangerousGoodsAccessibility);
            EnumHelper.BindComboBox<FedExHazardousMaterialsPackingGroup>(hazardousMaterialPackingGroup);
            EnumHelper.BindComboBox<FedExHazardousMaterialsQuantityUnits>(dangerousGoodsPackagingUnits);
        }

        /// <summary>
        /// Gets a value indicating whether [dangerous goods checked].
        /// </summary>
        /// <value>
        /// <c>true</c> if [dangerous goods checked]; otherwise, <c>false</c>.
        /// </value>
        public bool DangerousGoodsChecked => dangerousGoodsEnabled.Checked;

        /// <summary>
        /// Loads the dangerous goods data.
        /// </summary>
        /// <param name="package">The package.</param>
        public void LoadDangerousGoodsData(FedExPackageEntity package)
        {
            dangerousGoodsEnabled.ApplyMultiCheck(package.DangerousGoodsEnabled);

            dangerousGoodsMaterialType.ApplyMultiValue((FedExDangerousGoodsMaterialType) package.DangerousGoodsType);
            dangerousGoodsAccessibility.ApplyMultiValue((FedExDangerousGoodsAccessibilityType) package.DangerousGoodsAccessibilityType);
            dangerousGoodsCargoAircraftOnly.ApplyMultiCheck(package.DangerousGoodsCargoAircraftOnly);
            dangerousGoodsPackagingCounts.ApplyMultiText(package.DangerousGoodsPackagingCount.ToString());
            dangerousGoodsPackagingUnits.ApplyMultiValue((FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits);
            containerType.ApplyMultiText(package.ContainerType);
            numberOfContainers.ApplyMultiText(package.NumberOfContainers.ToString());

            emergencyContactPhone.ApplyMultiText(package.DangerousGoodsEmergencyContactPhone);
            offeror.ApplyMultiText(package.DangerousGoodsOfferor);

            signatoryContactName.ApplyMultiText(package.SignatoryContactName);
            signatoryPlace.ApplyMultiText(package.SignatoryPlace);
            signatoryTitle.ApplyMultiText(package.SignatoryTitle);

            hazardousMaterialId.ApplyMultiText(package.HazardousMaterialNumber);
            hazardClass.ApplyMultiText(package.HazardousMaterialClass);
            hazardousMaterialProperName.ApplyMultiText(package.HazardousMaterialProperName);
            hazardousMaterialTechnicalName.ApplyMultiText(package.HazardousMaterialTechnicalName);
            hazardousMaterialPackingGroup.ApplyMultiValue((FedExHazardousMaterialsPackingGroup) package.HazardousMaterialPackingGroup);
            hazardousMaterialQuantityValue.ApplyMultiText(package.HazardousMaterialQuantityValue.ToString());
            packingDetailsAircraftOnly.ApplyMultiCheck(package.PackingDetailsCargoAircraftOnly);
            packingInstructions.ApplyMultiText(package.PackingDetailsPackingInstructions);

            UpdateUI();
        }

        /// <summary>
        /// Saves the state of the controls to the package entities.
        /// </summary>
        /// <param name="packages">The packages.</param>
        public void SaveToPackages(IEnumerable<FedExPackageEntity> packages)
        {
            foreach (FedExPackageEntity package in packages)
            {
                SaveToPackage(package);
            }
        }

        /// <summary>
        /// Saves the state of the controls to the package.
        /// </summary>
        private void SaveToPackage(FedExPackageEntity package)
        {
            dangerousGoodsEnabled.ReadMultiCheck(c => package.DangerousGoodsEnabled = c);
            dangerousGoodsMaterialType.ReadMultiValue(v => package.DangerousGoodsType = (int) v);
            dangerousGoodsAccessibility.ReadMultiValue(v => package.DangerousGoodsAccessibilityType = (int) v);
            dangerousGoodsCargoAircraftOnly.ReadMultiCheck(c => package.DangerousGoodsCargoAircraftOnly = c);
            dangerousGoodsPackagingCounts.ReadMultiText(t => package.DangerousGoodsPackagingCount = ReadIntegerValue(t));
            dangerousGoodsPackagingUnits.ReadMultiValue(v => package.HazardousMaterialQuanityUnits = (int) v);
            containerType.ReadMultiText(v => package.ContainerType = v);
            numberOfContainers.ReadMultiText(v => package.NumberOfContainers = ReadIntegerValue(v));

            offeror.ReadMultiText(t => package.DangerousGoodsOfferor = t);
            emergencyContactPhone.ReadMultiText(t => package.DangerousGoodsEmergencyContactPhone = t);

            SaveSignatoryToPackage(package);

            SaveHazardousMaterialToPackage(package);
        }

        /// <summary>
        /// Saves the signatory to package.
        /// </summary>
        private void SaveSignatoryToPackage(FedExPackageEntity package)
        {
            signatoryContactName.ReadMultiText(t => package.SignatoryContactName = t);
            signatoryTitle.ReadMultiText(t => package.SignatoryTitle = t);
            signatoryPlace.ReadMultiText(t => package.SignatoryPlace = t);
        }

        /// <summary>
        /// Saves the hazardous material to package.
        /// </summary>
        private void SaveHazardousMaterialToPackage(FedExPackageEntity package)
        {
            hazardousMaterialId.ReadMultiText(t => package.HazardousMaterialNumber = t);
            hazardClass.ReadMultiText(t => package.HazardousMaterialClass = t);
            hazardousMaterialProperName.ReadMultiText(t => package.HazardousMaterialProperName = t);
            hazardousMaterialTechnicalName.ReadMultiText(t => package.HazardousMaterialTechnicalName = t);
            hazardousMaterialPackingGroup.ReadMultiValue(v => package.HazardousMaterialPackingGroup = (int) v);
            hazardousMaterialQuantityValue.ReadMultiText(t => package.HazardousMaterialQuantityValue = ReadDoubleValue(t));
            packingDetailsAircraftOnly.ReadMultiCheck(t => package.PackingDetailsCargoAircraftOnly = t);
            packingInstructions.ReadMultiText(t => package.PackingDetailsPackingInstructions = t);
        }

        /// <summary>
        /// Read the value from a string and return the effective integer value
        /// </summary>
        private int ReadIntegerValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int value;
            if (int.TryParse(text, out value))
            {
                return value;
            }

            return 0;
        }


        /// <summary>
        /// Read the value from a string and return the effective double value
        /// </summary>
        private double ReadDoubleValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            double lengthValue;
            if (double.TryParse(text, out lengthValue))
            {
                return lengthValue;
            }

            return 0;
        }

        /// <summary>
        /// Updates the UI to enable/disable controls based on whether dangerous goods is enabled.
        /// </summary>
        private void UpdateUI()
        {
            panelDangerousGoodsDetails.Visible = dangerousGoodsEnabled.Checked;

            UpdateHazardousMaterialsUI();
        }

        /// <summary>
        /// Updates the hazardous materials UI.
        /// </summary>
        private void UpdateHazardousMaterialsUI()
        {
            bool showHazardousMaterialGroupBox = false;

            // The hazardous materials group box (and all its controls) is only enabled if the dangerous goods option is checked
            // and the material type is Hazardous Materials
            if (dangerousGoodsMaterialType.SelectedValue != null)
            {
                FedExDangerousGoodsMaterialType dangerousGoodsType = (FedExDangerousGoodsMaterialType) dangerousGoodsMaterialType.SelectedValue;

                showHazardousMaterialGroupBox = dangerousGoodsEnabled.Checked && dangerousGoodsType == FedExDangerousGoodsMaterialType.HazardousMaterials;
            }

            hazardousMaterialGroupBox.Visible = showHazardousMaterialGroupBox;
        }

        /// <summary>
        /// Called when [material type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnMaterialTypeChanged(object sender, EventArgs e)
        {
            UpdateHazardousMaterialsUI();
        }

        /// <summary>
        /// Called when [dangerous goods enabled changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnDangerousGoodsEnabledChanged(object sender, EventArgs e)
        {
            UpdateUI();

            DangerousGoodsChanged?.Invoke(this, e);
        }
    }
}
