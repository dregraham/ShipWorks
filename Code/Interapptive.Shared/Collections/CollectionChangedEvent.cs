using System;

namespace Interapptive.Shared.Collections
{
    public delegate void CollectionChangedEventHandler<T>(object sender, CollectionChangedEventArgs<T> e) where T : class;

    /// <summary>
    /// EventArgs for the CollectionChanged event
    /// </summary>
    public class CollectionChangedEventArgs<T> : EventArgs where T : class
    {
        /// <summary>
        /// Consructor
        /// </summary>
        public CollectionChangedEventArgs(T newItem, T oldItem, int index)
        {
            if (newItem == null && oldItem == null)
            {
                throw new ArgumentException("Both newItem and oldItem cannot be null.");
            }

            NewItem = newItem;
            OldItem = oldItem;
            Index = index;
        }

        /// <summary>
        /// The newly added item in the collection
        /// </summary>
        public T NewItem { get; }

        /// <summary>
        /// The item that was removed or replaced in the collection
        /// </summary>
        public T OldItem { get; }

        /// <summary>
        /// The index of the addition, removal, or insertion.
        /// </summary>
        public int Index { get; }
    }
}
