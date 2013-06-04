using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Grid.Columns.SortProviders;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class EmailOutboundRelationColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible outbound email columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{5FC64EB7-CE5F-4fce-B2EC-5C0DF72BDC13}", true,
                        new GridEntityDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                        new GridColumnFieldValueProvider(EmailOutboundRelationFields.ObjectID),
                        new GridColumnObjectLabelSortProvider(EmailOutboundRelationFields.ObjectID)) { DefaultWidth = 150 },
                };

            return definitions;
        }
    }
}
