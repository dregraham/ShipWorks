using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModelTest"/> class.
        /// </summary>
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
            testObject.Load(upsAccount, b => { });

            Assert.Equal(localRatingEnabled, testObject.LocalRatingEnabled);
        }

        [Fact]
        public void Load_SetsStatusMessageToLastUpload_WhenRateTableIsUploaded()
        {
            var upsAccount = new UpsAccountEntity();

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });

            Assert.Equal(UpsLocalRatingViewModel.NoRatesUploadedMessage, testObject.RateStatusMessage);
        }

        [Fact]
        public void Load_SetsStatusMessageToNoRateTable_WhenRateTableIsNotUploaded()
        {
            var uploadDate = DateTime.UtcNow;
            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.RateUploadDate).Returns(uploadDate);
            
            var upsAccount = new UpsAccountEntity();
            upsAccount.UpsRateTableID = 42;

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });

            Assert.StartsWith($"Last Upload: {uploadDate.ToLocalTime():g}", testObject.RateStatusMessage);
        }

        [Fact]
        public void Load_SetsUploadMessageToExceptionMessage_WhenUpsLocalRatingExceptionIsThrown()
        {
            var upsAccount = new UpsAccountEntity();

            mock.Mock<IUpsLocalRateTable>().Setup(t => t.Load(upsAccount)).Throws(new UpsLocalRatingException("Error"));

            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });

            Assert.Equal("Error", testObject.UploadMessage);
        }

        [Theory]
        [InlineData(false, false, true, false)]
        [InlineData(false, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, true, true, true)]
        public void Save_CorrectlySetsLocalRatingEnabled(bool ratesUploaded, bool zonesUploaded, bool localRatingEnabled, bool expectedResult)
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            if (ratesUploaded)
            {
                upsAccount.UpsRateTable = new UpsRateTableEntity(10);
                mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.RateUploadDate).Returns(DateTime.Now);
            }

            if (zonesUploaded)
            {
                mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.ZoneUploadDate).Returns(DateTime.Now);
            }

            testObject.Load(upsAccount, b=> { });
            testObject.LocalRatingEnabled = localRatingEnabled;

            testObject.Save();

            Assert.Equal(expectedResult, upsAccount.LocalRatingEnabled);
        }

        [Fact]
        public void Save_SetsValidationMessageCorrectly_WhenRateTableIsNotUploadedAndLocalRatingEnabledIsTrue()
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            testObject.LocalRatingEnabled = true;

            testObject.Save();

            Assert.Equal("Please upload your rate table to enable local rating", testObject.UploadMessage);
        }
        
        [Fact]
        public void Save_SetsValidationMessageCorrectly_WhenZonesNotUploadedAndLocalRatingEnabledIsTrue()
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });
            testObject.LocalRatingEnabled = true;

            testObject.Save();

            Assert.Equal("Please upload your rate table to enable local rating", testObject.UploadMessage);
        }

        [Fact]
        public void Save_ReturnsFalse_WhenValidatingRatesIsTrue()
        {
            var upsAccount = new UpsAccountEntity();
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            testObject.LocalRatingEnabled = false;
            testObject.IsUploading = true;

            Assert.False(testObject.Save());
        }

        [Fact]
        public void DownloadFile_DisplaysError_WhenExceptionOccursSavingFile()
        {

            var messageHelper = mock.Mock<IMessageHelper>();
            var dialog = MockSaveDialog(DialogResult.OK);
            var testObject = mock.Create<UpsLocalRatingViewModel>();
            dialog.Setup(d => d.CreateFileStream()).Throws(new ShipWorksSaveFileDialogException("Error", It.IsAny<Exception>()));

            testObject.DownloadSampleRateFileCommand.Execute(null);

            messageHelper.Verify(m => m.ShowError("Error"));
        }

        [Fact]
        public void DownloadFile_ResourceStreamNotAccessed_WhenFileDialogDoesNotReturnOK()
        {
            var dialog = MockSaveDialog(DialogResult.Cancel);
            var testObject = mock.Create<UpsLocalRatingViewModel>();

            testObject.DownloadSampleRateFileCommand.Execute(null);

            dialog.Verify(d => d.CreateFileStream(), Times.Never);
            dialog.Verify(d=>d.ShowFile(), Times.Never);
        }

        [Fact]
        public void DownloadFile_CallsShowFile_WhenFileDialogReturnsOK()
        {
            using (var resultStream = new MemoryStream())
            {
                var mockSaveDialog = MockSaveDialog(DialogResult.OK, resultStream);

                var testObject = mock.Create<UpsLocalRatingViewModel>();

                testObject.DownloadSampleRateFileCommand.Execute(null);

                mockSaveDialog.Verify(d=>d.ShowFile(), Times.Once);
            }
        }

        [Fact]
        public void DownloadRateFile_FileSaved_WhenFileDialogReturnsOK()
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

                    testObject.DownloadSampleRateFileCommand.Execute(null);
                }

                // Convert the result stream to a string
                var resultBytes = resultStream.ToArray();
                byte[] sampleFileBytes;

                // Get the xslt file and convert it to a string
                Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
                using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleRatesFileResourceName))
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
        public void DownloadZoneFile_FileSaved_WhenFileDialogReturnsOK()
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

                    testObject.DownloadSampleZoneFileCommand.Execute(null);
                }

                // Convert the result stream to a string
                var resultBytes = resultStream.ToArray();
                byte[] sampleFileBytes;

                // Get the xslt file and convert it to a string
                Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLocalRatingViewModel));
                using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(UpsLocalRatingViewModel.SampleZoneFileResourceName))
                {
                    sampleFileBytes = new byte[resourceStream.Length];
                    resourceStream.Read(sampleFileBytes, 0, (int)resourceStream.Length);
                }

                // Verify that the contents of the embedded resource is identical to the file we
                // will save to disk.
                Assert.Equal(sampleFileBytes, resultBytes);
            }
        }

        [Fact]
        public async Task UploadRatingFile_DisplaysWarningMessage()
        {
            var messageHelper = mock.Mock<IMessageHelper>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            await testObject.CallUploadRatingFile();

            messageHelper.Verify(m => m.ShowWarning(WarningMessage));
        }

        [Fact]
        public async void UploadRatingFile_LoadsRateTableWithFileStream()
        {
            var rateTable = mock.Mock<IUpsLocalRateTable>();
            
            Stream fileStream = mock.Create<Stream>();
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.CreateFileStream()).Returns(fileStream);
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f=> f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b=> { });
            await testObject.CallUploadRatingFile();

            rateTable.Verify(t=> t.LoadRates(fileStream));
        }

        [Fact]
        public async void UploadRatingFile_SavesRateTableToAccount()
        {
            var upsAccount = new UpsAccountEntity();
            var rateTable = mock.Mock<IUpsLocalRateTable>();

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            await testObject.CallUploadRatingFile();

            rateTable.Verify(t => t.SaveRates(upsAccount));
        }

        [Fact]
        public async void UploadRatingFile_SetsStatusMessageToLastUpload()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity
            {
                UpsRateTable = new UpsRateTableEntity(42) {UploadDate = uploadDate}
            };

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.RateUploadDate).Returns(uploadDate);

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            await testObject.CallUploadRatingFile();

            Assert.Equal($"Last Upload: {uploadDate.ToLocalTime():g}", testObject.RateStatusMessage);
        }

        [Fact]
        public async void UploadRatingFile_SetsValidationMessageToSuccess()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity(42)
            {
                UpsRateTable = new UpsRateTableEntity { UploadDate = uploadDate }
            };
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            await testObject.CallUploadRatingFile();

            Assert.Equal("Local rates have been uploaded successfully", testObject.UploadMessage);
        }

        [Fact]
        public async Task UploadRatingFile_LogsSuccess()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity
            {
                UpsRateTable = new UpsRateTableEntity(42) { UploadDate = uploadDate }
            };
            var openFileDialog = mock.CreateMock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.RateUploadDate).Returns(DateTime.Now);
            
            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b=> { });
            await testObject.CallUploadRatingFile();

            log.Verify(l=> l.Info("Successfully uploaded rate table"));
        }

        [Fact]
        public async void UploadRatingFile_SetsValidationMessageToError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveRates(It.IsAny<UpsAccountEntity>()))
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => { });
            await testObject.CallUploadRatingFile();

            Assert.StartsWith("Local rates failed to upload:", testObject.UploadMessage);
        }

        [Fact]
        public async void UploadRatingFile_LogsError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveRates(It.IsAny<UpsAccountEntity>()))
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b=> { });

            await testObject.CallUploadRatingFile();

            log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<UpsLocalRatingException>()));
        }

        [Fact]
        public async void UploadRatingFile_SetsValidatingRatesToFalse_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => { });
            await testObject.CallUploadRatingFile();

            Assert.False(testObject.IsUploading);
        }

        [Fact]
        public async void UploadRatingFile_CallsIsBusyWithFalse_WhenExceptionOccurs()
        {
            List<bool> isBusyValues = new List<bool>();

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => isBusyValues.Add(b));
            await testObject.CallUploadRatingFile();

            Assert.Equal(2, isBusyValues.Count);
            Assert.True(isBusyValues[0]);
            Assert.False(isBusyValues[1]);
        }

        [Fact]
        public async Task UploadRatingFile_CallsIsBusyWithFalse_WhenSuccess()
        {
            List<bool> isBusyValues = new List<bool>();

            var upsAccount = new UpsAccountEntity();
            var openFileDialog = mock.CreateMock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.RateUploadDate).Returns(DateTime.Now);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => isBusyValues.Add(b));
            await testObject.CallUploadRatingFile();

            Assert.Equal(2, isBusyValues.Count);
            Assert.True(isBusyValues[0]);
            Assert.False(isBusyValues[1]);
        }

        [Fact]
        public async Task UploadRatingFile_SetsErrorValidatingRatesToTrue_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveRates(It.IsAny<UpsAccountEntity>()))
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b=> { });
            await testObject.CallUploadRatingFile();

            Assert.True(testObject.ErrorUploading);
        }

        [Fact]
        public async Task UploadZoneFile_LoadsZonesFromFileStream()
        {
            var rateTable = mock.Mock<IUpsLocalRateTable>();

            Stream fileStream = mock.Create<Stream>();
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.CreateFileStream()).Returns(fileStream);
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b => { });
            await testObject.CallUploadZoneFile();

            rateTable.Verify(t => t.LoadZones(fileStream));
        }

        [Fact]
        public async void UploadZoneFile_SavesZones()
        {
            var upsAccount = new UpsAccountEntity();
            var rateTable = mock.Mock<IUpsLocalRateTable>();

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });
            await testObject.CallUploadZoneFile();

            rateTable.Verify(t => t.SaveZones());
        }

        [Fact]
        public async void UploadZoneFile_SetsStatusMessageToLastUpload()
        {
            var uploadDate = DateTime.UtcNow;
            var upsAccount = new UpsAccountEntity();

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.ZoneUploadDate).Returns(uploadDate);

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });
            await testObject.CallUploadZoneFile();

            Assert.Equal($"Last Upload: {uploadDate.ToLocalTime():g}", testObject.ZoneStatusMessage);
        }

        [Fact]
        public async void UploadZoneFile_SetsValidationMessageToSuccess()
        {
            var upsAccount = new UpsAccountEntity(42);
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.ZoneUploadDate).Returns(DateTime.Now);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });
            await testObject.CallUploadZoneFile();

            Assert.Equal("Zones have been uploaded successfully", testObject.UploadMessage);
        }

        [Fact]
        public async Task UploadZoneFile_LogsSuccess()
        {
            var upsAccount = new UpsAccountEntity();
            var openFileDialog = mock.CreateMock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.ZoneUploadDate).Returns(DateTime.Now);

            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => { });
            await testObject.CallUploadZoneFile();

            log.Verify(l => l.Info("Successfully uploaded zone file"));
        }

        [Fact]
        public async void UploadZoneFile_SetsValidationMessageToError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveZones())
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => { });
            await testObject.CallUploadZoneFile();

            Assert.StartsWith("Zones failed to upload:", testObject.UploadMessage);
        }

        [Fact]
        public async void UploadZoneFile_LogsError_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            var log = mock.Mock<ILog>();
            var logFactory = mock.MockRepository.Create<Func<ILog>>();
            logFactory.Setup(f => f()).Returns(log);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveZones())
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b => { });

            await testObject.CallUploadZoneFile();

            log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<UpsLocalRatingException>()));
        }

        [Fact]
        public async void UploadZoneFile_SetsValidatingRatesToFalse_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => { });
            await testObject.CallUploadZoneFile();

            Assert.False(testObject.IsUploading);
        }

        [Fact]
        public async void UploadZoneFile_CallsIsBusyWithFalse_WhenExceptionOccurs()
        {
            List<bool> isBusyValues = new List<bool>();

            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(42), b => isBusyValues.Add(b));
            await testObject.CallUploadZoneFile();

            Assert.Equal(2, isBusyValues.Count);
            Assert.True(isBusyValues[0]);
            Assert.False(isBusyValues[1]);
        }

        [Fact]
        public async Task UploadZoneFile_CallsIsBusyWithFalse_WhenSuccess()
        {
            List<bool> isBusyValues = new List<bool>();

            var upsAccount = new UpsAccountEntity();
            var openFileDialog = mock.CreateMock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            mock.MockFunc(openFileDialog);

            mock.Mock<IUpsLocalRateTable>().SetupGet(t => t.ZoneUploadDate).Returns(DateTime.Now);

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(upsAccount, b => isBusyValues.Add(b));
            await testObject.CallUploadZoneFile();

            Assert.Equal(2, isBusyValues.Count);
            Assert.True(isBusyValues[0]);
            Assert.False(isBusyValues[1]);
        }

        [Fact]
        public async void UploadZoneFile_SetsErrorValidatingRatesToTrue_WhenExceptionOccurs()
        {
            var openFileDialog = mock.Mock<IOpenFileDialog>();
            openFileDialog.Setup(f => f.ShowDialog()).Returns(DialogResult.OK);
            var openFileDialogFactory = mock.MockRepository.Create<Func<IOpenFileDialog>>();
            openFileDialogFactory.Setup(f => f()).Returns(openFileDialog);

            mock.Mock<IUpsLocalRateTable>()
                .Setup(t => t.SaveZones())
                .Throws<UpsLocalRatingException>();

            var testObject = mock.Create<FakeUpsLocalRatingViewModel>();
            testObject.Load(new UpsAccountEntity(), b => { });
            await testObject.CallUploadZoneFile();

            Assert.True(testObject.ErrorUploading);
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