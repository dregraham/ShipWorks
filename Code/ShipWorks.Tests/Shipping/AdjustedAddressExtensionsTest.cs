using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Tests.Shipping
{
    public class AdjustedAddressExtensionsTest
    {
        public TestContext TestContext { get; set; }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_FedEx.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_FedEx.csv", 
            "AdjustedCountryCodeData_FedEx#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForFedEx()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.FedEx);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Ups.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Ups.csv",
            "AdjustedCountryCodeData_Ups#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForUpsOnlineTools()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.UpsOnLineTools);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Ups.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Ups.csv",
            "AdjustedCountryCodeData_Ups#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForUpsWorldShip()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.UpsWorldShip);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Usps.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Usps.csv",
            "AdjustedCountryCodeData_Usps#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForUsps()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Usps);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Ups.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Usps.csv",
            "AdjustedCountryCodeData_Usps#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForUspsExpress1()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Express1Usps);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Endicia.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Endicia.csv",
            "AdjustedCountryCodeData_Endicia#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForEndicia()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Endicia);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Endicia.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Endicia.csv",
            "AdjustedCountryCodeData_Endicia#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForEndiciaExpress1()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.Express1Endicia);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_iParcel.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_iParcel.csv",
            "AdjustedCountryCodeData_iParcel#csv", DataAccessMethod.Sequential)]
        public void AdjustedShipCountryCode_ForIParcel()
        {
            TestAdjustedShipCountryCode(ShipmentTypeCode.iParcel);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_FedEx.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_FedEx.csv",
            "AdjustedCountryCodeData_FedEx#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForFedEx()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.FedEx);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Ups.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Ups.csv",
            "AdjustedCountryCodeData_Ups#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForUpsOnlineTools()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.UpsOnLineTools);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Ups.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Ups.csv",
            "AdjustedCountryCodeData_Ups#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForUpsWorldShip()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.UpsWorldShip);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Usps.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Usps.csv",
            "AdjustedCountryCodeData_Usps#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForUsps()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Usps);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Usps.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Usps.csv",
            "AdjustedCountryCodeData_Usps#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForUspsExpress1()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Express1Usps);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Endicia.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Endicia.csv",
            "AdjustedCountryCodeData_Endicia#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForEndicia()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Endicia);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_Endicia.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_Endicia.csv",
            "AdjustedCountryCodeData_Endicia#csv", DataAccessMethod.Sequential)]
        public void AdjustedOriginCountryCode_ForEndiciaExpress1()
        {
            TestAdjustedOriginCountryCode(ShipmentTypeCode.Express1Endicia);
        }

        [Fact]
        [DeploymentItem(@"Shipping\AdjustedCountryCodeData_iParcel.csv")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AdjustedCountryCodeData_iParcel.csv",
            "AdjustedCountryCodeData_iParcel#csv", DataAccessMethod.Sequential)]
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
