namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Represents a token that the user can choose from.
    /// </summary>
    public class TokenSuggestion
    {
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
            Xsl = xsl;
            Description = description;
        }

        /// <summary>
        /// The actual XSL to be inserted
        /// </summary>
        public string Xsl { get; }

        /// <summary>
        /// The description of what the output of the token will be.
        /// </summary>
        public string Description { get; }
    }
}
