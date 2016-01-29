using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Net.Ship;
using System.Xml.Linq;
using ShipWorks.Shipping.Insurance;
using Moq;
using ShipWorks.Shipping.Carriers.iParcel;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Ship
{
    public class iParcelPackageInfoElementTest
    {
        private iParcelPackageInfoElement testObject;
        
        private ShipmentEntity shipment;
        private Mock<ITokenProcessor> tokenProcessor;

        public iParcelPackageInfoElementTest()
        {
            shipment = new ShipmentEntity()
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
                OriginCity = "Chicago",
                OriginStateProvCode = "IL",
                OriginPostalCode = "66666",
                OriginCountryCode = "RU",

                Order = new OrderEntity() { OrderTotal = 100.43M },

                IParcel = new IParcelShipmentEntity
                {
                    Reference = "reference-value",
                    Service = (int)iParcelServiceType.Preferred,
                    TrackByEmail = true,
                    TrackBySMS = true
                }
            };

            shipment.Order.OrderItems.Add(new OrderItemEntity { Description = "some description", Quantity = 2, Weight = 1.54, UnitPrice = 3.40M, SKU = "12345678" });
            shipment.Order.OrderItems.Add(new OrderItemEntity { Description = "another description", Quantity = 1, Weight = 5.54, UnitPrice = 4.90M, SKU = "12345678" });

            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, InsuranceValue = 50M, SkuAndQuantities = "000000, 5 | 111111, 6" });

            // Setup the token processor to just return the string that was passed in
            tokenProcessor = new Mock<ITokenProcessor>();
            tokenProcessor.Setup(t => t.Process(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns((string token, ShipmentEntity s) => token);

            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);
        }

        [Fact]
        public void Build_AddsPackagesElement()
        {
            XElement element = testObject.Build();

            Assert.Equal("Packages", element.Name.LocalName);
        }

        [Fact]
        public void Build_AddsPackageInfoElement()
        {
            XElement element = testObject.Build();

            Assert.NotNull(element.XPathSelectElement("/PackageInfo"));
        }

        [Fact]
        public void Build_PackageInfoPackageNum()
        {
            XElement element = testObject.Build();

            XElement queriedElement = element.XPathSelectElement("/PackageInfo/PackageNum");
            Assert.Equal("1", queriedElement.Value);
        }

        #region Package Info Tests
        
        [Fact]
        public void Build_PackageInfoPackageNum_WithMultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/PackageNum").ToList();
            
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_PackageInfoContainsGeneralNode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General").ToList();
            Assert.Equal(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsGeneralNode_WithMultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General").ToList();
            Assert.Equal(2, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsShipperNode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper").ToList();
            Assert.Equal(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsShipperNode_WithMultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper").ToList();
            Assert.Equal(2, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsConsigneeNode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee").ToList();
            Assert.Equal(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsConsigneeNode_WithMultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee").ToList();
            Assert.Equal(2, queriedElements.Count);
        }
        
        [Fact]
        public void Build_PackageInfoContainsContentsNode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents").ToList();
            Assert.Equal(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsContentsNode_WithMultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents").ToList();
            Assert.Equal(2, queriedElements.Count);
        }

        #endregion Package Info Tests



        #region General Node Tests

        [Fact]
        public void Build_GeneralNode_PaymentCurrency()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PaymentCurrency").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("USD", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DDPIsOne_WhenDeliveryDutyPaidIsTrue()
        {
            shipment.IParcel.IsDeliveryDutyPaid = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/DDP").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DDPIsZero_WhenDeliveryDutyPaidIsFalse()
        {
            shipment.IParcel.IsDeliveryDutyPaid = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/DDP").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_PackageNum()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PackageNum").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_PackageNum_WithMultiplePackage()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PackageNum").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_GeneralNode_Service()
        {
            XElement element = testObject.Build();

            // The actual value that gets applied is deferred to another class, so just check that the service node is present
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Service").ToList();
            Assert.Equal(1, queriedElements.Count);
        }

        [Fact]
        public void Build_GeneralNode_ServiceIsZero_WhenUsedForRatesIsTrue()
        {
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, true, true);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Service").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Reference()
        {
            shipment.IParcel.Reference = "Some reference";

            XElement element = testObject.Build();

            // The mocked token processor just returns whatever is passed to it
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Reference").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Some reference", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Reference_ProcessesTokens()
        {
            shipment.IParcel.Reference = "Some reference";

            XElement element = testObject.Build();

            tokenProcessor.Verify(t => t.Process(shipment.IParcel.Reference, shipment), Times.Once());
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_ForInternationalShipment()
        {
            shipment.IParcel.Packages[0].DeclaredValue = 4.23M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("4.23", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DoesNotContainCustomsValueCurrencyNode_ForDomesticShipment()
        {
            shipment.CustomsValue = 4.23M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, true, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValueCurrency").ToList();
            Assert.Equal(0, queriedElements.Count);
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_WhenShipmentIsInternational()
        {
            shipment.IParcel.Packages[0].DeclaredValue = 100M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("100", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_WhenShipmentIsInternational_MultiplePackages()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages[0].DeclaredValue = 100.22M;
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, DeclaredValue = 120M, SkuAndQuantities = "98765, 20" });

            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);
            
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("100.22", queriedElements[0].Value);
            Assert.Equal("120", queriedElements[1].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_SalePrice()
        {
            shipment.Order.OrderTotal = 100.43M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/SalePrice").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("100.43", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Items()
        {
            // Two items were already added in the Initialize method

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Items").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("2", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_WeightUnits()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/WeightUnits").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("LBS", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Weight()
        {
            // Package weight was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Weight").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0.77", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_MeasureUnits()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/MeasureUnits").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("IN", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Length()
        {
            // Package length was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Length").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("4", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Width()
        {
            // Package width was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Width").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("6", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Height()
        {
            // Package height was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Height").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("10", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_TrackingBarCode()
        {
            XElement element = testObject.Build();

            // Tracking bar code node should be present but has an empty string as the value
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/TrackingBarcode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal(string.Empty, queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_GeneralDescription()
        {
            XElement element = testObject.Build();

            // General description node should be present but has an empty string as the value
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/GeneralDescription").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal(string.Empty, queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsTrueAndProviderIsShipWorks()
        {
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
            shipment.IParcel.Packages[0].Insurance = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsTrueAndProviderIsInvalidTest()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Invalid;
            shipment.IParcel.Packages[0].Insurance = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsCarrierAndPackageValueGreaterThan100()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsShipWorksAndPackageValueGreaterThan100()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.ShipWorks;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsInvalidAndPackageValueGreaterThan100()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Invalid;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_ContainsInsuranceNode_WhenInsuranceIsTrueAndProviderIsCarrierAndPackageValueGreaterThan100()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            shipment.IParcel.Packages[0].Insurance = true;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("100.01", queriedElements[0].Value);
        }

        #endregion General Node Tests


        #region Shipper Tests

        [Fact]
        public void Build_ShipperNode_PackageNum()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PackageNum").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_PackageNum_WithMultiplePackage()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PackageNum").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ShipperNode_Name()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Name").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Bill Lumbergh", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_Address1()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Address1").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("500 First Street", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_Address2()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Address2").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Suite 200", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_City()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/City").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Chicago", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_StateProvince()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/StateProvince").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("IL", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_PostCode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PostCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("66666", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_CountryCode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/CountryCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("RU", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_CountryCodeIsAdjustedForUK()
        {
            shipment.OriginCountryCode = "UK";
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/CountryCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("GB", queriedElements[0].Value);
        }
        
        #endregion Shipper Tests

        



        #region Consignee Tests

        [Fact]
        public void Build_ConsigneeNode_PackageNum()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PackageNum").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_PackageNum_WithMultiplePackage()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PackageNum").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Name()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Name").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Peter Gibbons", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Address1()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Address1").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1 Main Street", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Address2()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Address2").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("Suite 500", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_City()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/City").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("St. Louis", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_StateProvince()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/StateProvince").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("MO", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_PostCode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PostCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("63102", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_CountryCode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/CountryCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("US", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_CountryCodeIsAdjustedForUK()
        {
            shipment.ShipCountryCode = "UK";
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/CountryCode").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("GB", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Phone()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Phone").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("555-555-5555", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Email()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Email").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("someone@nowhere.com", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackBySMSIsOne_WhenTrackBySMSIsTrue()
        {
            shipment.IParcel.TrackBySMS = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackBySMS").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackBySMSIsZero_WhenTrackBySMSIsFalse()
        {
            shipment.IParcel.TrackBySMS = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackBySMS").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackByEmailIsOne_WhenTrackByEmailIsTrue()
        {
            shipment.IParcel.TrackByEmail = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackByEmail").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackByEmailIsZero_WhenTrackByEmailIsFalse()
        {
            shipment.IParcel.TrackByEmail = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackByEmail").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
        }

        #endregion Consignee Tests



        #region Contents Tests

        [Fact]
        public void Build_ContentsNode_PackageNum()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/PackageNum").ToList();
            Assert.Equal(1, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ContentsNode_PackageNum_WithMultiplePackage()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/PackageNum").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[1].Value);
        }



        [Fact]
        public void Build_ItemNode_PackageNum()
        {
            XElement element = testObject.Build();

            // There are two items, so we have to double the elements returned
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/PackageNum").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ItemNode_PackageNum_WithMultiplePackage()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20 | 123456, 10" });

            XElement element = testObject.Build();

            // There are two items, so we have to double the elements returned
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/PackageNum").ToList();
            Assert.Equal(4, queriedElements.Count);
            Assert.Equal("1", queriedElements[0].Value);
            Assert.Equal("2", queriedElements[2].Value);
        }

        [Fact]
        public void Build_ItemNode_Description()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Description").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("000000", queriedElements[0].Value);
            Assert.Equal("111111", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Quantity()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Items").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("5", queriedElements[0].Value);
            Assert.Equal("6", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Weight()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Weight").ToList();
            Assert.Equal(2, queriedElements.Count);

            Assert.Equal("0", queriedElements[0].Value);
            Assert.Equal("0", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Value()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Value").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("0", queriedElements[0].Value);
            Assert.Equal("0", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_HSCode()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/HSCode").ToList();
            Assert.Equal(2, queriedElements.Count);

            for (int i = 0; i < queriedElements.Count; i++)
            {
                Assert.Equal(string.Empty, queriedElements[i].Value);
            }
        }

        [Fact]
        public void Build_ItemNode_SKU()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/SKU").ToList();
            Assert.Equal(2, queriedElements.Count);
            Assert.Equal("000000", queriedElements[0].Value);
            Assert.Equal("111111", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_CountryOfManufacture()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/CountryOfMan").ToList();
            Assert.Equal(2, queriedElements.Count);

            for (int i = 0; i < queriedElements.Count; i++)
            {
                Assert.Equal(string.Empty, queriedElements[i].Value);
            }
        }

        [Fact]
        public void Build_ItemNode_ContentInfo_SinglePackage()
        {
            XElement element = testObject.Build();

            List<XElement> skuElements = element.XPathSelectElements("/PackageInfo/Contents/Item/SKU").ToList();
            List<XElement> quantityElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Items").ToList();

            // Since the  majority of this is being delegated to a different class, just check that the SKU/Quantity 
            // fields are being populated as we would expect given the data setup in the Initialize method
            Assert.Equal(2, skuElements.Count);
            Assert.Equal(2, quantityElements.Count);

            Assert.Equal("000000", skuElements[0].Value);
            Assert.Equal("5", quantityElements[0].Value);

            Assert.Equal("111111", skuElements[1].Value);
            Assert.Equal("6", quantityElements[1].Value);
        }

        #endregion Contents Tests
    }
}
