using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Task editor for shrinking a database.  It currently does nothing, but is required, so just set the Height to 0.
    /// </summary>
    public partial class ShrinkDatabaseTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShrinkDatabaseTaskEditor(ShrinkDatabaseTask task)
        {
            InitializeComponent();
        }
    }
}
