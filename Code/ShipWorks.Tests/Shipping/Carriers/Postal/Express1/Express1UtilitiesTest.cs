using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    public class Express1UtilitiesTest
    {
        private const string errorMessage = "an error message";

        [Fact]
        public void ValidateDataIsProvided_ReturnsNoErrors_WhenValueProvided()
        {
            Assert.Equal(0, Express1Utilities.ValidateDataIsProvided("aValue", errorMessage).Count());
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsBlank()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided("", errorMessage);
            Assert.Equal(1, errors.Count());
            Assert.Equal(errors.First().Message, errorMessage);
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsSpaces()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided("  ", errorMessage);
            Assert.Equal(1, errors.Count());
            Assert.Equal(errors.First().Message, errorMessage);
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsNull()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided(null, errorMessage);
            Assert.Equal(1, errors.Count());
            Assert.Equal(errors.First().Message, errorMessage);
        }
    }
}
