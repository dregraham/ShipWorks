namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Start import with the chosen file
    /// </summary>
    public interface IProductImportFileSelector
    {
        /// <summary>
        /// Choose the file and start import
        /// </summary>
        void ChooseFileToImport(IProductImporterStateManager stateManager);

        /// <summary>
        /// Save the sample file
        /// </summary>
        void SaveSample();
    }
}
