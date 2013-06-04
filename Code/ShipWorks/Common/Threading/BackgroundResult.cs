using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// The result of a background opreation
    /// </summary>
    public class BackgroundResult<T>
    {
        BackgroundResultStatus status;
        List<BackgroundIssue<T>> issues;

        /// <summary>
        /// Success constructor
        /// </summary>
        public BackgroundResult()
        {
            status = BackgroundResultStatus.Success;
            issues = new List<BackgroundIssue<T>>();
        }

        /// <summary>
        /// Non-success constructor
        /// </summary>
        public BackgroundResult(BackgroundResultStatus status, List<BackgroundIssue<T>> issues)
        {
            this.status = status;
            this.issues = issues;
        }

        /// <summary>
        /// Overall status of the background operation
        /// </summary>
        public BackgroundResultStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Specific items that had issues and there associated issue data
        /// </summary>
        public List<BackgroundIssue<T>> Issues
        {
            get { return issues; }
        }
    }
}
