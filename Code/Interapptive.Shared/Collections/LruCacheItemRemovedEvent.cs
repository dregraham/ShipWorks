using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Delegate for the cache item removed event
    /// </summary>
    public delegate void LruCacheItemRemovedEventHandler<TKey, TValue>(object sender, LruCacheItemRemovedEventArgs<TKey, TValue> e);

    /// <summary>
    /// Event args for the CacheItemRemoved event
    /// </summary>
    public class LruCacheItemRemovedEventArgs<TKey, TValue> : EventArgs
    {
        TKey key;
        TValue item;

        /// <summary>
        /// Constructor
        /// </summary>
        public LruCacheItemRemovedEventArgs(TKey key, TValue item)
        {
            this.key = key;
            this.item = item;
        }

        /// <summary>
        /// The key that was used for the cached item
        /// </summary>
        public TKey Key
        {
            get { return key; }
        }

        /// <summary>
        /// The item that was removed
        /// </summary>
        public TValue Item
        {
            get { return item; }
        }
    }
}
