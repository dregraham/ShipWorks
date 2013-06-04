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
    /// An editor that is just a placeholder for when groups have no conditions
    /// </summary>
    public partial class ConditionPlaceholderEditor : ConditionElementEditor
    {
        ConditionGroup group;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionPlaceholderEditor(ConditionGroup group)
        {
            InitializeComponent();

            this.group = group;
        }

        /// <summary>
        /// The ConditionGroup that the placeholder is a child of
        /// </summary>
        public ConditionGroup ConditionGroup
        {
            get { return group; }
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
