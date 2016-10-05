using System.Data.SqlClient;
using ShipWorks.Data.Administration;

namespace ShipWorks.Tests.Core
{
    public class GeneralTests
    {

        public void DropAssemblies()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"BRIANPC\Development";
            cs.InitialCatalog = "ShipWorksLocal";
            cs.IntegratedSecurity = true;

            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    SqlAssemblyDeployer.DropAssemblies(con, transaction);
                    transaction.Commit();
                }
            }
        }


        public void DeployAssemblies()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"BRIAN-ALX\Development";
            cs.InitialCatalog = "ShipWorks_Test";
            cs.IntegratedSecurity = true;

            using (SqlConnection con = new SqlConnection(cs.ToString()))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    SqlAssemblyDeployer.DeployAssemblies(con, transaction);
                    transaction.Commit();
                }

                SqlSchemaUpdater.UpdateSchemaVersionStoredProcedure(con);
            }
        }
    }
}
