using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using ShipWorks.Tests.Integration.MSTest.Fixtures;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.iParcel;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.Ups;

namespace ShipWorks.Tests.Integration.MSTest
{
    /*
     * To fix the spreadsheet columns
     *  - Insert a new row at the top
     *  - Paste the following into every cell in the new first row:   = CONCATENATE(A2, A3, A4,"", "")
     *  - Insert a new row above the first row of data
     *  - Copy the VALUES of the row with the forumal and paste the VALUES into the new row
     *  - Delete the rows above the new value row
     */
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        [DataSource("DataSource_Ship_FedExUSGroundDomestic"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExUSGroundDomestic()
        {
            FedExUSGroundFixture testObject = new FedExUSGroundFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExGroundDomesticAlcohol"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExGroundDomesticAlcohol()
        {
            FedExUSGroundFixture testObject = new FedExUSGroundFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExSmartPost"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExSmartPost()
        {
            FedExSmartPostFixture testObject = new FedExSmartPostFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternationalAlcohol"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressInternationalAlcohol()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternational"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomesticAlcohol"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressDomesticAlcohol()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomestic"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaGroundDomIntl"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaGroundDomIntl()
        {
            FedExCAGroundDomesticInternational testObject = new FedExCAGroundDomesticInternational();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaExpressInternational"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaExpressDomestic"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaExpressDomestic()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }





        [DataSource("DataSource_Ship_iParcel"),
         DeploymentItem("DataSources\\iParcel.xlsx"),
         TestMethod()]
        [Ignore]
        public void Ship_iParcel()
        {
            iParcelFixture testObject = new iParcelFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_UPSRecert"),
         DeploymentItem("DataSources\\UPSRecert.xlsx"),
         TestMethod()]
        [Ignore]
        public void Ship_UPSRecert()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsFixture testObject = new UpsFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }

        [DataSource("DataSource_Ship_UPSRecert_MI"),
         DeploymentItem("DataSources\\UPSRecert.xlsx"),
         TestMethod()]
        [Ignore]
        public void Ship_UPSRecert_MI()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsMIFixture testObject = new UpsMIFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }
    }
}
