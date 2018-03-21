using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Win32.Native;
using Moq;
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

        [Fact]
        public void PrintAsync_SendsTelemetryEvent_WhenNotCancelled()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var trackedEventFunc = mock.MockFunc<string, ITrackedEvent>();

            TestTelemetry(new[] { profile.Object }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            trackedEventFunc.Verify(f => f("Shortcuts.Print"), Times.Once);
        }
        
        [Fact]
        public void PrintAsync_SendsNoTelemetryEvent_WhenCancelled()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");
            var trackedEventFunc = mock.MockFunc<string, ITrackedEvent>();

            TestTelemetry(new[] { profile.Object }, new PrintActionCompletedEventArgs(PrintAction.Print, null, true, null));

            trackedEventFunc.Verify(f => f(AnyString), Times.Never);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty(AnyString, AnyString), Times.Never);
        }

        [Fact]
        public void PrintAsync_SendsCorrectBarcodeCount()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.Shortcut).Returns(new ShortcutEntity()
            {
                Barcode = "blah"
            });

            TestTelemetry(Enumerable.Repeat(profile.Object, 25), new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Barcodes.Count", "25"), Times.Once);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Hotkeys.Count", "0"), Times.Once);
        }

        [Fact]
        public void PrintAsync_SendsCorrectHotkeyCount()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.Shortcut).Returns(new ShortcutEntity()
            {
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N1
            });

            TestTelemetry(Enumerable.Repeat(profile.Object, 25), new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Barcodes.Count", "0"), Times.Once);
            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Hotkeys.Count", "25"), Times.Once);
        }
        
        [Fact]
        public void PrintAsync_SendsTelemetrySuccessEvent_WhenSuccesfull()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");

            TestTelemetry(new[] { profile.Object }, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Result", "Success"), Times.Once);
        }

        [Fact]
        public void PrintAsync_SendsTelemetryFailedEvent_WhenUnsuccessfull()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");

            TestTelemetry(new[] { profile.Object }, new PrintActionCompletedEventArgs(PrintAction.Print, new Exception(), false, null));

            mock.Mock<ITrackedEvent>().Verify(t => t.AddProperty("Shortcuts.Print.Result", "Failed"), Times.Once);
        }

        [Fact]
        public void PrintAsync_PrintCompletedIsInvoked()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");

            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PrintAsync())
                .Raises(p => p.PrintCompleted += null, new PrintActionCompletedEventArgs(PrintAction.Print, null, false, null));

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            var eventWasDispatched = false;
            testObject.PrintCompleted += (sender, args) => eventWasDispatched = true;

            testObject.PrintAsync();

            Assert.True(eventWasDispatched);
        }

        [Fact]
        public void PrintAsync_PreviewCompletedIsInvoked()
        {
            var profile = mock.Mock<IShippingProfile>();
            profile.SetupGet(s => s.ShortcutKey).Returns("abcd");

            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PreviewAsync(It.IsAny<Form>()))
                .Raises(p => p.PreviewCompleted += null, new PrintActionCompletedEventArgs(PrintAction.Preview, null, false, null));

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), new[] { profile.Object }));

            var eventWasDispatched = false;
            testObject.PreviewCompleted += (sender, args) => eventWasDispatched = true;

            testObject.PreviewAsync(new Form());

            Assert.True(eventWasDispatched);
        }

        private void TestTelemetry(IEnumerable<IShippingProfile> profiles,
            PrintActionCompletedEventArgs printActionCompletedEventArgs)
        {
            var printJobFactory = mock.Mock<IPrintJobFactory>();
            var printJob = mock.Mock<IPrintJob>();
            printJob.Setup(p => p.PrintAsync())
                .Raises(p => p.PrintCompleted += null, printActionCompletedEventArgs);

            printJobFactory.Setup(p => p.CreatePrintJob(It.IsAny<IList<TemplateResult>>())).Returns(printJob);

            var testObject = mock.Create<BarcodePrintJob>(new TypedParameter(typeof(IEnumerable<IShippingProfile>), profiles));

            testObject.PrintAsync();
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}