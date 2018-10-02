using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
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
        private IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldLayoutRepository(IShippingSettings shippingSettings)
        {
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Fetch the section layouts from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SectionLayout> Fetch()
        {
            string jsonFieldLayouts = shippingSettings.FetchReadOnly().OrderLookupFieldLayout;

            return jsonFieldLayouts.IsNullOrWhiteSpace() ? 
                Defaults() : JsonConvert.DeserializeObject<IEnumerable<SectionLayout>>(jsonFieldLayouts);
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

        /// <summary>
        /// Load default layouts
        /// </summary>
        private IEnumerable<SectionLayout> Defaults()
        {
            List<SectionLayout> sectionLayouts = new List<SectionLayout>();

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "To Address",
                Id = "ToAddress",
                Row = 0,
                Column = 0,
                SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = "FullName", Name = "Full Name", Row = 0 },
                        new SectionFieldLayout() { Id = "Street", Name = "Street", Row = 1, Selected = false},
                        new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Row = 2 }
                    }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "From Address",
                Id = "FromAddress",
                Selected = false,
                Row = 0,
                Column = 1,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "FullName", Name = "Full Name", Row = 0 },
                    new SectionFieldLayout() { Id = "Street", Name = "Street", Row = 1 },
                    new SectionFieldLayout() { Id = "City", Name = "City", Row = 2, Selected = false },
                    new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Row = 3, Selected = false }
                }
            });

            sectionLayouts.Add(new SectionLayout()
            {
                Name = "Label Options",
                Id = "LabelOptions",
                Selected = false,
                Row = 1,
                Column = 0,
                SectionFields = new List<SectionFieldLayout>()
                {
                    new SectionFieldLayout() { Id = "ShipDate", Name = "Ship Date", Row = 0 },
                    new SectionFieldLayout() { Id = "USPSStealthPostage", Name = "USPS - Stealth Postage", Row = 1, Selected = false  },
                    new SectionFieldLayout() { Id = "RequestedLabelFormat", Name = "Requested Label Format", Row = 2 }
                }
            });

            return sectionLayouts;
        }
    }
}
