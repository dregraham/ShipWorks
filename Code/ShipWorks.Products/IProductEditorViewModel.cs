using System.ComponentModel;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products
{
    /// <summary>
    /// Represents the ProductEditorViewModel
    /// </summary>
    public interface IProductEditorViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Show the product editor
        /// </summary>
        Task<bool?> ShowProductEditor(ProductVariantEntity product);
    }
}