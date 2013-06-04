using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Data;

namespace ShipWorks.Filters.Content.Editors
{
    /// <summary>
    /// Editor that allows the user to edit a single condition
    /// </summary>
    public partial class ConditionEditor : ConditionElementEditor
    {
        // The condition we are currently editing
        Condition condition;

        // The value editor portion of the condition
        ValueEditor valueEditor;

        /// <summary>
        /// Raised when the type of condition changes
        /// </summary>
        public event ConditionTypeChangedEventHandler ConditionTypeChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionEditor(Condition condition)
        {
            InitializeComponent();

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            this.condition = condition;

            // Load the condition list
            conditionTypes.Initialize(condition.GetScopedEntityTarget());
            conditionTypes.SelectedConditionType = ConditionElementFactory.GetDescriptor(condition.GetType());

            // Setup the gui for the initial condition
            CreateValueEditor();

            conditionTypes.SelectedConditionTypeChanged += new EventHandler(OnConditionTypeChanged);
        }

        /// <summary>
        /// The selected condition type has changed.
        /// </summary>
        private void OnConditionTypeChanged(object sender, System.EventArgs e)
        {
            Condition oldCondition = condition;

            CreateCondition();
            CreateValueEditor();

            if (ConditionTypeChanged != null)
            {
                ConditionTypeChangedEventArgs args = new ConditionTypeChangedEventArgs(oldCondition, condition);
                ConditionTypeChanged(this, args);
            }

            RaiseContentChanged();
        }

        /// <summary>
        /// Create a new default condition based on the selected item
        /// </summary>
        private void CreateCondition()
        {
            // Get the condition type
            ConditionElementDescriptor descriptor = conditionTypes.SelectedConditionType;

            // Invoke the default constructor
            Condition newCondition = (Condition) descriptor.CreateInstance();

            ConditionGroup group = condition.ParentGroup;

            // Update the group
            group.Conditions[group.Conditions.IndexOf(condition)] = newCondition;

            // This is now the condition we are working with
            condition = newCondition;
        }

        /// <summary>
        /// Create the value editor for the selected condition type
        /// </summary>
        private void CreateValueEditor()
        {
            Control control = this;
            while (control != null)
            {
                control.SuspendLayout();
                control = control.Parent;
            }

            // If there is already another editor active, remove it now
            if (valueEditor != null)
            {
                valueEditor.ContentChanged -= new EventHandler(OnValueEditorContentChanged);
                valueEditor.Resize -= new EventHandler(OnValueEditorResize);
                Controls.Remove(valueEditor);
                valueEditor.Dispose();
            }

            // Create the editor
            valueEditor = condition.CreateEditor();

            if (valueEditor != null)
            {
                valueEditor.ContentChanged += new EventHandler(OnValueEditorContentChanged);
                valueEditor.Resize += new EventHandler(OnValueEditorResize);

                // Position it
                valueEditor.Location = new Point(conditionTypes.Right + 1, 0);

                // Add the editor control
                Controls.Add(valueEditor);
            }

            UpdateWidth();

            control = this;
            while (control != null)
            {
                control.ResumeLayout();
                control = control.Parent;
            }
        }

        /// <summary>
        /// The value in the value editor has changed
        /// </summary>
        void OnValueEditorContentChanged(object sender, EventArgs e)
        {
            RaiseContentChanged();
        }

        /// <summary>
        /// The child control has resized itself
        /// </summary>
        private void OnValueEditorResize(object sender, EventArgs e)
        {
            UpdateWidth();
        }

        /// <summary>
        /// Update the position of the child control
        /// </summary>
        private void UpdateWidth()
        {
            // Update our width to be just big enough
            if (valueEditor != null)
            {
                Width = valueEditor.Right;
            }
            else
            {
                Width = conditionTypes.Right;
            }
        }

        /// <summary>
        /// The condition we are currently editing
        /// </summary>
        public Condition Condition
        {
            get { return condition; }
        }

        /// <summary>
        /// The ConditionElement being edited
        /// </summary>
        public override ConditionElement ConditionElement
        {
            get
            {
                return Condition;
            }
        }
    }
}
