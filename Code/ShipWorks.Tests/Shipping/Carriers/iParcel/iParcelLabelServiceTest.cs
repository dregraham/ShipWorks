using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Autofac;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Content;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelLabelServiceTest
    {
        private iParcelLabelService labelService;

        private Mock<IiParcelServiceGateway> serviceGateway;
        private Mock<IiParcelRepository> repository;
        private Mock<IOrderManager> orderManager;
        private ShipmentEntity shipment;

        public iParcelLabelServiceTest()
        {
            shipment = new ShipmentEntity
            {
                ShipCity = "St. Louis",
                ShipCompany = "Initech",
                ShipCountryCode = "US",
                ShipEmail = "someone@nowhere.com",
                ShipFirstName = "Peter",
                ShipLastName = "Gibbons",
                ShipPhone = "555-555-5555",
                ShipPostalCode = "63102",
                ShipStateProvCode = "MO",
                ShipStreet1 = "1 Main Street",
                ShipStreet2 = "Suite 500",

                OriginFirstName = "Bill",
                OriginLastName = "Lumbergh",
                OriginStreet1 = "500 First Street",
                OriginStreet2 = "Suite 200",
                OriginCity = "St. Louis",
                OriginStateProvCode = "MO",
                OriginPostalCode = "63102",
                OriginCountryCode = "US",

                Order = new OrderEntity { OrderTotal = 100.43M },

                RequestedLabelFormat = (int)ThermalLanguage.None,

                IParcel = new IParcelShipmentEntity
                {
                    Reference = "reference-value",
                    Service = (int)iParcelServiceType.Preferred,
                    TrackByEmail = true,
                    TrackBySMS = true
                }
            };

            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6 });

            serviceGateway = new Mock<IiParcelServiceGateway>();
            serviceGateway.Setup(g => g.SubmitShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(new DataSet());
            serviceGateway.Setup(g => g.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetDeliveredPackageTrackingInfo());

            repository = new Mock<IiParcelRepository>();
            repository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity());
            repository.Setup(r => r.GetiParcelAccount(It.IsAny<ShipmentEntity>())).Returns(new IParcelAccountEntity());
            repository.Setup(r => r.SaveLabel(It.IsAny<ShipmentEntity>(), It.IsAny<DataSet>()));
            repository.Setup(r => r.SaveTrackingInfoToEntity(It.IsAny<ShipmentEntity>(), It.IsAny<DataSet>()));

            Mock<IExcludedServiceTypeRepository> excludedServiceTypeRepository = new Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepository.Setup(x => x.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(new List<ExcludedServiceTypeEntity>
                {
                    new ExcludedServiceTypeEntity((int)ShipmentTypeCode.iParcel, (int)iParcelServiceType.Saver)
                });


            RateCache.Instance.Clear();

            orderManager = new Mock<IOrderManager>();
            orderManager.Setup(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>()));

            labelService = new iParcelLabelService(repository.Object, serviceGateway.Object, orderManager.Object);
        }

        [Fact]
        public void ProcessShipment_ThrowsArgumentNullException_WhenShipmentEntityIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => labelService.Create(null));
        }

        [Fact]
        public void ProcessShipment_ThermalTypeIsZPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsZPL_Test()
        {
            shipment.RequestedLabelFormat = (int)ThermalLanguage.ZPL;

            labelService.Create(shipment);

            Assert.Equal((int)ThermalLanguage.ZPL, shipment.ActualLabelFormat);
        }

        [Fact]
        public void ProcessShipment_ThermalTypeIsEPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsEPL_Test()
        {
            shipment.RequestedLabelFormat = (int)ThermalLanguage.EPL;

            labelService.Create(shipment);

            Assert.Equal((int)ThermalLanguage.EPL, shipment.ActualLabelFormat);
        }

        [Fact]
        public void ProcessShipment_ThermalTypeIsNull_WhenThermalTypeSettingIsFalse_Test()
        {
            labelService.Create(shipment);

            Assert.Null(shipment.ActualLabelFormat);
        }

        [Fact]
        public void ProcessShipment_DelegatesToRepositoryForAccount_Test()
        {
            labelService.Create(shipment);

            repository.Verify(r => r.GetiParcelAccount(shipment), Times.Once());
        }

        [Fact]
        public void ProcessShipment_DelegatesToRepositoryForOrderDetails_Test()
        {
            labelService.Create(shipment);

            orderManager.Verify(r => r.PopulateOrderDetails(shipment), Times.Once());
        }

        [Fact]
        public void ProcessShipment_DelegatesToServiceGateway_Test()
        {
            labelService.Create(shipment);

            serviceGateway.Verify(g => g.SubmitShipment(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [Fact]
        public void ProcessShipment_DelegatesToRepositoryToSaveLabel_Test()
        {
            labelService.Create(shipment);

            repository.Verify(r => r.SaveLabel(shipment, It.IsAny<DataSet>()), Times.Once());
        }

        [Fact]
        public void ProcessShipment_DelegatesToRepositoryToSaveTracking_Test()
        {
            labelService.Create(shipment);

            repository.Verify(r => r.SaveTrackingInfoToEntity(shipment, It.IsAny<DataSet>()), Times.Once());
        }

        private DataSet GetDeliveredPackageTrackingInfo()
        {
            using (DataSet trackingDataSet = new DataSet())
            {
                using (StringReader stringReader = new StringReader(GetDeliveredPackageTrackingXml()))
                {
                    trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
                }

                return trackingDataSet;
            }
        }

        private string GetDeliveredPackageTrackingXml()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<iparcelTrackingResponse xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <PackageTrackingInfo>
	<TrackingNumber>123456789</TrackingNumber>
	<PackageDestinationLocation>
	  <City>Tokyo</City>
	  <PostalCode>120-0001</PostalCode>
	  <CountryCode>JP</CountryCode>
	</PackageDestinationLocation>
	<EstimatedArrivalDate>2004-08-24T00:00:00+09:00</EstimatedArrivalDate>
	<TrackingEventHistory>
	  <TrackingEventDetail>
		<EventCode>EVENT_301</EventCode>
		<EventCodeDesc>DELIVERED</EventCodeDesc>
		<EventDateTime>2004-08-24T11:00:00+09:00</EventDateTime>
		<EventLocation>
		  <City>TOKYO</City>
		  <PostalCode>121-0001</PostalCode>
		  <CountryCode>JP</CountryCode>
		</EventLocation>
		<AdditionalLocationInfo>PATIO</AdditionalLocationInfo>
		<SignedForByName>ISHIRO</SignedForByName>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_302</EventCode>
		<EventCodeDesc>OUT FOR DELIVERY</EventCodeDesc>
		<EventDateTime>2001-08-24T07:00:11+09:00</EventDateTime>
		<EventLocation>
		  <City>TOKYO</City>
		  <PostalCode>121-0001</PostalCode>
		  <CountryCode>JP</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_201</EventCode>
		<EventCodeDesc>ARRIVAL SCAN</EventCodeDesc>
		<EventDateTime>2004-08-24T05:05:00+09:00</EventDateTime>
		<EventLocation>
		  <City>TOKYO</City>
		  <PostalCode>121-0001</PostalCode>
		  <CountryCode>JP</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_205</EventCode>
		<EventCodeDesc>COMPLETED CUSTOMS CLEARANCE
PROCESS</EventCodeDesc>
		<EventDateTime>2004-08-23T13:30:03+09:00</EventDateTime>
		<EventLocation>
		  <City>NARITA</City>
		  <PostalCode>282-8600</PostalCode>
		  <CountryCode>JP</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_204</EventCode>
		<EventCodeDesc>INITIATED CUSTOMS CLEARANCE
PROCESS</EventCodeDesc>
		<EventDateTime>2004-08-22T19:30:15+09:00</EventDateTime>
		<EventLocation>
		  <City>NARITA</City>
		  <PostalCode>282-8600</PostalCode>
		  <CountryCode>JP</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_202</EventCode>
		<EventCodeDesc>DEPARTURE SCAN</EventCodeDesc>
		<EventDateTime>2004-08-21T05:30:08-08:00</EventDateTime>
		<EventLocation>
		  <City>LOS ANGELES</City>
		  <StateProvince>CA</StateProvince>
		  <PostalCode>90029</PostalCode>
		  <CountryCode>US</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_201</EventCode>
		<EventCodeDesc>ARRIVAL SCAN</EventCodeDesc>
		<EventDateTime>2004-08-20T23:30:45-08:00</EventDateTime>
		<EventLocation>
		  <City>LOS ANGELES</City>
		  <StateProvince>CA</StateProvince>
		  <PostalCode>90029</PostalCode>
		  <CountryCode>US</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	  <TrackingEventDetail>
		<EventCode>EVENT_102</EventCode>
		<EventCodeDesc>ORIGIN SCAN</EventCodeDesc>
		<EventDateTime>2004-08-20T17:30:03-08:00</EventDateTime>
		<EventLocation>
		  <City>FERNLEY</City>
		  <StateProvince>NV</StateProvince>
		  <PostalCode>89498</PostalCode>
		  <CountryCode>US</CountryCode>
		</EventLocation>
	  </TrackingEventDetail>
	</TrackingEventHistory>
  </PackageTrackingInfo>
</iparcelTrackingResponse>";
        }
    }
}