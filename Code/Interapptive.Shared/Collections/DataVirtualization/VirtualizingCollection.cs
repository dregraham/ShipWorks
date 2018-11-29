// Derived from https://github.com/lvaleriu/Virtualization
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Collections;

namespace DataVirtualization
{
    /// <summary>
    /// Specialized list implementation that provides data virtualization. The collection is divided up into pages,
    /// and pages are dynamically fetched from the IItemsProvider when required. Stale pages are removed after a
    /// configurable period of time.
    /// Intended for use with large collections on a network or disk resource that cannot be instantiated locally
    /// due to memory consumption or fetch latency.
    /// </summary>
    /// <remarks>
    /// The IList implementation is not fully complete, but should be sufficient for use as read only collection
    /// data bound to a suitable ItemsControl.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class VirtualizingCollection<T> : IVirtualizingCollection<T> where T : class
    {
        private readonly int pageSize = 100;
        private Lazy<int> count;
        private DataPage<T>[] pages;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageTimeout">The page timeout.</param>
        public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, TimeSpan pageTimeout) : this(itemsProvider, pageSize)
        {
            PageTimeout = pageTimeout;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        /// <param name="pageSize">Size of the page.</param>
        public VirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize) : this(itemsProvider)
        {
            this.pageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualizingCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="itemsProvider">The items provider.</param>
        public VirtualizingCollection(IItemsProvider<T> itemsProvider)
        {
            ItemsProvider = itemsProvider;
            count = new Lazy<int>(() => ItemsProvider.FetchCount());
        }

        /// <summary>
        /// Gets the items provider.
        /// </summary>
        /// <value>The items provider.</value>
        public IItemsProvider<T> ItemsProvider { get; }

        /// <summary>
        /// Gets the page timeout.
        /// </summary>
        /// <value>The page timeout.</value>
        public TimeSpan PageTimeout { get; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// The first time this property is accessed, it will fetch the count from the IItemsProvider.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count => count.Value;

        /// <summary>
        /// Gets the item at the specified index. This property will fetch
        /// the corresponding page from the IItemsProvider if required.
        /// </summary>
        /// <value></value>
        public DataWrapper<T> this[int index]
        {
            get
            {
                if (pages == null)
                {
                    pages = new DataPage<T>[(Count / pageSize) + 1];
                }

                // determine which page and offset within page
                int pageIndex = index / pageSize;
                int pageOffset = index % pageSize;

                // request primary page
                RequestPage(pageIndex);

                // if accessing upper 50% then request next page
                if (pageOffset > pageSize / 2 && pageIndex < Count / pageSize)
                {
                    RequestPage(pageIndex + 1);
                }

                // if accessing lower 50% then request previous page
                if (pageOffset < pageSize / 2 && pageIndex > 0)
                {
                    RequestPage(pageIndex - 1);
                }

                // return requested item
                return pages[pageIndex].Items[pageOffset];
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <remarks>
        /// This method should be avoided on large collections due to poor performance.
        /// </remarks>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DataWrapper<T>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add(DataWrapper<T> item) => throw new NotSupportedException();

        /// <summary>
        /// Add an item
        /// </summary>
        int IList.Add(object value) => throw new NotSupportedException();

        /// <summary>
        /// Add an item
        /// </summary>
        bool IList.Contains(object value) => Contains((DataWrapper<T>) value);

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// Always false.
        /// </returns>
        public bool Contains(DataWrapper<T> item) =>
            pages?.Where(x => x.Items != null).Any(x => x.Items.Contains(item)) == true;

        /// <summary>
        /// TODO
        /// </summary>
        public void Clear() => throw new NotSupportedException();

        /// <summary>
        /// Get the index of the specified value
        /// </summary>
        int IList.IndexOf(object value) => IndexOf((DataWrapper<T>) value);

        /// <summary>
        /// Get the index of the given item
        /// </summary>
        public int IndexOf(DataWrapper<T> item) =>
            (pages ?? Enumerable.Empty<DataPage<T>>())
                .Where(x => x.Items != null)
                .Select((x, i) => (IndexWithinPage: x.Items.IndexOf(item), Page: i))
                .Where(x => x.IndexWithinPage != -1)
                .Select(x => pageSize * x.Page + x.IndexWithinPage)
                .DefaultIfEmpty(-1)
                .First();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void Insert(int index, DataWrapper<T> item) => throw new NotSupportedException();

        /// <summary>
        /// Insert an item in the list
        /// </summary>
        void IList.Insert(int index, object value) => Insert(index, (DataWrapper<T>) value);

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.
        /// </exception>
        public void RemoveAt(int index) => throw new NotSupportedException();

        /// <summary>
        /// Remove the given item
        /// </summary>
        void IList.Remove(object value) => throw new NotSupportedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public bool Remove(DataWrapper<T> item) => throw new NotSupportedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.
        /// -or-
        /// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        /// -or-
        /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        /// -or-
        /// Type <paramref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(DataWrapper<T>[] array, int arrayIndex) => throw new NotSupportedException();

        /// <summary>
        /// Copy to the given array
        /// </summary>
        void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException();

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public object SyncRoot => this;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>Always false.
        /// </returns>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>Always true.
        /// </returns>
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>Always false.
        /// </returns>
        public bool IsFixedSize => false;

        /// <summary>
        /// Cleans up any stale pages that have not been accessed in the period dictated by PageTimeout.
        /// </summary>
        public void CleanUpPages()
        {
            if (pages == null)
            {
                return;
            }

            var expirationDate = DateTime.UtcNow.Subtract(PageTimeout);

            pages.WhereNotNull().ForEach(x => x.CleanUp(expirationDate));
        }

        /// <summary>
        /// Makes a request for the specified page, creating the necessary slots in the dictionary,
        /// and updating the page touch time.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        protected virtual void RequestPage(int pageIndex)
        {
            if (pages == null)
            {
                return;
            }

            if (pages[pageIndex] == null)
            {
                // Create a page of empty data wrappers.
                int pageLength = Math.Min(pageSize, Count - pageIndex * pageSize);

                var page = new DataPage<T>(pageIndex * pageSize, pageLength, ItemsProvider);
                pages[pageIndex] = page;

                Trace.WriteLine("Added page: " + pageIndex);
            }
            else
            {
                pages[pageIndex].TouchTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Removes all cached pages. This is useful when the count of the
        /// underlying collection changes.
        /// </summary>
        protected void EmptyCache()
        {
            if (pages != null)
            {
                for (int i = 0; i < pages.Length; i++)
                {
                    pages[i] = null;
                }
            }
        }
    }
}
