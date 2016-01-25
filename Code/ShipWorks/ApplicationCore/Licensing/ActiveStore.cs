namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// An active store retrieved from Tango
    /// </summary>
    public class ActiveStore
    {
        /// <summary>
        /// The store license key.
        /// </summary>
        public string StoreLicenseKey { get; set; }

        /// <summary>
        /// The store name
        /// </summary>
        public string Name { get; set; }
    }
}
