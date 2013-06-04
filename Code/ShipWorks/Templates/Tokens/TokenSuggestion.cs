using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Represents a token that the user can choose from.
    /// </summary>
    public class TokenSuggestion
    {
        string xsl;
        string description;

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenSuggestion(string xsl) :
            this(xsl, string.Empty)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TokenSuggestion(string xsl, string description)
        {
            this.xsl = xsl;
            this.description = description;
        }

        /// <summary>
        /// The actual XSL to be inserted
        /// </summary>
        public string Xsl
        {
            get { return xsl; }
        }

        /// <summary>
        /// The description of what the output of the token will be.
        /// </summary>
        public string Description
        {
            get { return description; }
        }
    }
}
