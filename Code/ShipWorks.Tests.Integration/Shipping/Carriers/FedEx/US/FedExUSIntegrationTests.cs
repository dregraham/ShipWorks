using System;
using System.Collections.Generic;
using Xunit;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.Domestic;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Ground;
using System.Data;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US
{
    public class FedExUSIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string fedExTestAccountNumber = "607253064";
        private const string ecodAccountNumber = "222326460";
        private bool justLabels = false;
        private readonly ITestOutputHelper output;

        public FedExUSIntegrationTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "US Grn Dom")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUSGroundDomestic(DataRow row)
        {
            var testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundFixture.UsGroundDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                // TransactionID 605733 should use ECOD account.
                List<string> transactionsForEcodAccount = new List<string>()
                {
                    "605733",
                    "605793",
                    "605797",
                    "605798"
                };

                testObject.FedExAccountNumber = transactionsForEcodAccount.Contains(testObject.CustomerTransactionId)
                    ? ecodAccountNumber : fedExTestAccountNumber;

                testObject.Ship();
            }
        }


        [ExcelData("DataSources\\FedExAll.xlsx", "Grn Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExGroundDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "607253064";

            var testObject = new FedExUSGroundAlcoholFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundAlcoholFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "US Exp Intl-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternationalAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "607253064";

            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "US Exp Intl")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternational(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "US Exp Dom-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "607253064";

            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "US Exp Dom")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomestic(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticMapping.UsExpDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "OneRate")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExOneRate_Test(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSOneRateMapping.UsOneRateMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [ExcelData("DataSources\\FedExAll.xlsx", "ETD")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExETD(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalEtdMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;
                testObject.CommercialInvoiceFileElectronically = true;

                testObject.Ship();
            }
        }
    }
}