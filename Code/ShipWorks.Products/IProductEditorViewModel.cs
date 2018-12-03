﻿using System.ComponentModel;
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
        bool? ShowProductEditor(ProductVariantEntity product);
    }
}