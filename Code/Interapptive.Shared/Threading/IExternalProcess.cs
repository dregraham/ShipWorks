using System;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// An interface intended to facilitate interaction with an external process
    /// </summary>
    public interface IExternalProcess
    {
        /// <summary>
        /// Launch the external process
        /// </summary>
        /// <param name="callbackAction">the action to invoice when the panel exits</param>
        /// <remarks>
        /// Invokes the callbackAction when the external process exits
        /// </remarks>
        void Launch(Action callbackAction);
    }
}
