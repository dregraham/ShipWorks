using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using System.Data;
using Xunit.Sdk;

namespace ShipWorks.Tests.Shipping
{
    public class AdjustedAddressExtensionsTest
    {
        [Fact]
        public void AdjustedShipCountryCode_OriginalShipmentIsNotChanged()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int) ShipmentTypeCode.Usps,
                ShipCountryCode = "PR"
            };

            shipment.AdjustedShipCountryCode();

            Assert.Equal("PR", shipment.ShipCountryCode);
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

            Assert.Equal("FR", result);
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

            Assert.Equal("FR", result);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_FedEx")]
        public void AdjustedShipCountryCode_ForFedEx(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.FedEx);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Ups")]
        public void AdjustedShipCountryCode_ForUpsOnlineTools(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.UpsOnLineTools);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Ups")]
        public void AdjustedShipCountryCode_ForUpsWorldShip(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.UpsWorldShip);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Usps")]
        public void AdjustedShipCountryCode_ForUsps(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.Usps);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Usps")]
        public void AdjustedShipCountryCode_ForUspsExpress1(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.Express1Usps);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Endicia")]
        public void AdjustedShipCountryCode_ForEndicia(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.Endicia);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Endicia")]
        public void AdjustedShipCountryCode_ForEndiciaExpress1(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.Express1Endicia);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_iParcel")]
        public void AdjustedShipCountryCode_ForIParcel(DataRow row)
        {
            TestAdjustedShipCountryCode(row, ShipmentTypeCode.iParcel);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_FedEx")]
        public void AdjustedOriginCountryCode_ForFedEx(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.FedEx);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Ups")]
        public void AdjustedOriginCountryCode_ForUpsOnlineTools(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.UpsOnLineTools);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Ups")]
        public void AdjustedOriginCountryCode_ForUpsWorldShip(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.UpsWorldShip);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Usps")]
        public void AdjustedOriginCountryCode_ForUsps(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.Usps);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Usps")]
        public void AdjustedOriginCountryCode_ForUspsExpress1(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.Express1Usps);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Endicia")]
        public void AdjustedOriginCountryCode_ForEndicia(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.Endicia);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_Endicia")]
        public void AdjustedOriginCountryCode_ForEndiciaExpress1(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.Express1Endicia);
        }

        [Theory]
        [CsvData(@"Shipping", "AdjustedCountryCodeData_iParcel")]
        public void AdjustedOriginCountryCode_ForIParcel(DataRow row)
        {
            TestAdjustedOriginCountryCode(row, ShipmentTypeCode.iParcel);
        }

        /// <summary>
        /// Test the adjusted ship country code
        /// </summary>
        private void TestAdjustedShipCountryCode(DataRow row, ShipmentTypeCode shipmentType)
        {
            string countryCode = row["Country"] as string;
            string state = row["State"] as string ?? string.Empty;
            string expectedCountry = row["Expected"] as string;

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int) shipmentType,
                ShipCountryCode = countryCode,
                ShipStateProvCode = state
            };

            string result = shipment.AdjustedShipCountryCode();
            Assert.Equal(expectedCountry, result);
        }

        /// <summary>
        /// Test the adjusted ship country code
        /// </summary>
        private void TestAdjustedOriginCountryCode(DataRow row, ShipmentTypeCode shipmentType)
        {
            string countryCode = row["Country"] as string;
            string state = row["State"] as string ?? string.Empty;
            string expectedCountry = row["Expected"] as string;

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int)shipmentType,
                OriginCountryCode = countryCode,
                OriginStateProvCode = state
            };

            string result = shipment.AdjustedOriginCountryCode();
            Assert.Equal(expectedCountry, result);
        }
    }
}
