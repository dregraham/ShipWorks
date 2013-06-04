using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions;
using System.Collections;
using System.Text.RegularExpressions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Editor for the OrderNumberCondition
    /// </summary>
    public partial class NumericStringValueEditor<T> : ValueEditor where T: struct, IComparable
    {
        NumericStringCondition<T> condition;

        NumericValueEditor<T> numericEditor;

        bool suspendChangeNotification = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public NumericStringValueEditor(NumericStringCondition<T> condition)
        {
            InitializeComponent();

            this.condition = condition;

            ArrayList enumList = new ArrayList 
                { 
                    StringOperator.Equals, 
                    StringOperator.NotEqual, 
                    StringOperator.BeginsWith, 
                    StringOperator.EndsWith,
                    NumericOperator.GreaterThan,
                    NumericOperator.GreaterThanOrEqual,
                    NumericOperator.LessThan,
                    NumericOperator.LessThanOrEqual,
                    NumericOperator.Between,
                    NumericOperator.NotBetween
                };

            labelOperator.InitializeFromEnumList(enumList);
            labelOperator.SelectedValue = condition.IsNumeric ? (Enum) condition.Operator : (Enum) condition.StringOperator;

            // Create the numeric editor
            numericEditor = new NumericValueEditor<T>(condition);
            numericEditor.HideOperator = true;
            numericEditor.Location = new Point(stringValueBox.Left, 0);
            Controls.Add(numericEditor);

            if (!condition.IsNumeric)
            {
                stringValueBox.Text = condition.StringValue;
            }

            // Listen for changes
            stringValueBox.TextChanged += new EventHandler(OnStringContentChanged);
            numericEditor.ContentChanged += new EventHandler(OnNumericContentChanged);

            UpdateEditorVisibility();
        }

        /// <summary>
        /// The operator has changed
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            suspendChangeNotification = true;

            // A StringOperator is selected
            if (labelOperator.SelectedValue is StringOperator)
            {
                condition.StringOperator = (StringOperator) labelOperator.SelectedValue;

                // If it used to be numeric, import the Value1 from the numeric editor
                if (condition.IsNumeric)
                {
                    condition.IsNumeric = false;

                    // We have to do this before we pull Value1, b\c the NumericValueEditor doesnt
                    // parse the value until it loses focus.
                    stringValueBox.Visible = true;
                    stringValueBox.Focus();

                    stringValueBox.Text = condition.Value1.ToString();
                    condition.StringValue = stringValueBox.Text;
                }
            }
            // A NumericOperator is selected.
            else
            {
                numericEditor.NumericOperator = (NumericOperator) labelOperator.SelectedValue;

                // Changing from a string operator
                if (!condition.IsNumeric)
                {
                    condition.IsNumeric = true;

                    // Transfer the value from the string box to the numeric box
                    T value;
                    if (NumericValueEditor<T>.TryParse(Regex.Replace(stringValueBox.Text, "[^\\d]", ""), out value))
                    {
                        numericEditor.Value1 = value;
                    }
                    else
                    {
                        numericEditor.Value1 = default(T);
                    }

                    // Show and focus the numeric editor
                    numericEditor.Visible = true;
                    numericEditor.Focus();
                }
            }

            suspendChangeNotification = false;

            // Update positioning
            UpdateEditorVisibility();

            // Notify of changes
            RaiseContentChanged();
        }

        /// <summary>
        /// Content from one of our underlying editors changed
        /// </summary>
        void OnStringContentChanged(object sender, EventArgs e)
        {
            condition.StringValue = stringValueBox.Text;

            if (!suspendChangeNotification)
            {
                RaiseContentChanged();
            }
        }

        /// <summary>
        /// Content from the underlying numeric editor changed
        /// </summary>
        void OnNumericContentChanged(object sender, EventArgs e)
        {
            if (!suspendChangeNotification)
            {
                RaiseContentChanged();
            }
        }

        /// <summary>
        /// Update the "real" value editor we use.
        /// </summary>
        private void UpdateEditorVisibility()
        {
            Control editor = condition.IsNumeric ? (Control) numericEditor : stringValueBox;

            numericEditor.Visible = condition.IsNumeric;
            stringValueBox.Visible = !condition.IsNumeric;

            editor.Left = labelOperator.Right + 3;
            Width = editor.Right;
        }
    }
}
