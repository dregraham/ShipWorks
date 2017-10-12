﻿using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory to create a RateShipmentRequest from a ShipmentEntity
    /// </summary>
    public interface IShipmentElementFactory
    {
        /// <summary>
        ///  Create a RateShipmentRequest from a ShipmentEntity
        /// </summary>
        RateShipmentRequest CreateRateRequest(ShipmentEntity shipment);

        /// <summary>
        /// Creates customs for a ShipEngine request
        /// </summary>
        InternationalOptions CreateCustoms(ShipmentEntity shipment, IShipEngineShipment shipEngineShipment);
        List<ShipmentPackage> CreatePackages(List<IPackageAdapter> packages);
    }
}
