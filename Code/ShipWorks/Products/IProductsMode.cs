﻿using System;
using System.ComponentModel;
using System.Reflection;
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
        [Obfuscation(Exclude = true)]
        ICommand RefreshProducts { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        ICommand EditProductVariantLink { get; }

        /// <summary>
        /// Edit a given product variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        ICommand EditProductVariantButton { get; }

        /// <summary>
        /// Copy the selected variant
        /// </summary>
        [Obfuscation(Exclude = true)]
        ICommand CopyAsVariant { get; }

        /// <summary>
        /// Add a product
        /// </summary>
        ICommand AddProduct { get; }

        /// <summary>
        /// Import a list of products
        /// </summary>
        ICommand ImportProducts { get; }

        /// <summary>
        /// Export a list of products
        /// </summary>
        ICommand ExportProducts { get; }

        /// <summary>
        /// List of products
        /// </summary>
        [Obfuscation(Exclude = true)]
        IDataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products { get; }

        /// <summary>
        /// Collection of selected products
        /// </summary>
        [Obfuscation(Exclude = true)]
        BindingList<long> SelectedProductIDs { get; set; }

        /// <summary>
        /// The list of selected products has changed
        /// </summary>
        [Obfuscation(Exclude = true)]
        ICommand SelectedProductsChanged { get; }

        /// <summary>
        /// Current sorting of the products list
        /// </summary>
        [Obfuscation(Exclude = true)]
        IBasicSortDefinition CurrentSort { get; set; }

        /// <summary>
        /// Show inactive products in addition to active
        /// </summary>
        [Obfuscation(Exclude = true)]
        bool ShowInactiveProducts { get; set; }

        /// <summary>
        /// Initialize the mode
        /// </summary>
        void Initialize(Action<Control> addControl, Action<Control> removeControl);
    }
}
