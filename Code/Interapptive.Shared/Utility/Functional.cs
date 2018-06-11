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
        public static Func<TResult> ToFunc<TResult>(Func<TResult> func) => func;
        public static Func<T, TResult> ToFunc<T, TResult>(Func<T, TResult> func) => func;
        public static Func<T1, T2, TResult> ToFunc<T1, T2, TResult>(Func<T1, T2, TResult> func) => func;
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
        public static GenericResult<TReturn> Retry<TReturn>(this Func<TReturn> func, int retries, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger)
        {
            GenericResult<int> retryCounter = retries;

            while (retryCounter.Success)
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    retryCounter = retryCounter.Bind(counter => HandleException(TimeSpan.FromSeconds(1), ex, counter, shouldRetry, logger));
                }
            }

            return retryCounter.Exception;
        }


        ///// <summary>
        ///// Retry a function a specified number of times
        ///// </summary>
        ///// <typeparam name="TReturn">Type of return value</typeparam>
        ///// <param name="func">Function to retry</param>
        ///// <param name="retries">Max number of retries</param>
        ///// <param name="shouldRetry">Can the function be retried?</param>
        //public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry) =>
        //    RetryAsync(func, retries, shouldRetry, (l, t, p) => Unit.Default);

        ///// <summary>
        ///// Retry a function a specified number of times
        ///// </summary>
        ///// <typeparam name="TReturn">Type of return value</typeparam>
        ///// <param name="func">Function to retry</param>
        ///// <param name="retries">Max number of retries</param>
        ///// <param name="shouldRetry">Can the function be retried?</param>
        ///// <param name="logger">Logger to use to on failure</param>
        //public static Task<TReturn> RetryAsync<TReturn>(this Func<Task<TReturn>> func, int retries, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger)
        //{
        //    GenericResult<int> retryCounter = retries;

        //    while (retryCounter.Success)
        //    {
        //        try
        //        {
        //            return func();
        //        }
        //        catch (Exception ex)
        //        {
        //            retryCounter = retryCounter.Bind(counter => HandleException(TimeSpan.FromSeconds(1), ex, counter, shouldRetry, logger));
        //        }
        //    }

        //    return retryCounter.Exception;
        //}

        /// <summary>
        /// Handle the exception, if possible
        /// </summary>
        private static GenericResult<int> HandleException(TimeSpan retryDelay, Exception ex, int retryCounter, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger)
        {
            if (shouldRetry(ex))
            {
                logger(Level.Warn, "{0} detected while trying to execute.  Retrying {1} more times.", new object[] { ex.GetType().Name, retryCounter });

                if (retryCounter == 0)
                {
                    logger(Level.Error, "Could not execute due to maximum retry failures reached.", Enumerable.Empty<object>().ToArray());
                    return ex;
                }

                // Wait before trying again, give the other guy some time to resolve itself
                Thread.Sleep(retryDelay);

                return retryCounter - 1;
            }

            return ex;
        }

        ///// <summary>
        ///// Handle the exception, if possible
        ///// </summary>
        //private static async Task<GenericResult<int>> HandleExceptionAsync(TimeSpan retryDelay, Exception ex, int retryCounter, Func<Exception, bool> shouldRetry, Func<Level, string, object[], Unit> logger)
        //{
        //    if (shouldRetry(ex))
        //    {
        //        logger(Level.Warn, "{0} detected while trying to execute.  Retrying {1} more times.", new object[] { ex.GetType().Name, retryCounter });

        //        if (retryCounter == 0)
        //        {
        //            logger(Level.Error, "Could not execute due to maximum retry failures reached.", Enumerable.Empty<object>().ToArray());
        //            return ex;
        //        }

        //        // Wait before trying again, give the other guy some time to resolve itself
        //        await Task.Delay(retryDelay).ConfigureAwait(false);

        //        return retryCounter - 1;
        //    }

        //    return ex;
        //}
    }
}
