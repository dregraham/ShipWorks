using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using ShipWorks.Shipping.Services;

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
        /// Create a PurchaseLabelRequest from a shipment, packages and service code
        /// </summary>
        PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment, List<IPackageAdapter> packages, string serviceCode);

        /// <summary>
        /// Creates customs items for a ShipEngine request
        /// </summary>
        List<CustomsItem> CreateCustomsItems(ShipmentEntity shipment);

        /// <summary>
        /// Creates pacakges for a shipEngine
        /// </summary>
        List<ShipmentPackage> CreatePackages(List<IPackageAdapter> packages);
    }
}
