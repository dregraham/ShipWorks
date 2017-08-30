using System;
using System.Threading.Tasks;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Handle an exception and return a result
    /// </summary>
    public class ExceptionResultHandler<TException> where TException : Exception
    {
        /// <summary>
        /// Execute an action, handling an exception
        /// </summary>
        public IResult Execute(Action action)
        {
            try
            {
                action();
                return Result.FromSuccess();
            }
            catch (TException ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Execute an action, handling an exception
        /// </summary>
        public GenericResult<T> Execute<T>(Func<T> action)
        {
            try
            {
                return GenericResult.FromSuccess(action());
            }
            catch (TException ex)
            {
                return GenericResult.FromError<T>(ex);
            }
        }

        /// <summary>
        /// Execute an action, handling an exception
        /// </summary>
        public async Task<IResult> ExecuteAsync(Func<Task> action)
        {
            try
            {
                await action().ConfigureAwait(false);
                return Result.FromSuccess();
            }
            catch (TException ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Execute an action, handling an exception
        /// </summary>
        public async Task<GenericResult<T>> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action().ConfigureAwait(false);
                return GenericResult.FromSuccess(result);
            }
            catch (TException ex)
            {
                return GenericResult.FromError<T>(ex);
            }
        }
    }
}
