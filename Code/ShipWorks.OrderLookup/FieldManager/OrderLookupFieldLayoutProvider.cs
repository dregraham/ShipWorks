using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.OrderLookup.FieldManager
{
    [Component(RegistrationType.Self)]
    public class OrderLookupFieldLayoutProvider : IOrderLookupFieldLayoutProvider
    {
        private readonly IShippingSettings shippingSettings;
        private readonly IOrderLookupFieldLayoutDefaults defaultsProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldLayoutProvider(IShippingSettings shippingSettings, 
            IOrderLookupFieldLayoutDefaults defaultsProvider)
        {
            this.shippingSettings = shippingSettings;
            this.defaultsProvider = defaultsProvider;
        }

        /// <summary>
        /// Fetch the section layouts from the database.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SectionLayout> Fetch()
        {
            // Create a clone so we don't modify the defaultsProvider's copy
            List<SectionLayout> defaultSectionLayouts = defaultsProvider.GetDefaults().Select(d => d.Clone()).ToList();

            string jsonFieldLayouts = shippingSettings.FetchReadOnly().OrderLookupFieldLayout;
            IEnumerable<SectionLayout> fieldLayouts = null;

            if (!jsonFieldLayouts.TryParseJson(out fieldLayouts))
            {
                return defaultSectionLayouts;
            }

            foreach (SectionLayout defaultSectionLayout in defaultSectionLayouts.Intersect(fieldLayouts, (opd1, opd2) => opd1.Id == opd2.Id))
            {
                SectionLayout sectionLayout = fieldLayouts.First(fl => fl.Id == defaultSectionLayout.Id);
                defaultSectionLayout.Copy(sectionLayout);

                foreach (SectionFieldLayout defaultSectionFieldLayout in defaultSectionLayout.SectionFields.Intersect(sectionLayout.SectionFields, (opd1, opd2) => opd1.Id == opd2.Id))
                {
                    SectionFieldLayout sectionFieldLayout = sectionLayout.SectionFields.First(sf => sf.Id == defaultSectionFieldLayout.Id);
                    defaultSectionFieldLayout.Copy(sectionFieldLayout);
                }
            }

            return defaultSectionLayouts;
        }
    }
}
