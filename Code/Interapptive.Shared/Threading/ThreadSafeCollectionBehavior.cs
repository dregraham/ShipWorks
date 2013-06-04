using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// How the thread safe collection should behave
    /// </summary>
    public enum ThreadSafeCollectionBehavior
    {
        /// <summary>
        /// When using foreach (aka while enumerating) the entire collection will be locked for the entire
        /// time the enumerator has not been run to completion.
        /// </summary>
        ForEachLocked,

        /// <summary>
        /// When foreach beings (aka when the enumerator is requested) the collection is copied into a new list,
        /// and it is that new list that is enumerated.
        /// </summary>
        ForEachCopy
    }
}
