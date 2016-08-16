using System;
using System.ComponentModel.DataAnnotations.Resources;
using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Phone validation attribute
    /// Decompiled from .Net 4.6.2
    /// We will replace this if/when we get to .Net 4.6.2
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PhoneAttribute : DataTypeAttribute
    {
        private static readonly Regex regex = PhoneAttribute.CreateRegEx();

        /// <summary>
        /// Initializes a new instance of the PhoneAttribute class.
        /// </summary>
        public PhoneAttribute()
          : base(DataType.PhoneNumber)
        {
            ErrorMessage = @"The {0} field is not a valid phone number.";
        }

        /// <summary>
        /// Determines whether the specified phone number is in a valid phone number format.
        /// </summary>
        /// 
        /// <returns>
        /// true if the phone number is valid; otherwise, false.
        /// </returns>
        /// <param name="value">The value to validate.</param>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            string input = value as string;
            if (PhoneAttribute.regex != null)
            {
                if (input != null)
                    return PhoneAttribute.regex.Match(input).Length > 0;
                return false;
            }
            if (input == null)
                return false;
            string str = PhoneAttribute.RemoveExtension(input.Replace("+", string.Empty).TrimEnd());
            bool flag = false;
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                return false;
            foreach (char c in str)
            {
                if (!char.IsDigit(c) && !char.IsWhiteSpace(c) && "-.()".IndexOf(c) == -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Create the phone attribute regex
        /// </summary>
        /// <returns></returns>
        private static Regex CreateRegEx()
        {
            return new Regex("^(\\+\\s?)?((?<!\\+.*)\\(\\+?\\d+([\\s\\-\\.]?\\d+)?\\)|\\d+)([\\s\\-\\.]?(\\(\\d+([\\s\\-\\.]?\\d+)?\\)|\\d+))*(\\s?(x|ext\\.?)\\s?\\d+)?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        }

        /// <summary>
        /// Remove the extension from the phone number
        /// </summary>
        /// <param name="potentialPhoneNumber"></param>
        private static string RemoveExtension(string potentialPhoneNumber)
        {
            int length1 = potentialPhoneNumber.LastIndexOf("ext.", StringComparison.InvariantCultureIgnoreCase);
            if (length1 >= 0 && PhoneAttribute.MatchesExtension(potentialPhoneNumber.Substring(length1 + 4)))
                return potentialPhoneNumber.Substring(0, length1);
            int length2 = potentialPhoneNumber.LastIndexOf("ext", StringComparison.InvariantCultureIgnoreCase);
            if (length2 >= 0 && PhoneAttribute.MatchesExtension(potentialPhoneNumber.Substring(length2 + 3)))
                return potentialPhoneNumber.Substring(0, length2);
            int length3 = potentialPhoneNumber.LastIndexOf("x", StringComparison.InvariantCultureIgnoreCase);
            if (length3 >= 0 && PhoneAttribute.MatchesExtension(potentialPhoneNumber.Substring(length3 + 1)))
                return potentialPhoneNumber.Substring(0, length3);
            return potentialPhoneNumber;
        }

        /// <summary>
        /// Does the extension match?
        /// </summary>
        /// <param name="potentialExtension"></param>
        private static bool MatchesExtension(string potentialExtension)
        {
            potentialExtension = potentialExtension.TrimStart();
            if (potentialExtension.Length == 0)
                return false;
            foreach (char c in potentialExtension)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
