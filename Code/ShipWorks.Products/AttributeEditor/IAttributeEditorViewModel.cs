using System.ComponentModel;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.AttributeEditor
{
    /// <summary>
    /// Interface for the AttributeEditorViewModel
    /// </summary>
    public interface IAttributeEditorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Load the view model with the given product variant
        /// </summary>
        Task Load(ProductVariantEntity productVariantEntity);

        /// <summary>
        /// Save the attributes to the product variant
        /// </summary>
        void Save();
    }
}