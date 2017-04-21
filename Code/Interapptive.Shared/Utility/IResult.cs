namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Basic interface for testing success of an operation
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Message accompanying the object
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Whether or not the operation was a success
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Whether or not the operation was a failure
        /// </summary>
        /// <remarks>This is so that we can test for failure instead of not success</remarks>
        bool Failure { get; }
    }
}