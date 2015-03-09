using System;
using System.ComponentModel;
using System.Drawing;
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

            // Fill the addrestype combo
            addressOperator.InitializeFromEnumType(typeof(BillShipAddressOperator));
            addressOperator.SelectedValue = billShipCondition.AddressOperator;

            // Fill the value combo
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
                    int choiceWidth = (int)g.MeasureString(choice.Name, Font).Width;

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
            BillShipAddressOperator addressOp = (BillShipAddressOperator) addressOperator.SelectedValue;

            equalityOperator.Visible = !IsBillShipComparison(addressOp);

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
            ((IBillShipAddressCondition) condition).AddressOperator = addressOp;

            UpdateValueVisibility();

            // Changint the operator can affect validity
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
                condition.Value = (T)targetValue.SelectedValue;
            }
        }
    }
}
