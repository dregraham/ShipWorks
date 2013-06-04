using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// A list of issues that have occurred in the background.  The type of object that represents extra data about the issue
    /// </summary>
    public class BackgroundIssue<T>
    {
        T item;
        object detail;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundIssue(T item)
        {
            this.item = item;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundIssue(T item, object detail)
        {
            this.item = item;
            this.detail = detail;
        }

        /// <summary>
        /// The item that had the issue
        /// </summary>
        public T Item
        {
            get { return item; }
        }

        /// <summary>
        /// Optional detail information associated with the issue
        /// </summary>
        public object Detail
        {
            get { return detail; }
        }
    }
}
