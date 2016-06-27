using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Array and string maximum length validation attribute
    /// Decompiled from .Net 4.6.2
    /// We will replace this if/when we get to .Net 4.6.2
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MaxLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the MaxLengthAttribute class.
        /// </summary>
        public MaxLengthAttribute()
          : base((Func<string>)(() => DefaultErrorMessageString))
        {
            this.Length = -1;
        }

        /// <summary>
        /// Initializes a new instance of the MaxLengthAttribute class based on the <paramref name="length"/> parameter.
        /// </summary>
        /// <param name="length">The maximum allowable length of array or string data.</param>
        public MaxLengthAttribute(int length)
          : base((Func<string>)(() => DefaultErrorMessageString))
        {
            this.Length = length;
        }

        /// <summary>
        /// Gets the maximum allowable length of the array or string data.
        /// </summary>
        /// 
        /// <returns>
        /// The maximum allowable length of the array or string data.
        /// </returns>
        public int Length { get; private set; }

        /// <summary>
        /// Default error message format string
        /// </summary>
        private static string DefaultErrorMessageString => "The field {0} must be a string or array type with a maximum length of '{1}'.";

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        /// 
        /// <returns>
        /// true if the value is null, or if the value is less than or equal to the specified maximum length; otherwise, false.
        /// </returns>
        /// <param name="value">The object to validate.</param><exception cref="InvalidOperationException">Length is zero or less than negative one.</exception>
        public override bool IsValid(object value)
        {
            this.EnsureLegalLengths();
            if (value == null)
                return true;
            string str = value as string;
            int num = str == null ? ((Array)value).Length : str.Length;
            if (-1 != this.Length)
                return num <= this.Length;
            return true;
        }

        /// <summary>
        /// Applies formatting to a specified error message.
        /// </summary>
        /// 
        /// <returns>
        /// A localized string to describe the maximum acceptable length.
        /// </returns>
        /// <param name="name">The name to include in the formatted string.</param>
        public override string FormatErrorMessage(string name)
        {
            return string.Format((IFormatProvider)CultureInfo.CurrentCulture, this.ErrorMessageString, new object[2]
            {
        (object) name,
        (object) this.Length
            });
        }

        /// <summary>
        /// Ensure value has legal lengths
        /// </summary>
        private void EnsureLegalLengths()
        {
            if (this.Length == 0 || this.Length < -1)
                throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, 
                    "MaxLengthAttribute must have a Length value that is greater than zero. Use MaxLength() without parameters to indicate that the string or array can have the maximum allowable length.", 
                    new object[0]));
        }
    }
}
