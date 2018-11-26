using System;
using System.Windows.Forms;
using System.Windows.Input;
using DataVirtualization;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Main products view model
    /// </summary>
    public interface IProductsMode : IDisposable
    {
        /// <summary>
        /// Command to refresh the products list
        /// </summary>
        ICommand RefreshProducts { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        ICommand EditProductVariant { get; }

        /// <summary>
        /// Add a product
        /// </summary>
        ICommand AddProduct { get; }

        /// <summary>
        /// List of products
        /// </summary>
        DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products { get; }

        /// <summary>
        /// Current sorting of the products list
        /// </summary>
        IBasicSortDefinition CurrentSort { get; set; }

        /// <summary>
        /// Show inactive products in addition to active
        /// </summary>
        bool ShowInactiveProducts { get; set; }

        /// <summary>
        /// Initialize the mode
        /// </summary>
        void Initialize(Action<Control> addControl, Action<Control> removeControl);
    }
}
