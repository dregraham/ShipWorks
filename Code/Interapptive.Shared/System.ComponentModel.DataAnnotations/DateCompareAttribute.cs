using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;
using System.Text.RegularExpressions;
using Shared.System.ComponentModel.DataAnnotations;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Date validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DateCompareAttribute : DataTypeAttribute
    {
        public DateCompareType minDateCompareType { get; set; }
        public ValueCompareOperatorType ValueCompareOperator { get; set; }

        /// <summary>
        /// Initializes a new instance of the DateCompareAttribute class.
        /// </summary>
        public DateCompareAttribute(DateCompareType value, ValueCompareOperatorType valueCompareOperatorType)
          : base(DataType.Date)
        {
            minDateCompareType = value;
            ValueCompareOperator = valueCompareOperatorType;
        }

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
            DateTime testValue = (DateTime)value;
            switch (minDateCompareType)
            {
                case DateCompareType.Today:
                    isValid = Compare(testValue.Date, DateTime.Today.Date);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return isValid;
        }

        /// <summary>
        /// Compare the values based on the value compare operator
        /// </summary>
        private bool Compare(DateTime testValue, DateTime constantValue)
        {
            bool value;

            switch (ValueCompareOperator)
            {
                case ValueCompareOperatorType.LessThan:
                    value = testValue < constantValue;
                    break;
                case ValueCompareOperatorType.LessThanOrEqualTo:
                    value = testValue <= constantValue;
                    break;
                case ValueCompareOperatorType.Equal:
                    value = testValue == constantValue;
                    break;
                case ValueCompareOperatorType.GreaterThanOrEqualTo:
                    value = testValue >= constantValue;
                    break;
                case ValueCompareOperatorType.GreaterThan:
                    value = testValue > constantValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return value;
        }
    }
}
