using System;
using System.Collections;
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
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Products.Import;

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
        private BindingList<long> selectedProductIDs = new BindingList<long>();
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
                items =>
                {
                    SelectedProductIDs.Clear();
                    items?.OfType<IDataWrapper<IProductListItemEntity>>().Select(x => x.EntityID).ForEach(x => selectedProductIDs.Add(x));
                });

            ImportProductsSplash = viewModelFactory.CreateImportSplash(RefreshProductsAction);

            ExportProducts = new RelayCommand(ExportProductsAction);
            ImportProducts = new RelayCommand(ImportProductsAction);
            AddProduct = new RelayCommand(async () => await AddProductAction().ConfigureAwait(true));
            ToggleActivation = new RelayCommand<long>((entityId) => ToggleActivationAction(entityId).Forget());
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
        /// Toggle the active status of a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ToggleActivation { get; }

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
        public BindingList<long> SelectedProductIDs
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
                    RaisePropertyChanged(nameof(IsSearchActive));
                    RefreshProductsAction();
                }
            }
        }

        /// <summary>
        /// Is there a product search active
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSearchActive => !string.IsNullOrEmpty(SearchText);

        /// <summary>
        /// Splash view for importing products
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IProductImporterSplashViewModel ImportProductsSplash { get; private set; }

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
                        result.Value.Aliases.RemoveWhere(a => !a.IsDefault);
                        await EditProduct(result.Value, "New Variant").ConfigureAwait(false);
                    }
                }

                Telemetry.TrackButtonClick("ShipWorks.Button.Click.Variant");
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

            Telemetry.TrackButtonClick("ShipWorks.Button.Click.Products.Import");
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

            Telemetry.TrackButtonClick("ShipWorks.Click.Products.Add");
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
        /// Toggle action to display whether a product is active or inactive.
        /// </summary>
        private async Task ToggleActivationAction(long entityId)
        {
            IProductListItemEntity product = Products.Data.Where(x => x.EntityID == entityId)
                .Select(x => x.Data).FirstOrDefault();

            if (product == null)
            {
                return;
            }

            bool makeItActive = !product.IsActive;
            string text = (makeItActive ? "Activating" : "Deactivating") + " products";

            try
            {
                await productCatalog
                .SetActivation(new[] { entityId }, makeItActive)
                .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                messageHelper.ShowError($"There was a problem {text.ToLower()}:\n\n{ex.Message}");
            }

            // Refresh is required to show the active/inactive state of the row.
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
