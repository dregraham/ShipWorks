using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Combines the result of one or two ConditionGroup's based on a join type
    /// </summary>
    [ConditionElement("Container", "Container")]
    public class ConditionGroupContainer : ConditionElement
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ConditionGroupContainer));

        // How to join them
        ConditionGroupJoinType joinType = ConditionGroupJoinType.And;

        // Groups to apply the operator
        ConditionGroup firstGroup;
        ConditionGroupContainer secondGroup;

        /// <summary>
        /// The container that contains this container as its second group
        /// </summary>
        ConditionGroupContainer parentContainer;

        /// <summary>
        /// Creates an empty container
        /// </summary>
        public ConditionGroupContainer()
        {

        }

        /// <summary>
        /// Constructor.  A ConditionGroupContain must contain at least one group.
        /// </summary>
        public ConditionGroupContainer(ConditionGroup firstGroup)
        {
            FirstGroup = firstGroup;
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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            StringBuilder sb = new StringBuilder();

            if (firstGroup != null && secondGroup != null)
            {
                context.IndentLevel++;
            }

            // Gen the sql
            string firstSql = firstGroup.GenerateSql(context);
            string secondSql = secondGroup != null ? secondGroup.GenerateSql(context) : "";

            if (firstGroup != null && secondGroup != null)
            {
                context.IndentLevel--;
            }

            // See if we need to output the first group
            if (!string.IsNullOrEmpty(firstSql))
            {
                sb.Append(firstSql);

                // See if we are going to need the second
                if (!string.IsNullOrEmpty(secondSql))
                {
                    sb.AppendFormat("\n{0}{1}\n", context.IndentString, (joinType == ConditionGroupJoinType.And ? "AND" : "OR"));
                }
            }

            // Generate the second group sql
            if (!string.IsNullOrEmpty(secondSql))
            {
                sb.Append(secondSql);
            }

            if (firstGroup != null && secondGroup != null && sb.Length > 0)
            {
                sb.Insert(0, context.IndentString + "(\n");
                sb.AppendFormat("\n{0})", context.IndentString);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Controls how the results of the contained groups will be joined.
        /// </summary>
        public ConditionGroupJoinType JoinType
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
        /// Get the first group the join applies to
        /// </summary>
        public ConditionGroup FirstGroup
        {
            get
            {
                return firstGroup;
            }
            set
            {
                if (firstGroup == value)
                {
                    return;
                }

                ConditionGroup oldGroup = firstGroup;

                // Set this now to avoid recursion with the child setting us
                firstGroup = value;

                if (oldGroup != null && oldGroup.ParentContainer == this)
                {
                    oldGroup.ParentContainer = null;
                }

                // Make sure to sync parent
                if (firstGroup != null)
                {
                    firstGroup.ParentContainer = this;
                }
            }
        }

        /// <summary>
        /// Get the second group the join applies to.  This is another container, which makes
        /// our hierarchy of conditions possible.
        /// </summary>
        public ConditionGroupContainer SecondGroup
        {
            get
            {
                return secondGroup;
            }
            set
            {
                if (secondGroup == value)
                {
                    return;
                }

                ConditionGroupContainer oldContainer = secondGroup;

                // Set this now to avoid recursion with the child setting us
                secondGroup = value;

                if (oldContainer != null && oldContainer.ParentContainer == this)
                {
                    oldContainer.ParentContainer = null;
                }

                // Make sure to sync parent
                if (secondGroup != null)
                {
                    secondGroup.ParentContainer = this;
                }
            }
        }

        /// <summary>
        /// The container that contains this container as its second group
        /// </summary>
        [XmlIgnore]
        public ConditionGroupContainer ParentContainer
        {
            get
            {
                return parentContainer;
            }
            set
            {
                if (parentContainer == value)
                {
                    return;
                }

                ConditionGroupContainer oldParent = parentContainer;

                // Update now - this eliminates recursion when the parent calls this
                parentContainer = value;

                // Remove ourselves from the old parent
                if (oldParent != null && oldParent.SecondGroup == this)
                {
                    oldParent.SecondGroup = null;
                }

                // Add ourselves to the new parent
                if (parentContainer != null)
                {
                    parentContainer.SecondGroup = this;
                }
            }
        }
    }
}
