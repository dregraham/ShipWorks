using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    [TestClass]
    public class FimsFakeWebClientTest
    {
        IFimsWebClient testObject = new FimsFakeWebClient();

        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        /// We use the Username of the ship request to let the fake web client know if we want a success or failure
        /// response.
        /// </summary>
        [TestMethod]
        public void Ship_ReturnsResponseCodeOf1_WhenUsernameIsSuccess()
        {
            IFimsShipResponse fimsShipResponse;
            IFimsShipRequest fimsShipRequest = new FimsShipRequest(null, "success", "asdf");
            fimsShipResponse = testObject.Ship(fimsShipRequest);

            Assert.AreEqual("1", fimsShipResponse.ResponseCode);
            Assert.IsNotNull(fimsShipResponse.LabelPdfData);
        }

        /// <summary>
        /// We use the Username of the ship request to let the fake web client know if we want a success or failure
        /// response.
        /// </summary>
        [TestMethod]
        public void Ship_ReturnsResponseCodeOf0_WhenUsernameIsFailure()
        {
            IFimsShipResponse fimsShipResponse;
            IFimsShipRequest fimsShipRequest = new FimsShipRequest(null, "Failure", "asdf");
            fimsShipResponse = testObject.Ship(fimsShipRequest);

            Assert.AreEqual("0", fimsShipResponse.ResponseCode);
            Assert.IsNull(fimsShipResponse.LabelPdfData);
        }
    }
}
