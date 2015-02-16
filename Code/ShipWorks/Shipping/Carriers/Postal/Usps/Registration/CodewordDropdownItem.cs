using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Class for populating the Stamps.com codeword/security question combo boxes.
    /// </summary>
    public class CodewordDropdownItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodewordDropdownItem"/> class.
        /// </summary>
        /// <param name="stampsCodewordType">Type of the stamps codeword.</param>
        /// <param name="questionText">The question text.</param>
        public CodewordDropdownItem(CodewordType2 stampsCodewordType, string questionText)
        {
            CodewordType = stampsCodewordType;
            QuestionText = questionText;
        }

        /// <summary>
        /// Gets or sets the type of the codeword.
        /// </summary>
        /// <value>
        /// The type of the codeword.
        /// </value>
        public CodewordType2 CodewordType { get; set; }

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
