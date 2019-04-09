using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    public class FedExUploadImagesImageDetailManipulatorTest
    {
        private FedExUploadImagesImageDetailManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private UploadImagesRequest nativeRequest;

        private FedExAccountEntity account;

        public FedExUploadImagesImageDetailManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "12345", Letterhead = "Letterhead", Signature = "Signature" };
            
            nativeRequest = new UploadImagesRequest
            {
                Images = new UploadImageDetail[0]
            };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            testObject = new FedExUploadImagesImageDetailManipulator();
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
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, new TrackRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }
    }
}