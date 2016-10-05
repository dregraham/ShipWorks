using Divelements.SandGrid;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Audit;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    public static class AuditChangeColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible audit log columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{E19C7E77-B066-457f-AE21-FA1D892BA450}", true,
                        new GridEnumDisplayType<AuditChangeType>(EnumSortMethod.Value), "Change", AuditChangeType.Update,
                        AuditChangeFields.ChangeType) { DefaultWidth = 24 },

                    new GridColumnDefinition("{92439D7D-6484-4808-B0C0-32B5CE1074F6}", true,
                        new GridEntityDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                        new GridColumnFieldValueProvider(AuditChangeFields.EntityID),
                        new GridColumnObjectLabelSortProvider(AuditChangeFields.EntityID)) { AutoSizeMode = ColumnAutoSizeMode.Spring },
                };

            return definitions;
        }
    }
}
