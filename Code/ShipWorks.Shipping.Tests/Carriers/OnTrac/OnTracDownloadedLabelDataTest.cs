using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracDownloadedLabelDataTest : IDisposable
    {
        readonly AutoMock mock;

        public OnTracDownloadedLabelDataTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Save_CopiesResponseData_ToShipmentEntity()
        {
            var shipment = new ShipmentEntity();
            var response = new ShipmentResponse
            {
                Tracking = "abc123",
                TotalChrg = 3.21,
                BilledWeight = 4.56,
                Label = exampleLabelData
            };

            var testObject = mock.Create<OnTracDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(response));
            testObject.Save();

            Assert.Equal("abc123", shipment.TrackingNumber);
            Assert.Equal((decimal) 3.21, shipment.ShipmentCost);
            Assert.Equal(4.56, shipment.BilledWeight);
        }

        [Fact]
        public void Save_ClearsExistingLabels()
        {
            var shipment = new ShipmentEntity { ShipmentID = 1234 };
            var response = new ShipmentResponse { Label = exampleLabelData };
            var testObject = mock.Create<OnTracDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(response));

            testObject.Save();

            mock.Mock<IObjectReferenceManager>().Verify(x => x.ClearReferences(1234));
        }

        [Fact]
        public void Save_SavesLabelData_WhenFormatIsSet()
        {
            var shipment = new ShipmentEntity { ShipmentID = 1234, ActualLabelFormat = 1 };
            var response = new ShipmentResponse { Label = "a" };
            var testObject = mock.Create<OnTracDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(response));

            testObject.Save();

            mock.Mock<IDataResourceManager>().Verify(x => x.CreateFromBytes(new byte[] { 97 }, 1234, "LabelPrimary"));
        }

        [Fact]
        public void Save_SavesLabelData_WhenFormatIsNotSet()
        {
            var shipment = new ShipmentEntity { ShipmentID = 1234 };
            var response = new ShipmentResponse { Label = exampleLabelData };
            var testObject = mock.Create<OnTracDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(response));

            testObject.Save();

            mock.Mock<IDataResourceManager>().Verify(x => x.CreateFromBytes(label, 1234, "LabelPrimary"));
        }

        [Fact]
        public void Save_ThrowsOnTracException_WhenFormatIsNotSetAndImageIsInvalid()
        {
            var shipment = new ShipmentEntity();
            var response = new ShipmentResponse { Label = "a" };
            var testObject = mock.Create<OnTracDownloadedLabelData>(TypedParameter.From(shipment), TypedParameter.From(response));

            Assert.Throws<OnTracException>(() => testObject.Save());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        // The byte array is the decoded version of the exampleLabelData
        const string exampleLabelData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==";
        private readonly byte[] label = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0,
            0, 0, 1, 0, 0, 1, 140, 8, 2, 0, 0, 0, 148, 46, 95, 108, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233,
            0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195,
            0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 0, 25, 73, 68, 65, 84, 56, 79, 99, 216, 181, 139, 97, 20, 140,
            130, 81, 48, 10, 70, 193, 40, 192, 15, 24, 24, 0, 2, 73, 1, 117, 146, 109, 93, 196, 0, 0, 0, 0, 73, 69, 78,
            68, 174, 66, 96, 130 };
    }
}
