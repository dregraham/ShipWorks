namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Interface for creating PdfDocuments
    /// </summary>
    public interface IPdfDocumentFactory
    {
        /// <summary>
        /// Create an IPdfDocument for a given 
        /// </summary>
        IPdfDocument Create(PdfDocumentType pdfDocumentType);
    }
}
