using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.ScanForms;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    public class Express1UtilitiesTest
    {
        private const string errorMessage = "an error message";

        [Fact]
        public void ValidateDataIsProvided_ReturnsNoErrors_WhenValueProvided_Test()
        {
            Assert.AreEqual(Express1Utilities.ValidateDataIsProvided("aValue", errorMessage).Count(), 0);
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsBlank_Test()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided("", errorMessage);
            Assert.AreEqual(errors.Count(), 1);
            Assert.AreEqual(errors.First().Message, errorMessage);
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsSpaces_Test()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided("  ", errorMessage);
            Assert.AreEqual(errors.Count(), 1);
            Assert.AreEqual(errors.First().Message, errorMessage);
        }

        [Fact]
        public void ValidateDataIsProvided_ReturnsErrors_WhenValueIsNull_Test()
        {
            IEnumerable<Express1ValidationError> errors = Express1Utilities.ValidateDataIsProvided(null, errorMessage);
            Assert.AreEqual(errors.Count(), 1);
            Assert.AreEqual(errors.First().Message, errorMessage);
        }
    }
}
