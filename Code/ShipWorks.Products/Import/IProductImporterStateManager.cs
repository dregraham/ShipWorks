namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Manage the current state of the product import process
    /// </summary>
    public interface IProductImporterStateManager
    {
        /// <summary>
        /// Change the state
        /// </summary>
        void ChangeState(IProductImportState nextState);

        /// <summary>
        /// Close the dialog
        /// </summary>
        void Close();
    }
}