using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Filter on the address validation status of an entity
    /// </summary>
    [ConditionElement("Validation Status", "Order.Address.ValidationStatus")]
    public abstract class AddressValidationStatusCondition : Condition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected AddressValidationStatusCondition()
        {
            StatusTypes = new List<AddressValidationStatusType>();
        }

        /// <summary>
        /// Gets or sets the selected StatusTypes.
        /// </summary>
        public List<AddressValidationStatusType> StatusTypes { get; set; }

        /// <summary>
        /// Gets or sets the selected Comparison operator.
        /// </summary>
        public EqualityOperator Operator { get; set; }

        /// <summary> 
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string not = string.Empty;
            
            if (Operator == EqualityOperator.NotEqual)
            {
                not = "not";
            }

            string param = StatusTypes == null || StatusTypes.Count == 0 ? context.RegisterParameter(-1) : string.Join(",", StatusTypes.Select(x => context.RegisterParameter((int)x)));
            string column = context.GetColumnReference(ValidationField);

            return string.Format("{0} {1} in ({2})", column, not, param);
        }

        /// <summary>
        /// Create the editor for this filter
        /// </summary>
        /// <returns></returns>
        public override ValueEditor CreateEditor()
        {
            return new AddressValidationStatusValueEditor(this);
        }

        /// <summary>
        /// Get the validation field that should be filtered on
        /// </summary>
        protected abstract EntityField2 ValidationField { get; }
    }
}
