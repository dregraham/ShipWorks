using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Rating service for FedEx
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.FedEx)]
    public class FedExShipEngineRatingClient : IRatingService
    {
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly ShipmentType shipmentType;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipEngineRatingClient(
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipEngineWebClient shipEngineWebClient,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.FedEx];
            this.shipEngineWebClient = shipEngineWebClient;
            this.shipmentTypeManager = shipmentTypeManager;
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.FedEx);
        }

        /// <summary>
        /// Get rates from FedEx via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any FedEx accounts, so let the user know they need an account.
            if (!FedExAccountManager.AccountsReadOnly.Any())
            {
                throw new ShippingException("An account is required to view FedEx rates.");
            }
            
            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                List<IPackageAdapter> packages = GetPackages(shipment);
                
                var confirmationType = (FedExSignatureType) shipment.FedEx.Signature;
                request.Shipment.Confirmation = (AddressValidatingShipment.ConfirmationEnum) FedExUtility.ShipmentConfirmationMap(confirmationType);

                request.RateOptions.PackageTypes = GetPackageTypes(packages);
                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.FedEx).ConfigureAwait(false)).Result;

                var availableServiceTypeIds = shipmentType.GetAvailableServiceTypes().ToList();
                var availableServiceTypeApiValues = availableServiceTypeIds
                    .Cast<FedExServiceType>()
                    .Select(t => EnumHelper.GetApiValue(t))
                    .ToList();

                // Only need to add the Smart Post values if it's in the available types
                if (availableServiceTypeIds.Contains((int) FedExServiceType.SmartPost))
                {
                    var smartPostApiValues = Enum.GetValues(typeof(FedExSmartPostIndicia))
                        .Cast<FedExSmartPostIndicia>()
                        .Select(x => EnumHelper.GetApiValue(x));
                    availableServiceTypeApiValues.AddRange(smartPostApiValues);
                }

                rateShipmentResponse.RateResponse.Rates = rateShipmentResponse.RateResponse.Rates
                    .OrderBy(r => r.ShippingAmount.Amount)
                    .ToList();
                var rateGroupFactory = new FedExRateGroupFactory(availableServiceTypeIds);
                return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.FedEx, availableServiceTypeApiValues);
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }

        static string[] availablePackageTypes = new[] { "package", "fedex_pak", "fedex_tube", "fedex_envelope", "fedex_envelope_onerate", "fedex_extra_large_box_onerate", "fedex_large_box_onerate", "fedex_medium_box_onerate", "fedex_pak_onerate", "fedex_small_box_onerate", "fedex_tube_onerate" };

        private List<string> GetPackageTypes(List<IPackageAdapter> packages)
        {
            var package = packages.FirstOrDefault();
            var result = new List<string>();

            if (package != null)
            {
                var packageType = EnumHelper.GetApiValue((FedExPackagingType) package.PackagingType);
                result.Add(packageType);
                var oneRateType = packageType + "_onerate";
                if (availablePackageTypes.Contains(oneRateType))
                {
                    result.Add(oneRateType);
                }
            }
            else
            {
                result.AddRange(availablePackageTypes);
            }

            return result;
        }

        private List<IPackageAdapter> GetPackages(ShipmentEntity shipment)
        {
            return shipmentTypeManager.Get(shipment.ShipmentTypeCode).GetPackageAdapters(shipment).ToList();
        }
    }
}
