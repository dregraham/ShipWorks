using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Triggers.Editors
{
    /// <summary>
    /// Base editor class for all action triggers
    /// </summary>
    [ToolboxItem(false)]
    public partial class ActionTriggerEditor : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTriggerEditor()
        {
            InitializeComponent();
        }
    }
}
