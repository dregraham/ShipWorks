﻿using ShipWorks.Stores;
using System;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Factory for the ChannelConfirmDelete Dialog
    /// </summary>
    public class ChannelConfirmDeleteFactory : IChannelConfirmDeleteFactory
    {
        private readonly Func<IConfirmChannelDeleteViewModel> viewModelFactory;
        private readonly Func<string, IDialog> dialogFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelConfirmDeleteFactory(Func<IConfirmChannelDeleteViewModel> viewModelFactory, Func<string, IDialog> dialogFactory)
        {
            this.viewModelFactory = viewModelFactory;
            this.dialogFactory = dialogFactory;
        }

        /// <summary>
        /// Creates a ChannelConfirmationDeleteDlg using the given store type
        /// </summary>
        public IDialog GetConfirmDeleteDlg(StoreTypeCode storeType, IWin32Window owner)
        {
            // Get a new Dialog
            IDialog dlg = dialogFactory("ChannelConfirmDeleteDlg");

            // Set owner
            dlg.LoadOwner(owner);

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
