﻿// Derived from https://github.com/lvaleriu/Virtualization
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;

namespace DataVirtualization
{
    /// <summary>
    /// Represents a page of data in a virtual collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataPage<T> where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataPage(int firstIndex, int pageLength, IItemsProvider<T> itemsProvider)
        {
            var populateAction = CreatePopulateAction(firstIndex, pageLength, itemsProvider);

            Items = itemsProvider.GetIDsInRange(firstIndex, pageLength)
                .Select((entityID, i) => new DataWrapper<T>(firstIndex + i, entityID, populateAction))
                .OfType<IDataWrapper<T>>()
                .ToList();

            TouchTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Items in the page
        /// </summary>
        public IList<IDataWrapper<T>> Items { get; }

        /// <summary>
        /// Last time the page was touched
        /// </summary>
        public DateTime TouchTime { get; set; }

        /// <summary>
        /// Is the page in use?
        /// </summary>
        public bool IsInUse => Items.Any(wrapper => wrapper.IsInUse);

        /// <summary>
        /// Create the populate wrapper action
        /// </summary>
        private Action CreatePopulateAction(int firstIndex, int pageLength, IItemsProvider<T> itemsProvider)
        {
            Task loadingTask = null;

            return () =>
            {
                if (loadingTask == null)
                {
                    loadingTask = itemsProvider
                        .FetchRange(firstIndex, pageLength)
                        .ContinueWith(t =>
                        {
                            loadingTask = null;
                            Populate(t.Result);
                        });
                }
            };
        }

        /// <summary>
        /// Populate the page
        /// </summary>
        public void Populate(IEnumerable<T> newItems) =>
            newItems
                .Zip(Items, (x, y) => (Item: x, Wrapper: y))
                .ForEach(x => x.Wrapper.Data = x.Item);

        /// <summary>
        /// Clean up the page
        /// </summary>
        internal void CleanUp(DateTime expiration)
        {
            if (IsInUse || TouchTime > expiration)
            {
                return;
            }

            Items.ForEach(x => x.CleanUp());
        }
    }
}