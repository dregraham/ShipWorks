using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the built-in column definitions
    /// </summary>
    public static class ActionErrorColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible action error columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{54E26C87-920D-461d-BFC9-FC1AB63ECC36}", true,
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Triggered", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        ActionQueueFields.TriggerDate),

                    new GridColumnDefinition("{A34E560E-DDC6-44c6-A532-A262C9CB9D3E}", true,
                        new GridComputerLimitedListDisplayType(), "Can Run On", @"\\ShippingPC",
                        new GridColumnFunctionValueProvider(x => ((ActionQueueEntity)x).ComputerLimitedList),
                        new GridColumnSortProvider(ActionQueueFields.InternalComputerLimitedList)),

                    new GridColumnDefinition("{10668EBE-8755-47A0-94E7-06E4DB466766}",
                        new GridComputerDisplayType(), "Triggered On", @"\\ShippingPC",
                        new GridColumnFieldValueProvider(ActionQueueFields.TriggerComputerID),
                        new GridColumnAdvancedSortProvider(ComputerFields.Name, ComputerFields.ComputerID, ActionQueueFields.TriggerComputerID, JoinHint.Right)),

                        new GridColumnDefinition("{CF990023-0CD9-443e-9741-73EC64B12FBD}", true,
                        new GridTextDisplayType(), "Action", "Order Downloaded",
                        ActionQueueFields.ActionName) { DefaultWidth = 150 },

                    new GridColumnDefinition("{F4A53390-F075-4287-9B98-3298BC747F00}", true,
                        new GridEntityDisplayType(), "Triggered By", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                        new GridColumnFieldValueProvider(ActionQueueFields.EntityID),
                        new GridColumnObjectLabelSortProvider(ActionQueueFields.EntityID)) { DefaultWidth = 200 },

                };

            // Return the definition set
            return definitions;
        }
    }
}
