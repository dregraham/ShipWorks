using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor that is for address based conditions
    /// </summary>
    public partial class BillShipAddressStringValueEditor : ValueEditor
    {
        BillShipAddressStringCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillShipAddressStringValueEditor(BillShipAddressStringCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            targetValue.Text = condition.TargetValue;

            // Fill the address type combo
            addressOperator.InitializeFromEnumType(typeof(BillShipAddressOperator));
            addressOperator.SelectedValue = condition.AddressOperator;

            // Fill the value combo
            stringOperator.InitializeFromEnumType(typeof(StringOperator));
            stringOperator.SelectedValue = condition.Operator;
            UpdateValueVisibility();

            // Start listening for changes
            stringOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
            addressOperator.SelectedValueChanged += new EventHandler(OnChangeOperator);
            targetValue.TextChanged += new EventHandler(OnTargetTextChanged);
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            StringOperator op = (StringOperator) stringOperator.SelectedValue;

            stringOperator.Visible = !IsBillShipComparison(addressOp);
            targetValue.Visible = !IsBillShipComparison(addressOp) && op != StringOperator.IsEmpty;
            valueEditorPanel.Visible = condition.Operator == StringOperator.IsInList || condition.Operator == StringOperator.NotIsInList;

            stringOperator.Left = addressOperator.Right + 3;
            targetValue.Left = stringOperator.Right + 3;
            valueEditorPanel.Left = targetValue.Right;

            if (!stringOperator.Visible)
            {
                Width = stringOperator.Left;
            }
            else
            {
                if (targetValue.Visible)
                {
                    Width = targetValue.Right + errorSpace + valueEditorPanel.Width;
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
        private static bool IsBillShipComparison(BillShipAddressOperator addressOp)
        {
            return (addressOp == BillShipAddressOperator.ShipBillEqual || addressOp == BillShipAddressOperator.ShipBillNotEqual);
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            StringOperator op = (StringOperator) stringOperator.SelectedValue;
            condition.Operator = op;

            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            condition.AddressOperator = addressOp;

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
            if (!IsBillShipComparison(condition.AddressOperator) && condition.Operator == StringOperator.Matches)
            {
                try
                {
                    new Regex(targetValue.Text);
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
