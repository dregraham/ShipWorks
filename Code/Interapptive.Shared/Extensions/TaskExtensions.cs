using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extension methods for Tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Re-throw an exception on the UI thread
        /// </summary>
        public static Task RethrowException(this Task task, Func<Control> getOwner)
        {
            return task.ContinueWith(x =>
            {
                if (x.IsFaulted && x.Exception != null)
                {
                    getOwner().BeginInvoke((Action) (() => { throw x.Exception; }));
                }
            });
        }

        /// <summary>
        /// Execute a new task based on the current task
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <returns>
        /// The new task if the original task was successful, otherwise the error of the original task
        /// </returns>
        public static async Task<TResult> Bind<T, TResult>(this Task<T> task, Func<T, Task<TResult>> func) =>
            await func(await task);

        /// <summary>
        /// Execute a new task based on the current task
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <returns>
        /// The onSuccess task if the original task was successful, otherwise the onFailure task
        /// </returns>
        public static Task<TResult> Bind<T, TResult>(this Task<T> task, Func<T, Task<TResult>> onSuccess, Func<Exception, Task<TResult>> onFailure) =>
            task.ContinueWith(t => t.Status == TaskStatus.Faulted ?
                onFailure(t.Exception.InnerException) :
                onSuccess(t.Result)).Unwrap();

        /// <summary>
        /// Map the value of a task
        /// </summary>
        /// <returns>
        /// Task that contains the mapped value if the original task succeeds, otherwise the error from the original task
        /// </returns>
        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> func) =>
            func(await task);

        /// <summary>
        /// Map the value of a task
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <returns>
        /// Task that contains the mapped value if the original task succeeds, otherwise a task that maps the original task's error
        /// </returns>
        public static Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> onSuccess, Func<Exception, TResult> onFailure) =>
            task.ContinueWith(t => t.Status == TaskStatus.Faulted ?
                onFailure(t.Exception.InnerException) :
                onSuccess(t.Result));

        /// <summary>
        /// Perform an action with the value of a task
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <returns>
        /// The original task and value if it was successful, otherwise the original error
        /// </returns>
        public static async Task<T> Do<T>(this Task<T> task, Action<T> func)
        {
            var value = await task;
            func(value);
            return value;
        }

        /// <summary>
        /// Perform an action with the result of a task
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <param name="task">Task on which to perform an action</param>
        /// <param name="onSuccess">Action to perform if the task succeeded</param>
        /// <param name="onFailure">Action to perform if the task failed</param>
        /// <returns>
        /// The original task and value if it was successful, otherwise the original error
        /// </returns>
        public static Task<T> Do<T>(this Task<T> task, Action<T> onSuccess, Action<Exception> onFailure) =>
            task.Bind(
                x => { onSuccess(x); return Task.FromResult(x); },
                ex => { onFailure(ex); return Task.FromException<T>(ex); });

        /// <summary>
        /// Recover from a failed task by providing a fall back value
        /// </summary>
        /// <typeparam name="T">Type of value the task returns</typeparam>
        /// <param name="task">Task that should be recovered</param>
        /// <param name="fallback">Func that provides a fall back value if the task failed</param>
        /// <returns>
        /// Task with the original value if the task was successful, otherwise returns fall back value
        /// </returns>
        public static Task<T> Recover<T>(this Task<T> task, Func<Exception, T> fallback) =>
            task.ContinueWith(t => t.Status == TaskStatus.Faulted ?
                fallback(t.Exception) :
                t.Result);
    }
}
