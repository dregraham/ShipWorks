using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    
    public class FimsFakeWebClientTest
    {
        IFimsWebClient testObject = new FimsFakeWebClient();

        public FimsFakeWebClientTest()
        {
        }

        /// <summary>
        /// We use the Username of the ship request to let the fake web client know if we want a success or failure
        /// response.
        /// </summary>
        [Fact]
        public void Ship_ReturnsResponseCodeOf1_WhenUsernameIsSuccess()
        {
            IFimsShipResponse fimsShipResponse;
            IFimsShipRequest fimsShipRequest = new FimsShipRequest(null, "success", "asdf");
            fimsShipResponse = testObject.Ship(fimsShipRequest);

            Assert.Equal("1", fimsShipResponse.ResponseCode);
            Assert.NotNull(fimsShipResponse.LabelPdfData);
        }

        /// <summary>
        /// We use the Username of the ship request to let the fake web client know if we want a success or failure
        /// response.
        /// </summary>
        [Fact]
        public void Ship_ReturnsResponseCodeOf0_WhenUsernameIsFailure()
        {
            IFimsShipResponse fimsShipResponse;
            IFimsShipRequest fimsShipRequest = new FimsShipRequest(null, "Failure", "asdf");
            fimsShipResponse = testObject.Ship(fimsShipRequest);

            Assert.Equal("0", fimsShipResponse.ResponseCode);
            Assert.Null(fimsShipResponse.LabelPdfData);
        }
    }
}
