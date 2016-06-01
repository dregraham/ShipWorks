using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Interapptive.Shared.Threading;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Generic collection with events raised when contents change.  The colection is thread-safe, and the CollectionChanged event
    /// is raised while the lock is still being held.
    /// </summary>
    public class ObservableCollection<T> : ThreadSafeCollection<T> where T: class
    {
        CollectionChangedEventHandler<T> collectionChanged;
        object eventLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public ObservableCollection()
            : base(ThreadSafeCollectionBehavior.ForEachLocked)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ObservableCollection(ThreadSafeCollectionBehavior lockBehavior)
            : base(lockBehavior)
        {

        }

        /// <summary>
        /// Raised when the collection is changed.  The thread-safe lock is held while the event is raised so be 
        /// careful not to do anything too time consuming if using multiple threads.
        /// </summary>
        public event CollectionChangedEventHandler<T> CollectionChanged
        {
            add
            {
                lock (eventLock)
                {
                    collectionChanged = (CollectionChangedEventHandler<T>) Delegate.Combine(collectionChanged, value);
                }
            }
            remove
            {
                lock (eventLock)
                {
                    collectionChanged = (CollectionChangedEventHandler<T>) Delegate.Remove(collectionChanged, value);
                }
            }
        }

        /// <summary>
        /// Inserts an element into the collection at the specified index.
        /// </summary>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            RaiseCollectionChanged(item, null, index);
        }

        /// <summary>
        /// Removes the element at the specified index of the collection
        /// </summary>
        protected override void RemoveItem(int index)
        {
            T item = this[index];

            base.RemoveItem(index);

            RaiseCollectionChanged(null, item, index);
        }

        /// <summary>
        /// Replaces the element at the specified index
        /// </summary>
        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];

            base.SetItem(index, item);

            RaiseCollectionChanged(item, oldItem, index);
        }

        /// <summary>
        /// Raises the CollectionChanged event
        /// </summary>
        private void RaiseCollectionChanged(T newItem, T oldItem, int index)
        {
            if (collectionChanged != null)
            {
                collectionChanged(this, new CollectionChangedEventArgs<T>(newItem, oldItem, index));
            }
        }

        /// <summary>
        /// Removes all elements from the collection
        /// </summary>
        protected override void ClearItems()
        {
            while (Count > 0)
            {
                RemoveItem(0);
            }
        }

        /// <summary>
        /// Adds the items to the collection
        /// </summary>
        public void AddRange(IEnumerable<T> newItems)
        {
            foreach (T newItem in newItems)
            {
                Add(newItem);
            }
        }
    }
}
