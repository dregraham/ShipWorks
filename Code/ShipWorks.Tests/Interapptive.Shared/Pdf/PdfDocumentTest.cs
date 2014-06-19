using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Interapptive.Shared.Pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rebex.Mail;

namespace ShipWorks.Tests.Interapptive.Shared.Pdf
{
    /// <summary>
    /// Test class to verify that the WeightUtility method(s) work correctly.
    /// </summary>
    [TestClass]
    public class PdfDocumentTest
    {
        // Verify that a one to one conversion works correctly
        [TestMethod]
        public void Convert_SinglePagePdf_ReturnsOneImageStream_Test()
        {
            List<Stream> images;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "ShipWorks.Tests.Resources.SinglePagePdf.pdf";
    
            using (Stream pdfFileStream = assembly.GetManifestResourceStream(resourceName))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfFileStream))
                {
                    images = pdfDocument.ToImages().ToList();

                    // Grab the first one and see if we can make it a bitmap.
                    Bitmap bitmap = new Bitmap(images.First());
                    bitmap.Dispose();
                }
            }

            Assert.AreEqual(1, images.Count);
        }

        // Verify that a multipage conversion works correctly
        [TestMethod]
        public void Convert_MultiPagePdf_ReturnsMultipleImageStreams_Test()
        {
            List<Stream> images;
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "ShipWorks.Tests.Resources.MultiPagePdf.pdf";

            using (Stream pdfFileStream = assembly.GetManifestResourceStream(resourceName))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfFileStream))
                {
                    images = pdfDocument.ToImages().ToList();

                    foreach (Stream imageStream in images)
                    {
                        // See if we can make the image into a bitmap.
                        Bitmap bitmap = new Bitmap(images.First());
                        bitmap.Dispose();
                    }
                }
            }

            Assert.AreEqual(9, images.Count);
        }
    }
}
