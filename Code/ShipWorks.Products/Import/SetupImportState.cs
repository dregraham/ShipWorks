using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO;

namespace ShipWorks.Products.Import
{
    [Component(RegistrationType.Self)]
    public class SetupImportState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;
        private readonly Func<string, IProductImporterStateManager, ImportingState> createImportingState;
        private readonly IFileSelector fileSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        public SetupImportState(
            IProductImporterStateManager stateManager,
            IFileSelector fileSelector,
            Func<string, IProductImporterStateManager, ImportingState> createImportingState)
        {
            this.fileSelector = fileSelector;
            this.createImportingState = createImportingState;
            this.stateManager = stateManager;

            CloseDialog = new RelayCommand(CloseDialogAction);
            StartImport = new RelayCommand(StartImportAction);
        }

        /// <summary>
        /// Command to close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Command to start the import
        /// </summary>
        [Obfuscation]
        public ICommand StartImport { get; }

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
            fileSelector.GetFilePathToOpen("Excel|*.xls;*.xlsx|Comma Separated|*.csv|Tab Delimited|*.tab|All Files|*.*")
                .Do(x => stateManager.ChangeState(createImportingState(x, stateManager)));

        /// <summary>
        /// Close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
