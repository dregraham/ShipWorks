using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Rates;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Rates
{
    public class OnTracRateRequestTest
    {
        private readonly Mock<IHttpResponseReader> mockedHttpResponseReader;
        private readonly Mock<IApiLogEntry> mockedLogger;
        private readonly Mock<HttpVariableRequestSubmitter> mockedSubmitter;
        private readonly Mock<ILog> log;
        private readonly ShipmentEntity shipment;
        private readonly OnTracRates testObject;

        public OnTracRateRequestTest()
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

        [Fact]
        public void GetRates_RatesRetrieved_ValidResponse()
        {
            var rateGroup = RunSuccessfullGetRates();

            Assert.Equal(OnTracServiceType.Ground, (OnTracServiceType)rateGroup.Rates.First().Tag);
        }

        [Fact]
        public void GetRates_RateIsNotIncluded_WhenServiceTypeHasBeenExcluded()
        {
            var rateGroup = RunSuccessfullGetRates(OnTracShipmentType.ServiceTypes.Where(x => x != OnTracServiceType.Ground));

            Assert.Equal(0, rateGroup.Rates.Count);
        }

        [Fact]
        public void GetRates_ResponseLogged_ValidResponse()
        {
            RunSuccessfullGetRates();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        [Fact]
        public void GetRates_RequestLogged_ValidResponse()
        {
            RunSuccessfullGetRates();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<IHttpRequestSubmitter>()), Times.Once());
        }

        [Fact]
        public void GetRates_UrlInCorrectFormat_ValidResponse()
        {
            RunSuccessfullGetRates();

            Assert.True(
                mockedSubmitter.Object.Uri.ToString().EndsWith(
                    "/OnTracServices.svc/v2/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;15;1X2X3;"));
        }

        [Fact]
        public void GetRates_RequestUsedGetMethod_ValidResponse()
        {
            RunSuccessfullGetRates();

            Assert.Equal(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        private RateGroup RunSuccessfullGetRates(IEnumerable<OnTracServiceType> availableServiceTypes = null)
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments
                {
                    Shipment = new[]
                    {
                        new ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse.Shipment
                        {
                            Rates = new[]
                            {
                                new Rate
                                {
                                    Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                                }
                            }
                        }
                    },
                    Error = ""
                }
            };   

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            RateGroup rateGroup = testObject.GetRates(shipment, availableServiceTypes ?? OnTracShipmentType.ServiceTypes);
            return rateGroup;
        }

        [Fact]
        public void GetRates_RequestedWeightIsZero_PackageTypeIsLetter()
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments
                {
                    Shipment = new[]
                    {
                        new ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse.Shipment
                        {
                            Rates = new[]
                            {
                                new Rate
                                {
                                    Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                                }
                            }
                        }
                    },
                    Error = ""
                }
            };

            shipment.OnTrac.PackagingType = (int)OnTracPackagingType.Letter;

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes);

            Assert.True(
                mockedSubmitter.Object.Uri.ToString().EndsWith(
                    "/OnTracServices.svc/v2/42/rates?pw=testpass&packages=uid;90210;63102;True;10;False;450;0;0X0X0;"));

        }

        [Fact]
        public void GetRates_ThrowsOnTracException_WhenErrorInShipment()
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments
                {
                    Shipment = new[]
                    {
                        new ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse.Shipment
                        {
                            Rates = new[]
                            {
                               new Rate
                               {
                                    Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                               }
                            },
                            Error = "AHHHH!!!"
                        }
                    },
                    Error = ""
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            Assert.Throws<OnTracApiErrorException>(() => testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes));
        }

        [Fact]
        public void GetRates_ThrowsOnTracException_WhenNoShipmentReturned()
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments()
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            Assert.Throws<OnTracException>(() => testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes));
        }

        [Fact]
        public void GetRates_ThrowsOnTracException_WhenRequestErrorReturned()
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments
                {
                    Shipment = new ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse.Shipment[0],
                    Error = "dude"
                }
            };

            string serializedValidResponse = SerializationUtility.SerializeToXml(rateShipmentList);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(serializedValidResponse);

            Assert.Throws<OnTracApiErrorException>(() => testObject.GetRates(shipment, OnTracShipmentType.ServiceTypes));
        }

        [Fact]
        public void GetRates_LogsUnknownRateType_WhenUnknownRateTypeReturned()
        {
            OnTracRateResponse rateShipmentList = new OnTracRateResponse
            {
                Shipments = new Shipments
                {
                    Shipment = new[]
                    {
                        new ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse.Shipment
                        {
                            Rates = new[]
                            {
                               new Rate
                               {
                                    Service = "XX"
                               },
                               new Rate
                               {
                                    Service = EnumHelper.GetApiValue(OnTracServiceType.Ground)
                               }
                            }
                        }
                    },
                    Error = ""
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
