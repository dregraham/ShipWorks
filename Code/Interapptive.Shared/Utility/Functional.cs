using System;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Methods to allow easier functional composition
    /// </summary>
    public static class Functional
    {
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
        public static GenericResult<bool> ParseBool(string arg)
        {
            if (bool.TryParse(arg, out bool result))
            {
                return GenericResult.FromSuccess(result);
            }

            return GenericResult.FromError<bool>($"Could not parse {arg} as bool");
        }
    }
}
