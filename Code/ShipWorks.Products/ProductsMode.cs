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
        private readonly Func<IProductEditorViewModel> productEditorViewModelFunc;
        private readonly IProductsViewHost view;
        private IDataWrapper<IVirtualizingCollection<IProductListItemEntity>> products;
        private IList<long> selectedProductIDs = new List<long>();
        private IBasicSortDefinition currentSort;
        private string searchText;
        private bool showInactiveProducts;
        private readonly IProductsCollectionFactory productsCollectionFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IProductCatalog productCatalog;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IProductViewModelFactory viewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(
            Func<IProductEditorViewModel> productEditorViewModelFunc,
            IProductsViewHost view,
            IProductsCollectionFactory productsCollectionFactory,
            IMessageHelper messageHelper,
            IProductViewModelFactory viewModelFactory,
            IProductCatalog productCatalog,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.viewModelFactory = viewModelFactory;
            this.productsCollectionFactory = productsCollectionFactory;
            this.messageHelper = messageHelper;
            this.productEditorViewModelFunc = productEditorViewModelFunc;
            this.view = view;
            this.productCatalog = productCatalog;
            this.sqlAdapterFactory = sqlAdapterFactory;
            CurrentSort = new BasicSortDefinition(ProductVariantAliasFields.Sku.Name, ListSortDirection.Ascending);

            RefreshProducts = new RelayCommand(RefreshProductsAction);
            EditProductVariantLink = new RelayCommand<long>(async l => await EditProductVariantAction(l).ConfigureAwait(true));
            EditProductVariantButton = new RelayCommand(async () => await EditProductVariantButtonAction().ConfigureAwait(true), () => SelectedProductIDs?.Count() == 1);
            CopyAsVariant = new RelayCommand(async () => await CopyAsVariantAction().ConfigureAwait(true), () => SelectedProductIDs?.Count() == 1);
            SelectedProductsChanged = new RelayCommand<IList>(
                items => SelectedProductIDs = items?.OfType<IDataWrapper<IProductListItemEntity>>().Select(x => x.EntityID).ToList());

            DeactivateProductCommand =
                new RelayCommand(() => SetProductActivation(false).Forget(), () => SelectedProductIDs?.Any() == true);

            ActivateProductCommand =
                new RelayCommand(() => SetProductActivation(true).Forget(), () => SelectedProductIDs?.Any() == true);

            ExportProducts = new RelayCommand(ExportProductsAction);
            ImportProducts = new RelayCommand(ImportProductsAction);
            AddProduct = new RelayCommand(async () => await AddProductAction().ConfigureAwait(true));
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
        /// Copy the selected variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CopyAsVariant { get; }

        /// <summary>
        /// Command to import a list of products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ImportProducts { get; }

        /// <summary>
        /// Command to export a list of products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ExportProducts { get; }

        /// <summary>
        /// The list of selected products has changed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SelectedProductsChanged { get; }

        /// <summary>
        /// List of products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products
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
        /// Export products
        /// </summary>
        private void ExportProductsAction() =>
            viewModelFactory.CreateExport().ExportProducts();

        /// <summary>
        /// Import products
        /// </summary>
        private void ImportProductsAction() =>
            viewModelFactory.CreateImport().ImportProducts().Do(RefreshProductsAction);

        /// <summary>
        /// Edit the selected Product
        /// </summary>
        private Task EditProductVariantButtonAction() =>
            EditProductVariantAction(SelectedProductIDs.FirstOrDefault());

        /// <summary>
        /// Copy the selected variant and edit
        /// </summary>
        private async Task CopyAsVariantAction()
        {
            if (SelectedProductIDs.IsCountEqualTo(1) && SelectedProductIDs.FirstOrDefault() > 0)
            {
                ProductVariantEntity selectedProductVariant;
                using (ISqlAdapter adapter = sqlAdapterFactory.Create())
                {
                    selectedProductVariant = productCatalog.FetchProductVariantEntity(adapter, SelectedProductIDs.First());
                }

                if (selectedProductVariant != null)
                {
                    GenericResult<ProductVariantEntity> result = productCatalog.CloneVariant(selectedProductVariant);

                    if (result.Failure)
                    {
                        messageHelper.ShowError(result.Message);
                    }
                    else
                    {
                        await EditProduct(result.Value, "New Variant").ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Edit the given product variant
        /// </summary>
        private async Task EditProductVariantAction(long productVariantID)
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
                await EditProduct(productVariantAlias.ProductVariant, "Edit Product").ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Add a product
        /// </summary>
        private async Task AddProductAction()
        {
            ProductVariantEntity productVariant = new ProductVariantEntity()
            {
                Product = new ProductEntity()
                {
                    IsActive = true,
                    IsBundle = false
                }
            };

            productVariant.Aliases.Add(new ProductVariantAliasEntity()
            {
                AliasName = string.Empty,
                IsDefault = true
            });

            await EditProduct(productVariant, "New Product").ConfigureAwait(true);
        }

        /// <summary>
        /// Edit the given product
        /// </summary>
        private async Task EditProduct(ProductVariantEntity productVariantEntity, string dialogTitle)
        {
            if ((await productEditorViewModelFunc().ShowProductEditor(productVariantEntity, dialogTitle).ConfigureAwait(true)) ?? false)
            {
                RefreshProductsAction();
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
