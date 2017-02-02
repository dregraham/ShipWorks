namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    public struct Result
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private Result(bool success, string message)
        {
            Message = message;
            Success = success;
        }

        /// <summary>
        /// Message accompanying the object
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Whether or not the operation was a success
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Whether or not the operation was a failure
        /// </summary>
        /// <remarks>This is so that we can test for failure instead of not success</remarks>
        public bool Failure => !Success;

        /// <summary>
        /// Get a successful result
        /// </summary>
        public static Result FromSuccess() => new Result(true, null);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static Result FromError(string message) => new Result(false, message);
    }
}
