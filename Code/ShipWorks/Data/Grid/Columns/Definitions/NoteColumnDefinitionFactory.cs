using Divelements.SandGrid;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.Columns.DisplayTypes;
using ShipWorks.Data.Grid.Columns.SortProviders;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Stores.Content;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Data.Grid.Columns.Definitions
{
    /// <summary>
    /// Factory class for creating the builtin column definitions
    /// </summary>
    public static class NoteColumnDefinitionFactory
    {
        /// <summary>
        /// Create the default grid column definitions for all possible note columns.
        /// </summary>
        public static GridColumnDefinitionCollection CreateDefinitions()
        {
            GridColumnDefinitionCollection definitions = new GridColumnDefinitionCollection
                     {
                        new GridColumnDefinition("{62768936-EAF6-4d36-B7E3-6FD0335B6878}", true,
                            new GridEntityDisplayType(), "Related To", new GridEntityDisplayInfo(6, EntityType.OrderEntity, "Order 1028"),
                            new GridColumnFieldValueProvider(NoteFields.EntityID),
                            new GridColumnObjectLabelSortProvider(NoteFields.EntityID)) { DefaultWidth = 24 },

                        new GridColumnDefinition("{80C02876-F115-436c-AADC-F12912D0B7D3}", true,
                            new GridDateDisplayType { UseDescriptiveDates = true, TimeDisplayFormat = TimeDisplayFormat.None },
                            "Date", DateTimeUtility.ParseEnUS("03/04/2001 1:30").ToUniversalTime(),
                            NoteFields.Edited) { DefaultWidth = 80 },

                        new GridColumnDefinition("{88869064-D3FE-4284-8EE2-00E08C5733EE}", true,
                            new GridUserDisplayType(), "User", new object[] { "Joe", Resources.user_16 },
                            NoteFields.UserID,
                            UserFields.Username),

                        new GridColumnDefinition("{01090116-74AD-4b90-903C-C01C48E4F84A}", true,
                            new GridEnumDisplayType<NoteSource>(EnumSortMethod.Value), "Source", NoteSource.ShipWorksUser,
                            NoteFields.Source) { DefaultWidth = 70 },

                        new GridColumnDefinition("{E35D7549-C3DB-4db0-B25C-BC20652CAB20}", true,
                            new GridEnumDisplayType<NoteVisibility>(EnumSortMethod.Value), "Visibility", NoteVisibility.Internal,
                            NoteFields.Visibility) { DefaultWidth = 60 },

                        new GridColumnDefinition("{043677ED-9DA7-48fc-BB2B-EEAAD5EB3BF9}", true,
                            new GridTextDisplayType(), "Note", "This is a note.",
                            NoteFields.Text) { AutoSizeMode = ColumnAutoSizeMode.Spring, AutoWrap = true},

                        new GridColumnDefinition("{BF48FE0C-D46E-44e2-8A8E-EB24435CF1F5}", true,
                            new GridActionDisplayType(o =>
                                {
                                    return o == null || UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, (long) o) ? "Edit" : "";
                                },
                                GridLinkAction.Edit), "Edit", "Edit",
                            NoteFields.EntityID,
                            NoteFields.NoteID)
                            {
                                DefaultWidth = 31,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, (long) data) : true
                            },

                         new GridColumnDefinition("{AABA7DE6-8976-49de-9662-E7B6EB5089AC}", true,
                            new GridActionDisplayType(o =>
                                {
                                    return o == null || UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, (long) o) ? "Delete" : "";
                                },
                                GridLinkAction.Delete), "Delete", "Delete",
                            NoteFields.EntityID,
                            NoteFields.NoteID)
                            {
                                DefaultWidth = 45,
                                ApplicableTest = (data) => data != null ? UserSession.Security.HasPermission(PermissionType.EntityTypeEditNotes, (long) data) : true
                            },
                   };

            return definitions;
        }
    }
}
