using Interapptive.Shared.Extensions;
using log4net.Core;
using System;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Methods to allow easier functional composition
    /// </summary>
    public static class Functional
    {
        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<TResult> ToFunc<TResult>(Func<TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T, TResult> ToFunc<T, TResult>(Func<T, TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, TResult> ToFunc<T1, T2, TResult>(Func<T1, T2, TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, T3, TResult> ToFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) => func;

        /// <summary>
        /// Call a method with a disposable object
        /// </summary>
        /// <typeparam name="TDisposable">Type of disposable object</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <param name="disposable">Object that will be disposed after func is done</param>
        /// <param name="func">Function that will be called with the disposable object</param>
        /// <returns>Value returned by func</returns>
        public static TResult Using<TDisposable, TResult>(TDisposable disposable, Func<TDisposable, TResult> func) where TDisposable : IDisposable
        {
            using (disposable)
            {
                return func(disposable);
            }
        }

        /// <summary>
        /// Call a method with a disposable object
        /// </summary>
        /// <typeparam name="TDisposable">Type of disposable object</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <param name="disposable">Object that will be disposed after func is done</param>
        /// <param name="func">Function that will be called with the disposable object</param>
        /// <returns>Value returned by func</returns>
        public static Task<TResult> UsingAsync<TDisposable, TResult>(TDisposable disposable, Func<TDisposable, Task<TResult>> func) where TDisposable : IDisposable =>
            UsingAsync(Task.FromResult(disposable), func);

        /// <summary>
        /// Call a method with a disposable object
        /// </summary>
        /// <typeparam name="TDisposable">Type of disposable object</typeparam>
        /// <typeparam name="TResult">Return type</typeparam>
        /// <param name="disposable">Object that will be disposed after func is done</param>
        /// <param name="func">Function that will be called with the disposable object</param>
        /// <returns>Value returned by func</returns>
        public static async Task<TResult> UsingAsync<TDisposable, TResult>(Task<TDisposable> disposable, Func<TDisposable, Task<TResult>> func) where TDisposable : IDisposable
        {
            using (var x = await disposable.ConfigureAwait(false))
            {
                return await func(x).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Parse a boolean value
        /// </summary>
        /// <param name="arg">String to try and parse</param>
        /// <returns></returns>
        public static GenericResult<bool> ParseBool(string arg) =>
            bool.TryParse(arg, out bool result) ?
                result :
                GenericResult.FromError<bool>($"Could not parse {arg} as bool");

        /// <summary>
        /// Try executing a function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="func">Function to execute</param>
        /// <returns>Value if function succeeds, failure otherwise</returns>
        public static GenericResult<T> Try<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Try executing a function
        /// </summary>
        /// <typeparam name="T">Return type of the function</typeparam>
        /// <param name="func">Function to execute</param>
        /// <returns>Value if function succeeds, failure otherwise</returns>
        public static Task<T> TryAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return Task.FromException<T>(ex);
            }
        }

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, Func<Exception, bool> shouldRetry) =>
            Retry(func, retries, shouldRetry, (l, t, p) => Unit.Default);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger) =>
            Retry(func, retries, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry) =>
            RetryAsync(func, retries, shouldRetry, (l, t, p) => Unit.Default);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger) =>
            RetryAsync(func, retries, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        private static GenericResult<TReturn> Retry<TReturn>(
                this Func<TReturn> func,
                int retries,
                Func<Exception, Result> shouldRetry,
                Func<Level, string, object[], Unit> logger) =>
            Try(func)
                .Match(
                    x => x,
                    ex => CanRetry(retries, shouldRetry, logger, ex)
                        .Do(() => Thread.Sleep(1))
                        .Bind(() => Retry(func, retries - 1, shouldRetry, logger)));

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        private static Task<TReturn> RetryAsync<TReturn>(
                this Func<Task<TReturn>> func,
                int retries,
                Func<Exception, Result> shouldRetry,
                Func<Level, string, object[], Unit> logger) =>
            TryAsync(func)
                .Bind(
                    x => Task.FromResult(x),
                    ex => CanRetry(retries, shouldRetry, logger, ex)
                        .BindAsync(() => Task.Delay(1).ToTyped<Unit>())
                        .Bind(_ => RetryAsync(func, retries - 1, shouldRetry, logger)));

        /// <summary>
        /// Can the function be retried
        /// </summary>
        /// <param name="retries">How many times can we retry</param>
        /// <param name="shouldRetry">Should the function be retried</param>
        /// <param name="logger">Logger to use for failures</param>
        /// <param name="ex">Exception that caused the failure</param>
        /// <returns></returns>
        private static Result CanRetry(int retries, Func<Exception, Result> shouldRetry, Func<Level, string, object[], Unit> logger, Exception ex) =>
            shouldRetry(ex)
                .Do(() => LogRetryWarning(logger, ex.GetType().Name, retries))
                .Bind(() => AreRetriesExhausted(retries).OnFailure(_ => LogRetryError(logger)));

        /// <summary>
        /// Log a warning if the method fails
        /// </summary>
        /// <param name="logger">Logger to use</param>
        /// <param name="arguments">Arguments to log</param>
        private static void LogRetryWarning(Func<Level, string, object[], Unit> logger, params object[] arguments) =>
            logger(Level.Warn, "{0} detected while trying to execute.  Retrying {1} more times.", arguments);

        /// <summary>
        /// Log an error if there were too many retries
        /// </summary>
        /// <param name="logger">Logger to use</param>
        private static void LogRetryError(Func<Level, string, object[], Unit> logger) =>
            logger(Level.Error, "Could not execute due to maximum retry failures reached.", Enumerable.Empty<object>().ToArray());

        /// <summary>
        /// Can the function be retried based on the retry count
        /// </summary>
        /// <param name="retries"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static Result AreRetriesExhausted(int retries) =>
            retries > 0 ? Result.FromSuccess() : Result.FromError(string.Empty);
    }
}
