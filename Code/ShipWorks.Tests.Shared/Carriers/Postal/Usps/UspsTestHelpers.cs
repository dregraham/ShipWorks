using Moq;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Tests.Shared.Carriers.Postal.Usps
{
    /// <summary>
    /// Helper methods for USPS shipments
    /// </summary>
    public static class UspsTestHelpers
    {
        /// <summary>
        /// Setup a default address validation response, since not doing this will cause the process to hang
        /// </summary>
        /// <remarks>This hang is because we're mixing async calls with waits. Until we can use async calls
        /// for more of the web client, we'll need to use this.</remarks>
        public static void SetupAddressValidationResponse(Mock<ISwsimV55> mock)
        {
            mock.Setup(x => x.CleanseAddressAsync(It.IsAny<object>(), It.IsAny<Address>(), It.IsAny<string>()))
                .Raises(x => x.CleanseAddressCompleted += null, new CleanseAddressCompletedEventArgs(new object[] {
                    "", // Result
                    new Address(),
                    true,
                    true,
                    ResidentialDeliveryIndicatorType.Yes,
                    false,
                    false,
                    new Address[] { },
                    new StatusCodes(),
                    new RateV20[] { },
                }, null, false, null));
        }
    }
}
