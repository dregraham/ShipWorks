using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data;
using System.Drawing;
using ShipWorks.Common.IO.Hardware.Printers;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// A default implementation of the IScanFormPrinter and IScanFormBatchPrinter interfaces.
    /// </summary>
    public class DefaultScanFormPrinter : IScanFormPrinter, IScanFormBatchPrinter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultScanFormPrinter"/> class.
        /// </summary>
        public DefaultScanFormPrinter()
        { }

        /// <summary>
        /// Prints the SCAN form.
        /// </summary>
        /// <param name="owner">The IWin32Window object that a print dialog will below to if needed.</param>
        /// <param name="scanForm">The SCAN form.</param>
        /// <returns>Returns [true] if the SCAN form was printed successfully; otherwise [false].</returns>
        public bool Print(IWin32Window owner, ScanForm scanForm)
        {
            DataResourceReference resource = DataResourceManager.LoadConsumerResourceReferences(scanForm.ScanFormId).FirstOrDefault();
            if (resource == null)
            {
                throw new InvalidOperationException("No resource saved for scan form " + scanForm.ScanFormId);
            }

            using (Image image = Image.FromFile(resource.GetCachedFilename()))
            {
                return PrintUtility.PrintImage(owner, "ShipWorks - SCAN Form", image, true, true);
            }
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
                    DataResourceReference resource = DataResourceManager.LoadConsumerResourceReferences(scanForm.ScanFormId).FirstOrDefault();
                    if (resource == null)
                    {
                        throw new InvalidOperationException("No resource saved for scan form " + scanForm.ScanFormId);
                    }

                    scanFormImages.Add(Image.FromFile(resource.GetCachedFilename()));
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
