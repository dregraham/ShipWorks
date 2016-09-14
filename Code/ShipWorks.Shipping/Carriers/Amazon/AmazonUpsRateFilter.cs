using System;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon rate filter to remove UPS if necessary
    /// </summary>
    public class AmazonUpsRateFilter : IAmazonRateGroupFilter
    {
        private readonly Func<ShipmentTypeCode, IAmazonNotLinkedFootnoteFactory> createFootnoteFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUpsRateFilter"/> class.
        /// </summary>
        public AmazonUpsRateFilter(ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository,
            Func<ShipmentTypeCode, IAmazonNotLinkedFootnoteFactory> createFootnoteFactory)
        {
            this.upsAccountRepository = upsAccountRepository;
            this.createFootnoteFactory = createFootnoteFactory;
        }

        /// <summary>
        /// Filters the specified rate group.
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            if (upsAccountRepository.Accounts.Any() || rateGroup.Rates.None(IsUpsRate))
            {
                return rateGroup;
            }

            RateGroup newRateGroup = rateGroup.CopyWithRates(rateGroup.Rates.Where(r => !IsUpsRate(r)));
            newRateGroup.AddFootnoteFactory(createFootnoteFactory(ShipmentTypeCode.UpsOnLineTools));

            return newRateGroup;
        }

        /// <summary>
        /// Is the rate an Amazon UPS rate
        /// </summary>
        private bool IsUpsRate(RateResult rate)
        {
            AmazonRateTag tag = rate.Tag as AmazonRateTag;
            if (tag == null)
            {
                return false;
            }

            return tag.CarrierName.IndexOf("ups", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}