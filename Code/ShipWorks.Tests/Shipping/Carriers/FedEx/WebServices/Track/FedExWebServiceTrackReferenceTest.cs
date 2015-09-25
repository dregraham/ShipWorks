using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.WebServices.Track
{
    public class FedExWebServiceTrackReferenceTest
    {
        [Fact]
        public void EmailNotificationEventType_IsSingleDimensionArray()
        {
            var trackNotificationPackage = new TrackNotificationPackage() { RecipientDetails = new EMailNotificationEventType[0] };

            // The object that is gerenated from the WSDL is a 2 dimensional array. It needs to be manually changed 
            // in the Reference.cs file.
            Assert.IsType<EMailNotificationEventType[]>(trackNotificationPackage.RecipientDetails);
        }
        
        [Fact]
        public void EmailNotificationEventType_BackingFieldIsTwoDimensionalArray()
        {
            TrackNotificationPackage trackNotificationPackage = new TrackNotificationPackage();
            EMailNotificationEventType[] array = {
                new EMailNotificationEventType()
            };

            // Will throw an error if backing field is not an array.
            trackNotificationPackage.RecipientDetails = array;

            Assert.Equal(array, trackNotificationPackage.RecipientDetails);
        }

        
    }
}
