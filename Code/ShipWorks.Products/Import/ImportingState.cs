using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

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
        private readonly Func<IImportProductsResult, IProductImporterStateManager, ImportSucceededState> createSuccessState;
        private readonly Func<Exception, IProductImporterStateManager, ImportFailedState> createFailedState;

        private int percentComplete;
        private readonly IProductImporter productImporter;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportingState(
            IProductImporterStateManager stateManager,
            IProductImporter productImporter,
            IProgressFactory progressFactory,
            Func<IImportProductsResult, IProductImporterStateManager, ImportSucceededState> createSuccessState,
            Func<Exception, IProductImporterStateManager, ImportFailedState> createFailedState,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.productImporter = productImporter;
            this.createFailedState = createFailedState;
            this.createSuccessState = createSuccessState;
            this.stateManager = stateManager;

            StopImport = new RelayCommand(StopImportAction);
            progressReporter = progressFactory.CreateReporter("Importing products...");
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
        /// Stop the import
        /// </summary>
        [Obfuscation]
        public ICommand StopImport { get; }

        /// <summary>
        /// Start the import
        /// </summary>
        public async Task StartImport(string filePath)
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
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e) =>
            e.Cancel = messageHelper.ShowQuestion("Closing will stop the import.  Are you sure?") != DialogResult.OK;

        /// <summary>
        /// Should products be reloaded after the dialog closes
        /// </summary>
        public bool ShouldReloadProducts => true;

        /// <summary>
        /// Cancel the import
        /// </summary>
        private void StopImportAction() => progressReporter.Cancel();

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