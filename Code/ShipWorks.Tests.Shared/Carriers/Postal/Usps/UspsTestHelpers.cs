﻿using Moq;
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
        public static void SetupAddressValidationResponse(Mock<IExtendedSwsimV111> mock)
        {
            var type = typeof(CleanseAddressCompletedEventArgs);

            var results = new object[]
            {
                "", // Result
                    new Address(),
                    true,
                    true,
                    ResidentialDeliveryIndicatorType.Yes,
                    false,
                    false,
                    new Address[] { },
                    new StatusCodes(),
                    new RateV40[] { },
                    "",
                    0
            };

            var eventArgs = (CleanseAddressCompletedEventArgs) type.Assembly.CreateInstance(type.FullName, false,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, new object[] { results, null, false, null }, null, null);

            mock.Setup(x => x.CleanseAddressAsync(It.IsAny<object>(), It.IsAny<Address>(), It.IsAny<string>(), It.IsAny<object>()))
                .Raises(x => x.CleanseAddressCompleted += null, eventArgs);
        }
    }
}
