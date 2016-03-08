namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Response from TangoWebClient.AddStore 
    /// </summary>
    public interface IAddStoreResponse
    {
        /// <summary>
        /// Error Message
        /// </summary>
        LicenseActivationState Result { get; }

        /// <summary>
        /// License Key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Success!
        /// </summary>
        bool Success { get; }
    }
}