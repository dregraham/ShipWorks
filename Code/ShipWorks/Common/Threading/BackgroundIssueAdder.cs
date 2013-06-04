using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Utility class that allows background issues to be added to a background issue list, without exposing the list
    /// </summary>
    public class BackgroundIssueAdder<T>
    {
        List<BackgroundIssue<T>> issues;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundIssueAdder(List<BackgroundIssue<T>> issues)
        {
            this.issues = issues;
        }

        /// <summary>
        /// Add a new issue that has no detail
        /// </summary>
        public void Add(T item)
        {
            Add(item, null);
        }

        /// <summary>
        /// Add a new issue with detail
        /// </summary>
        public void Add(T item, object detail)
        {
            issues.Add(new BackgroundIssue<T>(item, detail));
        }
    }
}
