using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns.Definitions;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Manages the definitions that define how a column looks within any grid.
    /// </summary>
    public static class GridColumnDefinitionManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GridColumnDefinitionManager));

        // All column definitions, their default values, and display data.  Specific to the logged in user.
        static Dictionary<GridColumnDefinitionSet, GridColumnDefinitionCollection> columnDefinitions = new Dictionary<GridColumnDefinitionSet, GridColumnDefinitionCollection>();

        /// <summary>
        /// Load user column data. To be called when a user logs in.
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            // If there's no UI, we don't need it
            if (!Program.ExecutionMode.IsUISupported)
            {
                return;
            }

            // Clear any remnants from the last user
            columnDefinitions.Clear();

            // Load all the display information that exists
            GridColumnFormatCollection columnFormats = GridColumnFormatCollection.Fetch(SqlAdapter.Default,
                GridColumnFormatFields.UserID == UserSession.User.UserID);

            // Load the definitions
            columnDefinitions = new Dictionary<GridColumnDefinitionSet, GridColumnDefinitionCollection>();
            columnDefinitions[GridColumnDefinitionSet.Orders] = LoadColumnDefinitions(OrderColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.OrderPanel] = LoadColumnDefinitions(OrderPanelColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.Customers] = LoadColumnDefinitions(CustomerColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.ShipmentStandard] = LoadColumnDefinitions(ShipmentStandardColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.ShipmentPanel] = LoadColumnDefinitions(ShipmentPanelColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.DownloadLog] = LoadColumnDefinitions(DownloadLogColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.EmailOutbound] = LoadColumnDefinitions(EmailOutboundColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.EmailOutboundPanel] = LoadColumnDefinitions(EmailOutboundPanelColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.ActionErrors] = LoadColumnDefinitions(ActionErrorColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.Notes] = LoadColumnDefinitions(NoteColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.Charges] = LoadColumnDefinitions(ChargeColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.PaymentDetails] = LoadColumnDefinitions(PaymentDetailColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.OrderItems] = LoadColumnDefinitions(OrderItemColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.PrintResult] = LoadColumnDefinitions(PrintResultColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.EmailOutboundRelation] = LoadColumnDefinitions(EmailOutboundRelationColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.Audit] = LoadColumnDefinitions(AuditColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.AuditChanges] = LoadColumnDefinitions(AuditChangeColumnDefinitionFactory.CreateDefinitions(), columnFormats);
            columnDefinitions[GridColumnDefinitionSet.ServiceStatus] = LoadColumnDefinitions(ServiceStatusColumnDefinitionFactory.CreateDefinitions(), columnFormats);

            // If there are left over columns, they must have been deleted from ShipWorks, and need removed from the database
            if (columnFormats.Count > 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    log.InfoFormat("Deleting {0} unused GridColumnFormat.", columnFormats.Count);
                    adapter.DeleteEntityCollection(columnFormats);
                }
            }
        }

        /// <summary>
        /// Get the column definitions for the given target.  This returns all possible definitions, regardless of Applicability.
        /// </summary>
        public static ICollection<GridColumnDefinition> GetColumnDefinitions(GridColumnDefinitionSet definitionSet)
        {
            List<GridColumnDefinition> definitions = columnDefinitions[definitionSet].ToList();

            return definitions;
        }

        /// <summary>
        /// Load the column definitions from the specified embeded definition file
        /// </summary>
        private static GridColumnDefinitionCollection LoadColumnDefinitions(GridColumnDefinitionCollection definitions, GridColumnFormatCollection columnFormats)
        {
            foreach (GridColumnDefinition definition in definitions)
            {
                CheckForDuplicateGuid(definition);

                // Find the column format for this column ID
                GridColumnFormatEntity formatEntity = LoadColumnFormatEntity(definition.ColumnGuid, columnFormats);

                // Get the display type to load the format into
                GridColumnDisplayType displayType = definition.DisplayType;

                // Load the display settings for the given column
                displayType.Initialize(formatEntity, definition.DisplayValueProvider);
            }

            return definitions;
        }

        /// <summary>
        /// Load the column format data from the existing collection or create a new one.  If loaded from the existing collection
        /// it will be removed from the collection, as it should only be needed once.
        /// </summary>
        private static GridColumnFormatEntity LoadColumnFormatEntity(Guid columnGuid, GridColumnFormatCollection columnFormats)
        {
            // Find the column display for this column ID
            GridColumnFormatEntity formatEntity = FindColumnFormat(columnFormats, columnGuid);

            if (formatEntity != null)
            {
                columnFormats.Remove(formatEntity);
            }
            else
            {
                log.InfoFormat("Missing GridColumnFormat for {0}", columnGuid);

                try
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        formatEntity = new GridColumnFormatEntity();
                        formatEntity.UserID = UserSession.User.UserID;
                        formatEntity.ColumnGuid = columnGuid;
                        formatEntity.Settings = "<Settings />";
                        adapter.SaveAndRefetch(formatEntity);
                    }
                }
                catch (ORMQueryExecutionException ex)
                {
                    SqlException sqlEx = ex.InnerException as SqlException;

                    // 2601: Cannot insert duplicate key
                    if (sqlEx != null && sqlEx.Number == 2601)
                    {
                        log.Info("Race condition inserting column, already inserted since we loaded collection.");

                        // Make sure it still doesn't exist
                        GridColumnFormatCollection fetched = GridColumnFormatCollection.Fetch(SqlAdapter.Default,
                            GridColumnFormatFields.UserID == UserSession.User.UserID &
                            GridColumnFormatFields.ColumnGuid == columnGuid);

                        formatEntity = fetched[0];
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return formatEntity;
        }

        /// <summary>
        /// Check for any definitions that have duplicate guids
        /// </summary>
        private static void CheckForDuplicateGuid(GridColumnDefinition definition)
        {
            List<GridColumnDefinition> allDefinitions = new List<GridColumnDefinition>();

            // Add in all the definitions to check.  Exclude the "base" column sets, where there a "derived" column set includes columns (and thus the IDs)
            // from the base.  What happens when a column shares a GUID is that the global formatting preferences apply to that column guid for whatever layout
            // it is used in.
            foreach (GridColumnDefinitionCollection definitions in columnDefinitions
                .Select(p => p.Value))
            {
                allDefinitions.AddRange(definitions);
            }

            List<Guid> allGuids = allDefinitions.Select(d => d.ColumnGuid).ToList();

            // We have a problem
            if (allGuids.Contains(definition.ColumnGuid))
            {
                throw new InvalidOperationException(string.Format("Column definition '{0}' uses duplicate guid '{1}'.", definition.HeaderText, definition.ColumnGuid));
            }
        }

        /// <summary>
        /// Find the entity in the collection with the specified id
        /// </summary>
        private static GridColumnFormatEntity FindColumnFormat(GridColumnFormatCollection columnFormats, Guid columnGuid)
        {
            foreach (GridColumnFormatEntity formatEntity in columnFormats)
            {
                if (formatEntity.ColumnGuid == columnGuid)
                {
                    return formatEntity;
                }
            }

            return null;
        }
    }
}
