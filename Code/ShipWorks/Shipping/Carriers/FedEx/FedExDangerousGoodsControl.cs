using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public partial class FedExDangerousGoodsControl : UserControl
    {
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

            emergencyContactPhone.ApplyMultiText(package.DangerousGoodsEmergencyContactPhone);
            offeror.ApplyMultiText(package.DangerousGoodsOfferor);

            hazardousMaterialId.ApplyMultiText(package.HazardousMaterialNumber);
            hazardClass.ApplyMultiText(package.HazardousMaterialClass);
            hazardousMaterialProperName.ApplyMultiText(package.HazardousMaterialProperName);
            hazardousMaterialTechnicalName.ApplyMultiText(package.HazardousMaterialTechnicalName);
            hazardousMaterialPackingGroup.ApplyMultiValue((FedExHazardousMaterialsPackingGroup)package.HazardousMaterialPackingGroup);
            hazardousMaterialQuantityValue.ApplyMultiText(package.HazardousMaterialQuantityValue.ToString());

            UpdateUI();
        }

        /// <summary>
        /// Saves the state of the controls to the package entities.
        /// </summary>
        /// <param name="packages">The packages.</param>
        [NDependIgnoreLongMethod]
        public void SaveToPackage(IEnumerable<FedExPackageEntity> packages)
        {
            foreach (FedExPackageEntity package in packages)
            {
                dangerousGoodsEnabled.ReadMultiCheck(c => package.DangerousGoodsEnabled = c);

                dangerousGoodsMaterialType.ReadMultiValue(v => package.DangerousGoodsType = (int)v);
                dangerousGoodsAccessibility.ReadMultiValue(v => package.DangerousGoodsAccessibilityType = (int)v);
                dangerousGoodsCargoAircraftOnly.ReadMultiCheck(c => package.DangerousGoodsCargoAircraftOnly = c);
                dangerousGoodsPackagingCounts.ReadMultiText(t => package.DangerousGoodsPackagingCount = ReadIntegerValue(t));
                dangerousGoodsPackagingUnits.ReadMultiValue(v => package.HazardousMaterialQuanityUnits = (int)v);

                offeror.ReadMultiText(t => package.DangerousGoodsOfferor = t);
                emergencyContactPhone.ReadMultiText(t => package.DangerousGoodsEmergencyContactPhone = t);

                hazardousMaterialId.ReadMultiText(t => package.HazardousMaterialNumber = t);
                hazardClass.ReadMultiText(t => package.HazardousMaterialClass = t);
                hazardousMaterialProperName.ReadMultiText(t => package.HazardousMaterialProperName = t);
                hazardousMaterialTechnicalName.ReadMultiText(t => package.HazardousMaterialTechnicalName = t);
                hazardousMaterialPackingGroup.ReadMultiValue(v => package.HazardousMaterialPackingGroup = (int)v);
                hazardousMaterialQuantityValue.ReadMultiText(t => package.HazardousMaterialQuantityValue = ReadDoubleValue(t));

            }
        }

        /// <summary>
        /// Read the value from a tstring and return the effective integer value
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
        }

    }
}
