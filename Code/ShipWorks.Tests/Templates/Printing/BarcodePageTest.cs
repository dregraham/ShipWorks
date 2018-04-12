using System.Collections.Generic;
using ShipWorks.Templates.Printing;
using Xunit;

namespace ShipWorks.Tests.Templates.Printing
{
    public class BarcodePageTest
    {
        [Fact]
        public void GetTemplateResult_ReturnsTemplateResultWithTitle_WhenPageHasBarcodes()
        {
            var testObject = new BarcodePage("Title", new[] { new PrintableBarcode("name","barcode","hotkey") } );
            Assert.Contains("Title", testObject.GetTemplateResult().ReadResult());
        }

        [Fact]
        public void GetTemplateResult_ReturnsTemplateResultWithoutTitle_WhenPageHasNoBarcodes()
        {
            var testObject = new BarcodePage("Title", new List<PrintableBarcode>());
            Assert.Equal("<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:40px;} td {text-align:center;} .barcode {font-family:'Free 3 of 9 Extended';font-size:36pt;} </style></head><body></body></html>", testObject.GetTemplateResult().ReadResult());
        }
    }
}
