using System;
using System.Collections.Generic;
using System.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.Domestic;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Freight;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Ground;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Tests
{
    [Collection("Fedex Tests")]
    public class FedExUSIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string fedExTestAccountNumber = "612480567";
        private string fedExUSFreightTestAccountNumber = "630081440";
        private string fedExCAFreightTestAccountNumber = "602091147";
        private const string ecodAccountNumber = "222326460";
        private readonly ITestOutputHelper output;
        private DataContext context;

        public FedExUSIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext((_mock, _builder) => { },
                ShipWorksInitializer.GetShipWorksInstance());

            justLabels = false;
            justForPhysicalPrint = false;
            physicalPrintType = ThermalLanguage.None;

            SetupPhysicalPrints();
        }

        [ExcelData(@"DataSources\FedExAll\Grn Alcohol.xlsx", "Grn Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_CreateBlankDb(DataRow row)
        {
            output.WriteLine($"Preparing customer transaction ID {row[5]}");
            var testObject = new FedExUSGroundAlcoholFixture();
        }

        [ExcelData(@"DataSources\FedExAll\US Grn Dom Intl And Home Del.xlsx", "US Grn Dom Intl And Home Del")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUSGround(DataRow row)
        {
            if (row["SaveLabel"] is DBNull)
            {
                return;
            }

            output.WriteLine($"Preparing customer transaction ID {row[5]}");

            var testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundFixture.UsGroundDomesticMapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
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

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\Grn Alcohol.xlsx", "Grn Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExGroundDomesticAlcohol(DataRow row)
        {
            var testObject = new FedExUSGroundAlcoholFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundAlcoholFixture.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Intl-Alcohol.xlsx", "US Exp Intl-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternationalAlcohol(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalAlcoholMapping.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Intl.xlsx", "US Exp Intl")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternational(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalFixture.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                if (row[2].ToString() == "IPE1" || row[2].ToString() == "IPE2")
                {
                    // IPE account number to use.
                    testObject.FedExAccountNumber = "604945186";
                }
                else
                {
                    testObject.FedExAccountNumber = fedExTestAccountNumber;
                }

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Dom-Alcohol.xlsx", "US Exp Dom-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "612480567";

            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticAlcoholMapping.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Dom.xlsx", "US Exp Dom")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomestic(DataRow row)
        {
            output.WriteLine($"Preparing customer transaction ID {row[5]}");
            if (row["SaveLabel"] is DBNull || (!(bool)row["SaveLabel"] && justLabels))
            {
                output.WriteLine("Skipping");
                return;
            }

            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticMapping.UsExpDomesticMapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\OneRate.xlsx", "OneRate")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExOneRate(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSOneRateMapping.UsOneRateMapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Freight.xlsx", "US Freight")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUSFreight(DataRow row)
        {
            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(row, testObject, FedExUSFreightPostFixture.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExUSFreightTestAccountNumber;

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }
    }
}