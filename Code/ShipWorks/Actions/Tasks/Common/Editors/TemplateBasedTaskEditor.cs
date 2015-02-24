using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Templates;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Editor for the print task 
    /// </summary>
    public partial class TemplateBasedTaskEditor : ActionTaskEditor
    {
        TemplateBasedTask task;

        /// <summary>
        /// Default constructor.  Only here to make visual inheritance work.
        /// </summary>
        protected TemplateBasedTaskEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateBasedTaskEditor(TemplateBasedTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return;
            }

            templateCombo.LoadTemplates();
            templateCombo.SelectedTemplate = TemplateManager.Tree.GetTemplate(task.TemplateID);

            // Listen for changes
            templateCombo.SelectedTemplateChanged += new EventHandler(OnSelectedTemplateChanged);
        }

        /// <summary>
        /// The selected template has changed
        /// </summary>
        void OnSelectedTemplateChanged(object sender, EventArgs e)
        {
            task.TemplateID = templateCombo.SelectedTemplate != null ? templateCombo.SelectedTemplate.TemplateID : 0;
        }
    }
}
