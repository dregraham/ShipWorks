using System;
using Xunit;
using ShipWorks.Shipping.Carriers.iParcel;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using System.Data;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping;
using System.IO;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Editing;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Settings;
using System.Collections.Generic;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelShipmentTypeTest
    {
        private iParcelShipmentType testObject;

        private Mock<IiParcelServiceGateway> serviceGateway;
        private Mock<IiParcelRepository> repository;

        private ShipmentEntity shipment;

        public iParcelShipmentTypeTest()
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
            repository.Setup(r => r.PopulateOrderDetails(It.IsAny<ShipmentEntity>()));

            Mock<IExcludedServiceTypeRepository> excludedServiceTypeRepository = new Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepository.Setup(x => x.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(new List<ExcludedServiceTypeEntity>
                {
                    new ExcludedServiceTypeEntity((int)ShipmentTypeCode.iParcel, (int)iParcelServiceType.Saver)
                });


            RateCache.Instance.Clear();

            testObject = new iParcelShipmentType(repository.Object, serviceGateway.Object, excludedServiceTypeRepository.Object);
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue_Test()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void TrackShipment_ThrowsArgumentNullException_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.TrackShipment(null));

        }

        [Fact]
        public void TrackShipment_ThrowsShippingException_WhenIParcelShipmentIsNull_Test()
        {
            shipment = new ShipmentEntity();
            Assert.Throws<ShippingException>(() => testObject.TrackShipment(shipment));
        }

        [Fact]
        public void TrackShipment_ThrowsShippingException_WhenIParcelPackageListIsEmpty_Test()
        {
            while (shipment.IParcel.Packages.Count > 0)
            {
                // remove all the packages from the shipment
                shipment.IParcel.Packages.RemoveAt(0);
            }

            Assert.Throws<ShippingException>(() => testObject.TrackShipment(shipment));
        }

        [Fact]
        public void TrackShipment_DelegatesToRepository_Test()
        {
            testObject.TrackShipment(shipment);

            repository.Verify(r => r.GetiParcelAccount(shipment), Times.Once());
        }

        [Fact]
        public void TrackShipment_DelegatesToServiceGateway_Test()
        {
            testObject.TrackShipment(shipment);

            serviceGateway.Verify(s => s.TrackShipment(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [Fact]
        public void TrackShipment_ExtractsTrackingInfo_ForDeliveredShipment_Test()
        {
            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.True(trackingResult.Summary.ToLower().Contains("<b>delivered</b> on 8/23/2004 9:00 pm"));
        }

        [Fact]
        public void TrackShipment_ExtractsTrackingInfo_ForShipmentNotDelivered_Test()
        {
            serviceGateway.Setup(s => s.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUndeliveredPackageTrackingInfo());

            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.Equal("<b>Package details received electronically from Seller</b>", trackingResult.Summary);
        }

        [Fact]
        public void GetRates_DelegatesToRepositoryForOrderDetails_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            repository.Verify(r => r.PopulateOrderDetails(shipment), Times.Once());
        }

        [Fact]
        public void GetRates_DelegatesToRepositoryForAccount_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            repository.Verify(r => r.GetiParcelAccount(shipment), Times.Once());
        }

        [Fact]
        public void GetRates_DelegatesToServiceGateway_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            serviceGateway.Verify(s => s.GetRates(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [Fact]
        public void GetRates_UnsupportedShipments_RateCountIsZero_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetUnsuppportedRatesInfo);

            RateGroup rates = testObject.GetRates(shipment);

            Assert.Equal(0, rates.Rates.Count);
        }


        [Fact]
        public void GetRates_RateCountIsTwo_Test()
        {
            serviceGateway.Setup(s => s.GetRates(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>())).Returns(GetRateResultsInfo());

            RateGroup rates = testObject.GetRates(shipment);

            Assert.Equal(2, rates.Rates.Count);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUS_WithDestinationInUK_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Other, OriginCountryCode = "US", ShipCountryCode = "UK" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUS_WithDestinationInUS_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Other, OriginCountryCode = "US", ShipCountryCode = "US" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUK_WithDestinationInRU_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Other, OriginCountryCode = "UK", ShipCountryCode = "RU" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUK_WithDestinationInUK_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Other, OriginCountryCode = "UK", ShipCountryCode = "UK" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUS_WithDestinationInUS_AndShipmentUsesAccountAddress_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Account, OriginCountryCode = "US", ShipCountryCode = "US" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUK_WithDestinationInRU_AndShipmentUsesAccountAddress_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Account, OriginCountryCode = "UK", ShipCountryCode = "RU" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUK_WithDestinationInUK_AndShipmentUsesAccountAddress_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int)ShipmentOriginSource.Account, OriginCountryCode = "UK", ShipCountryCode = "UK" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
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
