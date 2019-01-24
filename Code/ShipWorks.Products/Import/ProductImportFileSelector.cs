using System;
using System.IO;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO;
using ShipWorks.Core.Common.Threading;

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
            fileSelector.GetFilePathToOpen("Comma Separated|*.csv|Excel|*.xls;*.xlsx|Tab Delimited|*.tab|All Files|*.*")
                .Map(x => (FilePath: x, State: createImportingState(stateManager)))
                .Do(x => stateManager.ChangeState(x.State))
                .Do(x => x.State.StartImport(x.FilePath).Forget());

        /// <summary>
        /// Save the sample file
        /// </summary>
        public void SaveSample()
        {
            fileSelector
                .GetFilePathToSave("Comma Separated|*.csv", "ProductImport_Sample.csv")
                .Do(x => WriteToFile(x));
        }

        /// <summary>
        /// Write sample to file
        /// </summary>
        private void WriteToFile(string filePath)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("ShipWorks.Products.Import.ProductImportSample.csv"))
            {
                using (var file = File.OpenWrite(filePath))
                {
                    stream.CopyTo(file);
                }
            }
        }
    }
}
