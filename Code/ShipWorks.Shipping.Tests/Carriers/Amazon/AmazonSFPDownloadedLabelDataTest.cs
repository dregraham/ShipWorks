using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Pdf;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPDownloadedLabelDataTest : IDisposable
    {
        readonly ShipmentEntity defaultShipment = new ShipmentEntity
        {
            Order = new AmazonOrderEntity(),
            AmazonSFP = new AmazonSFPShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
        };

        [SuppressMessage("SonarLint", "S103:Lines should not be too long",
            Justification = "The long line is binary data for tests")]
        readonly Shipping.ShipEngine.DTOs.Label defaultLabel = new Shipping.ShipEngine.DTOs.Label()
        {
            TrackingNumber = "123",
            ShipmentId = "456",
            ShipmentCost = new MoneyDTO(MoneyDTO.CurrencyEnum.USD, 5),
            ServiceCode = "amazon_fedex_ground",
            LabelFormat = Shipping.ShipEngine.DTOs.Label.LabelFormatEnum.Pdf,
            LabelDownload = new MultiFormatDownloadLinkDTO()
            {
                Href = "https://www.google.com",
            }
        };

        readonly AutoMock mock = null;

        public AmazonSFPDownloadedLabelDataTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<ITemplateLabelUtility>()
                .Setup(u => u.LoadImageFromResouceDirectory(It.IsAny<string>()))
                .Returns<Image>(null);
        }

        [Fact]
        public void Save_CopiesLabelData_ToShipment()
        {
            var testObject = mock.Create<AmazonSfpShipEngineDownloadedLabelData>(TypedParameter.From(defaultShipment), TypedParameter.From(defaultLabel));
            testObject.Save();

            Assert.Equal("123", defaultShipment.TrackingNumber);
            Assert.Equal(5, defaultShipment.ShipmentCost);
            Assert.Equal("456", defaultShipment.AmazonSFP.AmazonUniqueShipmentID);
        }

        [Fact]
        public void Save_SavesLabelData_ToResourceManager()
        {
            var testObject = mock.Create<AmazonSfpShipEngineDownloadedLabelData>(TypedParameter.From(defaultShipment), TypedParameter.From(defaultLabel));
            testObject.Save();
            mock.Mock<IDataResourceManager>()
                .Verify(x => 
                    x.CreateFromPdf(
                        It.IsAny<PdfDocumentType>(),
                        It.IsAny<Stream>(),
                        It.IsAny<long>(),
                        It.IsAny<Func<int, string>>(),
                        It.IsAny<Func<MemoryStream, byte[]>>(),
                        It.IsAny<bool>()
                        ),
                    Times.Once
                    );
        }

        [Fact]
        public void Save_ThrowsDownloadException_WhenLoadingImageThrowsOutOfMemoryException()
        {
            var testObject = mock.Create<AmazonSfpShipEngineDownloadedLabelData>(TypedParameter.From(defaultShipment), TypedParameter.From(defaultLabel));
            
            mock.Mock<ITemplateLabelUtility>()
                .Setup(u => u.LoadImageFromResouceDirectory(It.IsAny<string>()))
                .Throws<OutOfMemoryException>();

            mock.Mock<IDataResourceManager>()
                .Setup(r => r.CreateFromPdf(
                    It.IsAny<PdfDocumentType>(),
                    It.IsAny<Stream>(),
                    It.IsAny<long>(),
                    It.IsAny<Func<int, string>>(),
                    It.IsAny<Func<MemoryStream, byte[]>>(),
                    It.IsAny<bool>()
                ))
                .Throws<DownloadException>();

            Assert.Throws<DownloadException>(() => testObject.Save());
        }

        [Fact]
        public void Save_SavesPdfData_ToResourceManager()
        {
            var testObject = mock.Create<AmazonSfpShipEngineDownloadedLabelData>(TypedParameter.From(defaultShipment), TypedParameter.From(defaultLabel));
            testObject.Save();
            mock.Mock<IDataResourceManager>().Verify(x =>
                x.CreateFromPdf(It.IsAny<PdfDocumentType>(), It.IsAny<Stream>(), defaultShipment.ShipmentID,
                    It.IsAny<Func<int, string>>(), It.IsAny<Func<MemoryStream, byte[]>>(), true));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
