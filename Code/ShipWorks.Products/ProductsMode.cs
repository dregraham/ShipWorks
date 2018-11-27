using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DataVirtualization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(IProductsViewHost view, IProductsCollectionFactory productsCollectionFactory, IMessageHelper messageHelper)
        {
            this.productsCollectionFactory = productsCollectionFactory;
            this.messageHelper = messageHelper;
            this.view = view;

            CurrentSort = new BasicSortDefinition(ProductVariantFields.Name.Name, ListSortDirection.Ascending);

            RefreshProducts = new RelayCommand(() => RefreshProductsAction());
            EditProductVariant = new RelayCommand<long>(EditProductVariantAction);
            SelectedProductsChanged = new RelayCommand<IList>(
                items => SelectedProducts = items?.OfType<DataWrapper<IProductListItemEntity>>().Select(x => x.Data).ToList());
        }

        /// <summary>
        /// Command to refresh the products list
        /// </summary>
        public ICommand RefreshProducts { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        public ICommand EditProductVariant { get; }

        /// <summary>
        /// The list of selected products has changed
        /// </summary>
        public ICommand SelectedProductsChanged { get; }

        /// <summary>
        /// List of products
        /// </summary>
        public DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products
        {
            get => products;
            private set => Set(ref products, value);
        }

        /// <summary>
        /// Collection of selected products
        /// </summary>
        public IList<IProductListItemEntity> SelectedProducts
        {
            get => selectedProducts;
            set => Set(ref selectedProducts, value);
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
        /// Search text
        /// </summary>
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
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do nothing for now
        }
    }
}
