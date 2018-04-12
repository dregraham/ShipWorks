using ShipWorks.Templates.Printing;
using Xunit;

namespace ShipWorks.Tests.Templates.Printing
{
    public class PrintableBarcodeTest
    {
        [Fact]
        public void GetHtmlBlock_ReturnsEmptyString_WhenBarcodeAndHotkeyAreEmpty()
        {
            Assert.Equal(string.Empty, new PrintableBarcode().GetHtmlBlock());
        }

        [Fact]
        public void GetHtmlBlock_ReturnsHtmlWithoutName_WhenNameIsEmpty()
        {
            var testObject = new PrintableBarcode("", "barcode", "hotkey");
            Assert.DoesNotContain("<b></b><br/>", testObject.GetHtmlBlock());
        }

        [Fact]
        public void GetHtmlBlock_ReturnsHtmlWithoutBarcode_WhenBarcodeIsEmpty()
        {
            var testObject = new PrintableBarcode("name", "", "hotkey");
            Assert.DoesNotContain("<span class='barcode'>**</span><br/>", testObject.GetHtmlBlock());
        }
        
        [Fact]
        public void GetHtmlBlock_ReturnsHtmlWithNameBarcodeAndHotkey()
        {
            var testObject = new PrintableBarcode("name", "barcode", "hotkey");
            Assert.Contains("name", testObject.GetHtmlBlock());
            Assert.Contains("barcode", testObject.GetHtmlBlock());
            Assert.Contains("hotkey", testObject.GetHtmlBlock());
        }
    }
}
