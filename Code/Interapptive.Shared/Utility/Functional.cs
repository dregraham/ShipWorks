using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Extensions;
using log4net;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Methods to allow easier functional composition
    /// </summary>
    public static class Functional
    {
        private static readonly TimeSpan defaultRetryDelay = TimeSpan.FromMilliseconds(1);

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<TResult> ToFunc<TResult>(this Func<TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T, TResult> ToFunc<T, TResult>(this Func<T, TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, TResult> ToFunc<T1, T2, TResult>(this Func<T1, T2, TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, T3, TResult> ToFunc<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) => func;

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<Unit> ToFunc(this Action func) => () => { func(); return Unit.Default; };

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T, Unit> ToFunc<T>(this Action<T> func) => (x) => { func(x); return Unit.Default; };

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> func) => (x, y) => { func(x, y); return Unit.Default; };

        /// <summary>
        /// Cast a method group as a function
        /// </summary>
        public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> func) => (x, y, z) => { func(x, y, z); return Unit.Default; };

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
            using (var x = await disposable.ConfigureAwait(true))
            {
                return await func(x).ConfigureAwait(true);
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
        /// Parse an integer value
        /// </summary>
        public static GenericResult<int> ParseInt(string value) =>
            int.TryParse(value, out int result) ?
                result :
                GenericResult.FromError<int>($"Could not parse {value} as int");

        /// <summary>
        /// Parse an decimal value
        /// </summary>
        public static GenericResult<decimal> ParseDecimal(string value) =>
            decimal.TryParse(value, out decimal result) ?
                result :
                GenericResult.FromError<decimal>($"Could not parse {value} as decimal");

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
            Retry(func, retries, defaultRetryDelay, shouldRetry);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, TimeSpan retryDelay, Func<Exception, bool> shouldRetry) =>
            Retry(func, retries, retryDelay, shouldRetry, NullLog.Default);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, Func<Exception, bool> shouldRetry, ILog logger) =>
            Retry(func, retries, defaultRetryDelay, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, TimeSpan retryDelay, Func<Exception, bool> shouldRetry, ILog logger) =>
            Retry(func, retries, retryDelay, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry) =>
            RetryAsync(func, retries, shouldRetry, NullLog.Default);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, TimeSpan retryDelay, Func<Exception, bool> shouldRetry) =>
            RetryAsync(func, retries, retryDelay, shouldRetry, NullLog.Default);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry, ILog logger) =>
            RetryAsync(func, retries, defaultRetryDelay, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

        /// <summary>
        /// Retry a function a specified number of times
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <param name="func">Function to retry</param>
        /// <param name="retries">Max number of retries</param>
        /// <param name="shouldRetry">Can the function be retried?</param>
        /// <param name="logger">Logger to use to on failure</param>
        public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, TimeSpan retryDelay, Func<Exception, bool> shouldRetry, ILog logger) =>
            RetryAsync(func, retries, retryDelay, ex => shouldRetry(ex) ? Result.FromSuccess() : ex, logger);

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
                TimeSpan retryDelay,
                Func<Exception, Result> shouldRetry,
                ILog logger) =>
            Try(func)
                .Match(
                    x => x,
                    ex => CanRetry(retries, shouldRetry, logger, ex)
                        .Do(() => Thread.Sleep(retryDelay))
                        .Bind(() => Retry(func, retries - 1, retryDelay, shouldRetry, logger)));

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
                TimeSpan retryDelay,
                Func<Exception, Result> shouldRetry,
                ILog logger) =>
            TryAsync(func)
                .Bind(
                    x => Task.FromResult(x),
                    ex => CanRetry(retries, shouldRetry, logger, ex)
                        .BindAsync(() => Task.Delay(retryDelay).ToTyped<Unit>())
                        .Bind(_ => RetryAsync(func, retries - 1, retryDelay, shouldRetry, logger)));

        /// <summary>
        /// Can the function be retried
        /// </summary>
        /// <param name="retries">How many times can we retry</param>
        /// <param name="shouldRetry">Should the function be retried</param>
        /// <param name="logger">Logger to use for failures</param>
        /// <param name="ex">Exception that caused the failure</param>
        /// <returns></returns>
        private static Result CanRetry(int retries, Func<Exception, Result> shouldRetry, ILog logger, Exception ex) =>
            shouldRetry(ex)
                .Bind(() => AreRetriesExhausted(retries, ex)
                    .Do(() => LogRetryWarning(logger, ex, retries))
                    .OnFailure(ex2 => LogRetryError(logger, ex2)));

        /// <summary>
        /// Log a warning if the method fails
        /// </summary>
        /// <param name="logger">Logger to use</param>
        /// <param name="arguments">Arguments to log</param>
        private static void LogRetryWarning(ILog logger, Exception ex, int retries) =>
            logger.Warn($"{ex.GetType().Name} detected while trying to execute.  Retrying {retries} more times.", ex);

        /// <summary>
        /// Log an error if there were too many retries
        /// </summary>
        /// <param name="logger">Logger to use</param>
        private static void LogRetryError(ILog logger, Exception ex) =>
            logger.Error("Could not execute due to maximum retry failures reached.", ex);

        /// <summary>
        /// Can the function be retried based on the retry count
        /// </summary>
        /// <param name="retries"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static Result AreRetriesExhausted(int retries, Exception ex) =>
            retries > 0 ? Result.FromSuccess() : ex;
    }
}
