﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            const string ResourceName = "ShipWorks.Tests.Resources.SinglePagePdf.pdf";

            using (Stream pdfFileStream = assembly.GetManifestResourceStream(ResourceName))
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
        public void Convert_MultiPagePdf_ReturnsSingleImageStream_Test()
        {
            List<Stream> images = new List<Stream>();

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                const string ResourceName = "ShipWorks.Tests.Resources.MultiPagePdf.pdf";

                using (Stream pdfFileStream = assembly.GetManifestResourceStream(ResourceName))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(pdfFileStream))
                    {
                        images = pdfDocument.ToImages().ToList();
                    }
                }

                // Since we're converting to TIFF there should only be one image for the entire PDF
                Assert.AreEqual(1, images.Count);
            }
            finally
            {
                foreach (Stream imageStream in images)
                {
                    images.First().Dispose();
                }
            }
        }

        [TestMethod]
        public void ToImages_ReturnsPngImages_Test()
        {
            List<ImageFormat> formats;
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string ResourceName = "ShipWorks.Tests.Resources.MultiPagePdf.pdf";
            

            using (Stream pdfFileStream = assembly.GetManifestResourceStream(ResourceName))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfFileStream))
                {
                    List<Stream> images = pdfDocument.ToImages().ToList();
                    formats = images.Select(i =>
                    {
                        Bitmap bitmap = new Bitmap(i);
                        ImageFormat format = bitmap.RawFormat;
                        bitmap.Dispose();

                        return format;
                    }).ToList();
                }
            }

            Assert.IsTrue(formats.All(f => f.Equals(ImageFormat.Png)));
        }
    }
}
