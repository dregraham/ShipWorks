using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Condition for filtering on whether a ShipWorks Insurance claim has been filed
    /// </summary>
    [ConditionElement("Claim", "Shipment.InsuranceClaimFiled")]
    public class ShipmentInsuranceClaimCondition : Condition
    {
        /// <summary>
        /// The operator to use when evaluating the condition
        /// </summary>
        public EqualityOperator Operator { get; set; }

        /// <summary>
        /// Generate the SQL that represents this condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(ShipmentFields.ShipmentID, InsurancePolicyFields.ShipmentID, 
                Operator == EqualityOperator.Equals ? SqlGenerationScopeType.EveryChild : SqlGenerationScopeType.NoChild))
            {
                return scope.Adorn(string.Format("{0} IS NOT NULL", context.GetColumnReference(InsurancePolicyFields.ClaimID)));
            }
        }

        /// <summary>
        /// Create the editor for the condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new ClaimFiledValueEditor(this);
        }
    }
}
