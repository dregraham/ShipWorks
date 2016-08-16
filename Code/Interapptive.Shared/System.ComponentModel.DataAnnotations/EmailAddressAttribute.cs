using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Email address validation attribute
    /// Decompiled from .Net 4.6.2
    /// We will replace this if/when we get to .Net 4.6.2
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : DataTypeAttribute
    {
        // This attribute provides server-side email validation equivalent to jquery validate,
        // and therefore shares the same regular expression.  See unit tests for examples.
        private static Regex regex = CreateRegEx();

        public EmailAddressAttribute()
            : base(DataType.EmailAddress)
        {
        }

        /// <summary>
        /// Is the passed in email address valid?
        /// </summary>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;

            // Use RegEx implementation if it has been created, otherwise use a non RegEx version.
            if (regex != null)
            {
                return valueAsString != null && regex.Match(valueAsString).Length > 0;
            }
            else {
                int atCount = 0;

                foreach (char c in valueAsString)
                {
                    if (c == '@')
                    {
                        atCount++;
                    }
                }

                return (valueAsString != null
                && atCount == 1
                && valueAsString[0] != '@'
                && valueAsString[valueAsString.Length - 1] != '@');
            }
        }

        /// <summary>
        /// Create a reg ex 
        /// </summary>
        private static Regex CreateRegEx()
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            // Legacy fallback (without explicit match timeout)
            return new Regex(pattern, options);
        }
    }
}