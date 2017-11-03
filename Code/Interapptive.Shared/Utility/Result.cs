﻿using System;
using System.Reactive;

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

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Map<TResult>(Func<GenericResult<TResult>> map) =>
            Match(map, ex => GenericResult.FromError<TResult>(ex));

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public GenericResult<TResult> Map<TResult>(Func<TResult> map) =>
            Match(() => GenericResult.FromSuccess(map()),
                ex => GenericResult.FromError<TResult>(ex));

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
    }
}
