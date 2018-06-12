using System;
using System.Reactive;
using System.Threading.Tasks;

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
            Exception = string.IsNullOrEmpty(message) ? null : new Exception(message);
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
        public static Result FromSuccess() => new Result(true, (string)null);

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

        /// <summary>
        /// Bind the value of the result
        /// </summary>
        /// <returns>
        /// A GenericResult containing the mapped value, or the original error
        /// </returns>
        public Result Bind(Func<Result> map) =>
            Match(map, ex => Result.FromError(ex));

        /// <summary>
        /// Bind the value of the result
        /// </summary>
        /// <returns>
        /// A GenericResult containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Bind<TResult>(Func<GenericResult<TResult>> map) =>
            Match(map, ex => GenericResult.FromError<TResult>(ex));

        /// <summary>
        /// Bind the value of the result
        /// </summary>
        /// <returns>
        /// A Task containing the mapped value, or the original error
        /// </returns>
        public Task<TResult> BindAsync<TResult>(Func<Task<TResult>> map) =>
            Match(map, ex => Task.FromException<TResult>(ex));

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A GenericResult containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Map<TResult>(Func<TResult> map) =>
            Match(() => GenericResult.FromSuccess(map()),
                ex => GenericResult.FromError<TResult>(ex));

        /// <summary>
        /// Perform an action
        /// </summary>
        /// <returns>
        /// A result containing success, or the original error
        /// </returns>
        public Result Do(Action map) =>
            Match(() => { map(); return Result.FromSuccess(); },
                ex => Result.FromError(ex));

        /// <summary>
        /// Perform an action when the result has failed
        /// </summary>
        public Result OnFailure(Action<Exception> action) =>
            Match(() => Result.FromSuccess(), ex => { action(ex); return ex; });

        /// <summary>
        /// Match on the result, calling the first method on success and the second on failure
        /// </summary>
        /// <returns>
        /// The result of the called function
        /// </returns>
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onError) =>
            Success ?
                onSuccess() :
                onError(Exception);

        /// <summary>
        /// Convert from a GenericResult to a Result
        /// </summary>
        public static implicit operator GenericResult<Unit>(Result result) =>
            result.Match(() => GenericResult.FromSuccess(Unit.Default), ex => GenericResult.FromError<Unit>(ex));

        /// <summary>
        /// Convert from a value of the result type to a successful result
        /// </summary>
        public static implicit operator Result(Unit value) =>
            Result.FromSuccess();

        /// <summary>
        /// Convert from an exception to an error result
        /// </summary>
        public static implicit operator Result(Exception exception) =>
            Result.FromError(exception);

        /// <summary>
        /// Convert from a Result to Task(Of T)
        /// </summary>
        public static implicit operator Task<Unit>(Result result) =>
            result.Match(() => Task.FromResult(Unit.Default), ex => Task.FromException<Unit>(ex));
    }
}
