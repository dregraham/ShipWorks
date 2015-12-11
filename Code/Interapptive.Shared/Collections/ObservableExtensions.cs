using System;
using System.Reactive.Disposables;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extensions on the Observable type
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Subscribe to an observable collection
        /// </summary>
        /// <remarks>This will attempt to resubscribe on an error</remarks>
        public static IDisposable SubscribeWithRetry<T>(this IObservable<T> source, Action<T> onNext) =>
            source.SubscribeWithRetry(onNext, x => true);

        /// <summary>
        /// Subscribe to an observable collection
        /// </summary>
        /// <remarks>This will attempt to resubscribe on an error, if the onError function returns true</remarks>
        public static IDisposable SubscribeWithRetry<T>(this IObservable<T> source, Action<T> onNext, Func<Exception, bool> onError) =>
            CreateSubscription(source, onNext, onError);

        /// <summary>
        /// Create the subscription
        /// </summary>
        private static IDisposable CreateSubscription<T>(IObservable<T> source, Action<T> onNext, Func<Exception, bool> onError)
        {
            IDisposable disposable = null;

            disposable = source.Subscribe(onNext, x =>
            {
                disposable?.Dispose();
                if (onError(x))
                {
                    CreateSubscription(source, onNext, onError);
                }
            });

            return Disposable.Create(() => disposable?.Dispose());
        }
    }
}
