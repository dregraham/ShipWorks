using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Pdf;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Pdf
{
    [CLSCompliant(false)]
    public class PdfDocumentTest
    {
        [Theory]
        [InlineData("ShipWorks.Tests.Resources.SinglePagePdf.pdf", 1)]
        [InlineData("ShipWorks.Tests.Resources.MultiPagePdf.pdf", 9)]
        public void Convert_CorrectNumberOfPagesConverted_WhenPdfStreamIsValid(string path, int expectedPageCount)
        {
            int pageCount = 0;

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream pdfFileStream = assembly.GetManifestResourceStream(path))
            {
                new PdfDocument().SavePages(pdfFileStream, (x, i) => pageCount++);
            }

            Assert.Equal(expectedPageCount, pageCount);
        }

        [Fact]
        public void ToImages_ReturnsPngImages()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            const string ResourceName = "ShipWorks.Tests.Resources.MultiPagePdf.pdf";

            using (Stream pdfFileStream = assembly.GetManifestResourceStream(ResourceName))
            {
                new PdfDocument().SavePages(pdfFileStream, (x, i) =>
                {
                    using (Bitmap bitmap = new Bitmap(x))
                    {
                        Assert.Equal(ImageFormat.Png, bitmap.RawFormat);
                    }

                    return true;
                });
            }
        }
    }
}
