using System.Collections.Generic;
using Interapptive.Shared.Pdf;

namespace ShipWorks.Data
{
    public interface IDataResourceManager
    {
        /// <summary>
        /// Crate database resource from PDF
        /// </summary>
        IEnumerable<DataResourceReference> CreateFromPdf(PdfDocument pdf, long consumerID, string label);

        /// <summary>
        /// Crate database resource from bytes
        /// </summary>
        DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label);
    }
}