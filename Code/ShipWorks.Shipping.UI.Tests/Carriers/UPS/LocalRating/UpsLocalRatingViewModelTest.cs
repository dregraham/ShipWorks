using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.UI.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRatingViewModelTest : IDisposable
    {
        readonly AutoMock mock;

        private const string WarningMessage =
            "Local rates is an experimental feature and for rating purposes only. It does not affect billing. Please ensure the rates uploaded match the rates on your UPS account.\n\n" +
            "Note: All previously uploaded rates will be overwritten with the new rates.";

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

        [Fact]
        public void Load_SetsStatusMessageToLastUpload_WhenRateTableIsUploaded()
        {
            var upsAccount = new UpsAccountEntity();

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);

            Assert.Equal("There is no rate table associated with the selected account", testObject.StatusMessage);
        }

        [Fact]
        public void Load_SetsStatusMessageToNoRateTable_WhenRateTableIsNotUploaded()
        {
            var uploadDate = DateTime.UtcNow;

            var upsAccount = new UpsAccountEntity();
            upsAccount.UpsRateTable = new UpsRateTableEntity(){UploadDate = uploadDate};
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);

            Assert.StartsWith($"Last Upload: {uploadDate:g}", testObject.StatusMessage);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Save_CorrectlySetsLocalRatingEnabled_WhenRateTableIsUploaded(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            upsAccount.UpsRateTable = new UpsRateTableEntity();
            testObject.Load(upsAccount);
            testObject.LocalRatingEnabled = localRatingEnabled;

            testObject.Save();

            Assert.Equal(localRatingEnabled, upsAccount.LocalRatingEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Save_SetsLocalRatingEnabledToFalse_WhenRateTableIsNotUploaded(bool localRatingEnabled)
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.LocalRatingEnabled = localRatingEnabled;

            testObject.Save();

            Assert.Equal(false, upsAccount.LocalRatingEnabled);
        }

        [Fact]
        public void Save_SetsValidationMessageCorrectly_WhenRateTableIsNotUploadedAndLocalRatingEnabledIsTrue()
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.LocalRatingEnabled = true;

            testObject.Save();

            Assert.Equal("Please upload your rate table to enable local rating", testObject.ValidationMessage);
        }

        [Fact]
        public void DownloadSampleFileAccount_ResourceStreamNotAccessed_WhenFileDialogDoesNotReturnOK()
        {
            var dialog = MockSaveDialog(DialogResult.Cancel);
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            testObject.DownloadSampleFileCommand.Execute(null);

            dialog.Verify(d => d.CreateFileStream(), Times.Never);
            dialog.Verify(d=>d.ShowFile(), Times.Never);
        }

        [Fact]
        public void DownloadSampleFileAccount_CallsShowFile_WhenFileDialogReturnsOK()
        {
            using (var resultStream = new MemoryStream())
            {
                var mockSaveDialog = MockSaveDialog(DialogResult.OK, resultStream);

                var testObject = mock.Create<UpsLocalRatingViewModel>();

                testObject.DownloadSampleFileCommand.Execute(null);

                mockSaveDialog.Verify(d=>d.ShowFile(), Times.Once);
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
                    MockSaveDialog(DialogResult.OK, createFileStream);

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

        [Fact]
        public void UploadRatingFile_DisplaysWarningMessage()
        {
            var messageHelper = mock.Mock<IMessageHelper>();

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.UploadRatingFileCommand.Execute(null);

            messageHelper.Verify(m => m.ShowWarning(WarningMessage));
        }

        [Fact]
        public void UploadRatingFile_LoadsRateTableWithFileStream()
        {
            var rateTable = mock.Mock<IUpsLocalRateTable>();
            
            Stream fileStream = mock.Create<Stream>();
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.CreateFileStream()).Returns(fileStream);
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f=> f()).Returns(openFileDialog);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.UploadRatingFileCommand.Execute(null);

            rateTable.Verify(t=> t.Load(fileStream));
        }

        [Fact]
        public void UploadRatingFile_SavesRateTableToAccount()
        {
            var upsAccount = new UpsAccountEntity();
            var rateTable = mock.Mock<IUpsLocalRateTable>();

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.UploadRatingFileCommand.Execute(null);

            rateTable.Verify(t => t.Save(upsAccount));
        }

        [Fact]
        public void UploadRatingFile_SetsStatusMessageToLastUpload()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity
            {
                UpsRateTable = new UpsRateTableEntity {UploadDate = uploadDate}
            };
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.UploadRatingFileCommand.Execute(null);

            Assert.Equal($"Last Upload: {uploadDate:g}", testObject.StatusMessage);
        }

        [Fact]
        public void UploadRatingFile_SetsValidationMessageToSuccess()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity
            {
                UpsRateTable = new UpsRateTableEntity { UploadDate = uploadDate }
            };
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.UploadRatingFileCommand.Execute(null);

            Assert.Equal("Local rates have been uploaded successfully", testObject.ValidationMessage);
        }

        [Fact]
        public void UploadRatingFile_LogsSuccess()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity
            {
                UpsRateTable = new UpsRateTableEntity { UploadDate = uploadDate }
            };
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount);
            testObject.UploadRatingFileCommand.Execute(null);

            log.Verify(l=> l.Info("Successfully uploaded rate table"));
        }

        [Fact]
        public void UploadRatingFile_SetsValidationMessageToError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            // Not calling load here will cause exception
            testObject.UploadRatingFileCommand.Execute(null);

            Assert.StartsWith("Local rates failed to upload:", testObject.ValidationMessage);
        }

        [Fact]
        public void UploadRatingFile_LogsError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            // Not calling load here will cause exception
            testObject.UploadRatingFileCommand.Execute(null);

            log.Verify(l => l.Error(It.IsAny<string>()));
        }
        
        private Mock<ISaveFileDialog> MockSaveDialog(DialogResult result, Stream stream = null)
        {
            var fileDialogMock = mock.MockRepository.Create<ISaveFileDialog>();
            mock.MockFunc(fileDialogMock);

            fileDialogMock.Setup(d => d.ShowDialog()).Returns(result);
            fileDialogMock.Setup(d => d.CreateFileStream()).Returns(stream);
            
            return fileDialogMock;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}