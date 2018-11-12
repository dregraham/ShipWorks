using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Class for retrieving and saving order lookup field layouts
    /// </summary>
    [Component]
    public class OrderLookupFieldLayoutRepository : IOrderLookupFieldLayoutRepository
    {
        private readonly IShippingSettings shippingSettings;
        private readonly IOrderLookupFieldLayoutProvider fieldLayoutProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldLayoutRepository(IShippingSettings shippingSettings,
            OrderLookupFieldLayoutProvider fieldLayoutProvider)
        {
            this.shippingSettings = shippingSettings;
            this.fieldLayoutProvider = fieldLayoutProvider;
        }

        /// <summary>
        /// Fetch the section layouts from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SectionLayout> Fetch()
        {
            return fieldLayoutProvider.Fetch();
        }

        /// <summary>
        /// Save the given layouts to the database.
        /// </summary>
        public void Save(IEnumerable<SectionLayout> sectionLayouts)
        {
            string json = JsonConvert.SerializeObject(sectionLayouts);
            ShippingSettingsEntity settings = shippingSettings.Fetch();
            settings.OrderLookupFieldLayout = json;
            shippingSettings.Save(settings);
        }
    }
}
