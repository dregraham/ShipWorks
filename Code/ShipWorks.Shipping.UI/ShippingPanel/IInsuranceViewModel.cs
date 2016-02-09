using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Interface for view model for displaying and saving shipment insurance information
    /// </summary>
    public interface IInsuranceViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        /// <summary>
        /// Stream of property change events
        /// </summary>
        IObservable<string> PropertyChangeStream { get; }

        /// <summary>
        /// Shipment adapter
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Shipment selected package adapter
        /// </summary>
        IPackageAdapter SelectedPackageAdapter { get; set; }

        /// <summary>
        /// Shipment selected package adapter
        /// </summary>
        IInsuranceChoice InsuranceChoice { get; set; }

        /// <summary>
        /// Shipment list of package adapters
        /// </summary>
        IEnumerable<IPackageAdapter> PackageAdapters { get; set; }

        /// <summary>
        /// Sets the insurance label text value
        /// </summary>
        string InsuranceLabelDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        string InsuranceTypeLabelDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        string InsuranceValueLabelDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance caption text
        /// </summary>
        string InsuranceInfoTipCaptionText { get; set; }

        /// <summary>
        /// Sets the insurance cost label text
        /// </summary>
        string InsuranceCostDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance type label text value
        /// </summary>
        string InsuranceInfoTipDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance cost label text
        /// </summary>
        string InsuranceLinkDisplayText { get; set; }

        /// <summary>
        /// Sets the insurance link tag
        /// </summary>
        object InsuranceLinkTag { get; set; }

        /// <summary>
        /// Sets the visibility of the InfoTip
        /// </summary>
        Visibility InfoTipVisibility { get; set; }

        /// <summary>
        /// Sets the visibility of the cost
        /// </summary>
        Visibility CostVisibility { get; set; }

        /// <summary>
        /// Sets the visibility of the cost
        /// </summary>
        Visibility LinkVisibility { get; set; }

        /// <summary>
        /// RelayCommand for showing the insurance promo dialog
        /// </summary>
        ICommand ShowInsurancePromoDialogCommand { get; }

        /// <summary>
        /// Load based on package adapters for a shipment
        /// </summary>
        void Load(IEnumerable<IPackageAdapter> currentPackageAdapters, IPackageAdapter currentPackageAdapter, ICarrierShipmentAdapter currentShipmentAdapter);

        /// <summary>
        /// Shipment selected package adapter insurance value
        /// </summary>
        decimal DeclaredValue { get; set; }

        /// <summary>
        /// Is the package insured
        /// </summary>
        bool Insurance { get; set; }
    }
}