using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Editors;
using System.Diagnostics;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions.Special;
using ShipWorks.Filters.Content.Conditions.Customers;
using ShipWorks.Data;
using ShipWorks.Filters.Content.Conditions.OrderItems;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.OrderCharges;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.UI.Utility;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Filters.Content.Conditions.Notes;
using ShipWorks.Filters.Content.Conditions.Emails;
using ShipWorks.UI;
using ShipWorks.Filters.Content.Conditions.PaymentDetails;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Class for editing a filter condition
    /// </summary>
    public partial class FilterDefinitionEditor : UserControl
    {
        // Panel that contains all the condition lines
        EditorPanel panel;

        // The definition we are editing
        FilterDefinition definition;

        // Controls if the editor is being shown
        bool showEditor = true;

        /// <summary>
        /// Raised when the definition edited
        /// </summary>
        public event EventHandler DefinitionEdited;

        /// <summary>
        /// Raised when the height required to show the condition editor without a horizontal scroll changes.
        /// </summary>
        public event EventHandler RequiredHeightChanged;

        #region EditorPanel

        /// <summary>
        /// Special Panel that allows for AutoScroll to work with Controls that have a Right anchor.
        /// </summary>
        class EditorPanel : Panel
        {
            /// <summary>
            /// Prevents the scroll location from changing as the active control changes
            /// </summary>
            protected override Point ScrollToControl(Control activeControl)
            {
                return new Point(AutoScrollPosition.X, AutoScrollPosition.Y);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterDefinitionEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The error provider that can be used by all child controls
        /// </summary>
        [Browsable(false)]
        public ErrorProvider ErrorProvider
        {
            get { return errorProvider; }
        }

        /// <summary>
        /// Indicates if the editor panel is currently shown
        /// </summary>
        [DefaultValue(true)]
        public bool ShowEditor
        {
            get
            {
                return showEditor;
            }
            set
            {
                showEditor = value;

                if (panel != null)
                {
                    panel.Visible = showEditor;
                }
            }
        }

        /// <summary>
        /// The Height required for the condition editor to be for the entire content to fit.
        /// </summary>
        [Browsable(false)]
        public int RequiredHeight
        {
            get
            {
                if (panel == null)
                {
                    return 0;
                }

                return panel.Controls.Cast<Control>().Sum(c => c.Height);
            }
        }

        /// <summary>
        /// The definition being edited.
        /// </summary>
        public FilterDefinition FilterDefinition
        {
            get { return definition; }
        }

        /// <summary>
        /// Validate and save to the definition.  If not valid, false is returned and its not saved.
        /// </summary>
        public bool SaveDefinition()
        {
            if (panel.Controls.Count == 0)
            {
                return false;
            }

            // Validate everything thats visible
            if (!ValidateChildren(ValidationConstraints.Visible))
            {
                return false;
            }

            definition.RootContainer = GetRootContainer();

            return true;
        }

        #region Loading

        /// <summary>
        /// Load the given filter definition for editing
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadDefinition(FilterDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            this.definition = definition;

            // We would lose focus as we load, causing a problem
            if (this.ContainsFocus)
            {
                Parent.Focus();
            }

            Cursor.Current = Cursors.WaitCursor;

            SuspendLayout();

            // We need a new panel to hold everything
            panel = new EditorPanel();
            panel.Visible = false;
            panel.SuspendLayout();

            panel.Location = new Point(0, 0);
            panel.Width = Width;
            panel.Height = Height;
            panel.Dock = DockStyle.Fill;

            // This adds all the filter lines
            AddContainer(definition.RootContainer, 0);

            panel.AutoScroll = true;
            Point autoScrollPosition = new Point(0, 0);

            Panel previous = null;

            // See have a panel showing right now
            if (Controls.Count == 1)
            {
                previous = (Panel) Controls[0];
                autoScrollPosition = new Point(-previous.AutoScrollPosition.X, -previous.AutoScrollPosition.Y);
            }

            // Add the new panel to show
            Controls.Add(panel);

            panel.ResumeLayout();

            panel.AutoScrollPosition = autoScrollPosition;
            panel.AutoScrollMinSize = CalculateAutoScrollMinSize(panel);
            panel.Layout += new LayoutEventHandler(OnPanelLayout);

            if (previous != null)
            {
                ClearEditorPanel(previous);
            }

            ResumeLayout();
            panel.Visible = showEditor;

            Cursor.Current = Cursors.Default;

            RaiseRequiredHeightChanged();
            RaiseDefinitionEdited();
        }

        /// <summary>
        /// Clear the given editor panel from the editor
        /// </summary>
        private void ClearEditorPanel(Panel previous)
        {
            List<ConditionLineControl> lines = previous.Controls.Cast<ConditionLineControl>().ToList();
            lines.ForEach(l => RemoveConditionLine(l));

            previous.Layout -= new LayoutEventHandler(OnPanelLayout);
            Controls.Remove(previous);
            previous.Dispose();
        }

        /// <summary>
        /// Add a condition group container to the editor panel.
        /// </summary>
        private void AddContainer(ConditionGroupContainer container, int indent)
        {
            // Add in the first group of the container
            AddGroup(container.FirstGroup, indent);

            // See if there is a nested container to add
            if (container.SecondGroup != null)
            {
                // Add the editor for the join between the groups (And \ Or)
                ConditionGroupContainerEditor containerEditor = new ConditionGroupContainerEditor(container);
                AddConditionLine(containerEditor, indent);

                // Add the second group
                AddContainer(container.SecondGroup, indent + 1);
            }
        }

        /// <summary>
        /// Add the ConditionGroup from the specified container to the editor
        /// </summary>
        private void AddGroup(ConditionGroup group, int indent)
        {
            // Create the editor for the join type for the group
            ConditionGroupEditor groupEditor = new ConditionGroupEditor(group);
            AddConditionLine(groupEditor, indent);

            // If there are any conditions in the group, add them in
            if (group.Conditions.Count > 0)
            {
                foreach (Condition condition in group.Conditions)
                {
                    AddCondition(condition, indent + 1);
                }
            }
            // Otherwise, add a placeholder
            else
            {
                ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(group);
                AddConditionLine(placeholderEditor, indent + 1);
            }
        }

        /// <summary>
        /// Adds a single condition to the definition editor
        /// </summary>
        private void AddCondition(Condition condition, int indent)
        {
            ConditionEditor editor = new ConditionEditor(condition);
            AddConditionLine(editor, indent);

            // If its a container condition, we have to add its child container
            ContainerCondition containerCondition = condition as ContainerCondition;
            if (containerCondition != null)
            {
                AddContainer(containerCondition.Container, indent + 1);
            }
        }

        /// <summary>
        /// Adds a single line to the definition editor
        /// </summary>
        private void AddConditionLine(ConditionElementEditor editor, int indent)
        {
            // Line number of this condition
            int lineNumber = panel.Controls.Count;

            // Insert it
            InsertConditionLine(lineNumber, editor, indent);
        }

        /// <summary>
        /// Inserts the condition line into the control, all other controls are pushed down.
        /// </summary>
        private void InsertConditionLine(int lineNumber, ConditionElementEditor editor, int indent)
        {
            panel.SuspendLayout();

            // If its a Condition Editor, we want to know if its line toggles the actual condition type it is
            ConditionEditor conditionEditor = editor as ConditionEditor;
            if (conditionEditor != null)
            {
                conditionEditor.ConditionTypeChanged += new ConditionTypeChangedEventHandler(OnConditionTypeChanged);
            }

            // We want to know when its content changes
            editor.ContentChanged += new EventHandler(OnContentChanged);

            // Create the condition line
            ConditionLineControl line = new ConditionLineControl(editor, indent);

            // Hook events
            line.Delete += new EventHandler(OnDeleteLine);
            line.AddCondition += new EventHandler(OnAddCondition);
            line.AddGroup += new EventHandler(OnAddGroup);

            // Set its location
            line.Dock = DockStyle.Top;

            // Add the line
            panel.Controls.Add(line);

            // Its reverse in the control collection as its line number, due to the way z-order works with DockStyle.Top
            panel.Controls.SetChildIndex(line, GetControlIndexFromLineNumber(lineNumber));

            // Keep the tab order in the same order as it is visually
            line.TabIndex = lineNumber;

            // Keep the alternating background colors looking correct
            UpdateLineBackgroundColors();

            panel.ResumeLayout();
        }

        /// <summary>
        /// Removes the given line from the editor and unhooks all events
        /// </summary>
        private void RemoveConditionLine(ConditionLineControl line)
        {
            line.Delete -= new EventHandler(OnDeleteLine);
            line.AddCondition -= new EventHandler(OnAddCondition);
            line.AddGroup -= new EventHandler(OnAddGroup);

            line.ConditionElementEditor.ContentChanged -= new EventHandler(OnContentChanged);

            ConditionEditor conditionEditor = line.ConditionElementEditor as ConditionEditor;
            if (conditionEditor != null)
            {
                conditionEditor.ConditionTypeChanged -= new ConditionTypeChangedEventHandler(OnConditionTypeChanged);
            }

            Controls.Remove(line);
            line.Dispose();
        }

        #endregion

        #region Action Handling

        /// <summary>
        /// Creates the condition that is used by default when the user adds a new condition
        /// </summary>
        private Condition CreateDefaultCondition(ConditionEntityTarget entityTarget)
        {
            switch (entityTarget)
            {
                case ConditionEntityTarget.Customer:
                    return new OrderCountCondition();

                case ConditionEntityTarget.Order:
                    return new OrderTotalCondition();

                case ConditionEntityTarget.OrderItem:
                    return new OrderItemSkuCondition();

                case ConditionEntityTarget.OrderCharge:
                    return new OrderChargeAmountCondition();

                case ConditionEntityTarget.PaymentDetail:
                    return new PaymentDetailLabelCondition();

                case ConditionEntityTarget.Shipment:
                    return new CarrierCondition();

                case ConditionEntityTarget.Note:
                    return new NoteTextCondition();

                case ConditionEntityTarget.Email:
                    return new EmailDateCondition();

                case ConditionEntityTarget.Printed:
                    return new PrintDateCondition();

                default:
                    throw new InvalidOperationException(string.Format("Unhandled ConditionEntityTarget type. {0}", entityTarget));
            }
        }

        /// <summary>
        /// Create a new condition
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnAddCondition(object sender, EventArgs e)
        {
            // The line that the Add button was clicked for
            ConditionLineControl actionLine = (ConditionLineControl) sender;
            int lineNumber = GetConditionLineNumber(actionLine);

            bool addBelowCondition = true;

            // Cant add conditions directly to a container
            if (actionLine.ConditionElementEditor is ConditionGroupContainerEditor)
            {
                throw new InvalidOperationException("Cannot add conditions to a ConditionGroupContainer.");
            }

            panel.SuspendLayout();

            int indentLevel = actionLine.Indent;

            Condition conditionToAdd = CreateDefaultCondition(actionLine.ConditionElementEditor.ConditionElement.GetScopedEntityTarget());

            // If its a group, then pretend like they clicked add on the first line of the group
            ConditionGroupEditor groupEditor = actionLine.ConditionElementEditor as ConditionGroupEditor;
            if (groupEditor != null)
            {
                ConditionLineControl firstConditionLine = (ConditionLineControl) panel.Controls[GetControlIndexFromLineNumber(lineNumber + 1)];

                // See if all thats in the group right now is the placeholder
                ConditionPlaceholderEditor placeholder = firstConditionLine.ConditionElementEditor as ConditionPlaceholderEditor;
                if (placeholder != null)
                {
                    // Remove the line in the gui
                    panel.Controls.Remove(firstConditionLine);
                }

                groupEditor.ConditionGroup.Conditions.Add(conditionToAdd);
                indentLevel++;

                if (addBelowCondition)
                {
                    lineNumber += (this.GetLinesRequiredByGroup(groupEditor.ConditionGroup) - 1);
                }
                else
                {
                    if (placeholder == null)
                    {
                        lineNumber = GetConditionLineNumber(firstConditionLine);
                    }
                    else
                    {
                        lineNumber++;
                    }
                }
            }

            // Could be a placeholder
            ConditionPlaceholderEditor placeholderEditor = actionLine.ConditionElementEditor as ConditionPlaceholderEditor;
            if (placeholderEditor != null)
            {
                // Add the new condiiton
                placeholderEditor.ConditionGroup.Conditions.Add(conditionToAdd);

                // Remove the line in the gui
                RemoveConditionLine(actionLine);
            }

            // Its a plain condition
            ConditionEditor conditionEditor = actionLine.ConditionElementEditor as ConditionEditor;
            if (conditionEditor != null)
            {
                if (addBelowCondition)
                {
                    int linesUsedByCondition = GetLinesRequiredByCondition(conditionEditor.Condition);
                    lineNumber += linesUsedByCondition;
                }

                // Get the group the selected condition belongs to
                ConditionGroup group = conditionEditor.Condition.ParentGroup;

                // add the new condition right under the group
                group.Conditions.Insert(group.Conditions.IndexOf(conditionEditor.Condition) + (addBelowCondition ? 1 : 0), conditionToAdd);
            }

            // Add the line
            ConditionEditor editor = new ConditionEditor(conditionToAdd);
            editor.Visible = false;

            InsertConditionLine(lineNumber, editor, indentLevel);

            RaiseRequiredHeightChanged();
            panel.ResumeLayout();

            editor.Visible = true;
            editor.Focus();

            RaiseDefinitionEdited();
        }

        /// <summary>
        /// Create a new group
        /// </summary>
        private void OnAddGroup(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // The line that the Add button was clicked for
            ConditionLineControl actionLine = (ConditionLineControl) sender;
            int lineNumber = GetConditionLineNumber(actionLine);

            // Get the join editor that is initiating the add
            ConditionGroupEditor groupEditor = (ConditionGroupEditor) actionLine.ConditionElementEditor;

            // Get the container that is being modified
            ConditionGroupContainer container = groupEditor.ConditionGroup.ParentContainer;

            // Create a new group
            ConditionGroup newGroup = new ConditionGroup();

            panel.SuspendLayout();

            // If the container of this group already has a second group, then we are doing an insert, and pushing
            // this whole container down as a child of a new container.
            if (container.SecondGroup != null)
            {
                ConditionGroupContainer newContainer = new ConditionGroupContainer(newGroup);
                newContainer.ParentContainer = container.ParentContainer;
                newContainer.SecondGroup = container;

                // Create the editor for the join type for the group
                ConditionGroupEditor newGroupEditor = new ConditionGroupEditor(newGroup);
                InsertConditionLine(lineNumber, newGroupEditor, actionLine.Indent);

                // Since its a new group, it needs a placeholder
                ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(newGroup);
                InsertConditionLine(lineNumber + 1, placeholderEditor, actionLine.Indent + 1);

                // We need a group joiner
                ConditionGroupContainerEditor containerEditor = new ConditionGroupContainerEditor(newContainer);
                InsertConditionLine(lineNumber + 2, containerEditor, actionLine.Indent);

                // Adjust the following indent
                AdjustLineIndent(lineNumber + 3, GetLinesRequiredByContainer(newContainer.SecondGroup), 1);
            }

            // The container of this group has no second group.  Se we are doing an add, and making the new group
            // the second group of the parent container.
            else
            {
                // Advance the insertion point past the first group
                lineNumber += GetLinesRequiredByGroup(groupEditor.ConditionGroup);

                // Set the second group
                container.SecondGroup = new ConditionGroupContainer(newGroup);

                // We need a group joiner
                ConditionGroupContainerEditor containerEditor = new ConditionGroupContainerEditor(container);
                InsertConditionLine(lineNumber, containerEditor, actionLine.Indent);

                // Create the editor for the join type for the group
                ConditionGroupEditor newGroupEditor = new ConditionGroupEditor(newGroup);
                InsertConditionLine(lineNumber + 1, newGroupEditor, actionLine.Indent + 1);

                // Its a new group, it needs a placeholder
                ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(newGroup);
                InsertConditionLine(lineNumber + 2, placeholderEditor, actionLine.Indent + 2);
            }

            // Adding groups can affect what you can do with each line
            UpdateLineActions();

            RaiseRequiredHeightChanged();
            panel.ResumeLayout();

            RaiseDefinitionEdited();
        }

        /// <summary>
        /// A condition line is being deleted
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnDeleteLine(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            panel.SuspendLayout();

            // The line that the delete button was clicked for
            ConditionLineControl actionLine = (ConditionLineControl) sender;
            int lineNumber = GetConditionLineNumber(actionLine);

            // We will be modifying the collection in the loop, so we need a copy.
            Control[] lines = new Control[panel.Controls.Count];
            panel.Controls.CopyTo(lines, 0);

            // Reverse it into line order, instead of control index z-order
            Array.Reverse(lines);

            // If its a group join for a container, delete the second group for the container
            ConditionGroupContainerEditor containerEditor = actionLine.ConditionElementEditor as ConditionGroupContainerEditor;
            if (containerEditor != null)
            {
                ConditionGroupContainer container = containerEditor.ConditionGroupContainer;

                // How many editor lines we will delete.  The lines in the group, plus the And\Or join line.
                int linesToDelete = GetLinesRequiredByContainer(container.SecondGroup) + 1;

                container.SecondGroup = null;
                container.JoinType = ConditionGroupJoinType.And;

                // Delete the lines from the group, pluse the old container group join line
                for (int i = lineNumber; linesToDelete > 0; i++)
                {
                    RemoveConditionLine((ConditionLineControl) lines[i]);
                    linesToDelete--;
                }
            }

            // If its a group, then remove the group from its parent container
            ConditionGroupEditor groupEditor = actionLine.ConditionElementEditor as ConditionGroupEditor;
            if (groupEditor != null)
            {
                ConditionGroup group = groupEditor.ConditionGroup;
                ConditionGroupContainer container = group.ParentContainer;

                // How many editor lines we will delete.  The lines in the group, plus the And\Or join line.
                int linesToDelete = GetLinesRequiredByGroup(group) + 1;

                // If there is a second group, we move it up to fill in where the first group was
                if (container.SecondGroup != null)
                {
                    ConditionGroupContainer containerMovingUp = container.SecondGroup;

                    // The parent will be null if we are the root container
                    if (container.ParentContainer != null)
                    {
                        container.ParentContainer.SecondGroup = container.SecondGroup;
                    }
                    else
                    {
                        // The second group is moving up to the root
                        container.SecondGroup = null;
                    }
                    
                    // Delete the lines from the group, pluse the old container group join line
                    for (int i = lineNumber; linesToDelete > 0; i++)
                    {
                        RemoveConditionLine((ConditionLineControl) lines[i]);
                        linesToDelete--;
                    }

                    AdjustLineIndent(lineNumber, GetLinesRequiredByContainer(containerMovingUp), -1);
                }
                // The group is the only one in the container, remove the container completely
                else
                {
                    ConditionGroupContainer parent = container.ParentContainer;

                    parent.SecondGroup = null;
                    parent.JoinType = ConditionGroupJoinType.And;

                    ConditionLineControl lineBefore = (ConditionLineControl) lines[lineNumber - 1];
                    ConditionEditor lineBeforeConditionEditor = lineBefore.ConditionElementEditor as ConditionEditor;
                    
                    // Remove all the lines, starting with the one above the action line, which is the And\Or line, or in the case of a ContainerCondition,
                    // would be the condition line.
                    for (int i = lineNumber - 1; linesToDelete > 0; i++)
                    {
                        RemoveConditionLine((ConditionLineControl) lines[i]);
                        linesToDelete--;
                    }

                    // If the line before us is a condition editor that is a container condition, then that condition is being wiped
                    if (lineBeforeConditionEditor != null && lineBeforeConditionEditor.Condition is ContainerCondition)
                    {
                        ConditionGroup parentGroup = lineBeforeConditionEditor.Condition.ParentGroup;

                        // Remove this conditions
                        parentGroup.Conditions.Remove(lineBeforeConditionEditor.Condition);

                        // If this group has no more conditions, we now need a placeholder
                        if (parentGroup.Conditions.Count == 0)
                        {
                            ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(parentGroup);
                            InsertConditionLine(lineNumber - 1, placeholderEditor, actionLine.Indent - 1);
                        }
                    }
                }
            }

            // Its a plain condition
            ConditionEditor conditionEditor = actionLine.ConditionElementEditor as ConditionEditor;
            if (conditionEditor != null)
            {
                ConditionGroup parentGroup = conditionEditor.Condition.ParentGroup;

                // Remove the condition from its group
                conditionEditor.Condition.ParentGroup = null;

                // How many editor lines we will delete.
                int linesToDelete = GetLinesRequiredByCondition(conditionEditor.Condition);

                // Delete the lines from the group, pluse the old container group join line
                for (int i = lineNumber; linesToDelete > 0; i++)
                {
                    RemoveConditionLine((ConditionLineControl) lines[i]);
                    linesToDelete--;
                }

                // If the group doesnt have any more conditions, we have to add a placeholder
                if (parentGroup.Conditions.Count == 0)
                {
                    ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(parentGroup);
                    InsertConditionLine(lineNumber, placeholderEditor, actionLine.Indent);
                }
            }

            UpdateLineBackgroundColors();
            UpdateLineActions();

            RaiseRequiredHeightChanged();
            panel.ResumeLayout();

            RaiseDefinitionEdited();
        }

        /// <summary>
        /// The content of the conditions is changing.  A value, a group, whatever.
        /// </summary>
        void OnContentChanged(object sender, EventArgs e)
        {
            RaiseDefinitionEdited();
        }

        /// <summary>
        /// A condition line is potentially changing from a standard to a container condition or vice versa
        /// </summary>
        void OnConditionTypeChanged(object sender, ConditionTypeChangedEventArgs e)
        {
            ConditionEditor editor = (ConditionEditor) sender;
            ConditionLineControl actionLine = (ConditionLineControl) editor.Parent;

            ContainerCondition oldContainer = e.OldCondition as ContainerCondition;
            ContainerCondition newContainer = e.NewCondition as ContainerCondition;

            // Changing from standard to standard, do nothing
            if (oldContainer == null && newContainer == null)
            {
                return;
            }

            panel.SuspendLayout();

            // If changing from container
            if (oldContainer != null)
            {
                // Get the following line
                int nextLineIndex = panel.Controls.IndexOf(actionLine) - 1;
                ConditionLineControl nextLine = (ConditionLineControl) panel.Controls[nextLineIndex];

                // The following line represents the join editor for the first group of the old container condition
                ConditionGroupContainer removedContainer = ((ConditionGroupEditor) nextLine.ConditionElementEditor).ConditionGroup.ParentContainer;

                // We will be modifying the collection in the loop, so we need a copy.
                Control[] lines = new Control[panel.Controls.Count];
                panel.Controls.CopyTo(lines, 0);

                // Remove as many lines as necessar
                int linesToRemove = GetLinesRequiredByContainer(removedContainer);
                for (int i = nextLineIndex; linesToRemove > 0; i--)
                {
                    RemoveConditionLine((ConditionLineControl) lines[i]);
                    linesToRemove--;
                }
            }

            // If changing to a container
            if (newContainer != null)
            {
                int lineNumber = GetConditionLineNumber(actionLine);

                // Create the editor for the join type for the group
                ConditionGroupEditor groupEditor = new ConditionGroupEditor(newContainer.Container.FirstGroup);
                InsertConditionLine(lineNumber + 1, groupEditor, actionLine.Indent + 1);

                // Since its a new group, it needs a placeholder
                ConditionPlaceholderEditor placeholderEditor = new ConditionPlaceholderEditor(newContainer.Container.FirstGroup);
                InsertConditionLine(lineNumber + 2, placeholderEditor, actionLine.Indent + 2);
            }

            UpdateLineBackgroundColors();
            UpdateLineActions();

            RaiseRequiredHeightChanged();
            panel.ResumeLayout();

            actionLine.Focus();
        }

        #endregion

        #region Utility

        /// <summary>
        /// Get the root container being edited
        /// </summary>
        private ConditionGroupContainer GetRootContainer()
        {
            return ((ConditionGroupEditor) ((ConditionLineControl) panel.Controls[panel.Controls.Count - 1]).ConditionElementEditor).ConditionGroup.ParentContainer;
        }

        /// <summary>
        /// Set the background colors of the lines to be alternating
        /// </summary>
        private void UpdateLineBackgroundColors()
        {
            // We have to adjust this due to our z-order being backwards
            int modBy = panel.Controls.Count % 2;

            Color one;
            Color two;

            if (IsInTabPage() && ThemeInformation.VisualStylesEnabled)
            {
                one = Color.White;
                two = Color.FromArgb(249, 249, 252);
            }
            else
            {
                one = Color.Transparent;

                // The FilterConditionControl test is so on XP it still looks right when in quick search
                if (Environment.OSVersion.Version.Major >= 6 || !(Parent is FilterConditionControl))
                {
                    two = Color.FromArgb(249, 249, 252);
                }
                else
                {
                    two = DisplayHelper.LightenColor(SystemColors.Control, .4);
                }
            }

            // Update the background color
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                panel.Controls[i].BackColor = (i % 2 == modBy) ? two : one;
                panel.Controls[i].TabIndex = panel.Controls.Count - i;
            }
        }

        /// <summary>
        /// Indicates if the control is hosted in a tab control
        /// </summary>
        private bool IsInTabPage()
        {
            Control parent = Parent;
            while (parent != null)
            {
                if (parent is TabPage)
                {
                    return true;
                }

                parent = parent.Parent;
            }

            return false;
        }

        /// <summary>
        /// Get the line number of the given condition line
        /// </summary>
        private int GetConditionLineNumber(ConditionLineControl conditionLine)
        {
            return panel.Controls.Count - 1 - panel.Controls.IndexOf(conditionLine);
        }

        /// <summary>
        /// Convert the line number to the correct control index.  This is necessary due to the way z-order works with DockStyle.Top
        /// </summary>
        private int GetControlIndexFromLineNumber(int lineNumber)
        {
            return (lineNumber >= panel.Controls.Count) ? 0 : (panel.Controls.Count - 1) - lineNumber;
        }

        /// <summary>
        /// The layout for the panel is changing
        /// </summary>
        void OnPanelLayout(object sender, LayoutEventArgs e)
        {
            Panel eventPanel = (Panel) sender;

            Size size = CalculateAutoScrollMinSize(eventPanel);
            eventPanel.AutoScrollMinSize = size;
        }

        /// <summary>
        /// Calculate the minimum size for which scroll bars should be shown
        /// </summary>
        private Size CalculateAutoScrollMinSize(Panel panel)
        {
            int minimumScrollWidth = 0;

            // Set them to default anchor
            foreach (Control control in panel.Controls)
            {
                minimumScrollWidth = Math.Max(minimumScrollWidth, control.MinimumSize.Width);
            }

            return new Size(minimumScrollWidth, 0);
        }

        /// <summary>
        /// Get the total number of editor lines that are required by the given condition
        /// </summary>
        private int GetLinesRequiredByCondition(Condition condition)
        {
            // One for the condition itself
            int lines = 1;

            // If its a container condition, add in the container
            ContainerCondition containerCondition = condition as ContainerCondition;
            if (containerCondition != null)
            {
                lines += GetLinesRequiredByContainer(containerCondition.Container);
            }

            return lines;
        }

        /// <summary>
        /// Get the total number of editor lines that are required by the given group
        /// </summary>
        private int GetLinesRequiredByGroup(ConditionGroup conditionGroup)
        {
            // One for the group itself.
            int lines = 1;

            // Add one for the placeholder
            if (conditionGroup.Conditions.Count == 0)
            {
                lines++;
            }
            else
            {
                foreach (Condition condition in conditionGroup.Conditions)
                {
                    lines += GetLinesRequiredByCondition(condition);
                }
            }

            return lines;
        }

        /// <summary>
        /// Get the total number of editor lines that will be used to display the given container
        /// </summary>
        private int GetLinesRequiredByContainer(ConditionGroupContainer container)
        {
            // Add in what we need for the first group
            int lines = GetLinesRequiredByGroup(container.FirstGroup);

            // If we have a second container
            if (container.SecondGroup != null)
            {
                // We need one line for the join
                lines++;

                // Then the lines for the container itself
                lines += GetLinesRequiredByContainer(container.SecondGroup);
            }

            return lines;
        }

        /// <summary>
        /// Adjust the indent level of all lines including and following the firstLine by the given amount
        /// </summary>
        private void AdjustLineIndent(int firstLine, int count, int indentBy)
        {
            for (int i = panel.Controls.Count - 1 - firstLine; i >= 0 && count > 0; i--)
            {
                ConditionLineControl line = (ConditionLineControl) panel.Controls[i];
                line.Indent += indentBy;

                count--;
            }
        }

        /// <summary>
        /// Update the actions available on each line
        /// </summary>
        private void UpdateLineActions()
        {
            foreach (ConditionLineControl control in panel.Controls)
            {
                control.UpdateActions();
            }
        }

        /// <summary>
        /// Raise the DefinitionEdited event
        /// </summary>
        private void RaiseDefinitionEdited()
        {
            if (DefinitionEdited != null)
            {
                DefinitionEdited(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raise the RequiredHeightChanged event
        /// </summary>
        private void RaiseRequiredHeightChanged()
        {
            if (RequiredHeightChanged != null)
            {
                RequiredHeightChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
