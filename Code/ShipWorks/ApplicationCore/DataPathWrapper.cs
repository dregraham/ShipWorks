using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Paths to directories ShipWorks uses.
    /// </summary>
    [Component]
    public class DataPathWrapper : IDataPath
    {
        /// <summary>
        /// Get the folder used to store settings for the given installed instance of ShipWorks.
        /// </summary>
        public string InstanceSettings => DataPath.InstanceSettings;
    }
}
