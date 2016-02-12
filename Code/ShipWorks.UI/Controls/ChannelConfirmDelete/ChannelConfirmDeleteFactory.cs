using ShipWorks.Stores;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Factory for the ChannelConfirmDelete Dialog
    /// </summary>
    public class ChannelConfirmDeleteFactory : IChannelConfirmDeleteFactory
    {
        private readonly Func<IChannelConfirmDeleteDlg> dialogFactory;
        private readonly Func<IConfirmChannelDeleteViewModel> viewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelConfirmDeleteFactory(Func<IChannelConfirmDeleteDlg> dialogFactory, Func<IConfirmChannelDeleteViewModel> viewModelFactory)
        {
            this.dialogFactory = dialogFactory;
            this.viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Creates a ChannelConfirmationDeleteDlg using the given store type
        /// </summary>
        public IChannelConfirmDeleteDlg GetConfirmDeleteDlg(StoreTypeCode storeType, Window owner)
        {
            // Get a new Dialog
            IChannelConfirmDeleteDlg dlg = dialogFactory();

            // Get a new View Model
            IConfirmChannelDeleteViewModel viewModel = viewModelFactory();

            // Load the store type into the view model
            viewModel.Load(storeType);

            // set the data context of the dialog to the view model
            dlg.DataContext = viewModel;

            // When deleting a channel from the AddStoreWizard, the owner is null, since the
            // AddStoreWizard is not WPF. So just throw it front and center screen and call it a day.
            if (owner == null)
            {
                dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dlg.Topmost = true;
            }
            else
            {
                // Set owner
                dlg.Owner = owner;
            }

            // return the dialog
            return dlg;
        }
    }
}
