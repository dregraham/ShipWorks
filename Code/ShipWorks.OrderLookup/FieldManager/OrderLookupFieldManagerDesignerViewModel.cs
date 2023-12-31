﻿using System.Collections.Generic;
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
                    Id = SectionLayoutIDs.To,
                    Expanded = true,
                    SectionFields = new List<SectionFieldLayout>()
                        {
                            new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name" },
                            new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street", Selected = false},
                            new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province" }
                        }
                },
                new SectionLayout()
                {
                    Name = "From Address",
                    Id = SectionLayoutIDs.From,
                    Selected = false,
                    SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.FullName, Name = "Full Name"},
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.Street, Name = "Street"},
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.City, Name = "City", Selected = false },
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.StateProvince, Name = "State Province", Selected = false }
                    }
                },
                new SectionLayout()
                {
                    Name = "Customs",
                    Id = SectionLayoutIDs.Customs,
                    Selected = false
                },
                new SectionLayout()
                {
                    Name = "Label Options",
                    Id = SectionLayoutIDs.LabelOptions,
                    Selected = false,
                    Expanded = true,
                    SectionFields = new List<SectionFieldLayout>()
                    {
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsShipDate, Name = "Ship Date" },
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsUspsHideStealth, Name = "USPS - Stealth Postage", Selected = false  },
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsNoPostage, Name = "USPS - No Postage" },
                        new SectionFieldLayout() { Id = SectionLayoutFieldIDs.LabelOptionsRequestedLabelFormat, Name = "Requested Label Format" }
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
