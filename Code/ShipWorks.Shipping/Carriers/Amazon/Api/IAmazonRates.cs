using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets Amazon Rates
    /// </summary>
    public interface IAmazonRates
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
