namespace ShipWorks.Shipping.ShipSense.Hashing
{
    /// <summary>
    /// A class representing the result of hashing of a knowledge base entry. This
    /// is to account for cases where there was insufficient data to accurately compute
    /// the hash; consumers of the hash can easily tell whether the result is valid or not.
    /// </summary>
    public class KnowledgebaseHashResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnowledgebaseHashResult"/> class.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="hashValue">The hash value.</param>
        public KnowledgebaseHashResult(bool isValid, string hashValue)
        {
            IsValid = isValid;
            HashValue = hashValue;
        }

        /// <summary>
        /// Gets a value indicating whether the hash value is considered valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the hash value.
        /// </summary>
        public string HashValue { get; private set; }
    }
}
