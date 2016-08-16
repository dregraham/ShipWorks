using System.Collections.Generic;
using System.Linq;
using ShipWorks.AddressValidation;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Filter on the address validation status of an entity
    /// </summary>
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
        protected string GenerateSql(string column, SqlGenerationContext context)
        {
            string not = string.Empty;

            if (Operator == EqualityOperator.NotEqual)
            {
                not = "not";
            }

            string param = StatusTypes == null || StatusTypes.Count == 0 ? context.RegisterParameter(-1) : string.Join(",", StatusTypes.Select(x => context.RegisterParameter((int) x)));
         
            return string.Format("{0} {1} in ({2})", column, not, param);
        }
    }
}
