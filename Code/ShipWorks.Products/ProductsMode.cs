using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using DataVirtualization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private IBasicSortDefinition currentSort;
        private bool showInactiveProducts;
        private readonly IProductsCollectionFactory productsCollectionFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IProductEditorViewModel productEditorViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(IProductsViewHost view, 
            IProductsCollectionFactory productsCollectionFactory, 
            IMessageHelper messageHelper,
            IProductEditorViewModel productEditorViewModel)
        {
            this.productsCollectionFactory = productsCollectionFactory;
            this.messageHelper = messageHelper;
            this.productEditorViewModel = productEditorViewModel;
            this.view = view;

            CurrentSort = new BasicSortDefinition(ProductListItemFields.Name.Name, ListSortDirection.Ascending);

            RefreshProducts = new RelayCommand(() => RefreshProductsAction());
            EditProductVariant = new RelayCommand<long>(EditProductVariantAction);
            AddProduct = new RelayCommand(() => AddProductAction());
        }

        /// <summary>
        /// Command for Adding a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddProduct { get; set; }

        /// <summary>
        /// Command to refresh the products list
        /// </summary>
        public ICommand RefreshProducts { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        public ICommand EditProductVariant { get; }

        /// <summary>
        /// List of products
        /// </summary>
        public DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products
        {
            get => products;
            private set => Set(ref products, value);
        }

        /// <summary>
        /// Current sorting of the products list
        /// </summary>
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
            Products = productsCollectionFactory.Create(ShowInactiveProducts, CurrentSort);
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
            productEditorViewModel.ShowProductEditor(new ProductVariantAliasEntity());
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
