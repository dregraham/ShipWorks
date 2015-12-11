using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// View model for displaying and saving shipment insurance information
    /// </summary>
    public partial class InsuranceViewModel 
    {
        private readonly PropertyChangedHandler handler;
        private readonly IShippingManager shippingManager;
        private IPackageAdapter selectedPackageAdapter;
        private IEnumerable<IPackageAdapter> packageAdapters;
        private IInsuranceChoice insuranceChoice;
        private string insuranceLabelDisplayText;
        private string insuranceTypeLabelDisplayText;
        private string insuranceValueLabelDisplayText;

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
        /// Shipment selected package adapter
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
