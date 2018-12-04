using System.ComponentModel;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has succeeded
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportSucceededState : ViewModelBase, IProductImportState
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ImportSucceededState(IProductImporterStateManager stateManager)
        {

        }

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {

        }
    }
}
