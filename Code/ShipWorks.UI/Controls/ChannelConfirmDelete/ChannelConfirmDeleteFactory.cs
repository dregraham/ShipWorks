using ShipWorks.Stores;
using System;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Factory for the ChannelConfirmDelete Dialog
    /// </summary>
    class ChannelConfirmDeleteFactory : IChannelConfirmDeleteFactory
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
        /// <param name="storeType"></param>
        /// <returns></returns>
        public IChannelConfirmDeleteDlg GetConfirmDeleteDlg(StoreTypeCode storeType)
        {
            // Get a new Dialog
            IChannelConfirmDeleteDlg dlg = dialogFactory();

            // Get a new View Model
            IConfirmChannelDeleteViewModel viewModel = viewModelFactory();

            // Load the store type into the view model
            viewModel.Load(storeType);

            // set the data context of the dialog to the view model
            dlg.DataContext = viewModel;

            // return the dialog
            return dlg;
        }

    }
}
