using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// View model for displaying and saving shipment insurance information
    /// </summary>
    public partial class InsuranceViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceViewModel"/> class.
        /// </summary>
        public InsuranceViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceViewModel(IShippingManager shippingManager) : this()
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Load based on package adapters for a shipment
        /// </summary>
        public void Load(IEnumerable<IPackageAdapter> currentPackageAdapters, IPackageAdapter currentPackageAdapter)
        {
            PackageAdapters = currentPackageAdapters;
            SelectedPackageAdapter = currentPackageAdapter;
            InsuranceChoice = SelectedPackageAdapter.InsuranceChoice;
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
