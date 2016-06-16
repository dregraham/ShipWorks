using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Array and string minimum length validation attribute
    /// Decompiled from .Net 4.6.2
    /// We will replace this if/when we get to .Net 4.6.2
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MinLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the MinLengthAttribute class.
        /// </summary>
        /// <param name="length">The length of the array or string data.</param>
        public MinLengthAttribute(int length)
        {
            this.Length = length;
        }

        /// <summary>
        /// Gets or sets the minimum allowable length of the array or string data.
        /// </summary>
        /// 
        /// <returns>
        /// The minimum allowable length of the array or string data.
        /// </returns>
        public int Length { get; private set; }

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified object is valid; otherwise, false.
        /// </returns>
        /// <param name="value">The object to validate.</param>
        public override bool IsValid(object value)
        {
            this.EnsureLegalLengths();
            if (value == null)
                return true;
            string str = value as string;
            return (str == null ? ((Array)value).Length : str.Length) >= this.Length;
        }

        /// <summary>
        /// Applies formatting to a specified error message.
        /// </summary>
        /// 
        /// <returns>
        /// A localized string to describe the minimum acceptable length.
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
            if (this.Length < 0)
                throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture,
                    "MinLengthAttribute must have a Length value that is zero or greater.", 
                    new object[0]));
        }
    }
}
