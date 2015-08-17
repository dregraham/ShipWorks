using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Class for populating the USPS codeword/security question combo boxes.
    /// </summary>
    public class CodewordDropdownItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodewordDropdownItem"/> class.
        /// </summary>
        /// <param name="uspsCodewordType">Type of the USPS codeword.</param>
        /// <param name="questionText">The question text.</param>
        public CodewordDropdownItem(CodewordType uspsCodewordType, string questionText)
        {
            CodewordType = uspsCodewordType;
            QuestionText = questionText;
        }

        /// <summary>
        /// Gets or sets the type of the codeword.
        /// </summary>
        /// <value>
        /// The type of the codeword.
        /// </value>
        public CodewordType CodewordType { get; set; }

        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>
        /// The question text.
        /// </value>
        public string QuestionText { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return QuestionText;
        }
    }
}
