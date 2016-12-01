﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Implementation of the trackable switch statement
    /// </summary>
    public static class SwitchImpl
    {
        /// <summary>
        /// Implementation of the trackable switch statement
        /// </summary>
        public static IObservable<IMessageTracker<TSource>> Switch<TSource>(this IObservable<IMessageTracker<IObservable<IMessageTracker<TSource>>>> sources,
            object listener, [CallerMemberName] string callerName = "")
        {
            return sources.Select(x => x.Value).Switch();
        }
    }
}
