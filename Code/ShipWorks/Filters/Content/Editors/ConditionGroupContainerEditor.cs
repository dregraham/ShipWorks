using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Editor for the join type between two condition groups
    /// </summary>
    public partial class ConditionGroupContainerEditor : ConditionElementEditor
    {
        ConditionGroupContainer container;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionGroupContainerEditor(ConditionGroupContainer container)
        {
            InitializeComponent();

            this.container = container;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            labelOperator.InitializeFromEnumType(typeof(ConditionGroupJoinType));
            labelOperator.SelectedValue = container.JoinType;
        }

        /// <summary>
        /// The operator for the set has changed
        /// </summary>
        private void OnChangeOperator(object sender, System.EventArgs e)
        {
            container.JoinType = (ConditionGroupJoinType) labelOperator.SelectedValue;

            RaiseContentChanged();
        }

        /// <summary>
        /// The container whose join type is being edited
        /// </summary>
        public ConditionGroupContainer ConditionGroupContainer
        {
            get
            {
                return container;
            }
        }

        /// <summary>
        /// The element the editor represents
        /// </summary>
        public override ConditionElement ConditionElement
        {
            get
            {
                return ConditionGroupContainer;
            }
        }
    }
}
