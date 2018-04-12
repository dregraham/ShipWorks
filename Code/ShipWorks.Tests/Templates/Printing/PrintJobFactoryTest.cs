using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Templates.Printing
{
    public class PrintJobFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public PrintJobFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateBarcodePrintJob_DelegatesToShippingProfileService()
        {
            var testObject = mock.Create<PrintJobFactory>();

            IPrintJob result = testObject.CreateBarcodePrintJob();

            mock.Mock<IShippingProfileService>().Verify(s => s.GetConfiguredShipmentTypeProfiles());
        }
        
        [Fact]
        public void CreateBarcodePrintJob_ReturnsBarcodePrintJob()
        {
            var testObject = mock.Create<PrintJobFactory>();

            IPrintJob result = testObject.CreateBarcodePrintJob(new List<IShippingProfile>());

            Assert.IsType(typeof(BarcodePrintJob), result);
        }

        [Fact]
        public void CreateBarcodePrintJob_ReturnsPrintJob()
        {
            var testObject = mock.Create<PrintJobFactory>();

            IPrintJob result = testObject.CreatePrintJob(new List<TemplateResult>());

            Assert.IsType(typeof(PrintJob), result);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
