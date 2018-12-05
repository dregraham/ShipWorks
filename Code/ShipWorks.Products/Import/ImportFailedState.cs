using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has failed
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportFailedState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;
        private readonly IProductImportFileSelector productImportFileSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportFailedState(Exception exception, IProductImporterStateManager stateManager, IProductImportFileSelector productImportFileSelector)
        {
            this.productImportFileSelector = productImportFileSelector;
            this.stateManager = stateManager;
            FailureReason = exception.Message;

            if (exception is FailedProductImportException failedImport)
            {
                SuccessCount = failedImport.SuccessCount;
                FailedCount = failedImport.FailedCount;
                ImportErrors = failedImport.FailedProducts;
            }

            CloseDialog = new RelayCommand(CloseDialogAction);
            StartImport = new RelayCommand(StartImportAction);
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Start the import
        /// </summary>
        [Obfuscation]
        public ICommand StartImport { get; }

        /// <summary>
        /// Reason for the failure
        /// </summary>
        [Obfuscation]
        public string FailureReason { get; }

        /// <summary>
        /// Number of products successfully imported
        /// </summary>
        [Obfuscation]
        public int SuccessCount { get; }

        /// <summary>
        /// Number of products that failed to import
        /// </summary>
        [Obfuscation]
        public int FailedCount { get; }

        /// <summary>
        /// Import specific errors
        /// </summary>
        [Obfuscation]
        public IDictionary<string, string> ImportErrors { get; }

        /// <summary>
        /// Are there any import errors
        /// </summary>
        [Obfuscation]
        public bool HasImportErrors => ImportErrors?.Any() == true;

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {

        }

        /// <summary>
        /// Start the import action
        /// </summary>
        private void StartImportAction() =>
            productImportFileSelector.ChooseFileToImport(stateManager);

        /// <summary>
        /// Action to close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
