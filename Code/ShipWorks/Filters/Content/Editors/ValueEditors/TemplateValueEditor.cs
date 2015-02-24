using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Editor for choosing templatesh
    /// </summary>
    public partial class TemplateValueEditor : ValueEditor
    {
        TemplateCondition condition;
        ContextMenuStrip contextMenuDeleted;

        /// <summary>
        /// To use visual inheritance there must be a constructor with zero arguments
        /// </summary>
        private TemplateValueEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateValueEditor(TemplateCondition condition) : this()
        {
            this.condition = condition;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (!DesignModeDetector.IsDesignerHosted())
            {
                templateComboBox.LoadTemplates();
                templateComboBox.SelectedTemplateID = condition.TemplateID;
                templateComboBox.SelectedTemplateChanged += new EventHandler(OnValueChanged);

                deletedTemplates.Visible = CreateDeletedTemplatesMenu();

                UpdateValueVisibility();
            }
        }

        /// <summary>
        /// The value of the operator has changed
        /// </summary>
        void OnValueChanged(object sender, EventArgs e)
        {
            condition.TemplateID = templateComboBox.SelectedTemplateID.HasValue ? templateComboBox.SelectedTemplateID.Value : 0;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// Update the position and visibility of the controls
        /// </summary>
        protected virtual void UpdateValueVisibility()
        {
            Width = panelTemplateSelection.Right + 5;
        }

        /// <summary>
        /// Clicked the deleted templates link
        /// </summary>
        private void OnClickDeletedTemplates(object sender, EventArgs e)
        {
            contextMenuDeleted.Show(deletedTemplates.Parent.PointToScreen(new Point(deletedTemplates.Left, deletedTemplates.Bottom)));
        }

        /// <summary>
        /// Create the menu for selecting from all the deleted templates
        /// </summary>
        private bool CreateDeletedTemplatesMenu()
        {
            List<ObjectLabelEntity> deleted = TemplateManager.DeletedTemplateLabels.ToList();

            if (deleted.Count == 0)
            {
                return false;
            }

            contextMenuDeleted = new ContextMenuStrip();

            foreach (ObjectLabelEntity label in deleted)
            {
                ToolStripItem menuItem = contextMenuDeleted.Items.Add(label.Label);
                menuItem.Tag = label.ObjectID;
                menuItem.Click += new EventHandler(OnSelectDeletedTemplate);
            }

            return true;
        }

        /// <summary>
        /// A deleted template has been selected
        /// </summary>
        void OnSelectDeletedTemplate(object sender, EventArgs e)
        {
            templateComboBox.SelectedTemplateID = (long) ((ToolStripItem) sender).Tag;
        }
    }
}
