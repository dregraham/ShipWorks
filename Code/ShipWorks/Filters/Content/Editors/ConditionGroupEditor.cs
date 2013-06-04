using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using ComponentFactory.Krypton.Toolkit;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Editor for choosing the join type for a ConditionGroup
    /// </summary>
    public partial class ConditionGroupEditor : ConditionElementEditor
    {
        // The ConditionGroup we are editing the join condition for.
        ConditionGroup group;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionGroupEditor(ConditionGroup group)
        {
            InitializeComponent();

            this.group = group;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            joinType.InitializeFromEnumType(typeof(ConditionJoinType));
            joinType.SelectedValue = group.JoinType;

            UpdateLayout();
        }

        /// <summary>
        /// Update the positioning of the controls
        /// </summary>
        private void UpdateLayout()
        {
            labelAfter.Left = joinType.Right - 3;
            this.Width = labelAfter.Right;
        }

        /// <summary>
        /// The group type has changed
        /// </summary>
        private void OnChangeJoinType(object sender, EventArgs e)
        {
            group.JoinType = (ConditionJoinType) joinType.SelectedValue;

            UpdateLayout();
            RaiseContentChanged();
        }

        /// <summary>
        /// Gets the ConditionGroup the editor represents
        /// </summary>
        public ConditionGroup ConditionGroup
        {
            get
            {
                return group;
            }
        }

        /// <summary>
        /// The element the editor represents
        /// </summary>
        public override ConditionElement ConditionElement
        {
            get
            {
                return ConditionGroup;
            }
        }
    }
}
