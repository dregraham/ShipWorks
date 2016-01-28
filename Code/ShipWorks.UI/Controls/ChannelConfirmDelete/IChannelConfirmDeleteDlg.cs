namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Interface for Confirm channel delete dlg
    /// </summary>
    public interface IChannelConfirmDeleteDlg
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