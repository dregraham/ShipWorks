﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// Applys shipping policies to possbily remove UPS rates.
    /// </summary>
    public class UpsBestRateRestrictionBrokerFilter : IShippingBrokerFilter
    {
        /// <summary>
        /// Filters the incoming list of brokers
        /// </summary>
        /// <param name="brokers">List of brokers that will be filtered</param>
        /// <returns>
        /// Filtered list of brokers
        /// </returns>
        /// <remarks>
        /// The incoming list of brokers should not be modified
        /// </remarks>
        public IEnumerable<IBestRateShippingBroker> Filter(IEnumerable<IBestRateShippingBroker> brokers)
        {
            List<IBestRateShippingBroker> listOfBrokers = brokers.ToList();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicense license = lifetimeScope.Resolve<ILicenseService>().GetLicenses().FirstOrDefault();
                license?.ApplyShippingPolicy(ShipmentTypeCode.BestRate, listOfBrokers);
            }

            return listOfBrokers;
        }
    }
}
