using System;
using System.Data.Common;
using System.Data.SqlClient;
using log4net;

namespace ShipWorks.Installer.Sql
{
    public class SqlSession : ISqlSession
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSession));

        /// <summary>
        /// Static constructor
        /// </summary>
        static SqlSession()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlSession()
            : this(new SqlSessionConfiguration())
        {

        }

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        public SqlSession(SqlSessionConfiguration configuration)
        {
            Configuration = new SqlSessionConfiguration(configuration);
        }

        public SqlSessionConfiguration Configuration { get; set; }

        /// <summary>
        /// Open a connection using the current properties of the SqlSession
        /// </summary>
        public DbConnection OpenConnection()
        {
            DbConnection con = SqlClientFactory.Instance.CreateConnection();
            con.ConnectionString = Configuration.GetConnectionString();
            con.Open();

            return con;
        }

        /// <summary>
        /// Returns a flag indicating if a connection can be made to SQL Server.
        /// </summary>
        public bool CanConnect()
        {
            try
            {
                return TestConnection();
            }
            catch (SqlException ex)
            {
                log.Error(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public bool TestConnection()
        {
            return TestConnection(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public bool TestConnection(TimeSpan timeout)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(Configuration.GetConnectionString());
            csb.ConnectTimeout = (int) timeout.TotalSeconds;
            csb.Pooling = false;
            using (DbConnection con = new SqlConnection(csb.ToString()))
            {
                con.Open();
                SqlUtility sqlUtil = new SqlUtility();
                var result = sqlUtil.ValidateOpenConnection(con);
                con.Close();
                return result;
            }
        }
    }
}
