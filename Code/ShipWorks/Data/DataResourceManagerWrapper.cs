using System.Collections.Generic;
using Interapptive.Shared.Pdf;

namespace ShipWorks.Data
{
    public class DataResourceManagerWrapper : IDataResourceManager
    {
        /// <summary>
        /// Saves PDF to database 
        /// </summary>
        public IEnumerable<DataResourceReference> CreateFromPdf(PdfDocument pdf, long consumerID, string label)
        {
            return DataResourceManager.CreateFromPdf(pdf, consumerID, label);
        }

        /// <summary>
        /// Saves Image to database
        /// </summary>
        public DataResourceReference CreateFromBytes(byte[] data, long consumerID, string label)
        {
           return DataResourceManager.CreateFromBytes(data, consumerID, label);
        }
    }
}