using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using log4net;

namespace ShipWorks.Installer.Sql
{
    /// <summary>
    /// Contains information and methods for connecting to a sql server
    /// </summary>
    public class SqlSession : ISqlSession
    {
        private readonly ILog log = LogManager.GetLogger(typeof(SqlSession));

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

        /// <summary>
        /// The configuration for this session
        /// </summary>
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
        public async Task<bool> CanConnect()
        {
            try
            {
                return await TestConnection();
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
        public async Task<bool> TestConnection()
        {
            return await TestConnection(TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Tries to connect to SQL Server.  Throws an exception on failure.
        /// </summary>
        public async Task<bool> TestConnection(TimeSpan timeout)
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(Configuration.GetConnectionString());
            csb.ConnectTimeout = (int) timeout.TotalSeconds;
            csb.Pooling = false;
            using (DbConnection con = new SqlConnection(csb.ToString()))
            {
                con.Open();
                SqlUtility sqlUtil = new SqlUtility();
                var result = await sqlUtil.ValidateOpenConnection(con).ConfigureAwait(false);
                con.Close();
                return result;
            }
        }
    }
}
