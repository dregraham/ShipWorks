using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions
{
    [ConditionElement("Ship Address Validation Status", "Order.AddressValidationStatus")]
    public class AddressValidationStatusCondition : Condition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddressValidationStatusCondition()
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
            string column = context.GetColumnReference(OrderFields.ShipAddressValidationStatus);

            return string.Format("{0} {1} in ({2})", column, not, param);
        }

        public override ValueEditor CreateEditor()
        {
            return new AddressValidationStatusValueEditor(this);
        }
    }


}
