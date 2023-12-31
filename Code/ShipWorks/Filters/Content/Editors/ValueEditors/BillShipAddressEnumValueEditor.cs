using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor that is for address based conditions
    /// </summary>
    public partial class BillShipAddressEnumValueEditor<T> : ValueEditor where T : struct
    {
        EnumCondition<T> condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillShipAddressEnumValueEditor(EnumCondition<T> condition)
        {
            InitializeComponent();

            IBillShipAddressCondition billShipCondition = condition as IBillShipAddressCondition;
            if (billShipCondition == null)
            {
                throw new InvalidOperationException("Cannot create a BillShipAddressEnumValueEditor with a condition that does not implement IBillShipAddressCondition");
            }

            this.condition = condition;

            // Fill the address type combo
            addressOperator.InitializeFromEnumType(typeof(BillShipAddressOperator));
            addressOperator.SelectedValue = billShipCondition.AddressOperator;

            // Fill the value combo
            equalityOperator.InitializeFromEnumType(typeof(EnumEqualityOperator));
            equalityOperator.SelectedValue = condition.Operator;

            // Load the combo
            targetValue.DisplayMember = "Name";
            targetValue.ValueMember = "Value";
            targetValue.DataSource = condition.ValueChoices;
            targetValue.SelectedValue = condition.Value;

            targetValueList.InitializeValuesList(condition.ValueChoices);
            targetValueList.SelectStatuses(condition.SelectedValues ?? Enumerable.Empty<T>());
            // If the value the condition wanted as it's default isn't in the list, select the first available
            if (targetValue.SelectedValue == null && targetValue.Items.Count > 0)
            {
                targetValue.SelectedIndex = 0;
            }

            UpdateComboBoxSize();
            UpdateValueVisibility(condition.Operator);

            // Start listening for changes
            equalityOperator.SelectedValueChanged += OnChangeOperator;
            addressOperator.SelectedValueChanged += OnChangeOperator;
            targetValue.SelectedValueChanged += OnTargetValueChanged;
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
        private void UpdateValueVisibility(EnumEqualityOperator op)
        {
            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;

            equalityOperator.Visible = !IsBillShipComparison(addressOp);
            equalityOperator.Left = addressOperator.Right + 3;

            if (!equalityOperator.Visible)
            {
                Width = equalityOperator.Left;

                targetValue.Visible = false;
                targetValueList.Visible = false;
            }
            else
            {
                if (op == EnumEqualityOperator.Equals || op == EnumEqualityOperator.NotEqual)
                {
                    SwitchVisibleDropdown(targetValue, targetValueList);
                }
                else
                {
                    SwitchVisibleDropdown(targetValueList, targetValue);
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
        /// Switch which dropdown is visible
        /// </summary>
        /// <param name="controlToUse">Control that will be shown</param>
        /// <param name="controlToHide">Control that will be hidden</param>
        private void SwitchVisibleDropdown(Control controlToUse, Control controlToHide)
        {
            controlToHide.Visible = false;
            controlToUse.Visible = true;
            controlToUse.Left = equalityOperator.Right + 5;
            Width = controlToUse.Right + 2;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            EnumEqualityOperator op = (EnumEqualityOperator) equalityOperator.SelectedValue;
            condition.Operator = op;

            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;
            ((IBillShipAddressCondition) condition).AddressOperator = addressOp;

            UpdateValueVisibility(op);

            // Changing the operator can affect validity
            ValidateChildren(ValidationConstraints.Visible);

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

            condition.SelectedValues = targetValueList.GetSelectedStatuses();
        }
    }
}
