using System;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Class for creating PdfDocuments
    /// </summary>
    [Component]
    public class PdfDocumentFactory : IPdfDocumentFactory
    {
        /// <summary>
        /// Create an IPdfDocument for a given 
        /// </summary>
        [SuppressMessage("ShipWorks", "SW0002")]
        public IPdfDocument Create(PdfDocumentType pdfDocumentType)
        {
            switch (pdfDocumentType)
            {
                case PdfDocumentType.BlackAndWhite:
                    return new PdfBlackAndWhiteDocument();
                case PdfDocumentType.Color:
                    return new PdfColorDocument();
                default:
                    throw new ArgumentOutOfRangeException(nameof(pdfDocumentType), pdfDocumentType, null);
            }
        }
    }
}
