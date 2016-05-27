using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;
using System.Text.RegularExpressions;
using Shared.System.ComponentModel.DataAnnotations;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Double value validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DoubleCompareAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the DoubleCompareAttribute class.
        /// </summary>
        public DoubleCompareAttribute(object value, ValueCompareOperatorType valueCompareOperator)
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

            if (value is double)
            {
                switch (ValueCompareOperator)
                {
                    case ValueCompareOperatorType.LessThan:
                        isValid = (double)value < Convert.ToDouble(Value);
                        break;
                    case ValueCompareOperatorType.LessThanOrEqualTo:
                        isValid = (double)value <= Convert.ToDouble(Value);
                        break;
                    case ValueCompareOperatorType.Equal:
                        isValid = Math.Abs((double)value - Convert.ToDouble(Value)) < 0.000001;
                        break;
                    case ValueCompareOperatorType.GreaterThanOrEqualTo:
                        isValid = (double)value >= Convert.ToDouble(Value);
                        break;
                    case ValueCompareOperatorType.GreaterThan:
                        isValid = (double)value > Convert.ToDouble(Value);
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
