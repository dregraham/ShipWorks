﻿using System.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.EMEA;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Tests
{
    [Collection("Fedex Tests")]
    public class FedExEMEAIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string FinlandAccountNumber = "604818907";
        private string ItalyAccountNumber = "602810100";
        private string PolandAccountNumber = "604808006";
        private string SpainAccountNumber = "604824729";
        private string SwedenAccountNumber = "604849268";
        private string FranceAccountNumber = "602550940";
        private readonly ITestOutputHelper output;
        private DataContext context;

        public FedExEMEAIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext((_mock, _builder) => { },
                ShipWorksInitializer.GetShipWorksInstance());

            justLabels = false;
            justForPhysicalPrint = false;
            physicalPrintType = ThermalLanguage.None;

            SetupPhysicalPrints();
        }

        [ExcelData(@"DataSources\FedExAll\EMEA.xlsx", "EMEA")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExEMEA(DataRow row)
        {
            var testObject = new FedExEMEAFixture();

            if (PopulateTestObject(row, testObject, FedExEMEAFixture.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[4]}");

                testObject.FedExAccountNumber = GetAccountForCountry(testObject.ShipperCountryCode);

                if (testObject.CustomerTransactionId == "EMEA-511")
                {
                    testObject.CommercialInvoiceFileElectronically = true;
                    testObject.ShipmentSignatureOptionType = "DIRECT";
                }
                if (testObject.CustomerTransactionId == "EMEA-512")
                {
                    testObject.BrokerAccountNumber = "123456";
                }

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        private string GetAccountForCountry(string countryCode)
        {
            switch (countryCode)
            {
                case "FI":
                    return FinlandAccountNumber;
                case "IT":
                    return ItalyAccountNumber;
                case "PL":
                    return PolandAccountNumber;
                case "ES":
                    return SpainAccountNumber;
                case "SE":
                    return SwedenAccountNumber;
                case "FR":
                    return FranceAccountNumber;
                default:
                    return "";
            }
        }
    }
}