using System.Collections.Generic;
using System.Linq;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// Field layout provider specifically for From addresses
    /// </summary>
    public class OrderLookupFromFieldLayoutProvider : IOrderLookupFieldLayoutProvider
    {
        private readonly OrderLookupFieldLayoutProvider orderLookupFieldLayoutProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromFieldLayoutProvider(OrderLookupFieldLayoutProvider orderLookupFieldLayoutProvider)
        {
            this.orderLookupFieldLayoutProvider = orderLookupFieldLayoutProvider;
        }

        /// <summary>
        /// Fetch the section layouts from the database, excluding To address fields.
        /// </summary>
        public IEnumerable<SectionLayout> Fetch()
        {
            return orderLookupFieldLayoutProvider.Fetch().Where(p => p.Id != SectionLayoutIDs.To);
        }
    }
}
