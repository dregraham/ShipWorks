using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions for Funcs and Actions
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Invoke an action asynchronously
        /// </summary>
        /// <param name="invoker">Object that will perform the invocation</param>
        /// <param name="action">Action that will be invoked</param>
        /// <returns>Task that completes when the action is done</returns>
        public static Task<Unit> InvokeAsync(this ISynchronizeInvoke invoker, Action action) =>
            InvokeAsync(invoker, action.ToFunc(Unit.Default));

        /// <summary>
        /// Invoke a func asynchronously
        /// </summary>
        /// <param name="invoker">Object that will perform the invocation</param>
        /// <param name="func">Func that will be invoked</param>
        /// <returns>Task that completes with the function's value when it is done</returns>
        public static async Task<TResult> InvokeAsync<TResult>(this ISynchronizeInvoke invoker, Func<TResult> action)
        {
            if (invoker.InvokeRequired)
            {
                var factory = new TaskFactory<object>();

                var result = await factory.FromAsync(invoker.BeginInvoke(action, null), invoker.EndInvoke);
                return (TResult) result;
            }

            return action();
        }

        /// <summary>
        /// Convert an action to a Func that returns the specified value
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="action">Action to convert to a Func</param>
        /// <param name="returnValue">Value that will be returned from the created Func</param>
        /// <returns>Func that will execute the action and return the value specified</returns>
        public static Func<T> ToFunc<T>(this Action action, T returnValue) =>
            () => { action(); return returnValue; };

        /// <summary>
        /// Convert an action to a Func that returns the specified value
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="action">Action to convert to a Func</param>
        /// <param name="returnValue">Value that will be returned from the created Func</param>
        /// <returns>Func that will execute the action and return the value specified</returns>
        public static Func<T, TResult> ToFunc<T, TResult>(this Action<T> action, Func<T, TResult> createReturn) =>
            x => { action(x); return createReturn(x); };
    }
}
