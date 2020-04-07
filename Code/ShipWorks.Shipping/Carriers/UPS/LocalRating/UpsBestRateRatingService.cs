using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Ups.ShipEngine;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Rating service for UPS when in best rates.
    /// </summary>
    public class UpsBestRateRatingService : UpsRatingService, IUpsBestRateRatingService
    {
        private readonly ILicenseService licenseService;
        private readonly IUpsRateClientFactory rateClientFactory;
        private UpsRatingMethod ratingMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsBestRateRatingService"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public UpsBestRateRatingService(
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            UpsApiTransitTimeClient transitTimeClient,
            UpsShipmentType shipmentType,
            ILicenseService licenseService,
            IUpsRateClientFactory rateClientFactory,
            IUpsShipEngineRatingService shipEngineRatingService)
            : base(accountRepository, transitTimeClient, shipmentType, rateClientFactory, shipEngineRatingService)
        {
            this.licenseService = licenseService;
            this.rateClientFactory = rateClientFactory;
        }

        /// <summary>
        /// Gets the ups rate client.
        /// </summary>
        protected override IUpsRateClient GetRatingClient(UpsAccountEntity account)
        {
            List<UpsRatingMethod> availableRatingMethods = EnumHelper.GetEnumList<UpsRatingMethod>()
                .Select(entry => entry.Value)
                .ToList();

            // Filter out any rating methods unavailable to the user
            licenseService.GetLicenses()?.FirstOrDefault()?.ApplyShippingPolicy(ShipmentTypeCode.BestRate, availableRatingMethods);

            if (account.LocalRatingEnabled)
            {
                ratingMethod = availableRatingMethods.Any(m => m == UpsRatingMethod.LocalWithApiFailover) ?
                    UpsRatingMethod.LocalWithApiFailover :
                    UpsRatingMethod.LocalOnly;
            }
            else
            {
                if (availableRatingMethods.Any(m => m == UpsRatingMethod.ApiOnly))
                {
                    ratingMethod = UpsRatingMethod.ApiOnly;
                }
                else
                {
                    throw new UpsBestRateRatingException();
                }
            }

            return rateClientFactory.GetClient(ratingMethod);
        }

        /// <summary>
        /// Gets the rate result for the shipment
        /// </summary>
        protected override GenericResult<List<UpsServiceRate>> GetRateResult(ShipmentEntity shipment,
            UpsAccountEntity account)
        {
            GenericResult<List<UpsServiceRate>> rateResult = base.GetRateResult(shipment, account);

            if (ratingMethod == UpsRatingMethod.LocalOnly && rateResult.Failure)
            {
                throw new UpsLocalRatingException(rateResult.Message);
            }

            return rateResult;
        }
    }
}
