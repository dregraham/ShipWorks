using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// Value editor for conditions that compare against a single state\province value
    /// </summary>
    public partial class StateProvinceValueEditor : ValueEditor
    {
        StateProvinceCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public StateProvinceValueEditor(StateProvinceCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            FillStateProvinceList();

            targetValue.Text = Geography.GetStateProvName(condition.TargetValue);

            // Fill the value combo
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;
            UpdateValueVisibility();

            // Start listening for changes
            equalityOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
            targetValue.TextChanged += new EventHandler(OnTargetTextChanged);
        }

        /// <summary>
        /// Fill the list of state province values
        /// </summary>
        private void FillStateProvinceList()
        {
            foreach (string state in Geography.States)
            {
                targetValue.Items.Add(state);
            }

            foreach (string province in Geography.Provinces)
            {
                targetValue.Items.Add(province);
            }
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            StringOperator op = (StringOperator) equalityOperator.SelectedValue;

            targetValue.Left = equalityOperator.Right + 3;
            Width = targetValue.Right + errorSpace;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            StringOperator op = (StringOperator) equalityOperator.SelectedValue;
            condition.Operator = op;

            UpdateValueVisibility();

            RaiseContentChanged();
        }

        /// <summary>
        /// The text in the target value box has changed
        /// </summary>
        void OnTargetTextChanged(object sender, EventArgs e)
        {
            RaiseContentChanged();
        }

        /// <summary>
        /// Save on validate
        /// </summary>
        private void OnValidating(object sender, CancelEventArgs e)
        {
            condition.TargetValue = Geography.GetStateProvCode(targetValue.Text);
        }
    }
}
