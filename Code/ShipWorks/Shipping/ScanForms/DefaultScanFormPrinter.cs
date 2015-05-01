using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data;
using System.Drawing;
using ShipWorks.Common.IO.Hardware.Printers;
using System.Windows.Forms;
using Interapptive.Shared.Collections;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// A default implementation of the IScanFormPrinter and IScanFormBatchPrinter interfaces.
    /// </summary>
    public class DefaultScanFormPrinter : IScanFormPrinter, IScanFormBatchPrinter
    {
        /// <summary>
        /// Prints the SCAN form.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanForm">The SCAN form.</param>
        /// <returns>Returns [true] if the SCAN form was printed successfully; otherwise [false].</returns>
        public bool Print(IWin32Window owner, ScanForm scanForm)
        {
            List<DataResourceReference> resources = GetScanFormResources(scanForm);

            return PrintUtility.PrintImages(owner, "ShipWorks - SCAN Form", resources.Select(r => Image.FromFile(r.GetCachedFilename())).ToList(), true, true);
        }

        private static List<DataResourceReference> GetScanFormResources(ScanForm scanForm)
        {
            List<DataResourceReference> resources = DataResourceManager.LoadConsumerResourceReferences(scanForm.ScanFormId);
            if (resources == null || resources.None())
            {
                throw new InvalidOperationException("No resource saved for scan form " + scanForm.ScanFormId);
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
                scanFormImages.ForEach(i =>
                { 
                    i.Dispose();
                    scanFormImages.Remove(i);
                });
                
            }
        }
    }
}
