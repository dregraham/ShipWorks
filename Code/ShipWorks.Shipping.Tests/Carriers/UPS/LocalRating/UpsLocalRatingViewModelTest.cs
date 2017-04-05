using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Common;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRatingViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        public UpsLocalRatingViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Load_CorrectlySetsLocalRatingEnabled(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity()
            {
                LocalRatingEnabled = localRatingEnabled
            };

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);

            Assert.Equal(localRatingEnabled, testObject.LocalRatingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Save_CorrectlySetsLocalRatingEnabled(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            testObject.LocalRatingEnabled = localRatingEnabled;

            testObject.Save(upsAccount);

            Assert.Equal(localRatingEnabled, upsAccount.LocalRatingEnabled);
        }

        [Fact]
        public void DownloadSampleFileAccount_ResourceStreamNotAccessed_WhenFileDialogDoesNotReturnOK()
        {
            var dialog = MockSaveDialog(DialogResult.Cancel);
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            testObject.DownloadSampleFileCommand.Execute(null);

            dialog.Verify(d => d.CreateFileStream(), Times.Never);
        }

        [Fact]
        public void DownloadSampleFileAccount_ResourceStreamAccessed_WhenFileDialogReturnsOK()
        {
            using (var resultStream = new MemoryStream())
            {
                var saveDialog = MockSaveDialog(DialogResult.OK, resultStream);
                saveDialog.SetupGet(x => x.SelectedFileName).Returns("blah");

                var testObject = mock.Create<UpsLocalRatingViewModel>();

                testObject.DownloadSampleFileCommand.Execute(null);

                mock.Mock<IProcess>().Verify(p => p.Start("blah"), Times.Once);
            }
        }

        [Fact]
        public void DownloadSampleFileAccount_FileSaved_WhenFileDialogReturnsOK()
        {
            using (var resultStream = new MemoryStream())
            {
                // Set up a stream that, when written to, writes to the result stream.
                var mockedStream = mock.Mock<Stream>();
                mockedStream
                    .Setup(s => s.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Callback((byte[] buffer, int offset, int count) => resultStream.Write(buffer, offset, count));
                mockedStream.SetupGet(s => s.CanWrite).Returns(true);

                // Setup the dialog so that it returns this mocked stream and execute the download command
                using (var createFileStream = mockedStream.Object)
                {
                    var saveDialog = MockSaveDialog(DialogResult.OK, createFileStream);
                    saveDialog.SetupGet(x => x.SelectedFileName).Returns("blah");

                    var testObject = mock.Create<UpsLocalRatingViewModel>();

                    testObject.DownloadSampleFileCommand.Execute(null);
                }

                // Convert the result stream to a string
                var resultBytes = resultStream.ToArray();
                byte[] sampleFileBytes;

                // Get the xslt file and convert it to a string
                Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
                using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleFileResourceName))
                {
                    sampleFileBytes = new byte[resourceStream.Length];
                    resourceStream.Read(sampleFileBytes, 0, (int) resourceStream.Length);
                }

                // Verify that the contents of the embedded resource is identical to the file we
                // will save to disk.
                Assert.Equal(sampleFileBytes, resultBytes);
            }
        }

        private Mock<IFileDialog> MockSaveDialog(DialogResult result, Stream stream = null)
        {
            var fileDialogMock = mock.MockRepository.Create<IFileDialog>();
            var dialogIndex = mock.MockRepository.Create<IIndex<FileDialogType, IFileDialog>>();

            fileDialogMock.Setup(d => d.ShowDialog()).Returns(result);
            fileDialogMock.Setup(d => d.CreateFileStream()).Returns(stream);

            dialogIndex.Setup(i => i[FileDialogType.Save]).Returns(fileDialogMock.Object);

            mock.Provide(dialogIndex.Object);

            return fileDialogMock;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}