using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// A factory for creating filters that will be applied to a rate group.
    /// </summary>
    public interface IRateGroupFilterFactory
    {
        /// <summary>
        /// Creates the filters that will need to be applied to a rate group.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A collection of IRateGroupFilter objects.</returns>
        IEnumerable<IRateGroupFilter> CreateFilters(ShipmentEntity shipment);
    }
}
