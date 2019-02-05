using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Processing;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Templates.Printing
{
    public class BarcodePrintJobTest : IDisposable
    {
        private readonly AutoMock mock;

        public BarcodePrintJobTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Ctor_DelegatesToPrintJobFactoryForPrintJob()
        {
            BarcodePage page = new BarcodePage("Some Test Data", new[] { new PrintableBarcode("shortcut name", "shortcut barcode", "shortcut keyboard") });

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), new[] { page }));

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == 
                "<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:40px;} td {text-align:center;} .barcode {font-family:'Free 3 of 9 Extended';font-size:36pt;} </style></head><body><h1>Some Test Data</h1><div><b>shortcut name</b><br/><span class='barcode'>*shortcut barcode*</span><br/>shortcut keyboard</div></body></html>")));
        }

        [Fact]
        public void PreviewAsync_DelegatesToInternalPrintJobPreviewAsync()
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            var form = new Form();

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), new List<BarcodePage>()));

            testObject.PreviewAsync(form);

            printJob.Verify(p => p.PreviewAsync(form));
        }

        [Fact]
        public void PrintAsync_DelegatesToInternalPrintJobPrintAsync()
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), new List<BarcodePage>()));

            testObject.PrintAsync();

            printJob.Verify(p => p.PrintAsync());
        }

        [Fact]
        public void PrintAsync_PrintCompletedIsInvoked()
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PrintAsync())
                .Raises(p => p.PrintCompleted += null, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), new List<BarcodePage>()));

            var eventWasDispatched = false;
            testObject.PrintCompleted += (sender, args) => eventWasDispatched = true;

            testObject.PrintAsync();

            Assert.True(eventWasDispatched);
        }

        [Fact]
        public void PrintAsync_PreviewCompletedIsInvoked()
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PreviewAsync(It.IsAny<Form>()))
                .Raises(p => p.PreviewCompleted += null, new PrintActionCompletedEventArgs(PrintAction.Preview, null, false, null));

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), new List<BarcodePage>()));

            var eventWasDispatched = false;
            testObject.PreviewCompleted += (sender, args) => eventWasDispatched = true;

            testObject.PreviewAsync(new Form());

            Assert.True(eventWasDispatched);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}