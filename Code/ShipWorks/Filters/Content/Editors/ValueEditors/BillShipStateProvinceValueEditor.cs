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
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor that is for state\province based conditions
    /// </summary>
    public partial class BillShipStateProvinceValueEditor : ValueEditor
    {
        BillShipStateProvinceCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillShipStateProvinceValueEditor(BillShipStateProvinceCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            FillStateProvinceList();

            targetValue.Text = Geography.GetStateProvName(condition.TargetValue);

            // Fill the addrestype combo
            addressOperator.InitializeFromEnumType(typeof(BillShipAddressOperator));
            addressOperator.SelectedValue = condition.AddressOperator;

            // Fill the value combo
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;
            UpdateValueVisibility();

            // Start listening for changes
            equalityOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
            addressOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
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
            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            StringOperator op = (StringOperator) equalityOperator.SelectedValue;

            equalityOperator.Visible = !IsBillShipComparison(addressOp);
            targetValue.Visible = !IsBillShipComparison(addressOp) && op != StringOperator.IsEmpty;

            equalityOperator.Left = addressOperator.Right + 3;
            targetValue.Left = equalityOperator.Right + 3;

            if (!equalityOperator.Visible)
            {
                Width = equalityOperator.Left;
            }
            else
            {
                if (targetValue.Visible)
                {
                    Width = targetValue.Right + errorSpace;
                }
                else
                {
                    Width = targetValue.Left;
                }
            }
        }

        /// <summary>
        /// Indicates if the operator is a comparison between the billing and shipping and not an external value
        /// </summary>
        private bool IsBillShipComparison(BillShipAddressOperator addressOp)
        {
            return (addressOp == BillShipAddressOperator.ShipBillEqual || addressOp == BillShipAddressOperator.ShipBillNotEqual);
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            StringOperator op = (StringOperator) equalityOperator.SelectedValue;
            condition.Operator = op;

            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            condition.AddressOperator = addressOp;

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
