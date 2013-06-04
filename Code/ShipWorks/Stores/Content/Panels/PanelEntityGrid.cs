using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Paging;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.ComponentModel;
using ShipWorks.Data.Utility;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Custom grid class for use in panels
    /// </summary>
    public class PanelEntityGrid : PagedEntityGrid
    {
        GroupingContext groupingContext;

        /// <summary>
        /// When in multi-selecting in groups, this provides the context on how to group.  If null, no grouping is performed
        /// </summary>
        [DefaultValue(null)]
        [Browsable(false)]
        public GroupingContext GroupingContext
        {
            get 
            {
                return groupingContext; 
            }
            set 
            {
                groupingContext = value;

                if (groupingContext != null)
                {
                    SuspendSortProcessing = true;
                    SortColumn = null;
                    SuspendSortProcessing = false;
                }

                OnSortChanged(null);
            }
        }

        /// <summary>
        /// Get the sort for the gateway
        /// </summary>
        protected override SortDefinition GetSortForGateway()
        {
            if (groupingContext == null)
            {
                return base.GetSortForGateway();
            }
            else
            {
                return new SortDefinition(
                        new List<SortClause> { new SortClause(CreateSortField(), null, SortOperator.Ascending)}, 
                        null);
            }
        }

        /// <summary>
        /// Create the sort field used to group the child rows by their paring key
        /// </summary>
        private EntityField2 CreateSortField()
        {
            EntityField2 parentKeyField = groupingContext.GroupByField;

            // Assign each key and ordering number in the order of the keys in our list
            StringBuilder sb = new StringBuilder("CASE {0} ");
            for (int i = 0; i < groupingContext.GroupingKeys.Count; i++)
            {
                sb.AppendFormat(" WHEN {0} THEN {1} ", groupingContext.GroupingKeys[i], i);
            }
            sb.AppendFormat(" ELSE '-1' END");

            parentKeyField.ExpressionToApply = new DbFunctionCall(sb.ToString(), new object[] { groupingContext.GroupByField });

            return parentKeyField;
        }
    }
}
