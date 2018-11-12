using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared;
using System.Xml;
using System.Xml.Serialization;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Filters.Content.SqlGeneration;
using Interapptive.Shared.Collections;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Combines a set of conditions using a condition join type to produce a single 
    /// </summary>
    [ConditionElement("Group", "Group")]
    public class ConditionGroup : ConditionElement
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ConditionGroup));

        // List of conditions in the group
        ThreadSafeObservableCollection<Condition> conditions = new ThreadSafeObservableCollection<Condition>();

        // How the results are combined
        ConditionJoinType joinType = ConditionJoinType.All;

        // The container that this group belongs to
        ConditionGroupContainer container;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionGroup()
        {
            conditions.CollectionChanged += new CollectionChangedEventHandler<Condition>(OnConditionCollectionChanged);
        }

        /// <summary>
        /// Get the entity target that is in scope for the group
        /// </summary>
        public override ConditionEntityTarget GetScopedEntityTarget()
        {
            if (ParentContainer == null)
            {
                throw new InvalidOperationException("No entity target is in scope.  No parent for the group is set.");
            }

            return ParentContainer.GetScopedEntityTarget();
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            if (conditions.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();            

            bool needOperator = false;

            context.IndentLevel++;

            // Output each child
            foreach (Condition condition in conditions)
            {
                string sql = condition.GenerateSql(context);

                // Only add it if its non-empty
                if (!string.IsNullOrEmpty(sql))
                {
                    if (needOperator)
                    {
                        sb.AppendFormat(" {0} \n", GetSqlJoinOperator());
                    }

                    sb.AppendFormat("{0}(", context.IndentString);
                    sb.Append(sql);
                    sb.Append(")");

                    needOperator = true;
                }
            }

            context.IndentLevel--;

            if (sb.Length == 0)
            {
                return string.Empty;
            }

            sb.Insert(0, context.IndentString + "(\n");
            sb.AppendFormat("\n{0})", context.IndentString);

            if (joinType == ConditionJoinType.None)
            {
                sb.Insert(0, context.IndentString + "NOT \n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the SQL operator to use based on our join type
        /// </summary>
        private string GetSqlJoinOperator()
        {
            if (joinType == ConditionJoinType.All)
            {
                return "AND";
            }
            else
            {
                return "OR";
            }
        }

        /// <summary>
        /// Controls how the results of each condition are combined to produce the group result
        /// </summary>
        public ConditionJoinType JoinType
        {
            get
            {
                return joinType;
            }
            set
            {
                joinType = value;
            }
        }

        /// <summary>
        /// All conditions to be evaluated for the group
        /// </summary>
        public IList<Condition> Conditions
        {
            get { return conditions; }
        }

        /// <summary>
        /// The container that this group belongs to
        /// </summary>
        [XmlIgnore]
        public ConditionGroupContainer ParentContainer
        {
            get
            {
                return container;
            }
            set
            {
                if (container == value)
                {
                    return;
                }

                ConditionGroupContainer oldParent = container;

                // Update now - this eliminates recursion when the parent calls this
                container = value;

                // Remove ourselves from the old parent
                if (oldParent != null && oldParent.FirstGroup == this)
                {
                    oldParent.FirstGroup = null;
                }

                // Add ourselves to the new parent
                if (container != null)
                {
                    container.FirstGroup = this;
                }
            }
        }

        /// <summary>
        /// Reacts to changes in the condition list
        /// </summary>
        void OnConditionCollectionChanged(object sender, CollectionChangedEventArgs<Condition> e)
        {
            if (e.OldItem != null && e.OldItem.ParentGroup == this)
            {
                e.OldItem.ParentGroup = null;
            }

            if (e.NewItem != null && e.NewItem.ParentGroup != this)
            {
                e.NewItem.ParentGroup = this;
            }
        }
    }
}
