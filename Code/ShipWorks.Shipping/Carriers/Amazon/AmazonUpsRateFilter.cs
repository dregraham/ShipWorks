﻿using System;
using System.Linq;
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
            if (upsAccountRepository.Accounts.Any())
            {
                return rateGroup;
            }

            RateGroup newRateGroup = rateGroup.CopyWithRates(rateGroup.Rates.Where(r => ((AmazonRateTag) r.Tag).CarrierName.IndexOf("ups", StringComparison.OrdinalIgnoreCase) == -1));
            newRateGroup.AddFootnoteFactory(createFootnoteFactory(ShipmentTypeCode.UpsOnLineTools));

            return newRateGroup;
        }
    }
}