using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Rates;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Rate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Rates
{
    [TestClass]
    public class OnTracRateRequestTest
    {
        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<HttpVariableRequestSubmitter> mockedSubmitter;

        ShipmentEntity shipment;

        OnTracRates testObject;

        [TestInitialize]
        public void Initialize()
        {
            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();

            //Setup mock Request Submitter
            mockedSubmitter = new Mock<HttpVariableRequestSubmitter>();
            mockedSubmitter.Setup(f => f.GetResponse()).Returns(mockedHttpResponseReader.Object);

            mockedLogger = new Mock<IApiLogEntry>();

            testObject = new OnTracRates(42, "testpass", mockedLogger.Object, mockedSubmitter.Object);

            shipment = new ShipmentEntity(1)
            {
                OriginPostalCode = "90210",
                ShipPostalCode = "63102",
                ResidentialResult = true,
                ContentWeight = 5,
                TotalWeight = 15,
                OnTrac = new OnTracShipmentEntity(1)
                {
                    CodAmount = 10,
                    SaturdayDelivery = false,
                    DeclaredValue = 450,
                    DimsWeight = 10,
                    DimsAddWeight = true,
                    DimsLength = 1,
                    DimsWidth = 2,
                    DimsHeight = 3,
                    PackagingType = (int)OnTracPackagingType.Package
                }
            };
        }

        [TestMethod]
        public void GetRates_RatesRetrieved_ValidResponse_Test()
        {
            var rateGroup = RunSuccessfullGetRates();

            Assert.AreEqual(OnTracServiceType.Ground, (OnTracServiceType)rateGroup.Rates.First().Tag);
        }

        [TestMethod]
        public void GetRates_ResponseLogged_ValidResponse_Test()
        {
            RunSuccessfullGetRates();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        [TestMethod]
        public void GetRates_RequestLogged_ValidResponse_Test()
        {
            RunSuccessfullGetRates();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<HttpRequestSubmitter>()), Times.Once());
        }

        [TestMethod]
        public void GetRates_UrlInCorrectFormat_ValidResponse_Test()
        {
            RunSuccessfullGetRates();

            Assert.IsTrue(
                mockedSubmitter.Object.Uri.ToString().EndsWith(
                    "/OnTracServices.svc/v1/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;15;1X2X3;"));
        }

        [TestMethod]
        public void GetRates_RequestUsedGetMethod_ValidResponse_Test()
        {
            RunSuccessfullGetRates();

            Assert.AreEqual(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        RateGroup RunSuccessfullGetRates()
        {
            RateShipmentList rateShipmentList = new RateShipmentList
            {
                Error = "",
                Shipments = new[]
                {
                    new RateShipment
                    {
                        Rates = new[]
                        {
                            new RateQuote
                            {
                                Service = serviceType.C
                            }
                        }
                    }
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            RateGroup rateGroup = testObject.GetRates(shipment);
            return rateGroup;
        }

        [TestMethod]
        public void GetRates_RequestedWeightIsZero_PackageTypeIsLetter()
        {
            RateShipmentList rateShipmentList = new RateShipmentList
            {
                Error = "",
                Shipments = new[]
                {
                    new RateShipment
                    {
                        Rates = new[]
                        {
                            new RateQuote
                            {
                                Service = serviceType.C
                            }
                        }
                    }
                }
            };


            shipment.OnTrac.PackagingType = (int)OnTracPackagingType.Letter;

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            RateGroup rateGroup = testObject.GetRates(shipment);

            Assert.IsTrue(
                mockedSubmitter.Object.Uri.ToString().EndsWith(
                    "/OnTracServices.svc/v1/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;0;0X0X0;"));

        }

        [TestMethod]
        [ExpectedException(typeof(OnTracApiErrorException))]
        public void GetRates_ThrowsOnTracException_WhenErrorInShipment_Test()
        {
            RateShipmentList rateShipmentList = new RateShipmentList
            {
                Error = "",
                Shipments = new[]
                {
                    new RateShipment
                    {
                        Error = "AHHHH!!!",
                        Rates = new[]
                        {
                            new RateQuote
                            {
                                Service = serviceType.C
                            }
                        }
                    }
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(OnTracException))]
        public void GetRates_ThrowsOnTracException_WhenNoShipmentReturned_Test()
        {
            RateShipmentList rateShipmentList = new RateShipmentList
            {
                Error = "",
                Shipments = new RateShipment[0]
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(OnTracApiErrorException))]
        public void GetRates_ThrowsOnTracException_WhenRequestErrorReturned_Test()
        {
            RateShipmentList rateShipmentList = new RateShipmentList
            {
                Error = "Da Da Da"
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment);
        }
    }
}
