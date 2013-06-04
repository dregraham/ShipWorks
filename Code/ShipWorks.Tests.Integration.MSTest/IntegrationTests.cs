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
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        [DataSource("DataSource_Ship_FedExUSGroundDomestic"),
         DeploymentItem("DataSources\\FedExUSGroundDomestic.xlsx"),
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
         DeploymentItem("DataSources\\FedExGroundDomesticAlcohol.xlsx"),
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
         DeploymentItem("DataSources\\FedExSmartPost.xlsx"),
         TestMethod()]
        public void Ship_FedExSmartPost()
        {
            FedExSmartPostFixture testObject = new FedExSmartPostFixture();
            try
            {
                PopulateValues(testObject, this.TestContext.DataRow);

                testObject.Ship();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_iParcel"),
         DeploymentItem("DataSources\\iParcel.xlsx"),
         TestMethod()]
        public void Ship_iParcel()
        {
            iParcelFixture testObject = new iParcelFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExExpressInternationalAlcohol.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressInternationalAlcohol()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExExpressInternational.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExExpressDomesticAlcohol.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressDomesticAlcohol()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExExpressDomestic.xlsx"),
         TestMethod()]
        public void Ship_FedExExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExCanadaGroundDomIntl.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaGroundDomIntl()
        {
            FedExCAGroundDomesticInternational testObject = new FedExCAGroundDomesticInternational();
            try
            {
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
         DeploymentItem("DataSources\\FedExCanadaExpressInternational.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
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
         DeploymentItem("DataSources\\FedExCanadaExpressDomestic.xlsx"),
         TestMethod()]
        public void Ship_FedExCanadaExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }

        [DataSource("DataSource_Ship_UPSRecert"),
         DeploymentItem("DataSources\\UPSRecert.xlsx"),
         TestMethod()]
        public void Ship_UPSRecert()
        {
            UpsFixture testObject = new UpsFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }

        [DataSource("DataSource_Ship_UPSRecert_MI"),
         DeploymentItem("DataSources\\UPSRecert.xlsx"),
         TestMethod()]
        public void Ship_UPSRecert_MI()
        {
            UpsMIFixture testObject = new UpsMIFixture();
            PopulateValues(testObject, this.TestContext.DataRow);
            testObject.Ship();
        }
    }
}
