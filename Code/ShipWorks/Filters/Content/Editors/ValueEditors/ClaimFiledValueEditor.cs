using System;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.Conditions.Shipments;

namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    /// <summary>
    /// A value editor whether a claim was filed
    /// </summary>
    public partial class ClaimFiledValueEditor : ValueEditor
    {
        private readonly ShipmentInsuranceClaimCondition condition;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClaimFiledValueEditor(ShipmentInsuranceClaimCondition condition)
        {
            InitializeComponent();

            this.condition = condition;

            labelOperator.InitializeFromEnumType(typeof(EqualityOperator), x => (EqualityOperator)x == EqualityOperator.Equals ? "has" : "has not");
            labelOperator.SelectedValue = condition.Operator;

            UpdateLayout();
        }

        /// <summary>
        /// Update the layout of the editor
        /// </summary>
        private void UpdateLayout()
        {
            beenFileLabel.Left = labelOperator.Right - 2;
        }

        /// <summary>
        /// Operator type is changing
        /// </summary>
        private void OnChangeOperator(object sender, EventArgs e)
        {
            condition.Operator = (EqualityOperator)labelOperator.SelectedValue;

            UpdateLayout();

            RaiseContentChanged();
        }
    }
}
