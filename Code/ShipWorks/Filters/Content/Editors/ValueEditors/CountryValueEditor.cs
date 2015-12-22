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
    /// Value editor for conditions dealing with a comparison to a single country
    /// </summary>
    public partial class CountryValueEditor : ValueEditor
    {
        CountryCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public CountryValueEditor(CountryCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            FillCountryList();

            targetValue.Text = Geography.GetCountryName(condition.TargetValue);

            // Fill the value combo
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;
            UpdateValueVisibility();

            // Start listening for changes
            equalityOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
            targetValue.TextChanged += new EventHandler(OnTargetTextChanged);
        }

        /// <summary>
        /// Fill the list of country values
        /// </summary>
        private void FillCountryList()
        {
            foreach (string country in Geography.Countries)
            {
                targetValue.Items.Add(country);
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
        /// The text has changed in the target value box
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
            condition.TargetValue = Geography.GetCountryCode(targetValue.Text);
        }
    }
}
