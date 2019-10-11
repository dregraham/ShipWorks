using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// View model for the product importer
    /// </summary>
    [Component]
    public class ProductImporterSplashViewModel : ViewModelBase, IProductImporterSplashViewModel, IProductImporterStateManager
    {
        private IProductImportState currentState;
        private ProductImporterViewModel importViewModel;
        private readonly Action refreshProducts;
        private readonly Func<ProductImporterViewModel> createImportViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterSplashViewModel(
            Action refreshProducts,
            Func<IProductImporterStateManager, SetupImportState> createInitialState,
            Func<ProductImporterViewModel> createImportViewModel)
        {
            this.refreshProducts = refreshProducts;
            this.createImportViewModel = createImportViewModel;

            CurrentState = createInitialState(this);
        }

        /// <summary>
        /// Command to close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand Closing => importViewModel?.Closing;

        /// <summary>
        /// Current state of the importer
        /// </summary>
        [Obfuscation]
        public IProductImportState CurrentState
        {
            get => currentState;
            set => Set(ref currentState, value);
        }

        /// <summary>
        /// Change the state
        /// </summary>
        public void ChangeState(IProductImportState nextState)
        {
            if (importViewModel != null)
            {
                importViewModel.ChangeState(nextState);
            }
            else
            {
                importViewModel = createImportViewModel();
                importViewModel.ImportProducts(nextState).Do(refreshProducts);
            }
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        public Action Close => importViewModel?.Close;
    }
}
