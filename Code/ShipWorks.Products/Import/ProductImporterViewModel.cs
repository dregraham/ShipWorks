using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// View model for the product importer
    /// </summary>
    [Component]
    public class ProductImporterViewModel : ViewModelBase, IProductImporterViewModel, IProductImporterStateManager
    {
        private readonly IProductImporter productImporter;
        private readonly IMessageHelper messageHelper;
        private readonly Func<IProductImporterViewModel, IProductImporterDialog> createDialog;
        private IProductImporterDialog dialog;

        private IProductImportState currentState;
        private string filePath;
        private bool isImporting;
        private int percentComplete;
        private readonly Func<IProductImporterStateManager, SetupImportState> createInitialState;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterViewModel(
            Func<IProductImporterViewModel, IProductImporterDialog> createDialog,
            Func<IProductImporterStateManager, SetupImportState> createInitialState,
            IProductImporter productImporter,
            IMessageHelper messageHelper)
        {
            this.createInitialState = createInitialState;
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            this.productImporter = productImporter;

            CloseDialog = new RelayCommand(() => CloseDialogAction());
            StartImport = new RelayCommand(() => StartImportAction().Forget());
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
        /// Current state of the importer
        /// </summary>
        [Obfuscation]
        public IProductImportState CurrentState
        {
            get => currentState;
            set => Set(ref currentState, value);
        }

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
        /// Is the dialog currently importing
        /// </summary>
        [Obfuscation]
        public bool IsImporting
        {
            get => isImporting;
            set => Set(ref isImporting, value);
        }

        /// <summary>
        /// Percent of the import completed
        /// </summary>
        [Obfuscation]
        public int PercentComplete
        {
            get => percentComplete;
            set => Set(ref percentComplete, value);
        }

        /// <summary>
        /// Import products
        /// </summary>
        public Result ImportProducts()
        {
            dialog = createDialog(this);

            CurrentState = createInitialState(this);

            return messageHelper.ShowDialog(dialog) == true ?
                Result.FromSuccess() :
                Result.FromError("Dialog canceled");
        }

        /// <summary>
        /// Change the state
        /// </summary>
        public void ChangeState(IProductImportState nextState) => CurrentState = nextState;

        /// <summary>
        /// Close the dialog
        /// </summary>
        public void Close() => dialog?.Close();

        /// <summary>
        /// Start the import
        /// </summary>
        private async Task StartImportAction()
        {
            var progressItem = new ProgressItem("Importing");

            try
            {
                progressItem.Changed += OnProgressItemUpdate;
                IsImporting = true;
                await productImporter.ImportProducts(FilePath, progressItem).ConfigureAwait(true);
            }
            finally
            {
                progressItem.Changed -= OnProgressItemUpdate;
                IsImporting = false;
            }
        }

        /// <summary>
        /// Handle an update of the progress dialog
        /// </summary>
        private void OnProgressItemUpdate(object sender, EventArgs e)
        {
            if (sender is IProgressReporter progressReporter)
            {
                PercentComplete = progressReporter.PercentComplete.Clamp(0, 100);
            }
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        private void CloseDialogAction() => dialog?.Close();
    }
}
