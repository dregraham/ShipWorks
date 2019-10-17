using System;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// An exception thrown when importing products
    /// </summary>
    public class ProductImporterException : Exception
    {
        /// <summary>
        /// Whether or not the product being imported was new
        /// </summary>
        public bool IsNew { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterException(Exception innerException, bool isNew) : base(innerException.Message, innerException)
        {
            IsNew = isNew;
        }
    }
}
