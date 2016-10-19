using System;
using System.Data;
using System.IO;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelShipmentTypeTest
    {
        private iParcelShipmentType testObject;
        private ShipmentEntity shipment;
        private AutoMock mock;

        public iParcelShipmentTypeTest()
        {
            shipment = Create.Shipment(new OrderEntity()).AsIParcel(x => x.WithPackage()).Build();
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<iParcelShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void TrackShipment_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.TrackShipment(null));
        }

        [Fact]
        public void TrackShipment_ThrowsShippingException_WhenIParcelShipmentIsNull()
        {
            shipment = new ShipmentEntity();
            Assert.Throws<ShippingException>(() => testObject.TrackShipment(shipment));
        }

        [Fact]
        public void TrackShipment_ThrowsShippingException_WhenIParcelPackageListIsEmpty()
        {
            shipment.IParcel.Packages.Clear();

            Assert.Throws<ShippingException>(() => testObject.TrackShipment(shipment));
        }

        [Fact]
        public void TrackShipment_DelegatesToRepository()
        {
            mock.Mock<IiParcelServiceGateway>()
                .Setup(g => g.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>()))
                .Returns(GetDeliveredPackageTrackingInfo());
            shipment.IParcel.IParcelAccountID = 996;

            testObject.TrackShipment(shipment);

            mock.Mock<ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity>>()
                .Verify(r => r.GetAccountReadOnly(996), Times.Once());
        }

        [Fact]
        public void TrackShipment_DelegatesToServiceGateway()
        {
            mock.Mock<IiParcelServiceGateway>()
                .Setup(g => g.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>()))
                .Returns(GetDeliveredPackageTrackingInfo());

            testObject.TrackShipment(shipment);

            mock.Mock<IiParcelServiceGateway>()
                .Verify(s => s.TrackShipment(It.IsAny<iParcelCredentials>(), shipment), Times.Once());
        }

        [Fact]
        public void TrackShipment_ExtractsTrackingInfo_ForDeliveredShipment()
        {
            mock.Mock<IiParcelServiceGateway>()
                .Setup(g => g.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>()))
                .Returns(GetDeliveredPackageTrackingInfo());

            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.True(trackingResult.Summary.ToLower().Contains("<b>delivered</b> on 8/23/2004 9:00 pm"));
        }

        [Fact]
        public void TrackShipment_ExtractsTrackingInfo_ForShipmentNotDelivered()
        {
            mock.Mock<IiParcelServiceGateway>()
                .Setup(g => g.TrackShipment(It.IsAny<iParcelCredentials>(), It.IsAny<ShipmentEntity>()))
                .Returns(GetUndeliveredPackageTrackingInfo());

            TrackingResult trackingResult = testObject.TrackShipment(shipment);

            Assert.Equal("<b>Package details received electronically from Seller</b>", trackingResult.Summary);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUS_WithDestinationInUK()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Other, OriginCountryCode = "US", ShipCountryCode = "UK" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUS_WithDestinationInUS()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Other, OriginCountryCode = "US", ShipCountryCode = "US" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUK_WithDestinationInRU()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Other, OriginCountryCode = "UK", ShipCountryCode = "RU" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUK_WithDestinationInUK()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Other, OriginCountryCode = "UK", ShipCountryCode = "UK" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsNullShippingBroker_ForShipmentOriginatingInUS_WithDestinationInUS_AndShipmentUsesAccountAddress()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Account, OriginCountryCode = "US", ShipCountryCode = "US" };

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<NullShippingBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUK_WithDestinationInRU_AndShipmentUsesAccountAddress()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Account, OriginCountryCode = "UK", ShipCountryCode = "RU" };

            iParcelShipmentType testObject = mock.Create<iParcelShipmentType>();

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
        }

        [Fact]
        public void GetShippingBroker_ReturnsiParcelBestRateBroker_ForShipmentOriginatingInUK_WithDestinationInUK_AndShipmentUsesAccountAddress()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity { OriginOriginID = (int) ShipmentOriginSource.Account, OriginCountryCode = "UK", ShipCountryCode = "UK" };

            iParcelShipmentType testObject = mock.Create<iParcelShipmentType>();

            IBestRateShippingBroker broker = testObject.GetShippingBroker(shipmentEntity);

            Assert.IsAssignableFrom<iParcelBestRateBroker>(broker);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAccountIDToZero_WithNoAccounts()
        {
            var profile = new ShippingProfileEntity { IParcel = new IParcelProfileEntity() };
            var testObject = mock.Create<iParcelShipmentType>();

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(0, profile.IParcel.IParcelAccountID);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAccountIDToFirstAccount_WithManyAccounts()
        {
            mock.Mock<ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity>>()
                .Setup(x => x.AccountsReadOnly)
                .Returns(new[]
                {
                    new IParcelAccountEntity { IParcelAccountID = 6 },
                    new IParcelAccountEntity { IParcelAccountID = 9 }
                });

            var profile = new ShippingProfileEntity { IParcel = new IParcelProfileEntity() };
            var testObject = mock.Create<iParcelShipmentType>();

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(6, profile.IParcel.IParcelAccountID);
        }




        private DataSet GetUndeliveredPackageTrackingInfo()
        {
            DataSet trackingDataSet = new DataSet();

            using (StringReader stringReader = new StringReader(GetUndeliveredPackageTrackingXml()))
            {
                trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
            }

            return trackingDataSet;
        }

        private DataSet GetDeliveredPackageTrackingInfo()
        {
            DataSet trackingDataSet = new DataSet();

            using (StringReader stringReader = new StringReader(GetDeliveredPackageTrackingXml()))
            {
                trackingDataSet.ReadXml(stringReader, XmlReadMode.Auto);
            }

            return trackingDataSet;
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
