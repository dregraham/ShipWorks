using System.Collections.Generic;
using System.Linq;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// Field layout provider specifically for To addresses
    /// </summary>
    public class OrderLookupToFieldLayoutProvider : IOrderLookupFieldLayoutProvider
    {
        private readonly OrderLookupFieldLayoutProvider orderLookupFieldLayoutProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupToFieldLayoutProvider(OrderLookupFieldLayoutProvider orderLookupFieldLayoutProvider)
        {
            this.orderLookupFieldLayoutProvider = orderLookupFieldLayoutProvider;
        }

        /// <summary>
        /// Fetch the section layouts from the database, excluding From address fields.
        /// </summary>
        public IEnumerable<SectionLayout> Fetch()
        {
            return orderLookupFieldLayoutProvider.Fetch().Where(p => p.Id != SectionLayoutIDs.From);
        }
    }
}
