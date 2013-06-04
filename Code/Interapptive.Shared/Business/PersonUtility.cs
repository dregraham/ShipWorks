using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Utility methods for dealing with person contact information
    /// </summary>
    public static class PersonUtility
    {
        static Dictionary<char, char> phoneKeypadMap = new Dictionary<char, char>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static PersonUtility()
        {
            phoneKeypadMap['a'] = '2';
            phoneKeypadMap['b'] = '2';
            phoneKeypadMap['c'] = '2';

            phoneKeypadMap['d'] = '3';
            phoneKeypadMap['e'] = '3';
            phoneKeypadMap['f'] = '3';

            phoneKeypadMap['g'] = '4';
            phoneKeypadMap['h'] = '4';
            phoneKeypadMap['i'] = '4';

            phoneKeypadMap['j'] = '5';
            phoneKeypadMap['k'] = '5';
            phoneKeypadMap['l'] = '5';

            phoneKeypadMap['m'] = '6';
            phoneKeypadMap['n'] = '6';
            phoneKeypadMap['o'] = '6';

            phoneKeypadMap['p'] = '7';
            phoneKeypadMap['q'] = '7';
            phoneKeypadMap['r'] = '7';
            phoneKeypadMap['s'] = '7';

            phoneKeypadMap['t'] = '8';
            phoneKeypadMap['u'] = '8';
            phoneKeypadMap['v'] = '8';

            phoneKeypadMap['w'] = '9';
            phoneKeypadMap['x'] = '9';
            phoneKeypadMap['y'] = '9';
            phoneKeypadMap['z'] = '9';
        }

        /// <summary>
        /// Extract the first 5 digits from a US postal code
        /// </summary>
        public static string GetZip5(string postalCode)
        {
            if (postalCode.Length >= 5)
            {
                return postalCode.Substring(0, 5);
            }

            return postalCode;
        }

        /// <summary>
        /// Extract the 4 digit postfix from a US postal code
        /// </summary>
        public static string GetZip4(string postalCode)
        {
            if (postalCode.Length >= 9)
            {
                return postalCode.Substring(postalCode.Length - 4, 4);
            }

            return "";
        }

        /// <summary>
        /// Replace all characters to the numeric equivalent and strip the phone number down to the most significant 10 digits.
        /// </summary>
        public static string GetPhoneDigits10(string phone)
        {
            StringBuilder sb = new StringBuilder(phone.ToLower());

            // Replace all characters with the digit equivalent
            foreach (KeyValuePair<char, char> keypadReplacment in phoneKeypadMap)
            {
                sb.Replace(keypadReplacment.Key, keypadReplacment.Value);
            }

            string digitsOnly = Regex.Replace(sb.ToString(), @"[^0-9]", "");

            if (digitsOnly.Length <= 10)
            {
                return digitsOnly;
            }
            else
            {
                if (digitsOnly.StartsWith("1"))
                {
                    return digitsOnly.Substring(1, 10);
                }
                else
                {
                    return digitsOnly.Substring(0, 10);
                }
            }
        }
    }
}
