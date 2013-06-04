using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// The base for all editors that can appear in a condition line
    /// </summary>
    [ToolboxItem(false)]
    public partial class ConditionElementEditor : UserControl
    {
        /// <summary>
        /// Event that is raised when the element has changed
        /// </summary>
        public event EventHandler ContentChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionElementEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the ConditionElement being edited
        /// </summary>
        public virtual ConditionElement ConditionElement
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Raise the ContentChanged event
        /// </summary>
        protected void RaiseContentChanged()
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }
    }
}
