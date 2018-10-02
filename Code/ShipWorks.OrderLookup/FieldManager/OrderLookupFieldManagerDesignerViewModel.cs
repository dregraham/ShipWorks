using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// View model for the order lookup field manager view model
    /// </summary>
    public class OrderLookupFieldManagerDesignerViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldManagerDesignerViewModel()
        {
            Sections = new List<SectionLayout>
            {
                new SectionLayout()
                {
                    Name = "To Address",
                    Id = "ToAddress",
                    Expanded = true,
                    SectionFields = new List<SectionFieldLayout>()
                        {
                            new SectionFieldLayout() { Id = "FullName", Name = "Full Name" },
                            new SectionFieldLayout() { Id = "Street", Name = "Street", Selected = false},
                            new SectionFieldLayout() { Id = "StateProvince", Name = "State Province" }
                        }
                },
                new SectionLayout()
                {
                    Name = "From Address",
                    Id = "FromAddress",
                    Selected = false,
                    SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = "FullName", Name = "Full Name"},
                        new SectionFieldLayout() { Id = "Street", Name = "Street"},
                        new SectionFieldLayout() { Id = "City", Name = "City", Selected = false },
                        new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Selected = false }
                    }
                },
                new SectionLayout()
                {
                    Name = "Label Options",
                    Id = "LabelOptions",
                    Selected = false,
                    Expanded = true,
                    SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = "ShipDate", Name = "Ship Date" },
                        new SectionFieldLayout() { Id = "USPSStealthPostage", Name = "USPS - Stealth Postage", Selected = false  },
                        new SectionFieldLayout() { Id = "RequestedLabelFormat", Name = "Requested Label Format" }
                    }
                }
            };
        }

        /// <summary>
        /// Sections of fields
        /// </summary>
        [Obfuscation]
        public IEnumerable<SectionLayout> Sections { get; private set; }
    }
}
