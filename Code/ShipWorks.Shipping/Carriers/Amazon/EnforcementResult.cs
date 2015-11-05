namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Result of enforcing whether an amazon label is allowed to be created
    /// </summary>
    public class EnforcementResult
    {
        private static readonly EnforcementResult success = new EnforcementResult(true, string.Empty);

        /// <summary>
        /// Constructor
        /// </summary>
        private EnforcementResult(bool isValid, string failureReason)
        {
            IsValid = isValid;
            FailureReason = failureReason;
        }

        /// <summary>
        /// Create an enforcement result that has failed
        /// </summary>
        /// <param name="failureReason"></param>
        public EnforcementResult(string failureReason) : this(false, failureReason)
        {

        }

        /// <summary>
        /// Successful enforcement result
        /// </summary>
        public static EnforcementResult Success => success;

        /// <summary>
        /// Was the enforcement valid
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Why did the enforcement fail
        /// </summary>
        public string FailureReason { get; }
    }
}