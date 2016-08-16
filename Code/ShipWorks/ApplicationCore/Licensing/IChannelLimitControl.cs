namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for the ChannelLimitControl
    /// </summary>
    public interface IChannelLimitControl
    {
        /// <summary>
        /// The dialogs data context
        /// </summary>
        object DataContext { get; set; }
    }
}