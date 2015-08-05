namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Result from validating Amazon credentials
    /// </summary>
    public struct AmazonValidateCredentialsResponse
    {
        private static readonly AmazonValidateCredentialsResponse successfulResponse = new AmazonValidateCredentialsResponse(true, string.Empty);

        /// <summary>
        /// Constructor
        /// </summary>
        private AmazonValidateCredentialsResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Was validation successful
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Message from validation
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// Get a successful response
        /// </summary>
        public static AmazonValidateCredentialsResponse Succeeded()
        {
            return successfulResponse;
        }

        /// <summary>
        /// Get a failed response
        /// </summary>
        public static AmazonValidateCredentialsResponse Failed(string message)
        {
            return new AmazonValidateCredentialsResponse(false, message);
        }
    }
}
