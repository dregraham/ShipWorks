using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Connection;
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
        private IList<long> selectedProductIDs = new List<long>();
        private IBasicSortDefinition currentSort;
        private string searchText;
        private bool showInactiveProducts;
        private readonly IProductsCollectionFactory productsCollectionFactory;
        private readonly IMessageHelper messageHelper;
        private readonly Func<IProductEditorViewModel> productEditorViewModelFunc;
        private readonly IProductCatalog productCatalog;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(IProductsViewHost view,
            IProductsCollectionFactory productsCollectionFactory,
            IMessageHelper messageHelper,
            Func<IProductEditorViewModel> productEditorViewModelFunc,
            IProductCatalog productCatalog,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.productsCollectionFactory = productsCollectionFactory;
            this.messageHelper = messageHelper;
            this.productEditorViewModelFunc = productEditorViewModelFunc;
            this.view = view;
            this.productCatalog = productCatalog;
            this.sqlAdapterFactory = sqlAdapterFactory;
            CurrentSort = new BasicSortDefinition(ProductVariantAliasFields.Sku.Name, ListSortDirection.Ascending);

            RefreshProducts = new RelayCommand(() => RefreshProductsAction());
            EditProductVariantLink = new RelayCommand<long>(EditProductVariantAction);
            EditProductVariantButton = new RelayCommand(EditProductVariantButtonAction, () => SelectedProductIDs?.Count() == 1);
            SelectedProductsChanged = new RelayCommand<IList>(
                items => SelectedProductIDs = items?.OfType<DataWrapper<IProductListItemEntity>>().Select(x => x.EntityID).ToList());

            DeactivateProductCommand =
                new RelayCommand(() => SetProductActivation(false).Forget(), () => SelectedProductIDs?.Any() == true);

            ActivateProductCommand =
                new RelayCommand(() => SetProductActivation(true).Forget(), () => SelectedProductIDs?.Any() == true);

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
        public ICommand EditProductVariantLink { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand EditProductVariantButton { get; }

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
        public IList<long> SelectedProductIDs
        {
            get => selectedProductIDs;
            set => Set(ref selectedProductIDs, value);
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
        /// Edit the selected Product
        /// </summary>
        private void EditProductVariantButtonAction() =>
            EditProductVariantAction(SelectedProductIDs.FirstOrDefault());

        /// <summary>
        /// Edit the given product variant
        /// </summary>
        private void EditProductVariantAction(long productVariantID)
        {
            if (productVariantID == 0)
            {
                return;
            }

            ProductVariantAliasEntity productVariantAlias;

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                productVariantAlias = productCatalog.FetchProductVariantEntity(adapter, productVariantID)?.Aliases.FirstOrDefault(a => a.IsDefault);
            }

            if (productVariantAlias != null)
            {
                EditProduct(productVariantAlias.ProductVariant);
            }
        }

        /// <summary>
        /// Add a product
        /// </summary>
        private void AddProductAction()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity()
            {
                Product = new ProductEntity()
                {
                    IsActive = true,
                    IsBundle = false,
                    Name = string.Empty
                }
            };

            productVariant.Aliases.Add(new ProductVariantAliasEntity()
            {
                AliasName = string.Empty,
                IsDefault = true
            });

            EditProduct(productVariant);
        }

        /// <summary>
        /// Edit the given product
        /// </summary>
        private void EditProduct(ProductVariantEntity productVariantEntity)
        {
            if (productEditorViewModelFunc().ShowProductEditor(productVariantEntity) ?? false)
            {
                Result saveResult;

                using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
                {
                    saveResult = productCatalog.Save(adapter, productVariantEntity.Product);
                    if (saveResult.Success)
                    {
                        adapter.Commit();
                    }
                    else
                    {
                        adapter.Rollback();
                    }
                }

                if (saveResult.Success)
                {
                    RefreshProductsAction();
                }
                else
                {
                    SqlException sqlException = saveResult.Exception.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 2601)
                    {
                        messageHelper.ShowError($"The SKU \"{productVariantEntity.Aliases.First(a => a.IsDefault).Sku}\" already exists. Please enter a unique value for the Product SKU.", saveResult.Exception);
                    }
                    else
                    {
                        messageHelper.ShowError(saveResult.Message);
                    }

                    EditProduct(productVariantEntity);
                }
            }
        }

        /// <summary>
        /// Activate a product
        /// </summary>
        private async Task SetProductActivation(bool makeItActive)
        {
            var text = (makeItActive ? "Activating" : "Deactivating") + " products";

            using (var item = messageHelper.ShowProgressDialog(text, text))
            {
                await productCatalog
                    .SetActivation(SelectedProductIDs, makeItActive, item.ProgressItem)
                    .ConfigureAwait(false);
            }

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
