using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// Field layout provider specifically for To addresses
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupToFieldLayoutProvider : OrderLookupFieldLayoutProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupToFieldLayoutProvider(IShippingSettings shippingSettings,
            IOrderLookupFieldLayoutDefaults defaultsProvider) : base(shippingSettings, defaultsProvider)
        {
        }

        /// <summary>
        /// Fetch the section layouts from the database, excluding From address fields.
        /// </summary>
        public override IEnumerable<SectionLayout> Fetch()
        {
            return base.Fetch().Where(p => p.Id != SectionLayoutIDs.From);
        }
    }
}
