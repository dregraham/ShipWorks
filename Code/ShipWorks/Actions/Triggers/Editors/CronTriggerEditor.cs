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
    public partial class CronTriggerEditor : ActionTriggerEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CronTriggerEditor"/> class.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public CronTriggerEditor(CronTrigger trigger)
        {
            InitializeComponent();
        }
    }
}
