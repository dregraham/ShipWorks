using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// A default implementation of the IScanFormPrinter and IScanFormBatchPrinter interfaces.
    /// </summary>
    public class DefaultScanFormPrinter : IScanFormPrinter, IScanFormBatchPrinter
    {
        private static ILog log = LogManager.GetLogger(typeof(DefaultScanFormPrinter));

        /// <summary>
        /// Prints the SCAN form.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanForm">The SCAN form.</param>
        /// <returns>Returns [true] if the SCAN form was printed successfully; otherwise [false].</returns>
        public bool Print(IWin32Window owner, ScanForm scanForm)
        {
            return PrintUtility.PrintImages(owner, "ShipWorks - SCAN Form", GetScanFormResourcesImages(scanForm).ToList(), true, true);
        }

        /// <summary>
        /// Return a list of Images for the given scan form
        /// </summary>
        private static IEnumerable<Image> GetScanFormResourcesImages(ScanForm scanForm)
        {
            return GetScanFormResources(scanForm).Select(r =>
                {
                    try
                    {
                        string fileName = r.GetCachedFilename();
                        log.Info($"About to print resource for scan form {scanForm.ScanFormId}.  Filename: {fileName}");

                        return Image.FromFile(fileName);
                    }
                    catch (Exception ex) when (ex is OutOfMemoryException || ex is FileNotFoundException || ex is ArgumentException)
                    {
                        string scanFormDesc = $"{scanForm.CreatedDate.ToLocalTime():MM/dd/yy h:mm tt}";
                        throw new ShippingException($"One of the images is invalid for scan form '{scanFormDesc}'.");
                    }
                });
        }

        /// <summary>
        /// Get a list of data resource references for given scan form
        /// </summary>
        private static List<DataResourceReference> GetScanFormResources(ScanForm scanForm)
        {
            List<DataResourceReference> resources = DataResourceManager.LoadConsumerResourceReferences(scanForm.ScanFormId);
            if (resources == null || resources.None())
            {
                throw new ShippingException("No resource saved for scan form " + scanForm.ScanFormId);
            }
            return resources;
        }

        /// <summary>
        /// Prints a batch of SCAN forms.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>Returns [true] if the SCAN forms were printed successfully; otherwise [false].</returns>
        /// <exception cref="System.InvalidOperationException">No resource saved for scan form  + scanForm.ScanFormId</exception>
        public bool Print(IWin32Window owner, ScanFormBatch scanFormBatch)
        {
            List<Image> scanFormImages = new List<Image>();

            try
            {
                foreach (ScanForm scanForm in scanFormBatch.ScanForms)
                {
                    scanFormImages.AddRange(GetScanFormResources(scanForm).Select(r => Image.FromFile(r.GetCachedFilename())));
                }

                return PrintUtility.PrintImages(owner, "ShipWorks - SCAN Forms", scanFormImages.ToList(), true, true);
            }
            finally
            {
                scanFormImages.ForEach(i => i.Dispose());
            }
        }
    }
}
