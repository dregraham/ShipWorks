//using System;
//using Autofac.Extras.Moq;
//using Interapptive.Shared.Utility;
//using Moq;
//using ShipWorks.Common.IO.Hardware.Printers;
//using ShipWorks.Data.Model.EntityClasses;
//using ShipWorks.Shipping.Carriers.Amazon.SFP;
//using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
//using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
//using ShipWorks.Stores.Platforms.Amazon;
//using ShipWorks.Tests.Shared;
//using Xunit;

//namespace ShipWorks.Shipping.Tests.Carriers.Amazon
//{
//    public class AmazonSFPCreateShipmentRequestTest : IDisposable
//    {
//        private TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");
//        readonly AutoMock mock;
//        readonly ShipmentEntity defaultShipment = new ShipmentEntity
//        {
//            Order = new AmazonOrderEntity(),
//            AmazonSFP = new AmazonSFPShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
//        };

//        public AmazonSFPCreateShipmentRequestTest()
//        {
//            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

//            mock.Mock<IAmazonSFPShipmentRequestDetailsFactory>()
//                .Setup(x => x.Create(It.IsAny<ShipmentEntity>(), It.IsAny<IAmazonOrder>()))
//                .Returns(new ShipmentRequestDetails { ShippingServiceOptions = new ShippingServiceOptions
//                {
//                    DeclaredValue = new DeclaredValue(),
//                    LabelFormat = defaultShipment.AmazonSFP.RequestedLabelFormat == (int) ThermalLanguage.ZPL ? "ZPL203" : null
//                } });
//        }

//        [Theory]
//        [InlineData("STAMPS_DOT_COM")]
//        [InlineData("USPS")]
//        public void Create_SetsDeclaredValueToZero_WhenCarrierIsUSPS(string carrier)
//        {
//            defaultShipment.AmazonSFP.InsuranceValue = 65;
//            defaultShipment.AmazonSFP.CarrierName = carrier;

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 0),
//                    defaultShipment.AmazonSFP, 
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        [Theory]
//        [InlineData("FEDEX")]
//        [InlineData("UPS")]
//        [InlineData("DHLMX")]
//        [InlineData("DHLM")]
//        [InlineData("DYNAMEX")]
//        public void Create_SetsDeclaredValueToSpecifiedValue_WhenCarrierIsNotUSPS(string carrier)
//        {
//            defaultShipment.Insurance = true;
//            defaultShipment.AmazonSFP.InsuranceValue = 65;
//            defaultShipment.AmazonSFP.CarrierName = carrier;

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 65),
//                    defaultShipment.AmazonSFP,
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        [Theory]
//        [InlineData("FEDEX")]
//        [InlineData("UPS")]
//        [InlineData("DHLMX")]
//        [InlineData("DHLM")]
//        [InlineData("DYNAMEX")]
//        public void Create_DoesNotSetDeclaredValueToSpecifiedValue_WhenCarrierIsNotUSPS_AndInsuranceIsNotSelected(string carrier)
//        {
//            defaultShipment.Insurance = false;
//            defaultShipment.AmazonSFP.InsuranceValue = 65;
//            defaultShipment.AmazonSFP.CarrierName = carrier;

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 0),
//                    defaultShipment.AmazonSFP,
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        [Theory]
//        [InlineData("FEDEX")]
//        [InlineData("UPS")]
//        [InlineData("DHLMX")]
//        [InlineData("DHLM")]
//        [InlineData("DYNAMEX")]
//        public void Create_SetsDeclaredValueTo100_WhenCarrierIsNotUSPSAndDeclaredValueIsGreaterThan100(string carrier)
//        {
//            defaultShipment.Insurance = true;
//            defaultShipment.AmazonSFP.InsuranceValue = 101;
//            defaultShipment.AmazonSFP.CarrierName = carrier;

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 100),
//                    defaultShipment.AmazonSFP,
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        [Theory]
//        [InlineData("FEDEX")]
//        [InlineData("UPS")]
//        [InlineData("DHLMX")]
//        [InlineData("DHLM")]
//        [InlineData("DYNAMEX")]
//        public void Create_SetsDeclaredValueTo0_WhenCarrierIsNotUSPSAndDeclaredValueIsGreaterThan100_ButInsuranceWasNotSelected(string carrier)
//        {
//            defaultShipment.Insurance = false;
//            defaultShipment.AmazonSFP.InsuranceValue = 101;
//            defaultShipment.AmazonSFP.CarrierName = carrier;

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 0),
//                    defaultShipment.AmazonSFP,
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        [Theory]
//        [InlineData(ThermalLanguage.None, null)]
//        [InlineData(ThermalLanguage.ZPL, "ZPL203")]
//        public void Create_SetsLabelFormatCorrectly(ThermalLanguage thermalLanguage, string expectedLabelFormat)
//        {
//            defaultShipment.AmazonSFP.RequestedLabelFormat = (int) thermalLanguage;

//            mock.Mock<IAmazonSFPShipmentRequestDetailsFactory>()
//                .Setup(x => x.Create(It.IsAny<ShipmentEntity>(), It.IsAny<IAmazonOrder>()))
//                .Returns(new ShipmentRequestDetails
//                {
//                    ShippingServiceOptions = new ShippingServiceOptions
//                    {
//                        DeclaredValue = new DeclaredValue(),
//                        LabelFormat = defaultShipment.AmazonSFP.RequestedLabelFormat == (int) ThermalLanguage.ZPL ? "ZPL203" : null
//                    }
//                });

//            var testObject = mock.Create<AmazonSFPCreateShipmentRequest>();
//            testObject.Submit(defaultShipment, telemetricResult);

//            mock.Mock<IAmazonSFPShippingWebClient>()
//                .Verify(x => x.CreateShipment(
//                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.LabelFormat == expectedLabelFormat),
//                    defaultShipment.AmazonSFP,
//                    It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
//        }

//        public void Dispose()
//        {
//            mock.Dispose();
//        }
//    }
//}
