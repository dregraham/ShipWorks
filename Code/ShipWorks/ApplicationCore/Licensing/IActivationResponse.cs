namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IActivationResponse
    {
        /// <summary>
        /// The customer Key
        /// </summary>
        string Key { get; }
        
        /// <summary>
        /// The customer Key for legacy users
        /// </summary>
        string LegacyKey { get;  }
        
        /// <summary>
        /// Whether or not this is a legacy account
        /// </summary>
        bool IsLegacyUser { get; }
        
        /// <summary>
        /// The associated stamps username. If empty, do not create new Stamps account in ShipWorks
        /// </summary>
        string AssociatedStampsUsername { get; }

        /// <summary>
        /// The stamps username to use when creating the first stamps account.
        /// </summary>
        string StampsUsername { get; set; }
    }
}