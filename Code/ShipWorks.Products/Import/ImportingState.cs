using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Importing statue
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportingState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;
        private readonly IProgressReporter progressReporter;
        private readonly Func<ImportProductsResult, IProductImporterStateManager, ImportSucceededState> createSuccessState;
        private readonly Func<Exception, IProductImporterStateManager, ImportFailedState> createFailedState;

        private int percentComplete;
        private readonly IProductImporter productImporter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportingState(
            IProductImporterStateManager stateManager,
            IProductImporter productImporter,
            Func<string, IProgressReporter> createProgressReporter,
            Func<ImportProductsResult, IProductImporterStateManager, ImportSucceededState> createSuccessState,
            Func<Exception, IProductImporterStateManager, ImportFailedState> createFailedState)
        {
            this.productImporter = productImporter;
            this.createFailedState = createFailedState;
            this.createSuccessState = createSuccessState;
            this.stateManager = stateManager;

            Cancel = new RelayCommand(CancelAction);
            progressReporter = createProgressReporter("Importing products...");
        }

        /// <summary>
        /// Percent of the import that's complete
        /// </summary>
        [Obfuscation]
        public int PercentComplete
        {
            get => percentComplete;
            set => Set(ref percentComplete, value);
        }

        /// <summary>
        /// Start the import
        /// </summary>
        /// <param name="filePath"></param>
        public void StartImport(string filePath) =>
            StartImportInternal(filePath).Forget();

        /// <summary>
        /// Cancel the import
        /// </summary>
        public ICommand Cancel { get; }

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Cancel the import
        /// </summary>
        private void CancelAction() => progressReporter.Cancel();

        /// <summary>
        /// Start the import
        /// </summary>
        private async Task StartImportInternal(string filePath)
        {
            try
            {
                progressReporter.Changed += OnProgressItemUpdate;
                var results = await productImporter.ImportProducts(filePath, progressReporter).ConfigureAwait(true);
                var nextState = results.Match<IProductImportState>(x => createSuccessState(x, stateManager), ex => createFailedState(ex, stateManager));
                stateManager.ChangeState(nextState);
            }
            catch (Exception ex)
            {
                stateManager.ChangeState(createFailedState(ex, stateManager));
            }
            finally
            {
                progressReporter.Changed -= OnProgressItemUpdate;
            }
        }

        /// <summary>
        /// The progress item has been updated
        /// </summary>
        private void OnProgressItemUpdate(object sender, EventArgs e)
        {
            if (sender is IProgressReporter progressReporter)
            {
                PercentComplete = progressReporter.PercentComplete.Clamp(0, 100);
            }
        }
    }
}