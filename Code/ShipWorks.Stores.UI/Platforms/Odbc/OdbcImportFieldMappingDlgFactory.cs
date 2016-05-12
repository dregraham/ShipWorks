using System;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Factory for creating the <see cref="OdbcImportFieldMappingDlg"/>
    /// </summary>
    public class OdbcImportFieldMappingDlgFactory
    {
        private readonly Func<string, IDialog> dialogFactory;
        private readonly IOdbcImportFieldMappingDlgViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportFieldMappingDlgFactory"/> class.
        /// </summary>
        /// <param name="dialogFactory">The dialog factory.</param>
        /// <param name="viewModel">The view model.</param>
        public OdbcImportFieldMappingDlgFactory(Func<string, IDialog> dialogFactory, IOdbcImportFieldMappingDlgViewModel viewModel)
        {
            this.dialogFactory = dialogFactory;
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Gets the odbc import field mapping dialog.
        /// </summary>
        public IDialog CreateOdbcImportFieldMappingDlg(IWin32Window owner, OdbcStoreEntity store)
        {
            // Get the dialog
            IDialog dialog = dialogFactory("OdbcImportFieldMappingDlg");
            dialog.LoadOwner(owner);
            viewModel.LoadStore(store);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}