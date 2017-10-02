using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    public struct Result : IResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private Result(bool success, string message)
        {
            Message = message;
            Success = success;
            Exception = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Result(bool success, Exception exception)
        {
            Message = exception?.Message;
            Exception = exception;
            Success = success;
        }

        /// <summary>
        /// Message accompanying the object
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Exception accompanying the object
        /// </summary>
        public Exception Exception { get; }

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
        public static Result FromSuccess() => new Result(true, (string) null);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static Result FromError(string message) => new Result(false, message);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static Result FromError(Exception ex) => new Result(false, ex);

        /// <summary>
        /// Call a method and handle its exception
        /// </summary>
        public static ExceptionResultHandler<TException> Handle<TException>() where TException : Exception =>
            new ExceptionResultHandler<TException>();
    }
}
