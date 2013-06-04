using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Diagnostics;
using System.Threading;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Implements a cache that dumps items that are the least recently used when it gets full.
    /// </summary>
    public class LruCache<TKey, TValue> where TValue: class
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(LruCache<TKey, TValue>));

        int maxItems;
        TimeSpan maxLifetime;

        class CacheEntry
        {
            public TValue Value { get; set; }
            public DateTime Created { get; set; }
            public DateTime Accessed { get; set; }
        }

        /// <summary>
        /// Raised when an item is removed from the cache
        /// </summary>
        public event LruCacheItemRemovedEventHandler<TKey, TValue> CacheItemRemoved;

        // Maps each entity to its node in the LinkedList
        Dictionary<TKey, LinkedListNode<CacheEntry>> listLocations = new Dictionary<TKey, LinkedListNode<CacheEntry>>();

        // The list of entities, sorted from lru -> mru
        LinkedList<CacheEntry> lruList = new LinkedList<CacheEntry>();

        // Thread safety
        object syncLock = new object();

        long totalMsSpent = 0;

        /// <summary>
        /// Constructor.  maxItems is how many items the cache can hold before it starts pushing out old ones.
        /// </summary>
        public LruCache(int maxItems)
            : this(maxItems, TimeSpan.Zero)
        {
            
        }

        /// <summary>
        /// Constructor.  maxItems is how many items the cache can hold before it starts pushing out old ones.  maxLifetime is how
        /// long a cache item is considered valid.  It is not actively pushed out when its lifetime is up, but if its asked for after
        /// its lifetime is over, then it will be pushed out.  Getting an item does not increase reset the start of its lifetime.
        /// </summary>
        public LruCache(int maxItems, TimeSpan maxLifetime)
        {
            this.maxItems = maxItems;
            this.maxLifetime = maxLifetime;
        }

        /// <summary>
        /// SyncLock so derived classes can have proper locking when overriding methods.
        /// </summary>
        protected object SyncLock
        {
            get { return syncLock; }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public virtual TValue this[TKey key]
        {
            get
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                lock (syncLock)
                {
                    try
                    {
                        LinkedListNode<CacheEntry> node;
                        if (!listLocations.TryGetValue(key, out node))
                        {
                            return null;
                        }
                        else
                        {
                            // See if its expired
                            if (maxLifetime > TimeSpan.Zero && node.Value.Created + maxLifetime < DateTime.UtcNow)
                            {
                                Remove(key, node);
                                return null;
                            }
                            else
                            {
                                // Move it to the end
                                lruList.Remove(node);
                                lruList.AddLast(node);

                                node.Value.Accessed = DateTime.UtcNow;

                                return node.Value.Value;
                            }
                        }
                    }
                    finally
                    {
                        Interlocked.Add(ref totalMsSpent, stopwatch.ElapsedMilliseconds);
                    }
                }
            }
            set
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                lock (syncLock)
                {
                    LinkedListNode<CacheEntry> node;

                    // If a node doesnt exist, add one to the end
                    if (!listLocations.TryGetValue(key, out node))
                    {
                        if (value != null)
                        {
                            // If we have too many items, remove any that have expired
                            if (lruList.Count >= maxItems)
                            {
                                RemoveExpiredEntries();
                            }

                            // If we still have too many items, remove the one accessed the longest ago (which should be the first in the list)
                            if (lruList.Count >= maxItems)
                            {
                                LinkedListNode<CacheEntry> removeNode = lruList.First;
                                TKey removeKey = listLocations.Where(p => p.Value == removeNode).Select(p => p.Key).Single();

                                Remove(removeKey, removeNode);
                            }

                            node = lruList.AddLast(new CacheEntry { Value = value, Created = DateTime.UtcNow, Accessed = DateTime.UtcNow });
                            listLocations[key] = node;
                        }
                    }
                    else
                    {
                        // If setting to null, remove it
                        if (value == null)
                        {
                            Remove(key, node);
                        }
                        else
                        {
                            // Update the value
                            node.Value.Value = value;

                            // Updating the value counts as recreating it
                            node.Value.Created = DateTime.UtcNow;
                            node.Value.Accessed = DateTime.UtcNow;

                            // Move it to the end of the lru
                            lruList.Remove(node);
                            lruList.AddLast(node);
                        }
                    }
                }

                Interlocked.Add(ref totalMsSpent, stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Return the list of keys currently found in the cache
        /// </summary>
        public List<TKey> Keys
        {
            get
            {
                lock (syncLock)
                {
                    return new List<TKey>(listLocations.Keys);
                }
            }
        }

        /// <summary>
        /// Determines if the cache contains the entity with the given id
        /// </summary>
        public bool Contains(TKey key)
        {
            lock (syncLock)
            {
                return listLocations.ContainsKey(key);
            }
        }

        /// <summary>
        /// Removes the specified item from the cache.  If the item is not in the cache no action is taken.
        /// </summary>
        public void Remove(TKey key)
        {
            lock (syncLock)
            {
                LinkedListNode<CacheEntry> node;
                if (listLocations.TryGetValue(key, out node))
                {
                    Remove(key, node);
                }
            }
        }

        /// <summary>
        /// Remove all entries in the cache
        /// </summary>
        public void Clear()
        {
            lock (syncLock)
            {
                // Have to do each one individually so the event gets raised
                foreach (KeyValuePair<TKey, LinkedListNode<CacheEntry>> pair in listLocations.ToList())
                {
                    Remove(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Remove all entries that have been around longer the allowable lifetime
        /// </summary>
        private void RemoveExpiredEntries()
        {
            if (maxLifetime <= TimeSpan.Zero)
            {
                return;
            }

            lock (syncLock)
            {
                // Go through each entry that has expired
                foreach (KeyValuePair<TKey, LinkedListNode<CacheEntry>> pair in listLocations.Where(p => p.Value.Value.Created + maxLifetime < DateTime.UtcNow).ToList())
                {
                    Remove(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Remove the specified node form the list
        /// </summary>
        private void Remove(TKey key, LinkedListNode<CacheEntry> node)
        {
            listLocations.Remove(key);
            lruList.Remove(node);

            LruCacheItemRemovedEventHandler<TKey, TValue> handler = CacheItemRemoved;
            if (handler != null)
            {
                handler(this, new LruCacheItemRemovedEventArgs<TKey,TValue>(key, node.Value.Value));
            }
        }
    }
}
