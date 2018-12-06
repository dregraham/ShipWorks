using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has succeeded
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportSucceededState : ViewModelBase, IProductImportState, IImportSuccessResults
    {
        private readonly IProductImporterStateManager stateManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportSucceededState(ImportProductsResult results, IProductImporterStateManager stateManager)
        {
            this.stateManager = stateManager;
            SuccessCount = results.SuccessCount;
            NewCount = results.NewCount;
            ExistingCount = results.ExistingCount;

            CloseDialog = new RelayCommand(CloseDialogAction);
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Total products
        /// </summary>
        [Obfuscation]
        public int SuccessCount { get; }

        /// <summary>
        /// New products
        /// </summary>
        [Obfuscation]
        public int NewCount { get; }

        /// <summary>
        /// Existing products
        /// </summary>
        [Obfuscation]
        public int ExistingCount { get; }

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {

        }

        /// <summary>
        /// Should products be reloaded after the dialog closes
        /// </summary>
        public bool ShouldReloadProducts => true;

        /// <summary>
        /// Action to close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
