using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Interapptive.Shared.Threading;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extensions on the Observable type
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Extension to process the first X events, but ignore subsequent events for the specified timespan.
        /// https://social.msdn.microsoft.com/Forums/windowsapps/en-US/b8be2034-dcdf-4577-b264-7dd9d0668318/throttle-with-count-and-time-and-result-returned-immidiattelly?forum=rx
        /// </summary>
        public static IObservable<T> IntervalCountThrottle<T>(this IObservable<T> source, TimeSpan interval, int count, IScheduler scheduler)
        {
            return Observable.Create<T>(o =>
            {
                IConnectableObservable<long> timer = Observable.Timer(TimeSpan.Zero, interval).Publish();
                IConnectableObservable<T> src = source.Publish();
                IDisposable main = src.SkipUntil(timer).Take(count).Repeat().Subscribe(o.OnNext, o.OnError, o.OnCompleted);
                return new CompositeDisposable(timer.Connect(), src.Connect(), main);
            });
        }

        /// <summary>
        /// Catch a specified exception, log it, and continue
        /// </summary>
        public static IObservable<T> CatchAndContinue<T, TException>(this IObservable<T> source,
            Action<TException> handleException) where TException : Exception
        {
            return source.Catch<T, TException>(ex =>
            {
                handleException(ex);
                return source.CatchAndContinue(handleException);
            });
        }

        /// <summary>
        /// Perform a select on the task pool while showing a Progress dialog
        /// </summary>
        /// <remarks>
        /// This method will create and open a dialog, switch to the task pool, perform the selection action,
        /// switch back to the Windows Forms event loop, close the dialog, and finally return the results of the select
        /// </remarks>
        public static IObservable<TReturn> SelectInBackgroundWithDialog<T, TReturn>(this IObservable<T> source,
            ISchedulerProvider schedulerProvider, Func<IDisposable> showProgressDialog, Func<T, TReturn> performAction)
        {
            return source.Select(x => new
            {
                WaitDialog = showProgressDialog(),
                Message = x
            })
            .ObserveOn(schedulerProvider.TaskPool)
            .Select(x => new
            {
                x.WaitDialog,
                Message = performAction(x.Message)
            })
            .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
            .Do(x => x.WaitDialog.Dispose())
            .Select(x => x.Message);
        }
    }
}
