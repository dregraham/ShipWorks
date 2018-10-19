﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Adapter for carrier specific fields that are actually common across them
    /// </summary>
    public interface ICarrierShipmentAdapter
    {
        /// <summary>
        /// Id of the carrier account
        /// </summary>
        long? AccountId { get; set; }

        /// <summary>
        /// The shipment associated with this adapter
        /// </summary>
        ShipmentEntity Shipment { get; }

        /// <summary>
        /// The store associated with this shipment
        /// </summary>
        StoreEntity Store { get; }

        /// <summary>
        /// The shipment type code of this shipment adapter
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        bool SupportsAccounts { get; }

        /// <summary>
        /// Does this shipment support rate shopping?
        /// </summary>
        bool SupportsRateShopping { get; }

        /// <summary>
        /// Does this shipment type support multiple packages?
        /// </summary>
        bool SupportsMultiplePackages { get; }

        /// <summary>
        /// Is this shipment a domestic shipment?
        /// </summary>
        bool IsDomestic { get; }

        /// <summary>
        /// Updates shipment dynamic data, total weight, etc
        /// </summary>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        IDictionary<ShipmentEntity, Exception> UpdateDynamicData();

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        bool SupportsPackageTypes { get; }

        /// <summary>
        /// DateTime of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        DateTime ShipDate { get; set; }

        /// <summary>
        /// Total weight of the shipment
        /// </summary>
        double TotalWeight { get; }

        /// <summary>
        /// Content weight of the shipment
        /// </summary>
        double ContentWeight { get; set; }

        /// <summary>
        /// Service type selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        int ServiceType { get; set; }

        /// <summary>
        /// The name of the service type
        /// </summary>
        string ServiceTypeName { get; }

        /// <summary>
        /// Clone the shipment adapter and shipment
        /// </summary>
        ICarrierShipmentAdapter Clone();

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        IEnumerable<IPackageAdapter> GetPackageAdapters();

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        IEnumerable<IPackageAdapter> GetPackageAdaptersAndEnsureShipmentIsLoaded();

        /// <summary>
        /// List of customs items for the shipment
        /// </summary>
        IEnumerable<IShipmentCustomsItemAdapter> GetCustomsItemAdapters();

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        bool CustomsAllowed { get; }

        /// <summary>
        /// Update the insurance fields on the shipment
        /// </summary>
        void UpdateInsuranceFields(ShippingSettingsEntity shippingSettings);

        /// <summary>
        /// Select the service from the given rate
        /// </summary>
        void SelectServiceFromRate(RateResult rate);

        /// <summary>
        /// Add a new package adapter
        /// </summary>
        IPackageAdapter AddPackage();

        /// <summary>
        /// Add a new package adapter
        /// </summary>
        /// <param name="manipulateEntity">
        /// Pass in an action to manipulate the package that gets added to the shipment
        /// </param>
        IPackageAdapter AddPackage(Action<INotifyPropertyChanged> manipulateEntity);

        /// <summary>
        /// Delete the specified package from the shipment
        /// </summary>
        void DeletePackage(IPackageAdapter package);

        /// <summary>
        /// Delete the specified package from the shipment
        /// </summary>
        /// <param name="manipulateEntity">
        /// Pass in an action to manipulate the package that gets deleted from the shipment
        /// </param>
        void DeletePackage(IPackageAdapter package, Action<INotifyPropertyChanged> manipulateEntity);

        /// <summary>
        /// Add a new customs item
        /// </summary>
        IShipmentCustomsItemAdapter AddCustomsItem();

        /// <summary>
        /// Delete the specified customs item
        /// </summary>
        void DeleteCustomsItem(IShipmentCustomsItemAdapter customsItem);

        /// <summary>
        /// Send a notification if service related properties change
        /// </summary>
        IDisposable NotifyIfServiceRelatedPropertiesChange(Action<string> raisePropertyChanged);

        /// <summary>
        /// Does the given rate match the service selected for the shipment
        /// </summary>
        bool DoesRateMatchSelectedService(RateResult rate);

        /// <summary>
        /// For rates that are not selectable, find their first child that is.
        /// </summary>
        RateResult GetChildRateForRate(RateResult parentRate, IEnumerable<RateResult> rates);

        /// <summary>
        /// Update the total weight of the shipment based on its ContentWeight and any packaging weight.
        /// </summary>
        void UpdateTotalWeight();
    }
}