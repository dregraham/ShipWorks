using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Control for displaying the fact that a filter condition was removed b\c its not supported in v3 from v2
    /// </summary>
    public partial class NotSupportedV2ValueEditor : ValueEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotSupportedV2ValueEditor(NotSupportedV2Condition condition)
        {
            InitializeComponent();

            labelDetails.Text = condition.Detail;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Width = labelDetails.Right;
        }
    }
}
