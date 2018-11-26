using System;
using System.ComponentModel;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.UI
{
    public interface IProductEditorViewModel : INotifyPropertyChanged
    {
        void ShowProductEditor(ProductVariantAliasEntity product);
    }
}