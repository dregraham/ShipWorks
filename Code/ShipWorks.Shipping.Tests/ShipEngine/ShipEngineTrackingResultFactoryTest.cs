using Autofac.Extras.Moq;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineTrackingResultFactoryTest
    {
        private AutoMock mock;
        private IShipEngineTrackingResultFactory testObject;
        private TrackingInformation shipEngineTrackingInfo;

        public ShipEngineTrackingResultFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipEngineTrackingInfo = new TrackingInformation
            {
                StatusDescription = "Delivered",
                Events = new List<TrackEvent>
                {
                    new TrackEvent
                    {
                        CarrierOccurredAt= new DateTime(2023, 2, 23, 16,5, 5, DateTimeKind.Utc),
                        OccurredAt = new DateTime(2017, 7, 7, 7, 7, 7),
                        CityLocality = "St Louis",
                        StateProvince = "MO",
                        CountryCode = "US",
                        PostalCode = "63102",
                        Description = "Delivered - Left at door"
                    }
                }
            };

            testObject = mock.Create<ShipEngineTrackingResultFactory>();
        }

        [Fact]
        public void Create_SetsTrackingResultSummaryToTrackingInfoStatusDescription()
        {
            shipEngineTrackingInfo.ActualDeliveryDate = new DateTime(2023, 2, 23, 15, 15, 4, DateTimeKind.Utc);
            shipEngineTrackingInfo.Events.First().Signer = "Kevin Croke";
            shipEngineTrackingInfo.EstimatedDeliveryDate = shipEngineTrackingInfo.ActualDeliveryDate;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal("<b>Delivered</b> on 2/23/2023 9:15:04 AM<br/><span style='color: rgb(80, 80, 80);'>Signed by: Kevin Croke</span>", result.Summary);
        }

        [Fact]
        public void Create_ShowsEstimatedDate_WhenNoActualDate()
        {
            shipEngineTrackingInfo.StatusDescription = "In Transit";
            shipEngineTrackingInfo.EstimatedDeliveryDate = new DateTime(2023, 2, 23, 15, 15, 4, DateTimeKind.Utc); ;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal("<b>In Transit</b><br/><span style='color: rgb(80, 80, 80);'>Should arrive: 2/23/2023 3:15:04 PM</span>", result.Summary);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailDateToTrackingInfoEventOccuredAt()
        {
            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal(shipEngineTrackingInfo.Events.First().CarrierOccurredAt.Value.ToString("M/dd/yyy"), result.Details.FirstOrDefault().Date);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailTimeToTrackingInfoEventOccuredAt()
        {
            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal(shipEngineTrackingInfo.Events.First().CarrierOccurredAt.Value.ToString("h:mm tt"), result.Details.FirstOrDefault().Time);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailLocationToTrackingInfoEventCityStatCountryPostal()
        {
            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal("St Louis, MO, US 63102", result.Details.FirstOrDefault().Location);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailActivityToTrackingInfoEventDescription()
        {
            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal("Delivered - Left at door", result.Details.FirstOrDefault().Activity);
        }

        [Fact]
        public void Create_ReturnsTrackingResultWithZeroTrackingResultDetails_WhenTrackingInformationHasZeroEvents()
        {
            shipEngineTrackingInfo.Events = null;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Empty(result.Details);
        }

        [Fact]
        public void Create_ReturnsTrackingResultWithMultipleTrackingResultDetails_WhenTrackingInformationHasMultipleEvents()
        {
            shipEngineTrackingInfo.Events.Add(new TrackEvent());

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal(2, result.Details.Count);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailLocationCorrectly_WhenTrackEventIsMissingLocationInfo()
        {
            shipEngineTrackingInfo.Events.FirstOrDefault().StateProvince = string.Empty;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal("St Louis, US 63102", result.Details.FirstOrDefault().Location);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailDateToEmptyString_WhenTrackingEventOccurredAtIsNull()
        {
            shipEngineTrackingInfo.Events.FirstOrDefault().OccurredAt = null;
            shipEngineTrackingInfo.Events.FirstOrDefault().CarrierOccurredAt = null;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal(string.Empty, result.Details.FirstOrDefault().Date);
        }

        [Fact]
        public void Create_SetsTrackingResultDetailTimeToEmptyString_WhenTrackingEventOccurredAtIsNull()
        {
            shipEngineTrackingInfo.Events.FirstOrDefault().OccurredAt = null;
            shipEngineTrackingInfo.Events.FirstOrDefault().CarrierOccurredAt = null;

            TrackingResult result = testObject.Create(shipEngineTrackingInfo);

            Assert.Equal(string.Empty, result.Details.FirstOrDefault().Time);
        }
    }
}
