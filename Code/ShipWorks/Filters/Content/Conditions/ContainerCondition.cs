using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.Editors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Special condition that actually is a container of other conditions
    /// </summary>
    public abstract class ContainerCondition : Condition
    {
        // We use a parent container to hold our actual container.  This is so that as the editor 
        // deletes\adds, we don't lose track of our real container.
        InternalConditionGroupContainer containerHolder;

        #region class InternalConditionGroupContainer

        /// <summary>
        /// A container that knows that its parent is this condition, and not another parent container, in terms of where
        /// to get its entitytarget scope from.
        /// </summary>
        class InternalConditionGroupContainer : ConditionGroupContainer
        {
            ContainerCondition scopeParent;

            /// <summary>
            /// Constructor
            /// </summary>
            public InternalConditionGroupContainer(ContainerCondition scopeParent)
            {
                this.scopeParent = scopeParent;
            }

            /// <summary>
            /// Get the EntityTarget by getting the target of the container
            /// </summary>
            public override ConditionEntityTarget GetScopedEntityTarget()
            {
                return scopeParent.GetChildEntityTarget();
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        protected ContainerCondition()
        {
            containerHolder = new InternalConditionGroupContainer(this);
            containerHolder.SecondGroup = new ConditionGroupContainer(new ConditionGroup());
        }

        /// <summary>
        /// Get the entity target of all children of this container.
        /// </summary>
        public abstract ConditionEntityTarget GetChildEntityTarget();

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            context.IndentLevel++;

            string sql = Container.GenerateSql(context);

            context.IndentLevel--;

            if (!string.IsNullOrEmpty(sql))
            {
                sql = "\n" + sql + "\n" + context.IndentString;
            }

            return sql;
        }

        /// <summary>
        /// The container that this condition is represented by
        /// </summary>
        public ConditionGroupContainer Container
        {
            get
            {
                return containerHolder.SecondGroup;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                containerHolder.SecondGroup = value;
            }
        }

        /// <summary>
        /// Create the editor.
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            // We are defined by our child items, we don't have an editor
            return null;
        }
    }
}
