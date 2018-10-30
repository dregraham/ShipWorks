using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// Field layout provider specifically for From addresses
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupFromFieldLayoutProvider : OrderLookupFieldLayoutProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromFieldLayoutProvider(IShippingSettings shippingSettings,
            IOrderLookupFieldLayoutDefaults defaultsProvider) : base(shippingSettings, defaultsProvider)
        {
        }

        /// <summary>
        /// Fetch the section layouts from the database, excluding To address fields.
        /// </summary>
        public override IEnumerable<SectionLayout> Fetch()
        {
            return base.Fetch().Where(p => p.Id != SectionLayoutIDs.To);
        }
    }
}
