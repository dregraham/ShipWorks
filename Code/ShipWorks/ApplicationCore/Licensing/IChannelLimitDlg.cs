namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for ChannelLimitDlg
    /// </summary>
    public interface IChannelLimitDlg
    {
        /// <summary>
        /// The dialogs data context
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// Dialog result
        /// </summary>
        /// <returns></returns>
        bool? DialogResult { get; set; }
    }
}