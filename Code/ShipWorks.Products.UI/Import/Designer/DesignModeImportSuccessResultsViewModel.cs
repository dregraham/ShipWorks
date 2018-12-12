using ShipWorks.Products.Import;

namespace ShipWorks.Products.UI.Import.Designer
{
    internal class DesignModeImportSuccessResultsViewModel : IImportSuccessResults
    {
        public int SuccessCount => 9876;

        public int NewCount => 9875;

        public int ExistingCount => 1;
    }
}
