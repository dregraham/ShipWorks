namespace Interapptive.Shared.Pdf
{
    /// <summary>
    /// Interface for creating PdfDocuments
    /// </summary>
    public interface IPdfDocumentFactory
    {
        IPdfDocument Create(PdfDocumentType pdfDocumentType);
    }
}
