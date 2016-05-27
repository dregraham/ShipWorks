using System;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extensions on the Observable type
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Throttle the stream, returning the first message in the time period instead of the last
        /// </summary>
        public static IObservable<T> IntervalCountThrottle<T>(this IObservable<T> source, TimeSpan interval, ISchedulerProvider scheduler)
        {
            IObservable<T> closeWindow = source.Delay(interval, scheduler.Default);

            return source.Window(() => closeWindow).Select(x => x.Take(1)).Merge();
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

        /// <summary>
        /// Ignore messages between when a close message is received and an open message is received
        /// </summary>
        /// <param name="messageStream">Primary stream of messages</param>
        /// <param name="closeStream">Stream that contains messages used to start ignoring</param>
        /// <param name="openStream">Stream that contains messages used to stop ignoring</param>
        public static IObservable<TMessage> IgnoreBetweenMessages<TMessage, TCloseWindow, TOpenWindow>(
            this IObservable<TMessage> messageStream,
            IObservable<TCloseWindow> closeStream, IObservable<TOpenWindow> openStream)
        {
            IObservable<TOpenWindow> openWindow = Observable.Return(default(TCloseWindow))
                .Merge(closeStream)
                .Select(_ => openStream.Take(1))
                .Switch();

            return messageStream.Window(openWindow, _ => closeStream).Merge();
        }
    }
}
