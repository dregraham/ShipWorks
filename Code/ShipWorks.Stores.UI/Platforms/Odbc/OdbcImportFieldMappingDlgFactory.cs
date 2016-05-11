using System;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcImportFieldMappingDlgFactory
    {
        private readonly Func<string, IDialog> dialogFactory;
        private readonly IOdbcImportFieldMappingDlgViewModel viewModel;

        public OdbcImportFieldMappingDlgFactory(Func<string, IDialog> dialogFactory, IOdbcImportFieldMappingDlgViewModel viewModel)
        {
            this.dialogFactory = dialogFactory;
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Gets the channel limit dialog.
        /// </summary>
        public IDialog GetOdbcImportFieldMappingDlg(IWin32Window owner)
        {
            // Get the dialog
            IDialog dialog = dialogFactory("OdbcImportFieldMappingDlg");
            dialog.LoadOwner(owner);
            dialog.DataContext = viewModel;

            return dialog;
        }
    }
}