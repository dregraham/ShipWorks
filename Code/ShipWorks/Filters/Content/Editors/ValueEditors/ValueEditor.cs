using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Controls;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Base editor for editing the various values a condition can be evalated against
    /// </summary>
    [ToolboxItem(false)]
    public partial class ValueEditor : UserControl
    {
        ErrorProvider errorProvider = null;

        // Gui width needed by the error display
        protected const int errorSpace = 18;

        /// <summary>
        /// Event that is raised when any time the content of the editor changes, regardless of validity.
        /// </summary>
        public event EventHandler ContentChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the error provider we use for error display
        /// </summary>
        private ErrorProvider FindErrorProvider()
        {
            if (errorProvider != null)
            {
                return errorProvider;
            }

            Control control = this.Parent;

            // We have to find the filter definition editor
            while (control != null)
            {
                FilterDefinitionEditor editor = control as FilterDefinitionEditor;
                if (editor != null)
                {
                    errorProvider = editor.ErrorProvider;
                    return errorProvider;
                }

                control = control.Parent;
            }

            return null;
        }

        /// <summary>
        /// Show the error icon, with the associated error tooltip message, for the given control.
        /// </summary>
        public void SetError(Control control, string error)
        {
            FindErrorProvider();

            if (errorProvider != null)
            {
                errorProvider.SetError(control, error);
            }
        }

        /// <summary>
        /// Clear the error for the given control.
        /// </summary>
        public void ClearError(Control control)
        {
            SetError(control, null);
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
