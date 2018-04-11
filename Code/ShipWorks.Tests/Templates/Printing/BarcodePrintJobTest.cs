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

            mock.Mock<IPrintJobFactory>().Verify(f => f.CreatePrintJob(It.Is<List<TemplateResult>>(t => t.First().ReadResult() == "<html><head><title></title><style>body {font-family:Arial; text-align:center;}table {margin-bottom:40px;} td {text-align:center;} .barcode {font-family:'Free 3 of 9 Extended';font-size:36pt;} </style></head><body><h1>Some Test Data</h1><br/>\r\n<div>\r\n<b>shortcut name</b><br/>\r\n<span class='barcode'>*shortcut barcode*</span><br/>\r\nshortcut keyboard\r\n</div>\r\n\r\n<br/>\r\n</body></html>")));
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
        public void PrintAsync_SendsTelemetryEvent_WhenNotCancelled()
        {
            var trackedEventFunc = mock.MockFunc<string, ITrackedEvent>();

            BarcodePage page = new BarcodePage("Some Test Data", new[] { new PrintableBarcode("shortcut name", "shortcut barcode", "abcd") });

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            trackedEventFunc.Verify(f => f("Shortcuts.Print"), Times.Once);
        }
        
        [Fact]
        public void PrintAsync_SendsNoTelemetryEvent_WhenCancelled()
        {
            var trackedEventFunc = mock.MockFunc<string, ITrackedEvent>();

            BarcodePage page = new BarcodePage("Some Test Data", new[] { new PrintableBarcode("shortcut name", "shortcut barcode", "abcd") });

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, null, true, null));

            trackedEventFunc.Verify(f => f(AnyString), Times.Never);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty(AnyString, AnyString), Times.Never);
        }

        [Fact]
        public void PrintAsync_SendsCorrectBarcodeCount()
        {
            BarcodePage page = new BarcodePage("Some Test Data", Enumerable.Repeat(new PrintableBarcode("shortcut name", "", "abcd"), 25));

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Barcodes.Count", "0"), Times.Once);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Hotkeys.Count", "25"), Times.Once);
        }

        [Fact]
        public void PrintAsync_SendsCorrectHotkeyCount()
        {
            BarcodePage page = new BarcodePage("Some Test Data", Enumerable.Repeat(new PrintableBarcode("shortcut name", "", "abcd"), 25));

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Barcodes.Count", "0"), Times.Once);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Hotkeys.Count", "25"), Times.Once);
        }
        
        [Fact]
        public void PrintAsync_SendsTelemetrySuccessEvent_WhenSuccesfull()
        {
            BarcodePage page = new BarcodePage("Some Test Data", new[] { new PrintableBarcode("shortcut name", "", "abcd") });

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Result", "Success"), Times.Once);
        }

        [Fact]
        public void PrintAsync_SendsTelemetryFailedEvent_WhenUnsuccessfull()
        {
            BarcodePage page = new BarcodePage("Some Test Data", new[] { new PrintableBarcode("shortcut name", "", "abcd") });

            TestTelemetry(new[] { page }, new PrintActionCompletedEventArgs(PrintAction.Print, new Exception(), false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Result", "Failed"), Times.Once);
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

        private void TestTelemetry(IEnumerable<BarcodePage> barcodePages,
            PrintActionCompletedEventArgs printActionCompletedEventArgs)
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PrintAsync())
                .Raises(p => p.PrintCompleted += null, printActionCompletedEventArgs);

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<BarcodePage>), barcodePages));

            testObject.PrintAsync();
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}