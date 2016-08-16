using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    public static class DoImpl
    {
        //
        // Summary:
        //     Invokes an action for each element in the observable sequence, and propagates
        //     all observer messages through the result sequence. This method can be used for
        //     debugging, logging, etc. of query behavior by intercepting the message stream
        //     to run arbitrary actions for messages on the pipeline.
        //
        // Parameters:
        //   source:
        //     Source sequence.
        //
        //   onNext:
        //     Action to invoke for each element in the observable sequence.
        //
        // Type parameters:
        //   TSource:
        //     The type of the elements in the source sequence.
        //
        // Returns:
        //     The source sequence with the side-effecting behavior applied.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     source or onNext is null.
        public static IObservable<IMessageTracker<TSource>> Do<TSource>(this IObservable<IMessageTracker<TSource>> source,
            object listener, Action<TSource> onNext, [CallerMemberName] string callerName = "")
        {
            return source.Do(x => x.Do(onNext, listener, callerName));
        }
    }
}
