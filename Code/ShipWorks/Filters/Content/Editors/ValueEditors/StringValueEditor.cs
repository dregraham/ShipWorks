using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor for conditions that work against a string.
    /// </summary>
    public partial class StringValueEditor : ValueEditor
    {
        StringCondition condition;

        Control targetValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public StringValueEditor(StringCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            labelOperator.InitializeFromEnumType(typeof(StringOperator));
            labelOperator.SelectedValue = condition.Operator;

            ICollection<string> standard = condition.GetStandardValues();
            if (standard != null)
            {
                foreach (string value in standard)
                {
                    targetValueList.Items.Add(value);
                }

                targetValue = targetValueList;
                targetValueBox.Visible = false;
            }
            else
            {
                targetValue = targetValueBox;
                targetValueList.Visible = false;
            }

            targetValue.Text = condition.TargetValue;
            targetValue.TextChanged += new EventHandler(OnTargetTextChanged);

            UpdateValueVisibility();
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            targetValue.Visible = condition.Operator != StringOperator.IsEmpty;
            editButton.Visible = condition.Operator == StringOperator.IsInList || condition.Operator == StringOperator.NotIsInList;

            targetValue.Left = labelOperator.Right + 3;
            editButton.Left = targetValue.Right + 5;

            if (condition.Operator != StringOperator.IsEmpty)
            {
                Width = targetValue.Right + errorSpace + editButton.Width;
            }
            else
            {
                Width = targetValue.Left;
            }
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            condition.Operator = (StringOperator) labelOperator.SelectedValue;

            UpdateValueVisibility();

            // Changing the operator can affect validity
            ValidateChildren(ValidationConstraints.Visible);

            RaiseContentChanged();
        }

        /// <summary>
        /// The text has changed in the target value box
        /// </summary>
        void OnTargetTextChanged(object sender, EventArgs e)
        {
            RaiseContentChanged();
        }

        /// <summary>
        /// Validate the string
        /// </summary>
        private void OnValidating(object sender, CancelEventArgs e)
        {
            // Only matters if we are a RegEx
            if (condition.Operator == StringOperator.Matches)
            {
                try
                {
                    Regex regex = new Regex(targetValue.Text);
                }
                catch (ArgumentException ex)
                {
                    SetError(targetValue, "The Regular Expression is not valid:\n    " + ex.Message);
                    e.Cancel = true;

                    return;
                }
            }

            ClearError(targetValue);
            condition.TargetValue = targetValue.Text;
        }

        /// <summary>
        /// Use the editor to edit the list of items
        /// </summary>
        private void OnEditButtonClick(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var editor = lifetimeScope.Resolve<IStringValueListEditorViewModel>();
                targetValue.Text = editor
                    .EditList(this.ParentForm, StringCondition.ValueAsItems(targetValue.Text))
                    .Match(
                        StringCondition.ItemsAsValue,
                        ex => targetValue.Text);
            }
        }
    }
}
