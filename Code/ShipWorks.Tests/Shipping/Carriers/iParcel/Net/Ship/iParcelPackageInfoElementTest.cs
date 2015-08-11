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

        [TestInitialize]
        public void Initialize()
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
        public void Build_AddsPackagesElement_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("Packages", element.Name.LocalName);
        }

        [Fact]
        public void Build_AddsPackageInfoElement_Test()
        {
            XElement element = testObject.Build();

            Assert.IsNotNull(element.XPathSelectElement("/PackageInfo"));
        }

        [Fact]
        public void Build_PackageInfoPackageNum_Test()
        {
            XElement element = testObject.Build();

            XElement queriedElement = element.XPathSelectElement("/PackageInfo/PackageNum");
            Assert.AreEqual("1", queriedElement.Value);
        }

        #region Package Info Tests
        
        [Fact]
        public void Build_PackageInfoPackageNum_WithMultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/PackageNum").ToList();
            
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_PackageInfoContainsGeneralNode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General").ToList();
            Assert.AreEqual(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsGeneralNode_WithMultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General").ToList();
            Assert.AreEqual(2, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsShipperNode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper").ToList();
            Assert.AreEqual(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsShipperNode_WithMultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper").ToList();
            Assert.AreEqual(2, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsConsigneeNode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee").ToList();
            Assert.AreEqual(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsConsigneeNode_WithMultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee").ToList();
            Assert.AreEqual(2, queriedElements.Count);
        }
        
        [Fact]
        public void Build_PackageInfoContainsContentsNode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents").ToList();
            Assert.AreEqual(1, queriedElements.Count);
        }

        [Fact]
        public void Build_PackageInfoContainsContentsNode_WithMultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents").ToList();
            Assert.AreEqual(2, queriedElements.Count);
        }

        #endregion Package Info Tests



        #region General Node Tests

        [Fact]
        public void Build_GeneralNode_PaymentCurrency_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PaymentCurrency").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("USD", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DDPIsOne_WhenDeliveryDutyPaidIsTrue_Test()
        {
            shipment.IParcel.IsDeliveryDutyPaid = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/DDP").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DDPIsZero_WhenDeliveryDutyPaidIsFalse_Test()
        {
            shipment.IParcel.IsDeliveryDutyPaid = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/DDP").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_PackageNum_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PackageNum").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_PackageNum_WithMultiplePackage_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/PackageNum").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_GeneralNode_Service_Test()
        {
            XElement element = testObject.Build();

            // The actual value that gets applied is deferred to another class, so just check that the service node is present
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Service").ToList();
            Assert.AreEqual(1, queriedElements.Count);
        }

        [Fact]
        public void Build_GeneralNode_ServiceIsZero_WhenUsedForRatesIsTrue_Test()
        {
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, true, true);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Service").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Reference_Test()
        {
            shipment.IParcel.Reference = "Some reference";

            XElement element = testObject.Build();

            // The mocked token processor just returns whatever is passed to it
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Reference").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Some reference", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Reference_ProcessesTokens_Test()
        {
            shipment.IParcel.Reference = "Some reference";

            XElement element = testObject.Build();

            tokenProcessor.Verify(t => t.Process(shipment.IParcel.Reference, shipment), Times.Once());
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_ForInternationalShipment_Test()
        {
            shipment.IParcel.Packages[0].DeclaredValue = 4.23M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("4.23", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_DoesNotContainCustomsValueCurrencyNode_ForDomesticShipment_Test()
        {
            shipment.CustomsValue = 4.23M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, true, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValueCurrency").ToList();
            Assert.AreEqual(0, queriedElements.Count);
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_WhenShipmentIsInternational_Test()
        {
            shipment.IParcel.Packages[0].DeclaredValue = 100M;
            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("100", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_ContainsCustomsValueNode_WhenShipmentIsInternational_MultiplePackages_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages[0].DeclaredValue = 100.22M;
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, DeclaredValue = 120M, SkuAndQuantities = "98765, 20" });

            testObject = new iParcelPackageInfoElement(shipment, tokenProcessor.Object, false, false);
            
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/CustomsValue").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("100.22", queriedElements[0].Value);
            Assert.AreEqual("120", queriedElements[1].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_SalePrice_Test()
        {
            shipment.Order.OrderTotal = 100.43M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/SalePrice").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("100.43", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Items_Test()
        {
            // Two items were already added in the Initialize method

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Items").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("2", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_WeightUnits_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/WeightUnits").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("LBS", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Weight_Test()
        {
            // Package weight was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Weight").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0.77", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_MeasureUnits_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/MeasureUnits").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("IN", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Length_Test()
        {
            // Package length was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Length").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("4", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Width_Test()
        {
            // Package width was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Width").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("6", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_Height_Test()
        {
            // Package height was set in the Initialize method
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/Height").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("10", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_TrackingBarCode_Test()
        {
            XElement element = testObject.Build();

            // Tracking bar code node should be present but has an empty string as the value
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/TrackingBarcode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual(string.Empty, queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_GeneralDescription_Test()
        {
            XElement element = testObject.Build();

            // General description node should be present but has an empty string as the value
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/GeneralDescription").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual(string.Empty, queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsTrueAndProviderIsShipWorks_Test()
        {
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
            shipment.IParcel.Packages[0].Insurance = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsTrueAndProviderIsInvalidTest()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Invalid;
            shipment.IParcel.Packages[0].Insurance = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsCarrierAndPackageValueGreaterThan100_Test()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsShipWorksAndPackageValueGreaterThan100_Test()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.ShipWorks;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_GeneralNode_InsuranceNodeIsZero_WhenInsuranceIsFalseAndProviderIsInvalidAndPackageValueGreaterThan100_Test()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Invalid;
            shipment.IParcel.Packages[0].Insurance = false;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }
        
        [Fact]
        public void Build_GeneralNode_ContainsInsuranceNode_WhenInsuranceIsTrueAndProviderIsCarrierAndPackageValueGreaterThan100_Test()
        {
            shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            shipment.IParcel.Packages[0].Insurance = true;
            shipment.IParcel.Packages[0].InsuranceValue = 100.01M;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/General/InsuranceValue").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("100.01", queriedElements[0].Value);
        }

        #endregion General Node Tests


        #region Shipper Tests

        [Fact]
        public void Build_ShipperNode_PackageNum_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PackageNum").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_PackageNum_WithMultiplePackage_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PackageNum").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ShipperNode_Name_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Name").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Bill Lumbergh", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_Address1_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Address1").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("500 First Street", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_Address2_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/Address2").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Suite 200", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_City_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/City").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Chicago", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_StateProvince_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/StateProvince").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("IL", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_PostCode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/PostCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("66666", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_CountryCode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/CountryCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("RU", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ShipperNode_CountryCodeIsAdjustedForUK_Test()
        {
            shipment.OriginCountryCode = "UK";
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Shipper/CountryCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("GB", queriedElements[0].Value);
        }
        
        #endregion Shipper Tests

        



        #region Consignee Tests

        [Fact]
        public void Build_ConsigneeNode_PackageNum_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PackageNum").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_PackageNum_WithMultiplePackage_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PackageNum").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Name_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Name").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Peter Gibbons", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Address1_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Address1").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1 Main Street", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Address2_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Address2").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("Suite 500", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_City_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/City").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("St. Louis", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_StateProvince_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/StateProvince").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("MO", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_PostCode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/PostCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("63102", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_CountryCode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/CountryCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("US", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_CountryCodeIsAdjustedForUK_Test()
        {
            shipment.ShipCountryCode = "UK";
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/CountryCode").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("GB", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Phone_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Phone").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("555-555-5555", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_Email_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/Email").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("someone@nowhere.com", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackBySMSIsOne_WhenTrackBySMSIsTrue_Test()
        {
            shipment.IParcel.TrackBySMS = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackBySMS").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackBySMSIsZero_WhenTrackBySMSIsFalse_Test()
        {
            shipment.IParcel.TrackBySMS = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackBySMS").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackByEmailIsOne_WhenTrackByEmailIsTrue_Test()
        {
            shipment.IParcel.TrackByEmail = true;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackByEmail").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ConsigneeNode_TrackByEmailIsZero_WhenTrackByEmailIsFalse_Test()
        {
            shipment.IParcel.TrackByEmail = false;

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Consignee/TrackByEmail").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
        }

        #endregion Consignee Tests



        #region Contents Tests

        [Fact]
        public void Build_ContentsNode_PackageNum_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/PackageNum").ToList();
            Assert.AreEqual(1, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ContentsNode_PackageNum_WithMultiplePackage_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20" });

            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/PackageNum").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[1].Value);
        }



        [Fact]
        public void Build_ItemNode_PackageNum_Test()
        {
            XElement element = testObject.Build();

            // There are two items, so we have to double the elements returned
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/PackageNum").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
        }

        [Fact]
        public void Build_ItemNode_PackageNum_WithMultiplePackage_Test()
        {
            // Setup the test by adding another package
            shipment.IParcel.Packages.Add(new IParcelPackageEntity { Weight = .77, DimsHeight = 10, DimsLength = 4, DimsWidth = 6, SkuAndQuantities = "98765, 20 | 123456, 10" });

            XElement element = testObject.Build();

            // There are two items, so we have to double the elements returned
            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/PackageNum").ToList();
            Assert.AreEqual(4, queriedElements.Count);
            Assert.AreEqual("1", queriedElements[0].Value);
            Assert.AreEqual("2", queriedElements[2].Value);
        }

        [Fact]
        public void Build_ItemNode_Description_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Description").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("000000", queriedElements[0].Value);
            Assert.AreEqual("111111", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Quantity_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Items").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("5", queriedElements[0].Value);
            Assert.AreEqual("6", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Weight_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Weight").ToList();
            Assert.AreEqual(2, queriedElements.Count);

            Assert.AreEqual("0", queriedElements[0].Value);
            Assert.AreEqual("0", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_Value_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Value").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("0", queriedElements[0].Value);
            Assert.AreEqual("0", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_HSCode_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/HSCode").ToList();
            Assert.AreEqual(2, queriedElements.Count);

            for (int i = 0; i < queriedElements.Count; i++)
            {
                Assert.AreEqual(string.Empty, queriedElements[i].Value);
            }
        }

        [Fact]
        public void Build_ItemNode_SKU_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/SKU").ToList();
            Assert.AreEqual(2, queriedElements.Count);
            Assert.AreEqual("000000", queriedElements[0].Value);
            Assert.AreEqual("111111", queriedElements[1].Value);
        }

        [Fact]
        public void Build_ItemNode_CountryOfManufacture_Test()
        {
            XElement element = testObject.Build();

            List<XElement> queriedElements = element.XPathSelectElements("/PackageInfo/Contents/Item/CountryOfMan").ToList();
            Assert.AreEqual(2, queriedElements.Count);

            for (int i = 0; i < queriedElements.Count; i++)
            {
                Assert.AreEqual(string.Empty, queriedElements[i].Value);
            }
        }

        [Fact]
        public void Build_ItemNode_ContentInfo_SinglePackage_Test()
        {
            XElement element = testObject.Build();

            List<XElement> skuElements = element.XPathSelectElements("/PackageInfo/Contents/Item/SKU").ToList();
            List<XElement> quantityElements = element.XPathSelectElements("/PackageInfo/Contents/Item/Items").ToList();

            // Since the  majority of this is being delegated to a different class, just check that the SKU/Quantity 
            // fields are being populated as we would expect given the data setup in the Initialize method
            Assert.AreEqual(2, skuElements.Count);
            Assert.AreEqual(2, quantityElements.Count);

            Assert.AreEqual("000000", skuElements[0].Value);
            Assert.AreEqual("5", quantityElements[0].Value);

            Assert.AreEqual("111111", skuElements[1].Value);
            Assert.AreEqual("6", quantityElements[1].Value);
        }

        #endregion Contents Tests
    }
}
