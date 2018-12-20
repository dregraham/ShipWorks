using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Factory for progress related objects
    /// </summary>
    [Component]
    public class ProgressFactory : IProgressFactory
    {
        /// <summary>
        /// Create a progress reporter
        /// </summary>
        public IProgressReporter CreateReporter(string name) => new ProgressItem(name);
    }
}
