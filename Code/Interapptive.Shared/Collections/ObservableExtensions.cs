﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging.TrackedObservable;
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

        /// <summary>
        /// Buffer messages until no messages have been received for a specific amount of time
        /// </summary>
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> stream, TimeSpan delay, IScheduler scheduler)
        {
            IObservable<T> closes = stream.Throttle(delay, scheduler);

            return stream.Buffer(closes);
        }

        /// <summary>
        /// Waits for dependentObservable to receive a new message or for timeout time to pass, then returns.
        /// </summary>
        /// <param name="source">IObservable<TSource> for which ContinueAfter will be subscribed.</TSource></param>
        /// <param name="dependentObservable">The IObservable for which messages will be waited.</param>
        /// <param name="timeout">If no dependentObservable messages are received within this timeout, ContinueAfter will return.</param>
        /// <param name="scheduler">IScheduler to use.</param>
        public static IObservable<TSource> ContinueAfter<TSource, TDependent>(this IObservable<TSource> source, IObservable<TDependent> dependentObservable, 
                                                                              TimeSpan timeout, IScheduler scheduler)
        {
            // Merge the timer observable with the dependent observable.
            // Timer returns a long, so to merge we need the same type, so just select some long.
            IObservable<long> merged = Observable.Merge(
                                dependentObservable.Select(x => long.MinValue),
                                Observable.Timer(timeout, scheduler))
                            .Take(1);

            // Convert from the long that Timer required to the source observables that came in
            return source.SelectMany(s => merged.Select(_ => s));
        }

        /// <summary>
        /// Waits for dependentObservable to receive a new message or for timeout time to pass, then returns.
        /// </summary>
        /// <param name="source">IObservable<TSource> for which ContinueAfter will be subscribed.</TSource></param>
        /// <param name="dependentObservable">The IObservable for which messages will be waited.</param>
        /// <param name="timeout">If no dependentObservable messages are received within this timeout, ContinueAfter will return.</param>
        /// <param name="scheduler">IScheduler to use.</param>
        /// <param name="returnFunction">Call with the source and dependent objects as parameters.  If a timeout occurs, default(TDependent) 
        /// is returned as the dependent object.</param>
        public static IObservable<TReturn> ContinueAfter<TSource, TDependent, TReturn>(this IObservable<TSource> source, IObservable<TDependent> dependentObservable, 
                                                         TimeSpan timeout, IScheduler scheduler, 
                                                         Func<TSource, TDependent, TReturn> returnFunction)
        {
            // Merge the timer observable with the dependent observable, returning a default TDependent if a timeout occurs
            IObservable<TDependent> merged = Observable.Merge(
                                dependentObservable.Select(x => x),
                                Observable.Timer(timeout, scheduler).Select(_ => default(TDependent)))
                            .Take(1);

            // Call the return function on the source and dependent types
            return source.SelectMany(s => merged.Select(d => returnFunction(s, d)));
        }


        /// <summary>
        /// Waits for dependentObservable to receive a new message or for timeout time to pass, then returns.
        /// </summary>
        /// <param name="source">IObservable<TSource> for which ContinueAfter will be subscribed.</TSource></param>
        /// <param name="dependentObservable">The IObservable for which messages will be waited.</param>
        /// <param name="timeout">If no dependentObservable messages are received within this timeout, ContinueAfter will return.</param>
        /// <param name="scheduler">IScheduler to use.</param>
        /// <param name="returnFunction">Call with the source and dependent objects as parameters.  If a timeout occurs, default(TDependent) 
        /// is returned as the dependent object.</param>
        public static IObservable<TReturn> ContinueAfter<TSource, TDependent, TReturn>(this IObservable<TSource> source, 
            Func<TSource, IObservable<TDependent>> dependentObservable,
            TimeSpan timeout, IScheduler scheduler,
            Func<TSource, TDependent, TReturn> returnFunction)
        {
            // Call the return function on the source and dependent types
            return source.SelectMany(s => Observable.Merge(
                                dependentObservable(s).Select(x => x),
                                Observable.Timer(timeout, scheduler).Select(_ => default(TDependent)))
                            .Take(1).Select(d => returnFunction(s, d)));
        }

        /// <summary>
        /// Gate an input stream using another stream as a signal to open the gate
        /// </summary>
        /// <typeparam name="T">Type of source observable</typeparam>
        /// <typeparam name="K">Type of openGate observable</typeparam>
        /// <param name="source">Observable that will be gated</param>
        /// <param name="openGate">Trigger that will open the gate each time an event is received</param>
        /// <returns></returns>
        /// <remarks>This method will let the first item through and then close the gate, buffering all subsequent
        /// input items. When a signal is received from the openGate stream, the gate is opened. If there are any
        /// buffered items, they are sent as a collection and the gate is closed again.</remarks>
        public static IObservable<IEnumerable<T>> Gate<T, K>(this IObservable<T> source, IObservable<K> openGate) =>
            Gate(source, openGate, DefaultScheduler.Instance);

        /// <summary>
        /// Gate an input stream using another stream as a signal to open the gate
        /// </summary>
        /// <typeparam name="T">Type of source observable</typeparam>
        /// <typeparam name="K">Type of openGate observable</typeparam>
        /// <param name="source">Observable that will be gated</param>
        /// <param name="openGate">Trigger that will open the gate each time an event is received</param>
        /// <param name="scheduler">Defines the scheduler that should be used when gating</param>
        /// <returns></returns>
        /// <remarks>This method will let the first item through and then close the gate, buffering all subsequent
        /// input items. When a signal is received from the openGate stream, the gate is opened. If there are any
        /// buffered items, they are sent as a collection and the gate is closed again.</remarks>
        public static IObservable<IEnumerable<T>> Gate<T, K>(this IObservable<T> source, IObservable<K> openGate, IScheduler scheduler)
        {
            GateState<T> currentState = GateState<T>.Create();

            return Observable.Create<IEnumerable<T>>(o =>
            {
                var gateSubscription = Observable.Merge(new[]
                {
                    openGate.Select(_ => Tuple.Create(GateAction.Open, default(T))),
                    source.Select(x => Tuple.Create(GateAction.Close, x))
                })
                .ObserveOn(scheduler)
                .Subscribe(x =>
                {
                    currentState = x.Item1 == GateAction.Open ?
                        OpenGate(o.OnNext, currentState) :
                        CloseGate(o.OnNext, currentState, x.Item2);
                },
                o.OnError,
                o.OnCompleted);

                return Disposable.Create(() =>
                {
                    gateSubscription.Dispose();
                    o = null;
                    currentState = GateState<T>.Create();
                });
            });
        }

        /// <summary>
        /// Action to perform on a gate
        /// </summary>
        private enum GateAction
        {
            Open,
            Close
        }

        /// <summary>
        /// Open a gate
        /// </summary>
        private static GateState<T> OpenGate<T>(Action<IEnumerable<T>> sendItems, GateState<T> state)
        {
            if (state.Buffer.Any())
            {
                sendItems(state.Buffer);
                return state.ClearBuffer().CloseGate();
            }

            return state.OpenGate();
        }

        /// <summary>
        /// Close a gate
        /// </summary>
        private static GateState<T> CloseGate<T>(Action<IEnumerable<T>> sendItems, GateState<T> state, T item)
        {
            if (state.IsGateOpen)
            {
                sendItems(new[] { item });
                return state.CloseGate();
            }

            return state.BufferItem(item);
        }

        /// <summary>
        /// State used by the Gate operator
        /// </summary>
        private struct GateState<T>
        {
            /// <summary>
            /// Create a new default state
            /// </summary>
            public static GateState<T> Create() =>
                new GateState<T>(Enumerable.Empty<T>(), true);

            /// <summary>
            /// Constructor
            /// </summary>
            private GateState(IEnumerable<T> buffer, bool isGateOpen)
            {
                Buffer = buffer;
                IsGateOpen = isGateOpen;
            }

            /// <summary>
            /// Buffered items
            /// </summary>
            public IEnumerable<T> Buffer { get; }

            /// <summary>
            /// Is the gate currently open
            /// </summary>
            public bool IsGateOpen { get; }

            /// <summary>
            /// Open the gate
            /// </summary>
            public GateState<T> OpenGate() => new GateState<T>(Buffer, true);

            /// <summary>
            /// Close the gate
            /// </summary>
            public GateState<T> CloseGate() => new GateState<T>(Buffer, false);

            /// <summary>
            /// Buffer the specified item
            /// </summary>
            public GateState<T> BufferItem(T item) =>
                new GateState<T>(Buffer.Concat(new[] { item }), IsGateOpen);

            /// <summary>
            /// Clear the buffer
            /// </summary>
            public GateState<T> ClearBuffer() =>
                new GateState<T>(Enumerable.Empty<T>(), IsGateOpen);
        }
    }
}
