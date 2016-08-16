using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Collection that can be used across multiple threads, including access to the enumerator.
    /// </summary>
    public class ThreadSafeCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        List<T> items;
        ReaderWriterLockSlim readWriteLock;

        ThreadSafeCollectionBehavior behavior;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadSafeCollection(ThreadSafeCollectionBehavior behavior)
        {
            this.items = new List<T>();
            this.readWriteLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            this.behavior = behavior;
        }

        /// <summary>
        /// Get the count of the items in the collection
        /// </summary>
        public int Count
        {
            get
            {
                using (var readLock = new ReaderWriterReadLock(readWriteLock))
                {
                    return items.Count;
                }
            }
        }

        /// <summary>
        /// Get or set the item at the given index
        /// </summary>
        public T this[int index]
        {
            get
            {
                using (var readLock = new ReaderWriterReadLock(readWriteLock))
                {
                    return items[index];
                }
            }
            set
            {
                using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
                {
                    if ((index < 0) || (index >= this.items.Count))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    SetItem(index, value);
                }
            }
        }

        /// <summary>
        /// Add a new item to the list
        /// </summary>
        public void Add(T item)
        {
            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                int count = items.Count;
                InsertItem(count, item);
            }
        }

        /// <summary>
        /// Clear all the items from the collection
        /// </summary>
        public void Clear()
        {
            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                ClearItems();
            }
        }

        /// <summary>
        /// Returns true of the specified item is contained in the collection
        /// </summary>
        public bool Contains(T item)
        {
            using (var readLock = new ReaderWriterReadLock(readWriteLock))
            {
                return items.Contains(item);
            }
        }

        /// <summary>
        /// Copy the items in the collection to the specified array starting at the given index.
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            using (var readLock = new ReaderWriterReadLock(readWriteLock))
            {
                items.CopyTo(array, index);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the items in the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (behavior == ThreadSafeCollectionBehavior.ForEachLocked)
            {
                // Lock around the entire iteration
                using (var readLock = new ReaderWriterReadLock(readWriteLock))
                {
                    foreach (T item in items)
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                List<T> copy;

                // Lock and copy, then release the lock
                using (var readLock = new ReaderWriterReadLock(readWriteLock))
                {
                    copy = items.ToList();
                }

                foreach (T item in copy)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Get the index of the first matching item in the collection, or -1 if not found
        /// </summary>
        public int IndexOf(T item)
        {
            using (var readLock = new ReaderWriterReadLock(readWriteLock))
            {
                return items.IndexOf(item);
            }
        }

        /// <summary>
        /// Insert a new item into the collection at the specified index
        /// </summary>
        public void Insert(int index, T item)
        {
            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                if ((index < 0) || (index > items.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                InsertItem(index, item);
            }
        }

        /// <summary>
        /// Remove the first instance of the item from the collection.  Returns false if the item was not found.
        /// </summary>
        public bool Remove(T item)
        {
            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                int index = items.IndexOf(item);
                if (index < 0)
                {
                    return false;
                }

                RemoveItem(index);
                return true;
            }
        }

        /// <summary>
        /// Remove the item from the collection that exists at the given index
        /// </summary>
        public void RemoveAt(int index)
        {
            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                if ((index < 0) || (index >= items.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                RemoveItem(index);
            }
        }

        /// <summary>
        /// Clear all the items in the collection. (unlocked)
        /// </summary>
        protected virtual void ClearItems()
        {
            items.Clear();
        }

        /// <summary>
        /// Insert the given item into the collection at the specified index. (unlocked)
        /// </summary>
        protected virtual void InsertItem(int index, T item)
        {
            items.Insert(index, item);
        }

        /// <summary>
        /// Remove the item from the collection at the specified index. (unlocked)
        /// </summary>
        protected virtual void RemoveItem(int index)
        {
            items.RemoveAt(index);
        }

        /// <summary>
        /// Update the item that exists at the given index. (unlocked)
        /// </summary>
        protected virtual void SetItem(int index, T item)
        {
            items[index] = item;
        }

        /// <summary>
        /// ICollection.Copy to explicit implementation
        /// </summary>
        void ICollection.CopyTo(Array array, int index)
        {
            using (var readLock = new ReaderWriterReadLock(readWriteLock))
            {
                ((ICollection) items).CopyTo(array, index);
            }
        }

        /// <summary>
        /// ICollection.IsReadOnly explicit implementation
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// ICollection.IsSynchronized explicit implementation
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// ICollection.SyncRoot
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection) items).SyncRoot; }
        }

        /// <summary>
        /// IEnumeratable.GetEnumerator explicit implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// IList.Add explicit implementation
        /// </summary>
        int IList.Add(object value)
        {
            VerifyValueType(value);

            using (var writeLock = new ReaderWriterWriteLock(readWriteLock))
            {
                this.Add((T) value);
                return (Count - 1);
            }
        }

        /// <summary>
        /// IList.Contains explicit implementation
        /// </summary>
        bool IList.Contains(object value)
        {
            if (!IsCompatibleObject(value))
            {
                return false;
            }

            return this.Contains((T) value);
        }

        /// <summary>
        /// IList.IndexOf explicit implementation
        /// </summary>
        int IList.IndexOf(object value)
        {
            if (!IsCompatibleObject(value))
            {
                return -1;
            }

            return this.IndexOf((T) value);
        }

        /// <summary>
        /// IList.Insert explicit implementation
        /// </summary>
        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);

            this.Insert(index, (T) value);
        }

        /// <summary>
        /// IList.Remove explicit implementation
        /// </summary>
        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                this.Remove((T) value);
            }
        }

        /// <summary>
        /// IList.IsFixedSize explicit implementation
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// IList.IsReadOnly explicit implementation
        /// </summary>
        bool IList.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// IList.indexor
        /// </summary>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value);

                this[index] = (T) value;
            }
        }

        /// <summary>
        /// Throws an exception if the given type should not be in this collection
        /// </summary>
        private static void VerifyValueType(object value)
        {
            if (!IsCompatibleObject(value))
            {
                throw new ArgumentException("The value is of the wrong type for the collection.");
            }
        }

        /// <summary>
        /// Indicates if the given object is allowed to be in this collection
        /// </summary>
        private static bool IsCompatibleObject(object value)
        {
            if (!(value is T) && ((value != null) || typeof(T).IsValueType))
            {
                return false;
            }

            return true;
        }

    }
}
