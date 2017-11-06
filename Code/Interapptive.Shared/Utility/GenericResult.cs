using System;

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
            new GenericResult<T>(true, value, (string) null);

        /// <summary>
        /// Get a successful result
        /// </summary>
        public static GenericResult<T> FromSuccess<T>(T value, string message) =>
            new GenericResult<T>(true, value, message);

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

        /// <summary>
        /// Get a not found read result from exception
        /// </summary>
        public static GenericResult<T> FromError<T>(Exception ex) =>
            new GenericResult<T>(false, default(T), ex);

        /// <summary>
        /// Get an error result
        /// </summary>
        public static GenericResult<T> FromError<T>(Exception ex, T value) =>
            new GenericResult<T>(false, value, ex);
    }

    /// <summary>
    /// Generic Class that can be used to return an object or message
    /// </summary>
    /// <remarks>
    /// Used to return an object or message
    /// </remarks>
    public struct GenericResult<T> : IResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private GenericResult(bool success, T value, string message, Exception exception)
        {
            Success = success;
            Message = message;
            Value = value;
            Exception = exception;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal GenericResult(bool success, T value, string message) :
            this(success, value, message, string.IsNullOrEmpty(message) ? null : new Exception(message))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal GenericResult(bool success, T value, Exception exception) :
            this(success, value, exception?.Message, exception)
        {

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
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Map<TResult>(Func<T, GenericResult<TResult>> map) =>
            Match(map, ex => GenericResult.FromError<TResult>(ex));

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Map<TResult>(Func<T, TResult> map) =>
            Match(x => GenericResult.FromSuccess(map(x)),
                ex => GenericResult.FromError<TResult>(ex));

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public Result Map(Action<T> map) =>
            Match(x => { map(x); return Result.FromSuccess(); },
                ex => Result.FromError(ex));

        /// <summary>
        /// Match on the result, calling the first method on success and the second on failure
        /// </summary>
        /// <returns>
        /// The result of the called function
        /// </returns>
        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onError) =>
            Success ?
                onSuccess(Value) :
                onError(Exception);

        /// <summary>
        /// Convert from a Result to a GenericResult
        /// </summary>
        public static implicit operator Result(GenericResult<T> result) =>
            result.Match(x => Result.FromSuccess(), ex => Result.FromError(ex));

        /// <summary>
        /// Convert from a value of the result type to a successful result
        /// </summary>
        public static implicit operator GenericResult<T>(T value) =>
            GenericResult.FromSuccess(value);

        /// <summary>
        /// Convert from an exception to an error result
        /// </summary>
        public static implicit operator GenericResult<T>(Exception exception) =>
            GenericResult.FromError<T>(exception);
    }
}
