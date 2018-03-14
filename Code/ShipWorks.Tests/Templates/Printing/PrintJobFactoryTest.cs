using System.Collections.Generic;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using Xunit;

namespace ShipWorks.Tests.Templates.Printing
{
    public class PrintJobFactoryTest
    {
        [Fact]
        public void CreateBarcodePrintJob_ReturnsBarcodePrintJob()
        {
            var testObject = new PrintJobFactory();

            IPrintJob result = testObject.CreateBarcodePrintJob(new List<IShippingProfile>());

            Assert.IsType(typeof(BarcodePrintJob), result);
        }

        [Fact]
        public void CreateBarcodePrintJob_ReturnsPrintJob()
        {
            var testObject = new PrintJobFactory();

            IPrintJob result = testObject.CreatePrintJob(new List<TemplateResult>());

            Assert.IsType(typeof(PrintJob), result);
        }
    }
}
