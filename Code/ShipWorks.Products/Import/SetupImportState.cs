using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    [Component(RegistrationType.Self)]
    public class SetupImportState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;
        private readonly IProductImportFileSelector productImportFileSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        public SetupImportState(IProductImporterStateManager stateManager, IProductImportFileSelector productImportFileSelector)
        {
            this.productImportFileSelector = productImportFileSelector;
            this.stateManager = stateManager;

            CloseDialog = new RelayCommand(CloseDialogAction);
            SaveSample = new RelayCommand(SaveSampleAction);
            StartImport = new RelayCommand(StartImportAction);
        }

        /// <summary>
        /// Command to close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Save the sample file
        /// </summary>
        [Obfuscation]
        public ICommand SaveSample { get; }

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
        /// Should products be reloaded after the dialog closes
        /// </summary>
        public bool ShouldReloadProducts => false;

        /// <summary>
        /// Save the sample file
        /// </summary>
        private void SaveSampleAction() => productImportFileSelector.SaveSample();

        /// <summary>
        /// Start the import action
        /// </summary>
        private void StartImportAction() =>
            productImportFileSelector.ChooseFileToImport(stateManager);

        /// <summary>
        /// Close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
