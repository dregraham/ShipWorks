using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Rating service for UPS when in best rates.
    /// </summary>
    public class UpsBestRateRatingService : UpsRatingService, IUpsBestRateRatingService
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsBestRateRatingService"/> class.
        /// </summary>
        public UpsBestRateRatingService(
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            UpsApiTransitTimeClient transitTimeClient,
            IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory,
            UpsShipmentType shipmentType,
            ILicenseService licenseService)
            : base(accountRepository, transitTimeClient, upsRateClientFactory, shipmentType, null)
        {
            this.licenseService = licenseService;
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

            UpsRatingMethod methodToUse;
            if (account.LocalRatingEnabled)
            {
                methodToUse = availableRatingMethods.Any(m => m == UpsRatingMethod.LocalWithApiFailover) ?
                    UpsRatingMethod.LocalWithApiFailover : 
                    UpsRatingMethod.LocalOnly;
            }
            else
            {
                if (availableRatingMethods.Any(m => m == UpsRatingMethod.ApiOnly))
                {
                    methodToUse = UpsRatingMethod.ApiOnly;
                }
                else
                {
                    throw new UpsException($"To get rates for account {account.Description}, local rating must be enabled.");
                }
            }

            return upsRateClientFactory[methodToUse];
        }

        /// <summary>
        /// Adds the footnote factory.
        /// </summary>
        protected override void AddUpsPromoFootnoteFactory(UpsAccountEntity account, RateGroup rateGroup)
        {
            // do nothing
        }
    }
}
