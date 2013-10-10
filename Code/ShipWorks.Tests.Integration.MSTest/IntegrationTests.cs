using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Tests.Integration.MSTest.Fixtures;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.iParcel;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.Ups;

namespace ShipWorks.Tests.Integration.MSTest
{
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        //private static List<ColumnPropertyMapDefinition> usGroundDomesticMapping = FedExUSGroundFixture.UsGroundDomesticMapping;


        [DataSource("DataSource_Ship_FedExUSGroundDomestic"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        //[Ignore]
        public void Ship_FedExUSGroundDomestic()
        {
            try
            {
                FedExUSGroundFixture testObject = new FedExUSGroundFixture();

                //GetPropertyNames(testObject);
                //GenerateColumnPropertyListCode();

                if (PopulatTestObject(testObject, FedExUSGroundFixture.UsGroundDomesticMapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
        }

        [DataSource("DataSource_Ship_FedExGroundDomesticAlcohol"),
         DeploymentItem("DataSources\\FedExAll.xlsx"),
         TestMethod()]
        [Ignore]
        public void Ship_FedExGroundDomesticAlcohol()
        {
            try
            {
                FedExUSGroundFixture testObject = new FedExUSGroundFixture();

                if (PopulatTestObject(testObject, FedExUSGroundFixture.UsGroundDomesticMapping))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExSmartPost()
        {
            FedExSmartPostFixture testObject = new FedExSmartPostFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
                PopulatTestObject(testObject, null);

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
        [Ignore]
        public void Ship_FedExExpressInternationalAlcohol()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomesticAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod()]
        public void Ship_FedExExpressDomesticAlcohol()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();

            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, FedExPrototypeFixture.Mapping))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExCanadaGroundDomIntl()
        {
            FedExCAGroundDomesticInternational testObject = new FedExCAGroundDomesticInternational();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExCanadaExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();
            try
            {
                System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
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
        [Ignore]
        public void Ship_FedExCanadaExpressDomestic()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            FedExPrototypeFixture testObject = new FedExPrototypeFixture();

            if (PopulatTestObject(testObject, null))
            {
                testObject.Ship();
            }
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

                if (PopulatTestObject(testObject, null))
                {
                    testObject.Ship();
                }
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

            if (PopulatTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_UPSRecert_MI"),
         DeploymentItem("DataSources\\UPSRecert.xlsx"),
         TestMethod()]
        [Ignore]
        public void Ship_UPSRecert_MI()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsMIFixture testObject = new UpsMIFixture();

            if (PopulatTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }
    }
}
