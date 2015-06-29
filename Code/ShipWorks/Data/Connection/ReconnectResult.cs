namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Result of a reconnect attempt
    /// </summary>
    public enum ReconnectResult
    {
        /// <summary>
        /// Reconnect succeeded
        /// </summary>
        Succeeded,

        /// <summary>
        /// Reconnect failed
        /// </summary>
        Failed,

        /// <summary>
        /// User canceled the reconnect attempt
        /// </summary>
        Canceled
    }
}