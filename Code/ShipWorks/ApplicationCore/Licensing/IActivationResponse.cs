namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IActivationResponse
    {
        /// <summary>
        /// The associated stamps username. If empty, do not create new Stamps account in ShipWorks
        /// </summary>
        string AssociatedStampsUserName { get; }

        /// <summary>
        /// The customer Key
        /// </summary>
        string Key { get; }
    }
}