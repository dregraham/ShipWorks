using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public class AmazonUpsRateFilter : IAmazonRateGroupFilter
    {
        private readonly Func<ShipmentTypeCode, IAmazonNotLinkedFootnoteFactory> createFootnoteFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUpsRateFilter"/> class.
        /// </summary>
        public AmazonUpsRateFilter(ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository,
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
            if (upsAccountRepository.Accounts.Any())
            {
                return rateGroup;
            }

            RateGroup newRateGroup = rateGroup.CopyWithRates(rateGroup.Rates.Where(r => ((AmazonRateTag) r.Tag).CarrierName.IndexOf("ups", StringComparison.OrdinalIgnoreCase) == -1));
            newRateGroup.AddFootnoteFactory(createFootnoteFactory(ShipmentTypeCode.UpsOnLineTools));
            //newRateGroup.AddFootnoteFactory(new AmazonNotLinkedFootnoteFactory(ShipmentTypeCode.UpsOnLineTools));

            return newRateGroup;
        }
    }
}