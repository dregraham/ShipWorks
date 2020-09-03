using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Distribution
{
    /// <summary>
    /// Wrapper class for BuiltinTemplates
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 1)]
    public class BuiltinTemplatesWrapper : IInitializeForCurrentSession
    {
        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            BuiltinTemplates.UpdateTemplates(null);
        }
    }
}
