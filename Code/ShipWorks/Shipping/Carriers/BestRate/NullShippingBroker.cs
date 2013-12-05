﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBroker interface that is an application of the 
    /// null object pattern. This is intended to be used for those shipping providers where 
    /// best rate is not applicable and/or not yet supported. 
    /// </summary>
    public class NullShippingBroker : IBestRateShippingBroker
    {
        /// <summary>
        /// Gets the rates for each of the accounts of a specific shipping provider based
        /// on the configuration of the best rate shipment data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="exceptionHandler"></param>
        /// <returns>An empty list of RateResult objects.</returns>
        public RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            return new RateGroup(new List<RateResult>());
        }

        /// <summary>
        /// Gets the insurance provider for the carrier.
        /// </summary>
        public InsuranceProvider GetInsuranceProvider(ShippingSettingsEntity settings)
        {
            return InsuranceProvider.Invalid;
        }

        /// <summary>
        /// Gets a value indicating whether there any accounts available to a broker.
        /// </summary>
        /// <value>
        /// <c>true</c> if the broker [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get
            {
                // Always return false since this is a null broker
                return false;
            }
        }
    }
}
