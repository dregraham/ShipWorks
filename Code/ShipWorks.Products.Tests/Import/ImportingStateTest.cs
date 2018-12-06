using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Products.Import;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Import
{
    public class ImportingStateTest : IDisposable
    {
        private readonly AutoMock mock;

        public ImportingStateTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_CreatesProgressReporter()
        {
            var testObject = mock.Create<ImportingState>();

            testObject.StopImport.Execute(null);

            mock.Mock<IProgressFactory>().Verify(x => x.CreateReporter("Importing products..."));
        }

        [Fact]
        public void ShouldReloadProducts_ReturnsTrue()
        {
            var testObject = mock.Create<ImportingState>();

            Assert.True(testObject.ShouldReloadProducts);
        }

        [Theory]
        [InlineData(DialogResult.OK, false)]
        [InlineData(DialogResult.Cancel, true)]
        public void CloseRequested_SetsCancel_BasedOnQuestionResult(DialogResult result, bool expectedCancel)
        {
            mock.Mock<IMessageHelper>().Setup(x => x.ShowQuestion(AnyString)).Returns(result);
            var args = new CancelEventArgs();
            var testObject = mock.Create<ImportingState>();

            testObject.CloseRequested(args);

            Assert.Equal(expectedCancel, args.Cancel);
        }

        [Fact]
        public void StopImport_DelegatesToProgressReporter()
        {
            var reporter = mock.FromFactory<IProgressFactory>()
                .Mock(x => x.CreateReporter(AnyString));

            var testObject = mock.Create<ImportingState>();

            testObject.StopImport.Execute(null);

            reporter.Verify(x => x.Cancel());
        }

        [Fact]
        public void PercentComplete_IsNotUpdated_WhenImportIsNotStarted()
        {
            var reporter = mock.FromFactory<IProgressFactory>()
                .Mock(x => x.CreateReporter(AnyString));

            var testObject = mock.Create<ImportingState>();

            reporter.SetupGet(x => x.PercentComplete).Returns(45);
            reporter.Raise(x => x.Changed += null, EventArgs.Empty);

            Assert.NotEqual(45, testObject.PercentComplete);
        }

        [Fact]
        public void PercentComplete_IsUpdated_AfterImportIsStarted()
        {
            var reporter = mock.FromFactory<IProgressFactory>()
                .Mock(x => x.CreateReporter(AnyString));

            var task = new Task<GenericResult<IImportProductsResult>>(() => GenericResult.FromSuccess<IImportProductsResult>(null));

            mock.Mock<IProductImporter>()
                .Setup(x => x.ImportProducts(AnyString, It.IsAny<IProgressReporter>()))
                .Returns(task);

            var testObject = mock.Create<ImportingState>();
            testObject.StartImport(string.Empty).Forget();

            reporter.SetupGet(x => x.PercentComplete).Returns(45);
            reporter.Raise(x => x.Changed += null, EventArgs.Empty);

            Assert.Equal(45, testObject.PercentComplete);
        }

        [Fact]
        public async Task PercentComplete_IsNotUpdated_AfterImportFinishes()
        {
            var reporter = mock.FromFactory<IProgressFactory>()
                .Mock(x => x.CreateReporter(AnyString));

            mock.Mock<IProductImporter>()
                .Setup(x => x.ImportProducts(AnyString, It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromSuccess(mock.Build<IImportProductsResult>()));

            var testObject = mock.Create<ImportingState>();
            await testObject.StartImport(string.Empty);

            reporter.SetupGet(x => x.PercentComplete).Returns(45);
            reporter.Raise(x => x.Changed += null, EventArgs.Empty);

            Assert.NotEqual(45, testObject.PercentComplete);
        }

        [Fact]
        public async Task StartImport_DelegatesToProductImporter()
        {
            var reporter = mock.FromFactory<IProgressFactory>()
                .Mock(x => x.CreateReporter(AnyString));

            var testObject = mock.Create<ImportingState>();
            await testObject.StartImport("foo");

            mock.Mock<IProductImporter>()
                .Verify(x => x.ImportProducts("foo", reporter.Object));
        }

        [Fact]
        public async Task StartImport_ChangesStateToSucces_WhenImportSucceeds()
        {
            mock.Mock<IProductImporter>()
                .Setup(x => x.ImportProducts(AnyString, It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromSuccess(mock.Build<IImportProductsResult>()));

            var testObject = mock.Create<ImportingState>();
            await testObject.StartImport("foo");

            mock.Mock<IProductImporterStateManager>()
                .Verify(x => x.ChangeState(It.IsAny<ImportSucceededState>()));
        }

        [Fact]
        public async Task StartImport_ChangesStateToFailure_WhenImportFails()
        {
            mock.Mock<IProductImporter>()
                .Setup(x => x.ImportProducts(AnyString, It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromError<IImportProductsResult>("Foo"));

            var testObject = mock.Create<ImportingState>();
            await testObject.StartImport("foo");

            mock.Mock<IProductImporterStateManager>()
                .Verify(x => x.ChangeState(It.IsAny<ImportFailedState>()));
        }

        [Fact]
        public async Task StartImport_ChangesStateToFailure_WhenImportThrows()
        {
            mock.Mock<IProductImporter>()
                .Setup(x => x.ImportProducts(AnyString, It.IsAny<IProgressReporter>()))
                .ThrowsAsync(new Exception("Foo"));

            var testObject = mock.Create<ImportingState>();
            await testObject.StartImport("foo");

            mock.Mock<IProductImporterStateManager>()
                .Verify(x => x.ChangeState(It.IsAny<ImportFailedState>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
