namespace ShipWorks.Common
{
    /// <summary>
    /// Wraps static calls of System.Diagnostics.Process
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Starts a process resource by specifying the name of a document or application file and 
        /// associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.
        /// </summary>
        void Start(string fileName);
    }
}
