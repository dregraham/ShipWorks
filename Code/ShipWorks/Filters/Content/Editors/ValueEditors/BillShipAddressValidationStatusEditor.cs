using System;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Orders.Address;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor that is for address based conditions
    /// </summary>
    public partial class BillShipAddressValidationStatusEditor : ValueEditor
    {
        OrderAddressValidationStatusCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillShipAddressValidationStatusEditor(OrderAddressValidationStatusCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            // Fill the addrestype combo
            addressOperator.InitializeFromEnumType(typeof(BillShipAddressOperator));
            addressOperator.SelectedValue = condition.AddressOperator;

            // Fill the value combo
            equalityOperator.InitializeFromEnumType(typeof(EqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;

            // Load the combo
            addressValidationStatus.SelectStatuses(condition.StatusTypes);
            addressValidationStatus.StatusChanged = () =>
            {
                condition.StatusTypes = addressValidationStatus.GetSelectedStatuses();
                RaiseContentChanged();
            };

            UpdateValueVisibility();

            // Start listening for changes
            equalityOperator.SelectedValueChanged += OnChangeOperator;
            addressOperator.SelectedValueChanged += OnChangeOperator;
        }

        /// <summary>
        /// Update the visibility of the value operators
        /// </summary>
        private void UpdateValueVisibility()
        {
            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;

            equalityOperator.Visible = !IsBillShipComparison(addressOp);

            equalityOperator.Left = addressOperator.Right + 3;
            addressValidationStatus.Left = equalityOperator.Right + 3;

            if (!equalityOperator.Visible)
            {
                Width = equalityOperator.Left;
            }
            else
            {
                if (addressValidationStatus.Visible)
                {
                    Width = addressValidationStatus.Right + errorSpace;
                }
                else
                {
                    Width = addressValidationStatus.Left;
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
            EqualityOperator op = (EqualityOperator)equalityOperator.SelectedValue;
            condition.Operator = op;

            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            condition.AddressOperator = addressOp;

            UpdateValueVisibility();

            // Changint the operator can affect validity
            ValidateChildren(ValidationConstraints.Visible);

            RaiseContentChanged();
        }
    }
}
