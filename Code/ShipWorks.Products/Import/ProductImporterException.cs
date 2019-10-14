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
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="isNew"></param>
        public ProductImporterException(Exception innerException, bool isNew) : base(innerException.Message, innerException)
        {
            IsNew = isNew;
        }
    }
}
