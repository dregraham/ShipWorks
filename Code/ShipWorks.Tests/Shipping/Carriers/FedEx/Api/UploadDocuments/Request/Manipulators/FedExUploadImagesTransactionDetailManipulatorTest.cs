using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
using TransactionDetail = ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument.TransactionDetail;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    public class FedExUploadImagesTransactionDetailManipulatorTest
    {
        private FedExUploadImagesTransactionDetailManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private UploadImagesRequest nativeRequest;

        private FedExAccountEntity account;

        public FedExUploadImagesTransactionDetailManipulatorTest()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", MeterNumber = "12345"};
            nativeRequest = new UploadImagesRequest {TransactionDetail = new TransactionDetail()};
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);
            carrierRequest.Setup(x => x.CarrierAccountEntity).Returns(account);

            testObject = new FedExUploadImagesTransactionDetailManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotUploadImagesRequest()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, new TrackRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsTransactionDetail_WhenTransactionDetailIsNull_ForUploadImages()
        {
            nativeRequest.TransactionDetail = null;
            testObject.Manipulate(carrierRequest.Object);
            TransactionDetail detail = ((UploadImagesRequest)carrierRequest.Object.NativeRequest).TransactionDetail;
            
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsTransactionDetail_WhenTransactionDetailIsNotNull_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            TransactionDetail detail = ((UploadImagesRequest)carrierRequest.Object.NativeRequest).TransactionDetail;

            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsCustomerTransactionId_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            TransactionDetail detail = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).TransactionDetail;

            Assert.Equal("UploadImagesRequest_v11", detail.CustomerTransactionId);
        }
    }
}