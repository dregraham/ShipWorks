using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    public class FedExUploadImagesClientDetailManipulatorTest
    {
        private FedExUploadImagesClientDetailManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private UploadImagesRequest nativeRequest;

        private FedExAccountEntity account;

        public FedExUploadImagesClientDetailManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "12345" };
            nativeRequest = new UploadImagesRequest { ClientDetail = new ClientDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);
            carrierRequest.Setup(x => x.CarrierAccountEntity).Returns(account);

            testObject = new FedExUploadImagesClientDetailManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotUploadImageRequest_AndIsNotUploadImagesReply()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, new UploadImagesReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);

            carrierRequest.Verify(x => x.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailIsNull_ForUploadImages()
        {
            // Only setup is to set the detail to null
            nativeRequest.ClientDetail = null;
            testObject.Manipulate(carrierRequest.Object);
            ClientDetail detail = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).ClientDetail;

            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            ClientDetail detail = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).ClientDetail;

            Assert.NotNull(detail);
        }
    }
}