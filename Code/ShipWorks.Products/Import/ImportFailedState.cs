using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has failed
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportFailedState : ViewModelBase, IProductImportState
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ImportFailedState(IProductImporterStateManager stateManager)
        {

        }
    }
}
