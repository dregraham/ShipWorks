using System;
using System.IO;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Pdf;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressDownloadedLabelDataTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly LinkDTO link;
        private readonly Label label;

        public DhlExpressDownloadedLabelDataTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity()
            {
                ShipmentID = 123,
                DhlExpress = new DhlExpressShipmentEntity(),
                ShipmentTypeCode = ShipmentTypeCode.DhlExpress
            };

            link = new LinkDTO() { Href = "http://www.google.com" };

            label = new Label()
            {
                LabelFormat = Label.LabelFormatEnum.Zpl,
                LabelDownload = link,
                TrackingNumber = "aaaaaaaa",
                ShipmentCost = new MoneyDTO() { Amount = 2 },
                ShipmentId = "abcd",
                LabelId = "defg"
            };
        }

        [Fact]
        public void Save_SetsLabelInfoOnShipment()
        {
            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            testObject.Save();

            Assert.Equal("aaaaaaaa", shipment.TrackingNumber);
            Assert.Equal(2, shipment.ShipmentCost);
            Assert.Equal("defg", shipment.DhlExpress.ShipEngineLabelID);
        }

        [Fact]
        public void Save_DelegatesToResourceDownloaderForLabelImage()
        {
            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            testObject.Save();

            mock.Mock<IShipEngineResourceDownloader>().Verify(r => r.Download(new Uri(link.Href)));
        }

        [Fact]
        public void Save_DelegatesToDataResourceManagerWhenLabelIsPdf()
        {
            mock.Mock<IShipEngineResourceDownloader>().Setup(r => r.Download(It.IsAny<Uri>())).Returns(new byte[0]);

            label.LabelFormat = Label.LabelFormatEnum.Pdf;

            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            testObject.Save();

            mock.Mock<IDataResourceManager>()
                .Verify(r => r.CreateFromPdf(
                    PdfDocumentType.BlackAndWhite, It.IsAny<MemoryStream>(), 123, It.IsAny<Func<int, string>>(), It.IsAny<Func<MemoryStream, byte[]>>()));
        }


        [Fact]
        public void Save_DelegatesToDataResourceManagerWhenLabelIsZpl()
        {
            var resource = new byte[0];
            mock.Mock<IShipEngineResourceDownloader>().Setup(r => r.Download(It.IsAny<Uri>())).Returns(resource);

            label.LabelFormat = Label.LabelFormatEnum.Zpl;

            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            testObject.Save();

            mock.Mock<IDataResourceManager>().Verify(r => r.CreateFromBytes(resource, 123, "LabelPrimary"));
        }

        [Fact]
        public void Save_ThrowsShipEngineException_WhenLabelFormatIsNotSupported()
        {
            label.LabelFormat = Label.LabelFormatEnum.Png;

            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Save());

            Assert.Equal("DHL Express returned an unsupported label format.", ex.Message);
        }

        [Fact]
        public void Save_ThrowsShipEngineException_WhenResourceDownloaderThrowsShipEngineException()
        {
            mock.Mock<IShipEngineResourceDownloader>().Setup(r => r.Download(It.IsAny<Uri>())).Throws(new ShipEngineException("something went wrong"));
            var testObject = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(label));

            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Save());

            Assert.Equal("something went wrong", ex.Message);
        }
    }
}
