using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// View model for the product importer
    /// </summary>
    [Component]
    public class ProductImporterViewModel : ViewModelBase, IProductImporterViewModel
    {
        private readonly IProductImporter productImporter;
        private readonly IMessageHelper messageHelper;
        private readonly Func<IProductImporterViewModel, IProductImporterDialog> createDialog;
        private IProductImporterDialog dialog;

        private string filePath;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterViewModel(
            Func<IProductImporterViewModel, IProductImporterDialog> createDialog,
            IProductImporter productImporter,
            IMessageHelper messageHelper)
        {
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            this.productImporter = productImporter;
        }

        /// <summary>
        /// Import products
        /// </summary>
        public Result ImportProducts()
        {
            dialog = createDialog(this);

            return messageHelper.ShowDialog(dialog) == true ?
                Result.FromSuccess() :
                Result.FromError("Dialog canceled");
        }

        /// <summary>
        /// Path to the file to import
        /// </summary>
        [Obfuscation]
        public string FilePath
        {
            get => filePath;
            set => Set(ref filePath, value);
        }
    }
}
