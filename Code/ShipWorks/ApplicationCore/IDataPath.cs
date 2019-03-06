namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Paths to directories ShipWorks uses.
    /// </summary>
    public interface IDataPath
    {
        /// <summary>
        /// Get the folder used to store settings for the given installed instance of ShipWorks.
        /// </summary>
        string InstanceSettings { get; }
    }
}