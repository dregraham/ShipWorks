﻿using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has succeeded
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportSucceededState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportSucceededState(ImportProductsResult results, IProductImporterStateManager stateManager)
        {
            this.stateManager = stateManager;
            TotalProducts = results.TotalCount;
            //AddedProducts = results.Add

            CloseDialog = new RelayCommand(CloseDialogAction);
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Total products
        /// </summary>
        [Obfuscation]
        public int TotalProducts { get; }

        /// <summary>
        /// Number of products added
        /// </summary>
        [Obfuscation]
        public int AddedProducts { get; }

        /// <summary>
        /// Number of updated products
        /// </summary>
        [Obfuscation]
        public int UpdatedProducts { get; }

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {

        }

        /// <summary>
        /// Action to close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
