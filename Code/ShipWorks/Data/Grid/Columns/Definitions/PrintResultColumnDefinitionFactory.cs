using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Grid.Columns.SortProviders;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class PrintResultColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible shipment columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            return new GridColumnDefinitionCollection
            {
                new GridColumnDefinition("{C21D3B07-9372-48f3-A9AF-848BB2512B61}", true,
                    new GridDateDisplayType { UseDescriptiveDates = true }, "Printed", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                    PrintResultFields.PrintDate),

                new GridColumnDefinition("{C0C2E642-D4C4-40ae-82E8-3F1EC6C0FF43}", true,
                    new GridEntityDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                    new GridColumnFieldValueProvider(PrintResultFields.ContextObjectID),
                    new GridColumnObjectLabelSortProvider(PrintResultFields.ContextObjectID)),

                new GridColumnDefinition("{23B46661-9CA9-4c6a-9BDF-E61F41AE2492}", true,
                    new GridEntityDisplayType() { IncludeTypePrefix = false }, "Template", new GridEntityDisplayInfo(25, EntityType.TemplateEntity, "Invoices\\Order Invoice"),
                    new GridColumnFieldValueProvider(PrintResultFields.TemplateID),
                    new GridColumnObjectLabelSortProvider(PrintResultFields.TemplateID)),

                new GridColumnDefinition("{32F85399-EBC2-4a41-899E-C9EF1EAAF282}",
                    new GridComputerDisplayType(), "Computer", "\\ShippingPC",
                    PrintResultFields.ComputerID,
                    ComputerFields.Name),

                new GridColumnDefinition("{7DA89C96-C705-4f8c-8E17-1F67D2387A64}", true,
                    new GridTextDisplayType(), "Printer", "HP DeskJet 8900",
                    PrintResultFields.PrinterName),

                new GridColumnDefinition("{05A24610-99DA-4f7e-800F-88CDD8E5070D}", true,
                    new GridTextDisplayType(), "Tray", "Tray 1",
                    PrintResultFields.PaperSourceName),

                new GridColumnDefinition("{D2C101F9-9542-4db3-A258-6C1C067C0C1F}", true,
                    new GridTextDisplayType(), "Copies", "1",
                    PrintResultFields.Copies) { DefaultWidth = 50 },

                new GridColumnDefinition("{B06B31D6-9DAF-4b4d-9482-061EB036E833}", true,
                    new GridActionDisplayType("View", GridLinkAction.View), "View", "View",
                    PrintResultFields.PrintResultID) { DefaultWidth = 31 },
            };
        }
   }
}
