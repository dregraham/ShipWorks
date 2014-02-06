﻿using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Extra information for a best rate rate result
    /// </summary>
    public class BestRateResultTag
    {
        /// <summary>
        /// Original value of the rate result's tag
        /// </summary>
        public object OriginalTag { get; set; }

        /// <summary>
        /// Identifier for the best rate rate result carrier and service type
        /// </summary>
        public string ResultKey { get; set; }

        /// <summary>
        /// Delegate that will be executed when a rate is selected
        /// </summary>
        public Action<ShipmentEntity> RateSelectionDelegate { get; set; }

        /// <summary>
        /// Function that will be executed when an account doesn't exist for a carrier, and the 
        /// new carrier setup wizard needs displayed.
        /// </summary>
        public Func<bool> SignUpAction { get; set; }

        /// <summary>
        /// Gets or sets the account description.
        /// </summary>
        public string AccountDescription { get; set; }
    }
}
