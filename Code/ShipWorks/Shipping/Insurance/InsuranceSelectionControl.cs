using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Simple control for allowing selection of ShipWorks insurance
    /// </summary>
    public partial class InsuranceSelectionControl : UserControl
    {
        List<IInsuranceChoice> loadedInsurance = new List<IInsuranceChoice>();

        /// <summary>
        /// The user has edited\changed something about the insurance
        /// </summary>
        public event EventHandler InsuranceOptionsChanged;

        // So we know when not to raise the changed event
        bool loading = false;

        // The last valud value. This tracks if the value has changed.
        private decimal? lastValue = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceSelectionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the list of insurance into the control
        /// </summary>
        public void LoadInsuranceChoices(IEnumerable<IInsuranceChoice> choices)
        {
            loading = true;
            bool isMultiPackage = false;
            InsuranceProvider? insuranceProvider = null;

            loadedInsurance = choices.ToList();

            // Update the insurance status and amount used
            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (IInsuranceChoice choice in loadedInsurance)
                {
                    if (insuranceProvider == null)
                    {
                        insuranceProvider = choice.InsuranceProvider;
                    }
                    else
                    {
                        if (insuranceProvider != choice.InsuranceProvider)
                        {
                            insuranceProvider = InsuranceProvider.Invalid;
                        }
                    }

                    useInsurance.ApplyMultiCheck(choice.Insured);
                    insuredValue.ApplyMultiAmount(choice.InsuranceValue);

                    isMultiPackage = isMultiPackage || ShipmentTypeManager.GetType(choice.Shipment).GetPackageAdapters(choice.Shipment).Count() > 1;
                }
            }

            SetLabelTextValues(insuranceProvider, loadedInsurance);

            // Update the insurance cost calculation if there's only one selected
            if (loadedInsurance.Count == 1 && !isMultiPackage)
            {
                UpdateCostDisplay(loadedInsurance[0].Shipment, loadedInsurance[0].InsuranceValue);
            }
            else
            {
                ClearInsuranceCost();
            }

            loading = false;

            OnChangeUseInsurance(null, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the label text values for the insurance choice fields.
        /// </summary>
        private void SetLabelTextValues(InsuranceProvider? insuranceProvider, List<IInsuranceChoice> choices)
        {
            if (insuranceProvider == InsuranceProvider.ShipWorks)
            {
                labelInsurance.Text = "Insurance:";
                useInsurance.Text = "ShipWorks Insurance";
                labelValue.Text = "Insured value:";
            }
            else
            {
                labelInsurance.Text = "Protection:";

                // Default
                useInsurance.Text = "Shipment Protection:";
                labelValue.Text = "Shipment value:";

                if (insuranceProvider == InsuranceProvider.Carrier)
                {
                    if (InsuranceChoice.AllUpsShipments(choices) || InsuranceChoice.AllFedExShipments(choices) || InsuranceChoice.AllOnTracShipments(choices) || InsuranceChoice.AlliParcelShipments(choices))
                    {
                        // loadedInsurance will always have at least one value when insurance provider is not null
                        string carrierName = ShippingManager.GetCarrierName((ShipmentTypeCode) loadedInsurance.First().Shipment.ShipmentType);

                        useInsurance.Text = string.Format("{0} Declared Value", carrierName);
                        labelValue.Text = "Declared value:";
                    }
                    else if (InsuranceChoice.AllEndiciaShipments(choices))
                    {
                        useInsurance.Text = "Endicia Insurance";
                        labelValue.Text = "Insured value:";
                    }
                    else if (InsuranceChoice.AllUspsShipments(choices))
                    {
                        useInsurance.Text = UspsUtility.StampsInsuranceDisplayName;
                        labelValue.Text = "Insured value";
                    }
                }
            }
        }

        /// <summary>
        /// Save the data from the control to the given list of choices
        /// </summary>
        public void SaveToInsuranceChoices()
        {
            foreach (InsuranceChoice choice in loadedInsurance)
            {
                useInsurance.ReadMultiCheck(c => choice.Insured = c);
                insuredValue.ReadMultiAmount(v => choice.InsuranceValue = v);
            }
        }

        /// <summary>
        /// Changing whether to use ShipWorks insurance
        /// </summary>
        private void OnChangeUseInsurance(object sender, EventArgs e)
        {
            bool inputEnabled = useInsurance.CheckState != CheckState.Unchecked;

            insuredValue.Enabled = inputEnabled;
            labelCost.ForeColor = inputEnabled ? Color.Green : Color.DarkGray;

            OnInsuranceOptionsChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update the displayed insurance cost
        /// </summary>
        private void UpdateCostDisplay(ShipmentEntity shipment, decimal value)
        {
            // Get the cost 
            InsuranceCost cost = InsuranceUtility.GetInsuranceCost(shipment, value);

            if (shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks)
            {
                ShowCarrierCost(cost);
            }
            else
            {
                ShowShipWorksInsuranceCost(shipment, cost);
            }
        }

        /// <summary>
        /// Updates the controls to show the ShipWorks insurance cost.
        /// </summary>
        private void ShowShipWorksInsuranceCost(ShipmentEntity shipment, InsuranceCost cost)
        {
            // See if there is an info message to display
            if (cost.InfoMessage != null)
            {
                infoTip.Visible = true;
                labelCost.Left = infoTip.Right + 1;

                infoTip.Caption = cost.InfoMessage;
            }
            else
            {
                infoTip.Visible = false;
                labelCost.Left = infoTip.Left;
            }

            // If there is no SW cost for this shipment, clear it
            if (cost.ShipWorks == null)
            {
                labelCost.Visible = false;

                linkSavings.Visible = false;
                linkSavings.Left = labelCost.Left;
            }
            else
            {
                // Show the cost
                labelCost.Visible = true;
                labelCost.Text = string.Format("${0:0.00}", cost.ShipWorks);

                // Adjust savings position
                linkSavings.Left = labelCost.Right;

                if (cost.ShipWorks > 0)
                {
                    linkSavings.Visible = true;
                    linkSavings.Tag = cost;

                    // Only show savings if there is a savings
                    if (cost.Carrier.HasValue && cost.Carrier > cost.ShipWorks)
                    {
                        linkSavings.Text = string.Format("(Compare to ${0:0.00})", cost.Carrier);
                    }
                    else
                    {
                        linkSavings.Text = "(Learn more)";
                    }
                }
                else
                {
                    linkSavings.Visible = false;
                }
            }

            if (cost.ShipWorks == null || cost.ShipWorks == 0)
            {
                if (cost.AdvertisePennyOne)
                {
                    linkSavings.Visible = true;
                    linkSavings.Text = "Add coverage for the first $100";
                    linkSavings.Tag = (ShipmentTypeCode) shipment.ShipmentType;
                }
            }
        }

        /// <summary>
        /// Updates the controls to show the carrier cost.
        /// </summary>
        /// <param name="cost">The cost.</param>
        private void ShowCarrierCost(InsuranceCost cost)
        {
            infoTip.Visible = false;
            labelCost.Visible = false;

            if (cost.ShipWorks > 0 && cost.Carrier.HasValue && cost.Carrier > cost.ShipWorks)
            {
                linkSavings.Visible = true;
                linkSavings.Left = infoTip.Left;

                linkSavings.Text = string.Format("(Learn how to save ${0:0.00})", cost.Carrier - cost.ShipWorks);
                linkSavings.Tag = cost;
            }
            else
            {
                linkSavings.Visible = false;
            }
        }

        /// <summary>
        /// Clear the control of insurance cost information display
        /// </summary>
        public void ClearInsuranceCost()
        {
            infoTip.Visible = false;
            labelCost.Visible = false;
            linkSavings.Visible = false;
        }

        /// <summary>
        /// Clicking the savings link to view information about ShipWorks insurance
        /// </summary>
        private void OnClickSave(object sender, EventArgs e)
        {
            if (linkSavings.Tag is InsuranceCost || linkSavings.Tag == null)
            {
                ShipmentEntity shipment = loadedInsurance[0].Shipment;
                InsuranceCost cost = (InsuranceCost) linkSavings.Tag;

                using (InsuranceBenefitsDlg dlg = new InsuranceBenefitsDlg(cost, shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks))
                {
                    dlg.ShowDialog(this);

                    if (dlg.ShipWorksInsuranceEnabled)
                    {
                        ShippingSettingsEntity settings = ShippingSettings.Fetch();

                        ShipmentTypeCode shipmentType = (ShipmentTypeCode)shipment.ShipmentType;
                        
                        if (ShipmentTypeManager.IsFedEx(shipmentType))
                        {
                            settings.FedExInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                        }
                        else if (ShipmentTypeManager.IsUps(shipmentType))
                        {
                            settings.UpsInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                        }
                        else if (shipmentType == ShipmentTypeCode.OnTrac)
                        {
                            settings.OnTracInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                        }
                        else if (shipmentType == ShipmentTypeCode.iParcel)
                        {
                            settings.IParcelInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                        }
                        else if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia)
                        {
                            settings.EndiciaInsuranceProvider = (int) InsuranceProvider.ShipWorks;
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid ShipmentType unhandled in savings link: " + shipment.ShipmentType);
                        }

                        ShippingSettings.Save(settings);

                        SaveToInsuranceChoices();

                        // We also need to update the shipment to reflect the change, so that the UI picks it up on the reload
                        shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

                        LoadInsuranceChoices(loadedInsurance);
                    }
                }
            }
            // Otherwise a ShipmentTypeCode as a tag represents the PennyOne display
            else
            {
                var shipmentTypeCode = (ShipmentTypeCode) linkSavings.Tag;

                using (InsurancePennyOneDlg dlg = new InsurancePennyOneDlg(ShippingManager.GetCarrierName(shipmentTypeCode), true))
                {
                    dlg.ShowDialog(this);

                    if (dlg.PennyOne)
                    {
                        ShippingSettingsEntity settings = ShippingSettings.Fetch();

                        if (ShipmentTypeManager.IsFedEx(shipmentTypeCode))
                        {
                            settings.FedExInsurancePennyOne = true;
                        }
                        else if(ShipmentTypeManager.IsUps(shipmentTypeCode))
                        {
                            settings.UpsInsurancePennyOne = true;
                        }
                        else if (shipmentTypeCode == ShipmentTypeCode.OnTrac)
                        {
                            settings.OnTracInsurancePennyOne = true;
                        }
                        else if (shipmentTypeCode == ShipmentTypeCode.iParcel)
                        {
                            settings.IParcelInsurancePennyOne = true;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("linkSavings.Tag",shipmentTypeCode,"Invalid ShipmentTypeCode for PennyOne Insurance");
                        }

                        ShippingSettings.Save(settings);

                        SaveToInsuranceChoices();

                        // We also need to update the shipment to reflect the change, so that the UI picks it up on the reload
                        ShipmentEntity shipment = loadedInsurance[0].Shipment;
                        ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                        
                        // The penny one setting would apply to every package in the shipment
                        for (int index = 0; index < shipmentType.GetParcelCount(shipment); index++)
                        {
                            shipmentType.GetParcelDetail(shipment, index).Insurance.InsurancePennyOne = true;
                        }

                        LoadInsuranceChoices(loadedInsurance);
                    }
                }
            }
        }

        /// <summary>
        /// Controls whether inputting insured value is enabled
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InputEnabled
        {
            get
            {
                return insuredValue.Enabled;
            }
            set
            {
                insuredValue.Enabled = value;
                labelCost.ForeColor = value ? Color.Green : Color.DarkGray;
            }
        }

        /// <summary>
        /// The text label for the insurance value
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ValueText
        {
            get { return labelValue.Text; }
            set { labelValue.Text = value; }
        }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        private void OnTextChanged(object sender, EventArgs e)
        {
            bool valueChanged = false;

            if (loadedInsurance != null && loadedInsurance.Count == 1 && !insuredValue.MultiValued)
            {
                decimal amount;

                if (decimal.TryParse(insuredValue.Text, NumberStyles.Currency, null, out amount))
                {
                    if (lastValue.HasValue && amount != lastValue.Value)
                    {
                        valueChanged = true;
                    }

                    lastValue = amount;
                }
            }

            if (valueChanged)
            {
                UpdateCostDisplay(loadedInsurance[0].Shipment, lastValue.Value);
                SaveToInsuranceChoices();

                insuredValue.IgnoreSet = true;
                OnInsuranceOptionsChanged(this, EventArgs.Empty);
                insuredValue.IgnoreSet = false;
            }
        }

        /// <summary>
        /// Indicates that the user has changed insurance values 
        /// </summary>
        private void OnInsuranceOptionsChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }

            if (InsuranceOptionsChanged != null)
            {
                InsuranceOptionsChanged(this, EventArgs.Empty);
            }
        }
    }
}
