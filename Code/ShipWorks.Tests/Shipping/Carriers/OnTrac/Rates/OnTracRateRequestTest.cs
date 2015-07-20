using System;
using System.Collections.Generic;
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
using ShipWorks.Shipping.Editing.Rating;
using ILog = log4net.ILog;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Rates
{
    [TestClass]
    public class OnTracRateRequestTest
    {
        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<HttpVariableRequestSubmitter> mockedSubmitter;

        Mock<ILog> log;

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

            Mock<ILogEntryFactory> mockedLogFactory = new Mock<ILogEntryFactory>();
            mockedLogFactory
                .Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(mockedLogger.Object);

            log = new Mock<ILog>();
            log.Setup(l => l.Info(It.IsAny<string>(), It.IsAny<InvalidOperationException>()));

            testObject = new OnTracRates(42, "testpass", mockedSubmitter.Object, mockedLogFactory.Object, log.Object);

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
        public void GetRates_RateIsNotIncluded_WhenServiceTypeHasBeenExcluded()
        {
            var rateGroup = RunSuccessfullGetRates(OnTracShipmentType.ServiceTypes.Where(x => x != OnTracServiceType.Ground));

            Assert.AreEqual(0, rateGroup.Rates.Count);
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
                    "/OnTracServices.svc/v2/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;15;1X2X3;"));
        }

        [TestMethod]
        public void GetRates_RequestUsedGetMethod_ValidResponse_Test()
        {
            RunSuccessfullGetRates();

            Assert.AreEqual(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        RateGroup RunSuccessfullGetRates(IEnumerable<OnTracServiceType> availableServiceTypes = null)
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
                                Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                            }
                        }
                    }
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            RateGroup rateGroup = testObject.GetRates(shipment, availableServiceTypes ?? OnTracShipmentType.ServiceTypes);
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
                                Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                            }
                        }
                    }
                }
            };


            shipment.OnTrac.PackagingType = (int)OnTracPackagingType.Letter;

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            RateGroup rateGroup = testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);

            Assert.IsTrue(
                mockedSubmitter.Object.Uri.ToString().EndsWith(
                    "/OnTracServices.svc/v2/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;0;0X0X0;"));

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
                                Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                            }
                        }
                    }
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);
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

            testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);
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

            testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);
        }

        [TestMethod]
        public void GetRates_LogsUnknownRateType_WhenUnknownRateTypeReturned_Test()
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
                                Service = "XX"
                            },
                            new RateQuote
                            {
                                Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                            }
                        }
                    }
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);

            log.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<InvalidOperationException>()), Times.Once());
        }
    }
}
