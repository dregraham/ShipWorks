﻿using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An factory interface for creating instances of IBestRateShippingBroker.
    /// </summary>
    public interface IBestRateShippingBrokerFactory
    {
        /// <summary>
        /// Creates all of the best rate shipping brokers available in the system.
        /// </summary>
        IEnumerable<IBestRateShippingBroker> CreateBrokers();
    }
}
