using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.EntityClasses;
using System.Diagnostics;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Grid.Columns;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using System.Security.Cryptography;
using ShipWorks.Templates.Processing;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using ShipWorks.Data.Administration;
using System.Text.RegularExpressions;

namespace ShipWorks.Tests.Core
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        [Ignore]
        public void DropAssemblies()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"BRIANPC\Development";
            cs.InitialCatalog = "ShipWorksLocal";
            cs.IntegratedSecurity = true;

            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                con.Open();

                SqlAssemblyDeployer.DropAssemblies(con);
            }
        }

        [TestMethod]
        [Ignore]
        public void DeployAssemblies()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"BRIAN-ALX\Development";
            cs.InitialCatalog = "ShipWorks_Test";
            cs.IntegratedSecurity = true;

            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                con.Open();

                SqlAssemblyDeployer.DeployAssemblies(con);

                SqlSchemaUpdater.UpdateSchemaVersionStoredProcedure(con);
            }
        }
    }
}
