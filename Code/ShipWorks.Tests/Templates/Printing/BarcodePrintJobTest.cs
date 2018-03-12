using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Templates.Printing
{
    public class BarcodePrintJobTest
    {
        private readonly AutoMock mock;

        public BarcodePrintJobTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void PreviewAsync_DelegatesToPrintJobFactoryForPrintJob()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            testObject.PreviewAsync(new Form());

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == "<html><head><title></title></head><body>Barcode:abcd\r\n</body></html>")));

        }
    }
}
