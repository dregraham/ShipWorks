using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;
using System.Text.RegularExpressions;
using Shared.System.ComponentModel.DataAnnotations;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Decimal value validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DecimalCompareAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the DecimalCompareAttribute class.
        /// </summary>
        public DecimalCompareAttribute(object value, ValueCompareOperatorType valueCompareOperator)
        {
            Value = value;
            ValueCompareOperator = valueCompareOperator;
        }

        /// <summary>
        /// The "constant" value to compare user values to.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// The ValueCompareOperatorType to use when comparing
        /// </summary>
        public ValueCompareOperatorType ValueCompareOperator { get; set; }

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            bool isValid;

            if (value is decimal)
            {
                switch (ValueCompareOperator)
                {
                    case ValueCompareOperatorType.LessThan:
                        isValid = (decimal)value < Convert.ToDecimal(Value);
                        break;
                    case ValueCompareOperatorType.LessThanOrEqualTo:
                        isValid = (decimal)value <= Convert.ToDecimal(Value);
                        break;
                    case ValueCompareOperatorType.Equal:
                        isValid = (decimal)value == Convert.ToDecimal(Value);
                        break;
                    case ValueCompareOperatorType.GreaterThanOrEqualTo:
                        isValid = (decimal)value >= Convert.ToDecimal(Value);
                        break;
                    case ValueCompareOperatorType.GreaterThan:
                        isValid = (decimal)value > Convert.ToDecimal(Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
