namespace ShipWorks.Data
{
    /// <summary>
    /// Helps track usage of various objects in the system.  Such as filters, templates, and resources.  There is also a version
    /// of this class that lives inside of the SqlServer assembly that does the real work, and can be used within triggers
    /// </summary>
    public class ObjectReferenceManagerWrapper : IObjectReferenceManager
    {
        /// <summary>
        /// Clear all references used by the given consumerID
        /// </summary>
        public void ClearReferences(long consumerID) =>
            ObjectReferenceManager.ClearReferences(consumerID);
    }
}
