﻿using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using System;
using ShipWorks.Data.Model.EntityInterfaces;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Builds a collection of UPS Services
    /// </summary>
    public class UpsShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly Func<ShipmentEntity, IUpsServiceManagerFactory> serviceManagerFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;
        private readonly UpsOltShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentServicesBuilder(UpsOltShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository,
            Func<ShipmentEntity, IUpsServiceManagerFactory> serviceManagerFactory, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository)
        {
            this.shipmentType = shipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.serviceManagerFactory = serviceManagerFactory;
            this.accountRepository = accountRepository;
        }

        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> overriddenShipments = shipments.Select(ShippingManager.GetOverriddenStoreShipment).ToList();

            // If a distinct on country code only returns a count of 1, all countries are the same
            bool allSameCountry = overriddenShipments.Select(s => string.Format("{0} {1}", s.AdjustedShipCountryCode(), s.AdjustedOriginCountryCode())).Distinct().Count() == 1;

            // If they are all of the same service class, we can load the service classes
            if (allSameCountry)
            {
                ShipmentEntity overriddenShipment = overriddenShipments.First();
                IUpsServiceManager carrierServiceManager = serviceManagerFactory(overriddenShipment).Create(overriddenShipment);

                string country = overriddenShipment.AdjustedShipCountryCode();

                IEnumerable<UpsServiceType> availableServices = shipmentType.GetAvailableServiceTypes(excludedServiceTypeRepository).Select(s => (UpsServiceType) s).ToList();

                var upsAccounts = accountRepository.AccountsReadOnly;
                if (upsAccounts.Any() && upsAccounts.None(a => string.IsNullOrEmpty(a.ShipEngineCarrierId)))
                {
                    // All UPS accounts are using ShipEngine, so only show the services supported by it

                    availableServices = availableServices.Where(s => UpsShipEngineServiceTypeUtility.IsServiceSupported(s, country));
                }

                // Get a list of service types that are valid for the overriddenShipments
                List<UpsServiceType> validServiceTypes = carrierServiceManager.GetServices(overriddenShipment)
                    .Select(s => s.UpsServiceType).ToList();

                // only include service types that are valid and enabled (availalbeServices)
                List<UpsServiceType> upsServiceTypesToLoad = validServiceTypes.Intersect(availableServices).ToList();

                if (shipments.Any())
                {
                    // Always include the service type that the shipment is currently configured in the
                    // event the shipment was configured prior to a service being excluded
                    // Always include the service that the shipments are currently configured with
                    // Only if the ServiceType is valid for the shipment type
                    IEnumerable<UpsServiceType> loadedServices = shipments.Select(s => (UpsServiceType)s.Ups.Service)
                        .Intersect(validServiceTypes).Distinct();
                    upsServiceTypesToLoad = upsServiceTypesToLoad.Union(loadedServices).ToList();
                }

                return upsServiceTypesToLoad
                    .ToDictionary(type => (int)type, type => EnumHelper.GetDescription(type));
            }

            return new Dictionary<int, string>();
        }
    }
}