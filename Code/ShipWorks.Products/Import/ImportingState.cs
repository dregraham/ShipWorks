using System;
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
        private readonly Func<IProductImporterStateManager, ImportSucceededState> createSuccessState;
        private readonly Func<IProductImporterStateManager, ImportFailedState> createFailedState;

        private int percentComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportingState(
            string filePath,
            IProductImporterStateManager stateManager,
            IProductImporter productImporter,
            Func<string, IProgressReporter> createProgressReporter,
            Func<IProductImporterStateManager, ImportSucceededState> createSuccessState,
            Func<IProductImporterStateManager, ImportFailedState> createFailedState)
        {
            this.createFailedState = createFailedState;
            this.createSuccessState = createSuccessState;
            this.stateManager = stateManager;

            Cancel = new RelayCommand(CancelAction);
            progressReporter = createProgressReporter("Importing products...");

            StartImport(filePath, productImporter).Forget();
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
        /// Cancel the import
        /// </summary>
        public ICommand Cancel { get; }

        /// <summary>
        /// Cancel the import
        /// </summary>
        private void CancelAction() => progressReporter.Cancel();

        /// <summary>
        /// Start the import
        /// </summary>
        private async Task StartImport(string filePath, IProductImporter productImporter)
        {
            try
            {
                progressReporter.Changed += OnProgressItemUpdate;
                var results = await productImporter.ImportProducts(filePath, progressReporter).ConfigureAwait(true);
                var nextState = results.Match<IProductImportState>(x => createSuccessState(stateManager), ex => createFailedState(stateManager));
                stateManager.ChangeState(nextState);
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