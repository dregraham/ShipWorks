using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// View model for displaying and saving shipment insurance information
    /// </summary>
    public partial class InsuranceViewModel 
    {
        private readonly PropertyChangedHandler handler;
        private readonly IShippingManager shippingManager;
        private readonly IShippingSettings shippingSettings;
        private IPackageAdapter selectedPackageAdapter;
        private IEnumerable<IPackageAdapter> packageAdapters;
        private IInsuranceChoice insuranceChoice;
        private ICarrierShipmentAdapter shipmentAdapter;
        private string insuranceLabelDisplayText;
        private string insuranceTypeLabelDisplayText;
        private string insuranceValueLabelDisplayText;
        private decimal insuranceValue;
        private IInsuranceUtility insuranceUtility;

        /// <summary>
        /// Shipment adapter
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICarrierShipmentAdapter ShipmentAdapter
        {
            get { return shipmentAdapter; }
            set { handler.Set(nameof(ShipmentAdapter), ref shipmentAdapter, value, true); }
        }

        /// <summary>
        /// Shipment selected package adapter
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IPackageAdapter SelectedPackageAdapter
        {
            get { return selectedPackageAdapter; }
            set
            {
                handler.Set(nameof(SelectedPackageAdapter), ref selectedPackageAdapter, value, true);

                // For None shipment type, and therefore NullPackageAdapter, these will be null
                if (SelectedPackageAdapter?.InsuranceChoice != null)
                {
                    InsuranceChoice = SelectedPackageAdapter.InsuranceChoice;

                    UpdateInsuranceLabelDisplayText();
                    UpdateInsuranceTypeLabelDisplayText();
                    UpdateInsuranceValueLabelDisplayText();
                }
            }
        }

        /// <summary>
        /// Shipment selected package adapter insurance value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal InsuranceValue
        {
            get { return insuranceValue; }
            set
            {
                handler.Set(nameof(InsuranceValue), ref insuranceValue, value, true);
                InsuranceChoice.InsuranceValue = insuranceValue;
                UpdateCostDisplay();
            }
        }

        /// <summary>
        /// Shipment selected package adapter insurance choice
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceChoice InsuranceChoice
        {
            get { return insuranceChoice; }
            set { handler.Set(nameof(InsuranceChoice), ref insuranceChoice, value, true); }
        }

        /// <summary>
        /// Shipment list of package adapters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IPackageAdapter> PackageAdapters
        {
            get { return packageAdapters; }
            set { handler.Set(nameof(PackageAdapters), ref packageAdapters, value, true); }
        }

        /// <summary>
        /// Sets the insurance label text value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceLabelDisplayText
        {
            get { return insuranceLabelDisplayText; }
            set { handler.Set(nameof(InsuranceLabelDisplayText), ref insuranceLabelDisplayText, value); }
        }

        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceTypeLabelDisplayText
        {
            get { return insuranceTypeLabelDisplayText; }
            set { handler.Set(nameof(InsuranceTypeLabelDisplayText), ref insuranceTypeLabelDisplayText, value); }
        }

        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceValueLabelDisplayText
        {
            get { return insuranceValueLabelDisplayText; }
            set { handler.Set(nameof(InsuranceValueLabelDisplayText), ref insuranceValueLabelDisplayText, value, true); }
        }

        
        private string insuranceInfoTipCaptionText;
        /// <summary>
        /// Sets the insurance caption text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceInfoTipCaptionText
        {
            get { return insuranceInfoTipCaptionText; }
            set { handler.Set(nameof(InsuranceInfoTipCaptionText), ref insuranceInfoTipCaptionText, value); }
        }


        private string insuranceCostDisplayText;
        /// <summary>
        /// Sets the insurance cost label text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceCostDisplayText
        {
            get { return insuranceCostDisplayText; }
            set { handler.Set(nameof(InsuranceCostDisplayText), ref insuranceCostDisplayText, value); }
        }

        private string insuranceInfoTipDisplayText;
        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceInfoTipDisplayText
        {
            get { return insuranceInfoTipDisplayText; }
            set { handler.Set(nameof(InsuranceInfoTipDisplayText), ref insuranceInfoTipDisplayText, value); }
        }

        private string insuranceLinkDisplayText;
        /// <summary>
        /// Sets the insurance cost label text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InsuranceLinkDisplayText
        {
            get { return insuranceLinkDisplayText; }
            set { handler.Set(nameof(InsuranceLinkDisplayText), ref insuranceLinkDisplayText, value); }
        }

        private object insuranceLinkTag;
        /// <summary>
        /// Sets the insurance link tag
        /// </summary>
        [Obfuscation(Exclude = true)]
        public object InsuranceLinkTag
        {
            get { return insuranceLinkTag; }
            set { handler.Set(nameof(InsuranceLinkTag), ref insuranceLinkTag, value); }
        }

        private Visibility infoTipVisibility;
        /// <summary>
        /// Sets the visibility of the InfoTip
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility InfoTipVisibility
        {
            get { return infoTipVisibility; }
            set { handler.Set(nameof(InfoTipVisibility), ref infoTipVisibility, value); }
        }

        private Visibility costVisibility;
        /// <summary>
        /// Sets the visibility of the cost
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility CostVisibility
        {
            get { return costVisibility; }
            set { handler.Set(nameof(CostVisibility), ref costVisibility, value); }
        }

        private Visibility linkVisibility;
        /// <summary>
        /// Sets the visibility of the cost
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility LinkVisibility
        {
            get { return linkVisibility; }
            set { handler.Set(nameof(LinkVisibility), ref linkVisibility, value); }
        }

        /// <summary>
        /// RelayCommand for showing the insurance promo dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand ShowInsurancePromoDialogCommand => new RelayCommand(ShowInsurancePromoDialog);




        /// <summary>
        /// Are all the insurance shipments FedEx?
        /// </summary>
        private bool AllFedExShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.FedEx);
        }

        /// <summary>
        /// Are all the insurance shipments Ups?
        /// </summary>
        private bool AllUpsShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType == ShipmentTypeCode.UpsOnLineTools ||
                                     (ShipmentTypeCode)c.Shipment.ShipmentType == ShipmentTypeCode.UpsWorldShip));
        }

        /// <summary>
        /// Are all the insurance shipments OnTrac?
        /// </summary>
        private bool AllOnTracShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.OnTrac);
        }

        /// <summary>
        /// Are all the insurance shipments iParcel?
        /// </summary>
        private bool AlliParcelShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.iParcel);
        }

        /// <summary>
        /// Are all the insurance shipments Endicia?
        /// </summary>
        private bool AllEndiciaShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.Endicia);
        }

        /// <summary>
        /// Are all the insurance shipments Usps?
        /// </summary>
        private bool AllUspsShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.Usps);
        }

        /// <summary>
        /// Are all choices a declared value type?  (UPS, FedEx, OnTrac, or iParcel)
        /// </summary>
        private bool AllDelcaredValueType(IEnumerable<IInsuranceChoice> choices)
        {
            return AllUpsShipments(choices) || AllFedExShipments(choices) || AllOnTracShipments(choices) || AlliParcelShipments(choices);
        }
    }
}
