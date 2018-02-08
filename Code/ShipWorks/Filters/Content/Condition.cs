using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using System.Xml;
using System.Xml.Serialization;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Most fundamental element of a filter.
    /// </summary>
    abstract public class Condition : ConditionElement
    {
        // The group the condition belongs to
        ConditionGroup parent;

        /// <summary>
        /// Creates the editor that is used to edit the condition.
        /// </summary>
        abstract public ValueEditor CreateEditor();

        /// <summary>
        /// By default a condition will just defer to the target of its parent
        /// </summary>
        public override ConditionEntityTarget GetScopedEntityTarget()
        {
            if (ParentGroup == null)
            {
                throw new InvalidOperationException("No entity target is in scope.  No parent for the condition is set.");
            }

            return ParentGroup.GetScopedEntityTarget();
        }

        /// <summary>
        /// The ConditionGroup that contains this condition
        /// </summary>
        [XmlIgnore]
        public ConditionGroup ParentGroup
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent == value)
                {
                    return;
                }

                ConditionGroup oldParent = parent;

                // Update now - this eliminates recursion when the parent calls this
                parent = value;

                // Remove ourselves from the old parent
                if (oldParent != null && oldParent.Conditions.Contains(this))
                {
                    oldParent.Conditions.Remove(this);
                }

                // Add ourselves to the new parent
                if (parent != null && !parent.Conditions.Contains(this))
                {
                    parent.Conditions.Add(this);
                }
            }
        }
    }
}
