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

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == "<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:50px;} td {text-align:center;} .barcode {font-family:Free 3 of 9 Extended;font-size:32pt;}</style></head><body><table><tr><td>  </td></tr><tr><td class='barcode'></td></tr><tr><td>abcd</td></tr></table>\r\n</body></html>")));

        }
    }
}
