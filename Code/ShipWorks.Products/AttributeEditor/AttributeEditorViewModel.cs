using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Products.AttributeEditor
{
    /// <summary>
    /// View model for the AttributeEditorControl
    /// </summary>
    [Component]
    public class AttributeEditorViewModel : ViewModelBase, IAttributeEditorViewModel
    {
        private string selectedAttributeName;
        private ObservableCollection<string> attributeNames;
        private string attributeValue;
        private ObservableCollection<ProductVariantAttributeValueEntity> productAttributes;
        private ProductVariantAttributeValueEntity selectedProductAttribute;
        private ProductVariantEntity productVariant;
        private readonly IMessageHelper messageHelper;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IProductCatalog productCatalog;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeEditorViewModel(IMessageHelper messageHelper, ISqlAdapterFactory sqlAdapterFactory, IProductCatalog productCatalog)
        {
            this.messageHelper = messageHelper;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productCatalog = productCatalog;

            AttributeNames = new ObservableCollection<string>();
            SelectedAttributeName = AttributeNames.FirstOrDefault();

            ProductAttributes = new ObservableCollection<ProductVariantAttributeValueEntity>();

            AddAttributeToProductCommand = new RelayCommand(AddAttributeToProduct);
            RemoveAttributeFromProductCommand = new RelayCommand(RemoveAttributeFromProduct, () => SelectedProductAttribute != null);
        }

        /// <summary>
        /// The attribute name the user has selected or entered
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SelectedAttributeName
        {
            get => selectedAttributeName;
            set => Set(ref selectedAttributeName, value);
        }

        /// <summary>
        /// The list of available attribute names
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<string> AttributeNames
        {
            get => attributeNames;
            set => Set(ref attributeNames, value);
        }

        /// <summary>
        /// Attribute value the user enters
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string AttributeValue
        {
            get => attributeValue;
            set => Set(ref attributeValue, value);
        }

        /// <summary>
        /// The list of attributes attached to the current product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ProductVariantAttributeValueEntity> ProductAttributes
        {
            get => productAttributes;
            set => Set(ref productAttributes, value);
        }

        /// <summary>
        /// The product attribute that the user has selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ProductVariantAttributeValueEntity SelectedProductAttribute
        {
            get => selectedProductAttribute;
            set => Set(ref selectedProductAttribute, value);
        }

        /// <summary>
        /// Command for adding an attribute to a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddAttributeToProductCommand { get; }

        /// <summary>
        /// Command for removing an attribute from a product
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand RemoveAttributeFromProductCommand { get; }

        /// <summary>
        /// Load the view model with the given product
        /// </summary>
        public async Task Load(ProductVariantEntity productVariantEntity)
        {
            productVariant = productVariantEntity;
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                IEnumerable<IProductAttributeEntity> attributes = await productCatalog.GetAvailableAttributesFor(sqlAdapter, productVariantEntity).ConfigureAwait(true);
                AttributeNames = new ObservableCollection<string>(attributes.Select(a => a.AttributeName));
            }

            AttributeNames = new ObservableCollection<string>(productVariant.Product.Attributes.Select(x => x.AttributeName));
            ProductAttributes = new ObservableCollection<ProductVariantAttributeValueEntity>(productVariant.Attributes);
        }

        /// <summary>
        /// Save the attributes to the product
        /// </summary>
        public void Save()
        {
            var variantAttributesToRemove = productVariant.Attributes
                .Where(a => ProductAttributes.None(selectedAttribute => selectedAttribute.ProductVariantAttributeValueID == a.ProductVariantAttributeValueID))
                .ToEntityCollection();

            productVariant.Attributes.RemovedEntitiesTracker = new EntityCollection<ProductVariantAttributeValueEntity>();

            productVariant.Attributes.RemoveRange(variantAttributesToRemove.ToList());
            
            var productsToAdd = ProductAttributes.Where(selectedAttribute => productVariant.Attributes.None(currentAttribute => currentAttribute.ProductVariantAttributeValueID == selectedAttribute.ProductVariantAttributeValueID));

            foreach (ProductVariantAttributeValueEntity attribute in productsToAdd)
            {
                productVariant.Product.Attributes.Add(attribute.ProductAttribute);
                productVariant.Attributes.Add(attribute);
            }
        }

        /// <summary>
        /// Add an attribute to the product with the selected name and entered value
        /// </summary>
        private void AddAttributeToProduct()
        {
            if (string.IsNullOrWhiteSpace(SelectedAttributeName))
            {
                messageHelper.ShowError("Please enter an attribute name.");
                return;
            }

            if (ProductAttributes.Any(x => x.ProductAttribute.AttributeName.Equals(SelectedAttributeName, StringComparison.InvariantCultureIgnoreCase)))
            {
                messageHelper.ShowError($"This product already contains an attribute named \"{SelectedAttributeName}\"");
                return;
            }

            ProductAttributeEntity attribute;

            // Get the product attribute with the given name
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                attribute = productCatalog.FetchProductAttribute(adapter, SelectedAttributeName, productVariant.ProductID);
            }

            // If we didn't find an attribute with the given name, create one
            if (attribute == null)
            {
                attribute = new ProductAttributeEntity
                {
                    ProductID = productVariant.ProductID,
                    AttributeName = SelectedAttributeName
                };
            }

            ProductAttributes.Add(new ProductVariantAttributeValueEntity
            {
                ProductVariantID = productVariant.ProductVariantID,
                ProductAttribute = attribute,
                AttributeValue = AttributeValue ?? string.Empty
            });
        }

        /// <summary>
        /// Remove the selected attribute from the product
        /// </summary>
        private void RemoveAttributeFromProduct()
        {
            ProductAttributes.Remove(SelectedProductAttribute);
        }
    }
}