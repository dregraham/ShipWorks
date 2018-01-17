using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.WebServices.Track
{
    public class FedExWebServiceTrackReferenceTest
    {
        [Fact]
        public void EmailNotificationEventType_IsSingleDimensionArray()
        {
            var trackNotificationPackage = new TrackNotificationPackage() { RecipientDetails = new NotificationEventType[0] };

            // The object that is generated from the WSDL is a 2 dimensional array. It needs to be manually changed 
            // in the Reference.cs file.
            Assert.IsType<NotificationEventType[]>(trackNotificationPackage.RecipientDetails);
        }

        [Fact]
        public void EmailNotificationEventType_BackingFieldIsTwoDimensionalArray()
        {
            TrackNotificationPackage trackNotificationPackage = new TrackNotificationPackage();
            NotificationEventType[] array = {
                new NotificationEventType()
            };

            // Will throw an error if backing field is not an array.
            trackNotificationPackage.RecipientDetails = array;

            Assert.Equal(array, trackNotificationPackage.RecipientDetails);
        }
    }
}
