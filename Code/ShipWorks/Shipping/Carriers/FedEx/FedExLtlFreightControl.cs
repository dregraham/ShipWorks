using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.FedEx;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Control to manage shipments LTL freight properties.
    /// </summary>
    public partial class FedExLtlFreightControl : UserControl
    {
        private readonly ImmutableList<FedExLtlSpecialServicesControlContainer> specialServicesControls;

        /// <summary>
        /// Some part of the packaging has changed the rate criteria
        /// </summary>
        public event EventHandler RateCriteriaChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLtlFreightControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FedExFreightShipmentRoleType>(role);
            EnumHelper.BindComboBox<FedExFreightCollectTermsType>(collectTerms);
            EnumHelper.BindComboBox<FedExFreightClassType>(freightClass);
            EnumHelper.BindComboBox<FedExFreightGuaranteeType>(freightGuaranteeType);

            specialServicesControls = ImmutableList.Create(
                new FedExLtlSpecialServicesControlContainer(callBeforeDelivery, FedExFreightSpecialServicesType.CallBeforeDelivery),
                new FedExLtlSpecialServicesControlContainer(freezableProtection, FedExFreightSpecialServicesType.FreezableProtection),
                new FedExLtlSpecialServicesControlContainer(limitedAccessPickup, FedExFreightSpecialServicesType.LimitedAccessPickup),
                new FedExLtlSpecialServicesControlContainer(limitedAccessDelivery, FedExFreightSpecialServicesType.LimitedAccessDelivery),
                new FedExLtlSpecialServicesControlContainer(poison, FedExFreightSpecialServicesType.Poison),
                new FedExLtlSpecialServicesControlContainer(food, FedExFreightSpecialServicesType.Food),
                new FedExLtlSpecialServicesControlContainer(doNotStackPallets, FedExFreightSpecialServicesType.DoNotStackPallets),
                new FedExLtlSpecialServicesControlContainer(doNotBreakDownPallets, FedExFreightSpecialServicesType.DoNotBreakDownPallets),
                new FedExLtlSpecialServicesControlContainer(topLoad, FedExFreightSpecialServicesType.TopLoad),
                new FedExLtlSpecialServicesControlContainer(extremeLength, FedExFreightSpecialServicesType.ExtremeLength),
                new FedExLtlSpecialServicesControlContainer(liftgateAtDelivery, FedExFreightSpecialServicesType.LiftgateAtDelivery),
                new FedExLtlSpecialServicesControlContainer(liftgateAtPickup, FedExFreightSpecialServicesType.LiftgateAtPickup),
                new FedExLtlSpecialServicesControlContainer(insideDelivery, FedExFreightSpecialServicesType.InsideDelivery),
                new FedExLtlSpecialServicesControlContainer(insidePickup, FedExFreightSpecialServicesType.InsidePickup),
                new FedExLtlSpecialServicesControlContainer(freightGuarantee, FedExFreightSpecialServicesType.FreightGuarantee));

            freightClass.SelectedValueChanged += OnRateCriteriaChanged;
            role.SelectedValueChanged += OnRateCriteriaChanged;
            collectTerms.SelectedValueChanged += OnRateCriteriaChanged;

            foreach (var ctrl in specialServicesControls.Select(ssc => ssc.Control))
            {
                ctrl.CheckedChanged += OnRateCriteriaChanged;
            }
        }

        /// <summary>
        /// Load all the shipment details
        /// </summary>
        public void LoadShipmentDetails(IEnumerable<ShipmentEntity> shipments)
        {
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    FedExShipmentEntity fedEx = shipment.FedEx;

                    totalHandlingUnits.ApplyMultiText(fedEx.FreightTotalHandlinUnits.ToString());
                    role.ApplyMultiValue(fedEx.FreightRole);
                    collectTerms.ApplyMultiValue(fedEx.FreightCollectTerms);
                    freightClass.ApplyMultiValue(fedEx.FreightClass);
                    freightGuaranteeType.ApplyMultiValue(fedEx.FreightGuaranteeType);
                    freightGuaranteeDate.ApplyMultiDate(fedEx.FreightGuaranteeDate);

                    FedExFreightSpecialServicesType currentSpecialServicesType = (FedExFreightSpecialServicesType) fedEx.FreightSpecialServices;
                    foreach (var item in specialServicesControls)
                    {
                        item.Control.ApplyMultiCheck(currentSpecialServicesType.HasFlag(item.SpecialServicesType));
                    }
                }

                UpdateFreightGuaranteeUI();
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public void SaveToShipments(IEnumerable<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                FedExShipmentEntity fedEx = shipment.FedEx;

                totalHandlingUnits.ReadMultiText(t => { int count; if (int.TryParse(t, out count)) shipment.FedEx.FreightTotalHandlinUnits = count; });
                role.ReadMultiValue(c => shipment.FedEx.FreightRole = (FedExFreightShipmentRoleType) c);
                collectTerms.ReadMultiValue(c => shipment.FedEx.FreightCollectTerms = (FedExFreightCollectTermsType) c);
                freightClass.ReadMultiValue(c => shipment.FedEx.FreightClass = (FedExFreightClassType) c);
                freightGuaranteeType.ReadMultiValue(x => shipment.FedEx.FreightGuaranteeType = (FedExFreightGuaranteeType) x);
                freightGuaranteeDate.ReadMultiDate(x => shipment.FedEx.FreightGuaranteeDate = x);

                fedEx.FreightSpecialServices = SaveSpecialServices((int) fedEx.FreightSpecialServices, specialServicesControls);
            }
        }

        /// <summary>
        /// Save special services for the given control map
        /// </summary>
        private int SaveSpecialServices(int currentSpecialServicesValue, IEnumerable<FedExLtlSpecialServicesControlContainer> map) =>
            map.Aggregate(currentSpecialServicesValue, ReadSpecialServiceFromControl);


        /// <summary>
        /// Read the special services value from the control
        /// </summary>
        private int ReadSpecialServiceFromControl(int currentSpecialServicesValue, FedExLtlSpecialServicesControlContainer item)
        {
            item.Control.ReadMultiCheck(c => currentSpecialServicesValue = ApplySpecialServicesType(c, currentSpecialServicesValue, item.SpecialServicesType));
            return currentSpecialServicesValue;
        }

        /// <summary>
        /// Turn the given specialServicesTypes on or off depending on the enabled state given
        /// </summary>
        private int ApplySpecialServicesType(bool enabled, int previous, FedExFreightSpecialServicesType specialServicesTypes) =>
            enabled ?
                previous | (int) specialServicesTypes :
                previous & ~(int) specialServicesTypes;

        /// <summary>
        /// Rate criteria changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            // Raise the rate criteria changed event
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Raises the RateCriteriaChanged event
        /// </summary>
        private void RaiseRateCriteriaChanged()
        {
            RateCriteriaChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                freightClass.SelectedValueChanged -= OnRateCriteriaChanged;
                role.SelectedValueChanged -= OnRateCriteriaChanged;
                collectTerms.SelectedValueChanged -= OnRateCriteriaChanged;

                foreach (var ctrl in specialServicesControls.Select(ssc => ssc.Control))
                {
                    ctrl.CheckedChanged -= OnRateCriteriaChanged;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Handle the Freight Guarantee check changed
        /// </summary>
        private void OnFreightGuaranteeCheckedChanged(object sender, EventArgs e) =>
            UpdateFreightGuaranteeUI();

        /// <summary>
        /// Update the freight guarantee UI
        /// </summary>
        private void UpdateFreightGuaranteeUI()
        {
            freightGuaranteePanel.Enabled = freightGuarantee.Checked;
        }
    }

    /// <summary>
    /// Special services control type map
    /// </summary>
    internal struct FedExLtlSpecialServicesControlContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLtlSpecialServicesControlContainer(CheckBox control, FedExFreightSpecialServicesType specialServiceType)
        {
            Control = control;
            SpecialServicesType = specialServiceType;
        }

        /// <summary>
        /// Check box
        /// </summary>
        public CheckBox Control { get; }

        /// <summary>
        /// Special Services  type
        /// </summary>
        public FedExFreightSpecialServicesType SpecialServicesType { get; }
    }
}
