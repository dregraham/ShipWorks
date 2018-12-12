using System.Reflection;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Data used by the Import success results view
    /// </summary>
    public interface IImportSuccessResults
    {
        /// <summary>
        /// Total products
        /// </summary>
        [Obfuscation]
        int SuccessCount { get; }

        /// <summary>
        /// New products
        /// </summary>
        [Obfuscation]
        int NewCount { get; }

        /// <summary>
        /// Existing products
        /// </summary>
        [Obfuscation]
        int ExistingCount { get; }
    }
}