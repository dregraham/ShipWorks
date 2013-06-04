using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition for filtering when an order was downloaded
    /// </summary>
    [ConditionElement("Downloaded", "Order.Downloaded")]
    public class DownloadedCondition : OrderDateCondition
    {
        DownloadedPresenceType presence = DownloadedPresenceType.InitialDownload;
        bool dateRangeSpecified = false;

        /// <summary>
        /// The presence of the order when it was downloaded.
        /// </summary>
        public DownloadedPresenceType Presence
        {
            get { return presence; }
            set { presence = value; }
        }

        /// <summary>
        /// Indicates if a date range for when the download occurred is specified.  If false (the default) its anytime.
        /// </summary>
        public bool DateRangeSpecified
        {
            get { return dateRangeSpecified; }
            set { dateRangeSpecified = value; }
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            StringBuilder sql = new StringBuilder();

            // First of all it has to be a manual order
            sql.AppendFormat("{0} = 0", context.GetColumnReference(OrderFields.IsManual));
            
            // Shortcut - if presense doesn't matter and date doesn't matter, we're done
            if (presence == DownloadedPresenceType.Either && !dateRangeSpecified)
            {
                return sql.ToString();
            }
            else
            {
                // First we have to get from Order -> DownloadDetail
                using (SqlGenerationScope scopeHistory = context.PushScope(OrderFields.OrderID, DownloadDetailFields.OrderID, SqlGenerationScopeType.AnyChild))
                {
                    // See if we need to filter on the initial download part
                    if (presence != DownloadedPresenceType.Either)
                    {
                        sql.AppendFormat(" AND {0} = {1}",
                            context.GetColumnReference(DownloadDetailFields.InitialDownload),
                            (presence == DownloadedPresenceType.InitialDownload) ? 1 : 0);
                    }

                    // Then back up to DownloadLog to filter on date
                    if (dateRangeSpecified)
                    {
                        using (SqlGenerationScope scopeLog = context.PushScope(EntityType.DownloadEntity, SqlGenerationScopeType.Parent))
                        {
                            // Add the date condition
                            sql.AppendFormat(" AND {0}", GenerateSql(context.GetColumnReference(DownloadFields.Started), context));

                            // Return our SQL, adorned with the scopes needed to do the JOINs we need
                            return scopeHistory.Adorn(scopeLog.Adorn(sql.ToString()));
                        }
                    }
                    else
                    {
                        return scopeHistory.Adorn(sql.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Create the specialized editor type for this condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new OrderDownloadedValueEditor(this);
        }
    }
}
