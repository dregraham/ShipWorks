namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Create a result of an operation
    /// </summary>
    public struct GenericResult
    {
        /// <summary>
        /// Get a successful result
        /// </summary>
        public static GenericResult<T> FromSuccess<T>(T value) =>
            new GenericResult<T>(true, value, null);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static GenericResult<T> FromError<T>(string message) =>
            new GenericResult<T>(false, default(T), message);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static GenericResult<T> FromError<T>(string message, T value) =>
            new GenericResult<T>(false, value, message);
    }

    /// <summary>
    /// Generic Class that can be used to return an object or message
    /// </summary>
    /// <remarks>
    /// Used to return an object or message
    /// </remarks>
    public struct GenericResult<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal GenericResult(bool success, T value, string message)
        {
            Success = success;
            Message = message;
            Value = value;
        }

        /// <summary>
        /// The object being returned
        /// </summary>
        public T Value { get; }

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
    }
}
