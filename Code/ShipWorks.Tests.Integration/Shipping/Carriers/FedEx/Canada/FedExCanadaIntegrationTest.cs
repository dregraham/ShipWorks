using System;
using Xunit;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.Domestic;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.International;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Ground;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using System.Data;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada
{
    public class FedExCanadaIntegrationTest : DataDrivenIntegrationTestBase
    {
        private const string fedExTestAccountNumber = "604601185";
        private const bool justLabels = true;
        private readonly ITestOutputHelper output;

        public FedExCanadaIntegrationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [ExcelData(@"DataSources\FedExAll.xlsx", "CA Exp Dom")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaExpressDomestic(DataRow row)
        {
            FedExCanadaExpressDomesticMapping testObject = new FedExCanadaExpressDomesticMapping();

            if (PopulateTestObject(row, testObject, FedExCanadaExpressDomesticMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "CA Grn Dom, Intl")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaGroundDomIntl(DataRow row)
        {
            FedExCAGroundDomesticInternationalFixture testObject = new FedExCAGroundDomesticInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExCanadaGroundDomesticInternationalMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }
        
        [ExcelData("DataSources\\FedExAll.xlsx", "CA Exp Intl")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaExpressInternational(DataRow row)
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExCanadaExpressInternationalMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");
                output.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }
    }
}