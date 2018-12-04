using System;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Exception for use when importing products.
    /// </summary>
    public class ProductImportException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImportException(string message) : base(message)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImportException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
