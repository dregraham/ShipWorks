using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Class for representing the definition of a paypal transaction exclusion rule.
    /// Some transaction types can't have their details downloaded from PayPal, this is the 
    /// class used to define them
    /// </summary>
    public class PayPalTransactionExclusion
    {
        // the text to be compared
        string text;

        // the type of comparison to be done
        PayPalTransactionExclusionType type;

        public PayPalTransactionExclusion(string text, PayPalTransactionExclusionType type)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.text = text;
            this.type = type;
        }

        /// <summary>
        /// Determines if the exclusion matches the text passed in
        /// </summary>
        public bool Matches(string targetText)
        {
            if (targetText == null)
            {
                throw new ArgumentNullException("targetText");
            }

            switch (type)
            {
                case PayPalTransactionExclusionType.Match:
                    return string.Compare(targetText, text, true, CultureInfo.InvariantCulture) == 0;
                case PayPalTransactionExclusionType.Regex:
                    return Regex.IsMatch(targetText, text, RegexOptions.IgnoreCase);
            }

            return false;
        }
    }
}
