using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Properties;
using ShipWorks.UI.Utility;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Represents a single line in the definition editor, including the action buttons and the condition editor.
    /// </summary>
    public partial class ConditionLineControl : UserControl
    {
        // How many pixels to indent by per indent level
        const int indentAmount = 40;

        // Level to indent our contained control by
        int indent = 0;

        // Minimum width the line should be
        int editorXPos;
        
        // The control that is currently editing the condition
        ConditionElementEditor editor;

        /// <summary>
        /// Raised when the delete button for the line is clicked
        /// </summary>
        public event EventHandler Delete;

        /// <summary>
        /// Raised when the New Condition button is clicked
        /// </summary>
        public event EventHandler AddCondition;

        /// <summary>
        /// Raised when the New Group button is clicked
        /// </summary>
        public event EventHandler AddGroup;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionLineControl(ConditionElementEditor editor, int indent)
        {
            InitializeComponent();

            if (editor == null)
            {
                throw new ArgumentNullException("editor");
            }

            editorXPos = toolbarDelete.Right + 5;

            this.toolbarDelete.Renderer = new NoBorderToolStripRenderer();
            this.toolBarAdd.Renderer = new NoBorderToolStripRenderer();

            this.editor = editor;
            this.indent = indent;

            // Monitor the child for resizing
            editor.Resize += new EventHandler(OnItemEditorResize);

            // Only groups (condition joins) can add new groups or conditions
            if (!(editor is ConditionGroupEditor))
            {
                toolBarAdd.Visible = false;
            }
		}

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            // Add the condition control
            Controls.Add(editor);

            UpdateLayout();
            UpdateActions();
        }

        /// <summary>
        /// Update the actions available from the toolbar
        /// </summary>
        public void UpdateActions()
        {
            bool canDelete = true;

            // We'll test this in multiple spots, so we cast once here
            ConditionGroupEditor groupEditor = editor as ConditionGroupEditor;

            // Can't delete a placeholder
            if (editor is ConditionPlaceholderEditor)
            {
                canDelete = false;
            }
            else
            {
                if (groupEditor != null)
                {
                    ConditionGroupContainer container = groupEditor.ConditionGroup.ParentContainer;

                    // Checking the parent container ensures we aren't a root.
                    // Checking the first group of the parent container determines if we are the first child of a psuedo-root.  That technique
                    //    is used both by FilterDefinition and ContainerCondition.
                    // Checking SecondGroup bacuse of there is a second group, deleting the container can move the second group up to take its place.
                    canDelete = (container.ParentContainer != null && container.ParentContainer.FirstGroup != null) || container.SecondGroup != null;
                }
            }

            buttonDelete.Enabled = canDelete;

            // If its a join editor, we could be adding or inserting a branch depending on the parent container
            if (groupEditor != null)
            {
                if (groupEditor.ConditionGroup.ParentContainer.SecondGroup != null)
                {
                    buttonAddGroup.Text = "Insert Branch";
                    buttonAddGroup.Image = Resources.branch_element_top;
                }
                else
                {
                    buttonAddGroup.Text = "Add Branch";
                    buttonAddGroup.Image = Resources.branch_element_bottom;
                }
            }
        }

        /// <summary>
        /// Gets the condition editor this condition line displays
        /// </summary>
        public ConditionElementEditor ConditionElementEditor
        {
            get
            {
                return editor;
            }
        }

        /// <summary>
        /// The amount the editor portion is indented
        /// </summary>
        public int Indent
        {
            get
            {
                return indent;
            }
            set
            {
                if (indent != value)
                {
                    indent = value;

                    UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Ideal width of the FilterLine
        /// </summary>
        private int IdealWidth
        {
            get
            {
                int width = editor.Right + 5;

                if (toolBarAdd.Visible)
                {
                    width += toolBarAdd.Width;
                }

                return width;
            }
        }

        /// <summary>
        /// Update the position and the width
        /// </summary>
        private void UpdateLayout()
        {
            SuspendLayout();

            editor.Location = new Point(editorXPos + indent * indentAmount, 0);

            // Update the width of the child control
            UpdateMinimumWidth();
            UpdateAddToolbarPosition();

            ResumeLayout();
        }

        /// <summary>
        /// The child control has resized itself
        /// </summary>
        private void OnItemEditorResize(object sender, EventArgs e)
        {
            UpdateMinimumWidth();
            UpdateAddToolbarPosition();
        }

        /// <summary>
        /// Update the position of the child control
        /// </summary>
        private void UpdateMinimumWidth()
        {
            MinimumSize = new Size(IdealWidth, Height);
        }

        /// <summary>
        /// Update the location of the add toolbar
        /// </summary>
        private void UpdateAddToolbarPosition()
        {
            toolBarAdd.Left = editor.Right + 2;
        }

        /// <summary>
        /// Delete button clicked
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (Delete != null)
            {
                Delete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Add Condition button clicked
        /// </summary>
        private void OnAddCondition(object sender, EventArgs e)
        {
            if (AddCondition != null)
            {
                AddCondition(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Add group button clicked
        /// </summary>
        private void OnAddGroup(object sender, EventArgs e)
        {
            if (AddGroup != null)
            {
                AddGroup(this, EventArgs.Empty);
            }
        }
    }
}
