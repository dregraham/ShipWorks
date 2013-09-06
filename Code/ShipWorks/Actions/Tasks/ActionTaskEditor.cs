using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Base editor class for all Action Task editors
    /// </summary>
    [ToolboxItem(false)]
    public partial class ActionTaskEditor : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows derived editors to update themselves based on the current trigger
        /// </summary>
        public virtual void NotifyTaskInputChanged(ActionTrigger trigger, ActionTaskInputSource inputSource, EntityType? inputType)
        {

        }

        /// <summary>
        /// Performs validation outside of the Windows Forms flow to make dealing with navigation easier
        /// </summary>
        /// <param name="errors">Collection of errors to which new errors will be added</param>
        public virtual void ValidateTask(ICollection<TaskValidationError> errors)
        {

        }
    }
}
