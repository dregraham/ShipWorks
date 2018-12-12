using System.ComponentModel;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.AttributeEditor
{
    public interface IAttributeEditorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Load the view model with the given product
        /// </summary>
        Task Load(ProductVariantEntity productVariantEntity);

        /// <summary>
        /// Save the attributes to the product
        /// </summary>
        void Save();
    }
}