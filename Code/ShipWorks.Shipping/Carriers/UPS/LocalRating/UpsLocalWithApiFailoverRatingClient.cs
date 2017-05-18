using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Rating client which attempts to get local rates and fails over to API rates
    /// </summary>
    public class UpsLocalWithApiFailOverRatingClient : IUpsRateClient
    {
        private readonly IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalWithApiFailOverRatingClient"/> class.
        /// </summary>
        public UpsLocalWithApiFailOverRatingClient(IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory)
        {
            this.upsRateClientFactory = upsRateClientFactory;
        }

        /// <summary>
        /// Attempt to get local rates. If that fails return API rates.
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            GenericResult<List<UpsServiceRate>> results;

            try
            {
                results = upsRateClientFactory[UpsRatingMethod.LocalOnly].GetRates(shipment);
            }
            catch (UpsLocalRatingException e)
            {
                results = GenericResult.FromError<List<UpsServiceRate>>(e.Message);
            }

            if (results.Failure || (results.Value?.None() ?? true))
            {
                results = upsRateClientFactory[UpsRatingMethod.ApiOnly].GetRates(shipment);
            }

            return results;
        }
    }
}