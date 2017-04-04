using System.Diagnostics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common
{
    /// <summary>
    /// Wraps static calls of System.Diagnostics.Process
    /// </summary>
    [Component]
    public class ProcessWrapper : IProcess
    {
        /// <summary>
        /// Starts a process resource by specifying the name of a document or application file
        /// </summary>
        public void Start(string fileName) => Process.Start(fileName);
    }
}