using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// View model for displaying and saving shipment insurance information
    /// </summary>
    public partial class InsuranceViewModel : IInsuranceViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceViewModel"/> class.
        /// </summary>
        public InsuranceViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            InsuranceInfoTipCaptionText = "ShipWorks Insurance";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceViewModel(IShippingManager shippingManager, IShippingSettings shippingSettings, IInsuranceUtility insuranceUtility) : this()
        {
            this.shippingManager = shippingManager;
            this.shippingSettings = shippingSettings;
            this.insuranceUtility = insuranceUtility;
        }

        /// <summary>
        /// Load based on package adapters for a shipment
        /// </summary>
        public void Load(IEnumerable<IPackageAdapter> currentPackageAdapters, IPackageAdapter currentPackageAdapter, ICarrierShipmentAdapter currentShipmentAdapter)
        {
            ShipmentAdapter = currentShipmentAdapter;
            PackageAdapters = currentPackageAdapters;
            SelectedPackageAdapter = currentPackageAdapter;
            InsuranceChoice = SelectedPackageAdapter.InsuranceChoice;
            InsuranceValue = InsuranceChoice.InsuranceValue;

            UpdateCostDisplay();
        }

        /// <summary>
        /// Update the displayed insurance cost
        /// </summary>
        private void UpdateCostDisplay()
        {
            if (PackageAdapters.Count() > 1)
            {
                InfoTipVisibility = Visibility.Collapsed;
                CostVisibility = Visibility.Collapsed;
                LinkVisibility = Visibility.Collapsed;
                return;
            }

            // Get the cost 
            InsuranceCost cost = insuranceUtility.GetInsuranceCost(shipmentAdapter.Shipment, InsuranceChoice.InsuranceValue);

            if (InsuranceChoice.InsuranceProvider != InsuranceProvider.ShipWorks)
            {
                ShowCarrierCost(cost);
            }
            else
            {
                ShowShipWorksInsuranceCost(cost);
            }
        }

        /// <summary>
        /// Updates the controls to show the carrier cost.
        /// </summary>
        /// <param name="cost">The cost.</param>
        private void ShowCarrierCost(InsuranceCost cost)
        {
            InfoTipVisibility = Visibility.Collapsed;
            CostVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Visible;

            if (cost.ShipWorks > 0 && cost.Carrier.HasValue && cost.Carrier > cost.ShipWorks)
            {
                CostVisibility = Visibility.Visible;

                InsuranceLinkDisplayText = string.Format("(Learn how to save ${0:0.00})", cost.Carrier - cost.ShipWorks);
                InsuranceLinkTag = cost;
            }
            else
            {
                InsuranceLinkDisplayText = string.Empty;
            }
        }

        /// <summary>
        /// Updates the controls to show the ShipWorks insurance cost.
        /// </summary>
        private void ShowShipWorksInsuranceCost(InsuranceCost cost)
        {
            // See if there is an info message to display
            if (cost.InfoMessage != null)
            {
                InfoTipVisibility = Visibility.Visible;
                InsuranceInfoTipDisplayText = cost.InfoMessage;
            }
            else
            {
                InfoTipVisibility = Visibility.Collapsed;
            }

            // If there is no SW cost for this shipment, clear it
            if (cost.ShipWorks == null)
            {
                CostVisibility = Visibility.Collapsed;
                LinkVisibility = Visibility.Collapsed;
            }
            else
            {
                // Show the cost
                CostVisibility = Visibility.Visible;
                InsuranceCostDisplayText = string.Format("${0:0.00}", cost.ShipWorks);

                if (cost.ShipWorks > 0)
                {
                    LinkVisibility = Visibility.Visible;
                    InsuranceLinkTag = cost;

                    // Only show savings if there is a savings
                    if (cost.Carrier.HasValue && cost.Carrier > cost.ShipWorks)
                    {
                        InsuranceLinkDisplayText = string.Format("(Compare to ${0:0.00})", cost.Carrier);
                    }
                    else
                    {
                        InsuranceLinkDisplayText = "(Learn more)";
                    }
                }
                else
                {
                    LinkVisibility = Visibility.Collapsed;
                    InsuranceLinkDisplayText = string.Empty;
                }
            }

            if ((cost.ShipWorks == null || cost.ShipWorks == 0) && cost.AdvertisePennyOne)
            {
                LinkVisibility = Visibility.Visible;
                InsuranceLinkDisplayText = "Add coverage for the first $100";
                InsuranceLinkTag = (ShipmentTypeCode)ShipmentAdapter.Shipment.ShipmentType;
            }
        }

        protected void ShowInsurancePromoDialog()
        {
            if (InsuranceLinkTag is InsuranceCost || InsuranceLinkTag == null)
            {
                ShowInsuranceBenefitsDialog();
            }
            // Otherwise a ShipmentTypeCode as a tag represents the PennyOne display
            else
            {
                ShowInsurancePennyOneDialog();
            }
        }

        /// <summary>
        /// Show the penny one dialog
        /// </summary>
        private void ShowInsurancePennyOneDialog()
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) InsuranceLinkTag;
            insuranceUtility.ShowInsurancePennyOneDlg(shipmentTypeCode);
        }

        /// <summary>
        /// Show the ShipWorks insurance benefits dialog
        /// </summary>
        private void ShowInsuranceBenefitsDialog()
        {
            ShipmentEntity shipment = ShipmentAdapter.Shipment;
            InsuranceCost insuranceCost = (InsuranceCost) InsuranceLinkTag;

            insuranceUtility.ShowInsuranceBenefitsDlg(shipment, insuranceCost);
        }

        /// <summary>
        /// Update the insurance label text
        /// </summary>
        private void UpdateInsuranceLabelDisplayText()
        {
            string value = "Protection:";
            if (InsuranceChoice?.InsuranceProvider == InsuranceProvider.ShipWorks)
            {
                value = "Insurance:";
            }

            InsuranceLabelDisplayText = value;
        }

        /// <summary>
        /// Update the insurance type label text
        /// </summary>
        private void UpdateInsuranceTypeLabelDisplayText()
        {
            string value = "Shipment Protection";

            // InsuranceChoice will be null for None shipment type (NullPackageAdapter)
            if (InsuranceChoice == null)
            {
                InsuranceTypeLabelDisplayText = value;
                return;
            }

            if (InsuranceChoice.InsuranceProvider == InsuranceProvider.ShipWorks)
            {
                value = "ShipWorks Insurance";
            }

            if (InsuranceChoice.InsuranceProvider == InsuranceProvider.Carrier)
            {
                List<IInsuranceChoice> choices = PackageAdapters.Select(pa => pa.InsuranceChoice).ToList();

                if (AllDelcaredValueType(choices))
                {
                    // loadedInsurance will always have at least one value when insurance provider is not null
                    string carrierName = shippingManager.GetCarrierName(SelectedPackageAdapter.InsuranceChoice.Shipment.ShipmentTypeCode);

                    value = $"{carrierName} Declared Value";
                }

                if (AllEndiciaShipments(choices))
                {
                    value = "Endicia Insurance";
                }

                if (AllUspsShipments(choices))
                {
                    value = "Stamps.com Insurance";
                }
            }

            InsuranceTypeLabelDisplayText = value;
        }

        /// <summary>
        /// Update the insurance value label text
        /// </summary>
        private void UpdateInsuranceValueLabelDisplayText()
        {
            // Default
            string value = "Shipment value:";

            if (InsuranceChoice.InsuranceProvider == InsuranceProvider.ShipWorks)
            {
                value = "Insured value:";
            }
            else
            {
                List<IInsuranceChoice> choices = PackageAdapters.Select(pa => pa.InsuranceChoice).ToList();

                if (InsuranceChoice.InsuranceProvider == InsuranceProvider.Carrier)
                {
                    if (AllUpsShipments(choices) || AllFedExShipments(choices) || AllOnTracShipments(choices) || AlliParcelShipments(choices))
                    {
                        value = "Declared value:";
                    }
                    else if (AllEndiciaShipments(choices))
                    {
                        value = "Insured value:";
                    }
                    else if (AllUspsShipments(choices))
                    {
                        value = "Insured value";
                    }
                }
            }

            InsuranceValueLabelDisplayText = value;
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            /* Add disposals if needed.  */
        }
    }
}
