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
        public void Ctor_DelegatesToPrintJobFactoryForPrintJob()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == "<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:40px;} td {text-align:center;} .barcode {font-family:Free 3 of 9 Extended;font-size:36pt;}</style></head><body><table><tr><td>  </td></tr><tr><td class='barcode'></td></tr><tr><td>abcd</td></tr></table>\r\n</body></html>")));
        }

        [Fact]
        public void PreviewAsync_DelegatesToInternalPrintJobPreviewAsync()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            var form = new Form();

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            testObject.PreviewAsync(form);

            printJob.Verify(p => p.PreviewAsync(form));
        }

        [Fact]
        public void PrintAsync_DelegatesToInternalPrintJobPrintAsync()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            testObject.PrintAsync();

            printJob.Verify(p => p.PrintAsync());
        }

        [Fact]
        public void Ctor_SkipsProfilesThatDoNotHaveBarcodesOrShortcuts()
        {
            var profile = mock.Mock<IShippingProfile>();
            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == "<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:40px;} td {text-align:center;} .barcode {font-family:Free 3 of 9 Extended;font-size:36pt;}</style></head><body></body></html>")));
        }
    }
}