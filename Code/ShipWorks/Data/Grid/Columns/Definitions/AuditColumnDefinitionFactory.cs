using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Audit.CoreExtensions.Grid;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class AuditColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible audit log columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                {
                    new GridColumnDefinition("{01AD5141-FD17-40ec-9EEE-0B1785D9CD73}", true,
                        new GridEnumDisplayType<AuditActionType>(EnumSortMethod.Value), "Action", AuditActionType.Logon,
                        AuditFields.Action),

                    new GridColumnDefinition("{F5B0E845-FF87-4548-8BE7-D59AF7876EE1}", true,
                        new GridUserDisplayType(), "User", new object[] { "Joe", Resources.user_16 },
                        AuditFields.UserID,
                        UserFields.Username),

                    new GridColumnDefinition("{D53A9004-D600-4c0e-86AA-596C9D0887B1}", true,
                        new GridDateDisplayType { UseDescriptiveDates = true }, "Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30 PM").ToUniversalTime(),
                        AuditFields.Date),

                    new GridColumnDefinition("{A9F774D4-20FD-43b1-BC76-6E08CA2FE880}",
                        new GridComputerDisplayType(), "Computer", "\\ShippingPC",
                        AuditFields.ComputerID,
                        ComputerFields.Name),

                    new GridColumnDefinition("{B8038A92-7DBD-4d26-A758-F853EECB0219}",
                        new GridAuditReasonDisplayType(), "Reason", "Automatic Download",
                        AuditFields.Reason),

                    new GridColumnDefinition("{35A2E5F2-88F8-4559-912D-1C7FA36A34BA}", true,
                        new GridEntityDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                        new GridColumnFieldValueProvider(AuditFields.EntityID),
                        new GridColumnObjectLabelSortProvider(AuditFields.EntityID)) { DefaultWidth = 225 },

                    new GridColumnDefinition("{31D4314A-F071-41c7-8E7E-EB4EEDCC3265}", true,
                        new AuditDetailDisplayType(), "Detail", new AuditEntity { HasEvents = true },
                        AuditFields.AuditID) { DefaultWidth = 45 },

                };

            return definitions;
        }
    }
}
