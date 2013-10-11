using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.iParcel;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.Ups;

namespace ShipWorks.Tests.Integration.MSTest
{
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        [DataSource("DataSource_Ship_FedExUSGroundDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExUSGroundDomestic()
        {
            FedExUSGroundFixture testObject = new FedExUSGroundFixture();

            try
            {
                if (PopulateTestObject(testObject, FedExUSGroundFixture.UsGroundDomesticMapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExGroundDomesticAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExGroundDomesticAlcohol()
        {
            FedExUSGroundAlcoholFixture testObject = new FedExUSGroundAlcoholFixture();
            try
            {
                if (PopulateTestObject(testObject, FedExUSGroundAlcoholFixture.Mapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExSmartPost")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExSmartPost()
        {
            FedExSmartPostFixture testObject = new FedExSmartPostFixture();
            try
            {
                if (PopulateTestObject(testObject, FedExSmartPostFixture.SmartPostMapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternationalAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExExpressInternationalAlcohol()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(testObject, FedExUsExpressInternationalAlcoholMapping.Mapping))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);                

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternational")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(testObject, FedExUSExpressInternationalFixture.Mapping))
            {
                Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", testObject.CustomerTransactionId));
                Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);  

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomesticAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExExpressDomesticAlcohol()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(testObject, FedExUSExpressDomesticAlcoholMapping.Mapping))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);  

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();
            try
            {
                if (PopulateTestObject(testObject, FedExUsExpressDomesticMapping.UsExpDomesticMapping))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);  
                    
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaGroundDomIntl")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod()]
        public void Ship_FedExCanadaGroundDomIntl()
        {
            FedExCAGroundDomesticInternational testObject = new FedExCAGroundDomesticInternational();
            try
            {
                if (PopulateTestObject(testObject, FedExCAGroundDomesticInternational.FedExCAGroundDomesticInternationalMapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaExpressInternational")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExCanadaExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            
            try
            {
                if (PopulateTestObject(testObject, FedExCaExpressInternationalMapping.CaExpressInternationalMapping))
                {
                    Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaExpressDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExCanadaExpressDomestic()
        {
            try
            {                
                FedExCanadaExpressDomesticFixture testObject = new FedExCanadaExpressDomesticFixture();

                if (PopulateTestObject(testObject, FedExCanadaExpressDomesticFixture.Mapping))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.Ship();
                }
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                // We have a legitimate exception
                throw;
            }
        }


        [DataSource("DataSource_Ship_iParcel")]
        [DeploymentItem("DataSources\\iParcel.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_iParcel()
        {
            iParcelFixture testObject = new iParcelFixture();

            if (PopulateTestObject(testObject, null))
            {
                Console.WriteLine(string.Format("Executing test ID {0}", this.TestContext.DataRow["TestID"]));
                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_UPSRecert")]
        [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_UPSRecert()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsFixture testObject = new UpsFixture();

            if (PopulateTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_UPSRecert_MI")]
        [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_UPSRecert_MI()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsMIFixture testObject = new UpsMIFixture();

            if (PopulateTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }
    }
}
