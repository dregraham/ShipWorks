using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// A Utility class for building test FedExProcessShipmentReplies
    /// </summary>
    class BuildFedExProcessShipmentReply
    {
        public static ProcessShipmentReply BuildValidFedExProcessShipmentReply()
        {
            return new ProcessShipmentReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                CompletedShipmentDetail = new CompletedShipmentDetail()
                {
                    MasterTrackingId = new TrackingId()
                    {
                        TrackingNumber = "MasterTrackingNumber",
                        FormId = "ABCD"
                    },
                    CompletedPackageDetails = new[]
                    {
                        new CompletedPackageDetail()
                        {
                            TrackingIds = new[]
                            {
                                new TrackingId()
                                {
                                    TrackingNumber = "Package1Tracking",
                                }
                            },
                            SequenceNumber = "1"
                        }
                    },
                    ShipmentRating = new ShipmentRating()
                    {
                        ActualRateType = ReturnedRateType.PREFERRED_ACCOUNT_PACKAGE,
                        ActualRateTypeSpecified = true,
                        ShipmentRateDetails = new[]
                        {
                            new ShipmentRateDetail()
                            {
                                RateType = ReturnedRateType.INCENTIVE,
                                TotalNetCharge = new Money(){ Amount = 100, Currency = "USD"},
                                TotalNetFedExCharge = new Money() {Amount = 90, Currency = "CAD"}
                            },
                            new ShipmentRateDetail()
                            {
                                RateType = ReturnedRateType.PREFERRED_ACCOUNT_PACKAGE,
                                TotalNetCharge = new Money(){ Amount = 42, Currency = "USD"},
                                TotalNetFedExCharge = new Money() {Amount = 40, Currency = "CAD"}

                            }
                        }
                    }
                }
            };
        }
    }
}
