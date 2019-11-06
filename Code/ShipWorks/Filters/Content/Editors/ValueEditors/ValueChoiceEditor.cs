using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using Interapptive.Shared;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor that is for ValueChoiceEditor based conditions
    /// </summary>
    public partial class ValueChoiceEditor<T> : ValueEditor
    {
        ValueChoiceCondition<T> condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueChoiceEditor(ValueChoiceCondition<T> condition)
        {
            InitializeComponent();

            this.condition = condition;

            // Load the equality operator
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;

            // Load the combo
            targetValue.DisplayMember = "Name";
            targetValue.ValueMember = "Value";
            targetValue.DataSource = condition.ValueChoices;
            targetValue.SelectedValue = condition.Value;

            // If the value the condition wanted as it's default isn't in the list, select the first available
            if (targetValue.SelectedValue == null && targetValue.Items.Count > 0)
            {
                targetValue.SelectedIndex = 0;
            }

            UpdateComboBoxSize();
            UpdateValueVisibility();

            targetValue.SelectedValueChanged += new EventHandler(OnTargetValueChanged);
        }

        /// <summary>
        /// Update the size of the combobox to be just wide enough to fit the contents.
        /// </summary>
        private void UpdateComboBoxSize()
        {
            // At the minimum, use 100
            int width = 100;

            using (Graphics g = CreateGraphics())
            {
                foreach (ValueChoice<T> choice in targetValue.Items)
                {
                    int choiceWidth = (int) g.MeasureString(choice.Name, Font).Width;

                    if (choiceWidth > width)
                    {
                        width = choiceWidth;
                    }
                }
            }

            targetValue.Width = width + SystemInformation.VerticalScrollBarWidth + 5;
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            targetValue.Left = equalityOperator.Right + 5;
            Width = targetValue.Right + 2;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            EqualityOperator op = (EqualityOperator) equalityOperator.SelectedValue;
            condition.Operator = op;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// The selected value has changed.
        /// </summary>
        void OnTargetValueChanged(object sender, EventArgs e)
        {
            RaiseContentChanged();
        }

        /// <summary>
        /// Save the selected value as its validating
        /// </summary>
        private void OnValidating(object sender, CancelEventArgs e)
        {
            if (targetValue.SelectedValue != null)
            {
                condition.Value = (T) targetValue.SelectedValue;
            }
        }
    }
}
