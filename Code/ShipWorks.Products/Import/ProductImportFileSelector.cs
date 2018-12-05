using System;
using System.IO;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Start import with the chosen file
    /// </summary>
    [Component]
    public class ProductImportFileSelector : IProductImportFileSelector
    {
        private readonly IFileSelector fileSelector;
        private readonly Func<IProductImporterStateManager, ImportingState> createImportingState;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImportFileSelector(
            IFileSelector fileSelector,
            Func<IProductImporterStateManager, ImportingState> createImportingState)
        {
            this.createImportingState = createImportingState;
            this.fileSelector = fileSelector;
        }

        /// <summary>
        /// Choose the file and start import
        /// </summary>
        public void ChooseFileToImport(IProductImporterStateManager stateManager) =>
            fileSelector.GetFilePathToOpen("Excel|*.xls;*.xlsx|Comma Separated|*.csv|Tab Delimited|*.tab|All Files|*.*")
                .Map(x => (FilePath: x, State: createImportingState(stateManager)))
                .Do(x => stateManager.ChangeState(x.State))
                .Do(x => x.State.StartImport(x.FilePath));

        /// <summary>
        /// Save the sample file
        /// </summary>
        public void SaveSample()
        {
            fileSelector
                .GetFilePathToSave("Comma Separated|*.csv")
                .Do(x => WriteToFile(x));
        }

        /// <summary>
        /// Write sample to file
        /// </summary>
        private void WriteToFile(string filePath)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream(typeof(ProductImportFileSelector), "ProductImportSample.csv"))
            {
                using (var file = File.OpenWrite(filePath))
                {
                    stream.CopyTo(file);
                }
            }
        }
    }
}
