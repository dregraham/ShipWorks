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
                    Row = 0,
                    Column = 0,
                    SectionFields = new List<SectionFieldLayout>()
                        {
                            new SectionFieldLayout() { Id = "FullName", Name = "Full Name", Row = 0 },
                            new SectionFieldLayout() { Id = "Street", Name = "Street", Row = 1, Selected = false},
                            new SectionFieldLayout() { Id = "StateProvince", Name = "State Province", Row = 2 }
                        }
                },
                new SectionLayout()
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
                },
                new SectionLayout()
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
