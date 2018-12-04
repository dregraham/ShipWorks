using System;
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

        private string filePath;
        private readonly Func<string, IProductImporterStateManager, ImportingState> createImportingState;

        /// <summary>
        /// Constructor
        /// </summary>
        public SetupImportState(
            IProductImporterStateManager stateManager,
            Func<string, IProductImporterStateManager, ImportingState> createImportingState)
        {
            this.createImportingState = createImportingState;
            this.stateManager = stateManager;

            CloseDialog = new RelayCommand(CloseDialogAction);
            StartImport = new RelayCommand(StartImportAction, () => !string.IsNullOrWhiteSpace(FilePath));
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
        /// Path to the file to import
        /// </summary>
        [Obfuscation]
        public string FilePath
        {
            get => filePath;
            set => Set(ref filePath, value);
        }

        /// <summary>
        /// Start the import action
        /// </summary>
        private void StartImportAction() =>
            stateManager.ChangeState(createImportingState(FilePath, stateManager));

        /// <summary>
        /// Close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
