using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using ShipWorks.Shipping.Services;
using System;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory to create a RateShipmentRequest from a ShipmentEntity
    /// </summary>
    public interface IShipEngineRequestFactory
    {
        /// <summary>
        ///  Create a RateShipmentRequest from a ShipmentEntity
        /// </summary>
        RateShipmentRequest CreateRateRequest(ShipmentEntity shipment);

        /// <summary>
        /// Create a PurchaseLabelWithoutShipmentRequest
        /// </summary>
        PurchaseLabelWithoutShipmentRequest CreatePurchaseLabelWithoutShipmentRequest(ShipmentEntity shipment);

        /// <summary>
        /// Create a PurchaseLabelRequest from a shipment, packages and service code
        /// </summary>
        PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment, List<IPackageAdapter> packages, string serviceCode, 
            Func<IPackageAdapter, string> getPackageCode, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance);

        /// <summary>
        /// Creates customs items for a ShipEngine request
        /// </summary>
        List<CustomsItem> CreateCustomsItems(ShipmentEntity shipment);

        /// <summary>
        /// Creates pacakges for a shipEngine
        /// </summary>
        List<ShipmentPackage> CreatePackageForRating(List<IPackageAdapter> packages, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance);

        /// <summary>
        /// Creates pacakges for a shipEngine
        /// </summary>
        List<ShipmentPackage> CreatePackageForLabel(List<IPackageAdapter> packages,
            Func<IPackageAdapter, string> getPackageCode, Action<ShipmentPackage, IPackageAdapter> addPackageInsurance);
    }
}
