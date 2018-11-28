using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using DataVirtualization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Products
{
    /// <summary>
    /// Main products mode view model
    /// </summary>
    [Component]
    public class ProductsMode : ViewModelBase, IProductsMode
    {
        private readonly IProductsViewHost view;
        private DataWrapper<IVirtualizingCollection<IProductListItemEntity>> products;
        private IList<IProductListItemEntity> selectedProducts;
        private IBasicSortDefinition currentSort;
        private string searchText;
        private bool showInactiveProducts;
        private readonly IProductsCollectionFactory productsCollectionFactory;
        private readonly IMessageHelper messageHelper;
        private readonly Func<IProductEditorViewModel> productEditorViewModelFunc;
        private readonly IProductCatalog productCatalog;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(IProductsViewHost view,
            IProductsCollectionFactory productsCollectionFactory,
            IMessageHelper messageHelper,
            Func<IProductEditorViewModel> productEditorViewModelFunc,
			IProductCatalog productCatalog)
        {
            this.productsCollectionFactory = productsCollectionFactory;
            this.messageHelper = messageHelper;
            this.productEditorViewModelFunc = productEditorViewModelFunc;
            this.view = view;
            this.productCatalog = productCatalog;

            CurrentSort = new BasicSortDefinition(ProductVariantFields.Name.Name, ListSortDirection.Ascending);

            RefreshProducts = new RelayCommand(() => RefreshProductsAction());
            EditProductVariant = new RelayCommand<long>(EditProductVariantAction);
            SelectedProductsChanged = new RelayCommand<IList>(
                items => SelectedProducts = items?.OfType<DataWrapper<IProductListItemEntity>>().Select(x => x.Data).ToList());

            DeactivateProductCommand =
                new RelayCommand(() => SetProductActivation(false).Forget(), () => SelectedProducts?.Any() == true);

            ActivateProductCommand =
                new RelayCommand(() => SetProductActivation(true).Forget(), () => SelectedProducts?.Any() == true);

			AddProduct = new RelayCommand(() => AddProductAction());
        }
		
        /// <summary>
        /// Command for Adding a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddProduct { get; set; }

        /// <summary>
        /// RelayCommand for activating a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ActivateProductCommand { get; }

        /// <summary>
        /// RelayCommand for deactivating a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeactivateProductCommand { get; }

        /// <summary>
        /// Command to refresh the products list
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand RefreshProducts { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand EditProductVariant { get; }

        /// <summary>
        /// The list of selected products has changed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SelectedProductsChanged { get; }

        /// <summary>
        /// List of products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products
        {
            get => products;
            private set => Set(ref products, value);
        }

        /// <summary>
        /// Collection of selected products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IList<IProductListItemEntity> SelectedProducts
        {
            get => selectedProducts;
            set => Set(ref selectedProducts, value);
        }

        /// <summary>
        /// Current sorting of the products list
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IBasicSortDefinition CurrentSort
        {
            get => currentSort;
            set
            {
                if (Set(ref currentSort, value))
                {
                    RefreshProductsAction();
                }
            }
        }

        /// <summary>
        /// Show inactive products in addition to active
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowInactiveProducts
        {
            get => showInactiveProducts;
            set
            {
                if (Set(ref showInactiveProducts, value))
                {
                    RefreshProductsAction();
                }
            }
        }

        /// <summary>
        /// Search text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SearchText
        {
            get => searchText;
            set
            {
                if (Set(ref searchText, value))
                {
                    RefreshProductsAction();
                }
            }
        }

        /// <summary>
        /// Initialize the mode
        /// </summary>
        public void Initialize(Action<Control> addControl, Action<Control> removeControl)
        {
            view.Initialize(this, addControl, removeControl);

            RefreshProductsAction();
        }

        /// <summary>
        /// Refresh the products list
        /// </summary>
        private void RefreshProductsAction()
        {
            Products = productsCollectionFactory.Create(ShowInactiveProducts, SearchText, CurrentSort);
        }

        /// <summary>
        /// Edit the given product variant
        /// </summary>
        private void EditProductVariantAction(long productVariantID)
        {
            messageHelper.ShowInformation($"You want to edit {productVariantID}, which will be implemented soon");
        }

        /// <summary>
        /// Add a product
        /// </summary>
        private void AddProductAction()
        {
            productEditorViewModelFunc().ShowProductEditor(new ProductVariantAliasEntity());
            RefreshProductsAction();
        }

        /// <summary>
        /// Activate a product
        /// </summary>
        private async Task SetProductActivation(bool makeItActive)
        {
            await productCatalog
                .SetActivation(SelectedProducts.Select(p => p.ProductVariantID), makeItActive)
                .ConfigureAwait(false);

            RefreshProductsAction();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do nothing for now
        }
    }
}
