using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
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
        /// The shipment type code of this shipment adapter
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        bool SupportsAccounts { get; }

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
        int ServiceType { get; set; }

        /// <summary>
        /// Customs Items for the shipment
        /// </summary>
        EntityCollection<ShipmentCustomsItemEntity> CustomsItems { get; set; }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        IEnumerable<IPackageAdapter> GetPackageAdapters();

        /// <summary>
        /// Gets specific number of package adapters for the shipment.
        /// </summary>
        IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages);

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
        /// Delete the specified package from the shipment
        /// </summary>
        void DeletePackage(IPackageAdapter package);
    }
}