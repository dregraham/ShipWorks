using System;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Dialog with a single progress item
    /// </summary>
    public interface ISingleItemProgressDialog : IDisposable
    {
        /// <summary>
        /// Progress item associated with the dialog
        /// </summary>
        IProgressReporter ProgressItem { get; }
    }
}
