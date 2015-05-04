using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shipping
{
    [TestClass]
    public class AdjustedAddressExtensionsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AdjustedShipCountryCode_OriginalShipmentIsNotChanged()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int) ShipmentTypeCode.Usps,
                ShipCountryCode = "PR"
            };

            shipment.AdjustedShipCountryCode();

            Assert.AreEqual("PR", shipment.ShipCountryCode);
        }

        [TestMethod]
        public void AdjustedShipCountryCode_ReturnsCountryCodeInUppercase_WhenAdjustmentIsDone()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int)ShipmentTypeCode.Usps,
                ShipCountryCode = "fr"
            };

            string result = shipment.AdjustedShipCountryCode();

            Assert.AreEqual("FR", result);
        }

        [TestMethod]
        public void AdjustedShipCountryCode_ReturnsCountryCodeInUppercase_WhenAdjustmentIsNotDone()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int)ShipmentTypeCode.OnTrac,
                ShipCountryCode = "fr"
            };

            string result = shipment.AdjustedShipCountryCode();

            Assert.AreEqual("FR", result);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_FedEx")]
        public void AdjustedShipCountryCode_ForFedEx()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.FedEx);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Ups")]
        public void AdjustedShipCountryCode_ForUpsOnlineTools()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.UpsOnLineTools);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Ups")]
        public void AdjustedShipCountryCode_ForUpsWorldShip()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.UpsWorldShip);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Usps")]
        public void AdjustedShipCountryCode_ForUsps()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Usps);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Usps")]
        public void AdjustedShipCountryCode_ForUspsExpress1()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Express1Usps);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Endicia")]
        public void AdjustedShipCountryCode_ForEndicia()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Endicia);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Endicia")]
        public void AdjustedShipCountryCode_ForEndiciaExpress1()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Express1Endicia);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_iParcel")]
        public void AdjustedShipCountryCode_ForIParcel()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.iParcel);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_FedEx")]
        public void AdjustedOriginCountryCode_ForFedEx()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.FedEx);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Ups")]
        public void AdjustedOriginCountryCode_ForUpsOnlineTools()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.UpsOnLineTools);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Ups")]
        public void AdjustedOriginCountryCode_ForUpsWorldShip()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.UpsWorldShip);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Usps")]
        public void AdjustedOriginCountryCode_ForUsps()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Usps);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Usps")]
        public void AdjustedOriginCountryCode_ForUspsExpress1()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Express1Usps);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Endicia")]
        public void AdjustedOriginCountryCode_ForEndicia()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Endicia);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_Endicia")]
        public void AdjustedOriginCountryCode_ForEndiciaExpress1()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Express1Endicia);
        }

        [TestMethod]
        [DeploymentItem("Shipping\\AdjustedCountryCodeData.xlsx")]
        [DataSource("DataSource_AdjustedCountryCodes_iParcel")]
        public void AdjustedOriginCountryCode_ForIParcel()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.iParcel);
        }

        /// <summary>
        /// Test the adjusted ship country code
        /// </summary>
        private void TestAdjustedShipCountryCode(ShipmentTypeCode shipmentType)
        {
            string countryCode = TestContext.DataRow["Country"] as string;
            string state = TestContext.DataRow["State"] as string ?? string.Empty;
            string expectedCountry = TestContext.DataRow["Expected"] as string;

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int) shipmentType,
                ShipCountryCode = countryCode,
                ShipStateProvCode = state
            };

            string result = shipment.AdjustedShipCountryCode();
            Assert.AreEqual(expectedCountry, result);
        }

        /// <summary>
        /// Test the adjusted ship country code
        /// </summary>
        private void TestAdjustedOriginCountryCode(ShipmentTypeCode shipmentType)
        {
            string countryCode = TestContext.DataRow["Country"] as string;
            string state = TestContext.DataRow["State"] as string ?? string.Empty;
            string expectedCountry = TestContext.DataRow["Expected"] as string;

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int)shipmentType,
                OriginCountryCode = countryCode,
                OriginStateProvCode = state
            };

            string result = shipment.AdjustedOriginCountryCode();
            Assert.AreEqual(expectedCountry, result);
        }
    }
}
