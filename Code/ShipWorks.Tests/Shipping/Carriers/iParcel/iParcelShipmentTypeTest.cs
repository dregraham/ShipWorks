﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using System.Data;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping;
using System.IO;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    [TestClass]
    public class iParcelShipmentTypeTest
    {
        private iParcelShipmentType testObject;

        private Mock<IiParcelServiceGateway> serviceGateway;
        private Mock<IiParcelRepository> repository;

        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
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
            repository.Setup(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>()));

            testObject = new iParcelShipmentType(repository.Object, serviceGateway.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProcessShipment_ThrowsArgumentNullException_WhenShipmentEntityIsNull_Test()
        {
            testObject.ProcessShipment(null);
        }


        [TestMethod]
        public void ProcessShipment_DelegatesToRepositoryForSettings_Test()
        {
            testObject.ProcessShipment(shipment);

            repository.Verify(r => r.GetShippingSettings(), Times.Once());
        }

        [TestMethod]
        public void ProcessShipment_ThermalTypeIsZPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsZPL_Test()
        {
            // ZPL is not actually supported, but it's value is not zero, so we can make sure that the value is set
            ShippingSettingsEntity settings = new ShippingSettingsEntity { IParcelThermal = true, IParcelThermalType = (int) ThermalLanguage.ZPL};
            repository.Setup(r => r.GetShippingSettings()).Returns(settings);

            testObject.ProcessShipment(shipment);

            Assert.AreEqual((int) ThermalLanguage.ZPL, shipment.ThermalType);
        }

        [TestMethod]
        public void ProcessShipment_ThermalTypeIsEPL_WhenThermalTypeSettingIsTrue_AndThermalTypeIsEPL_Test()
        {
            ShippingSettingsEntity settings = new ShippingSettingsEntity { IParcelThermal = true, IParcelThermalType = (int)ThermalLanguage.EPL };
            repository.Setup(r => r.GetShippingSettings()).Returns(settings);

            testObject.ProcessShipment(shipment);

            Assert.AreEqual((int)ThermalLanguage.EPL, shipment.ThermalType);
        }

        [TestMethod]
        public void ProcessShipment_ThermalTypeIsNull_WhenThermalTypeSettingIsFalse_Test()
        {
            ShippingSettingsEntity settings = new ShippingSettingsEntity { IParcelThermal = false };
            repository.Setup(r => r.GetShippingSettings()).Returns(settings);

            testObject.ProcessShipment(shipment);

            Assert.IsNull(shipment.ThermalType);
        }

        [TestMethod]
        public void ProcessShipment_DelegatesToRepositoryForAccount_Test()
        {
            testObject.ProcessShipment(shipment);

            repository.Verify(r => r.GetiParcelAccount(shipment),  Times.Once());
        }

        [TestMethod]
        public void ProcessShipment_DelegatesToRepositoryForOrderDetails_Test()
        {
            testObject.ProcessShipment(shipment);

            repository.Verify(r => r.PopulateOrderDetails(shipment), Times.Once());
        }

        [TestMethod]
        public void ProcessShipment_DelegatesToServiceGateway_Test()
        {
            testObject.ProcessShipment(shipment);

            serviceGateway.Verify(g => g.SubmitShipment(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [TestMethod]
        public void ProcessShipment_DelegatesToRepositoryToSaveLabel_Test()
        {
            testObject.ProcessShipment(shipment);

            repository.Verify(r => r.SaveLabel(shipment, It.IsAny<DataSet>()), Times.Once());
        }

        [TestMethod]
        public void ProcessShipment_DelegatesToRepositoryToSaveTracking_Test()
        {
            testObject.ProcessShipment(shipment);

            repository.Verify(r => r.SaveTrackingInfoToEntity(shipment, It.IsAny<DataSet>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TrackShipment_ThrowsArgumentNullException_Test()
        {
            testObject.TrackShipment(null);

        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void TrackShipment_ThrowsShippingException_WhenIParcelShipmentIsNull_Test()
        {
            shipment = new ShipmentEntity();
            testObject.TrackShipment(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(ShippingException))]
        public void TrackShipment_ThrowsShippingException_WhenIParcelPackageListIsEmpty_Test()
        {
            while (shipment.IParcel.Packages.Count > 0)
            {
                // remove all the packages from the shipment
                shipment.IParcel.Packages.RemoveAt(0);
            }

            testObject.TrackShipment(shipment);
        }

        [TestMethod]
        public void TrackShipment_DelegatesToRepository_Test()
        {
            testObject.TrackShipment(shipment);
            
            repository.Verify(r => r.GetiParcelAccount(shipment), Times.Once());
        }

        [TestMethod]
        public void TrackShipment_DelegatesToServiceGateway_Test()
        {
            testObject.TrackShipment(shipment);

            serviceGateway.Verify(s => s.TrackShipment(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [TestMethod]
        public void TrackShipment_ExtractsTrackingInfo_ForDeliveredShipment_Test()
        {
            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.IsTrue(trackingResult.Summary.ToLower().Contains("<b>delivered</b> on 8/23/2004 9:00 pm"));
        }

        [TestMethod]
        public void TrackShipment_ExtractsTrackingInfo_ForShipmentNotDelivered_Test()
        {
            serviceGateway.Setup(s => s.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUndeliveredPackageTrackingInfo());

            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.AreEqual("<b>Package details received electronically from Seller</b>", trackingResult.Summary);
        }

        [TestMethod]
        public void GetRates_DelegatesToRepositoryForOrderDetails_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            repository.Verify(r => r.PopulateOrderDetails(shipment), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToRepositoryForAccount_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            repository.Verify(r => r.GetiParcelAccount(shipment), Times.Once());
        }

        [TestMethod]
        public void GetRates_DelegatesToServiceGateway_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            serviceGateway.Verify(s => s.GetRates(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [TestMethod]
        public void GetRates_UnsupportedShipments_RateCountIsZero_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            Assert.AreEqual(0, rates.Rates.Count);
        }


        [TestMethod]
        public void GetRates_RateCountIsTwo_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetRateResultsInfo());

            RateGroup rates = testObject.GetRates(shipment);

            Assert.AreEqual(2, rates.Rates.Count);
        }




        private DataSet GetUndeliveredPackageTrackingInfo()
        {
            using (DataSet trackingDataSet = new DataSet())
            {
                using (StringReader stringReader = new StringReader(GetUndeliveredPackageTrackingXml()))
                {
                    trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
                }

                return trackingDataSet;
            }
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


        private DataSet GetRateResultsInfo()
        {
            using (DataSet trackingDataSet = new DataSet())
            {
                using (StringReader stringReader = new StringReader(GetValidRatesXml()))
                {
                    trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
                }

                return trackingDataSet;
            }
        }


        private DataSet GetUnsuppportedRatesInfo()
        {
            using (DataSet trackingDataSet = new DataSet())
            {
                using (StringReader stringReader = new StringReader(GetRatesWithUnsupportedServiceTypesXml()))
                {
                    trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
                }

                return trackingDataSet;
            }
        }

        private string GetValidRatesXml()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<iParcelPackageResponse xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
	<PackageInfo>
		<Reference>Order 144</Reference>
		<CostInfo>
			<Service>112</Service>
			<ServiceName>i-Parcel Immediate</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>101.8500</PackageShipping>
			<PackageTotalUSD>101.85000000</PackageTotalUSD>
			<PackageTotalCurrency>101.85</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<CostInfo>
			<Service>115</Service>
			<ServiceName>i-Parcel Preferred</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>88.1800</PackageShipping>
			<PackageTotalUSD>88.18000000</PackageTotalUSD>
			<PackageTotalCurrency>88.18</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>AMEX</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>JCB</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>MASTERCARD</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>PAYPAL</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>VISA</PaymentMethod>
			</PaymentMethods>
			<AddressInfo>
				<AddressEntered>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressEntered>
				<AddressLessDiacritics>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressLessDiacritics>
			</AddressInfo>
		</PaymentMethods>
	</PackageInfo>
</iParcelPackageResponse>";
        }

        private string GetRatesWithUnsupportedServiceTypesXml()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<iParcelPackageResponse xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
	<PackageInfo>
		<Reference>Order 144</Reference>
		<CostInfo>
			<Service>112</Service>
			<ServiceName>i-Parcel Immediate</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>-2</PackageShipping>
			<PackageTotalUSD>-2.00000000</PackageTotalUSD>
			<PackageTotalCurrency>-2</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<CostInfo>
			<Service>115</Service>
			<ServiceName>i-Parcel Preferred</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>-2</PackageShipping>
			<PackageTotalUSD>-2.00000000</PackageTotalUSD>
			<PackageTotalCurrency>-2</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>AMEX</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>JCB</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>MASTERCARD</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>PAYPAL</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>VISA</PaymentMethod>
			</PaymentMethods>
			<AddressInfo>
				<AddressEntered>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressEntered>
				<AddressLessDiacritics>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressLessDiacritics>
			</AddressInfo>
		</PaymentMethods>
	</PackageInfo>
	<PackageInfo>
		<Reference>Order 144</Reference>
		<CostInfo>
			<Service>112</Service>
			<ServiceName>i-Parcel Immediate</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>54.4200</PackageShipping>
			<PackageTotalUSD>54.42000000</PackageTotalUSD>
			<PackageTotalCurrency>54.42</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<CostInfo>
			<Service>115</Service>
			<ServiceName>i-Parcel Preferred</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>43.3400</PackageShipping>
			<PackageTotalUSD>43.34000000</PackageTotalUSD>
			<PackageTotalCurrency>43.34</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>AMEX</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>JCB</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>MASTERCARD</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>PAYPAL</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>VISA</PaymentMethod>
			</PaymentMethods>
			<AddressInfo>
				<AddressEntered>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressEntered>
				<AddressLessDiacritics>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressLessDiacritics>
			</AddressInfo>
		</PaymentMethods>
	</PackageInfo>
	<PackageInfo>
		<Reference>Order 144</Reference>
		<CostInfo>
			<Service>112</Service>
			<ServiceName>i-Parcel Immediate</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>57.5300</PackageShipping>
			<PackageTotalUSD>57.53000000</PackageTotalUSD>
			<PackageTotalCurrency>57.53</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<CostInfo>
			<Service>115</Service>
			<ServiceName>i-Parcel Preferred</ServiceName>
			<PackageDuty>0.00000000</PackageDuty>
			<PackageTax>0.00000000</PackageTax>
			<PackageShipping>46.2800</PackageShipping>
			<PackageTotalUSD>46.28000000</PackageTotalUSD>
			<PackageTotalCurrency>46.28</PackageTotalCurrency>
			<InCurrency>USD</InCurrency>
			<PackageInsurance>0.0000</PackageInsurance>
		</CostInfo>
		<PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>AMEX</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>JCB</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>MASTERCARD</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>PAYPAL</PaymentMethod>
			</PaymentMethods>
			<PaymentMethods>
				<Country>US</Country>
				<PaymentMethod>VISA</PaymentMethod>
			</PaymentMethods>
			<AddressInfo>
				<AddressEntered>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressEntered>
				<AddressLessDiacritics>Ship Test|One Memorial Drive|Suite 2000|St. Louis|MO|63102|US</AddressLessDiacritics>
			</AddressInfo>
		</PaymentMethods>
	</PackageInfo>
</iParcelPackageResponse>";
        }

        private string GetUndeliveredPackageTrackingXml()
        {
            return @"<iparcelTrackingResponse xmlns="""">
						<PackageTrackingInfo>
							<TrackingNumber>1216156584US</TrackingNumber>
							<PackageDestinationLocation>
								<City>Saint Louis</City>
								<StateProvince>MO</StateProvince>
								<PostalCode>63102</PostalCode>
								<CountryCode>US</CountryCode>
							</PackageDestinationLocation>
							<TrackingEventHistory>
								<TrackingEventDetail>
									<EventCode>Event</EventCode>
									<EventCodeDesc>Package details received electronically from Seller</EventCodeDesc>
									<EventDateTime>2013-03-21T14:31:25-05:00</EventDateTime>
									<EventLocation>
										<City>Elizabeth</City>
										<StateProvince>NJ</StateProvince>
										<CountryCode>US</CountryCode>
									</EventLocation>
								</TrackingEventDetail>
							</TrackingEventHistory>
						</PackageTrackingInfo>
					</iparcelTrackingResponse>";
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
