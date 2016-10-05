using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Represents a SQL script that can be executed
    /// </summary>
    public class SqlScript
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlScript));

        string name;
        string sql;

        List<string> batches;

        /// <summary>
        /// Gets the name of the script
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Raised when a batch has successfully completed
        /// </summary>
        public event SqlScriptBatchCompletedEventHandler BatchCompleted;

        /// <summary>
        /// Contstructor
        /// </summary>
        public SqlScript(string name, string sql)
        {
            this.name = name;
            this.sql = sql;

            this.batches = Regex.Split(sql, @"^\s*?GO(?!TO)", RegexOptions.Multiline).Select(b => b.Trim()).Where(b => b.Length != 0).ToList();
        }

        /// <summary>
        /// The SQL that makes up the script
        /// </summary>
        public string Content
        {
            get
            {
                return sql;
            }
        }

        /// <summary>
        /// The ScriptContent broken down into batches that can be sent to SQL Server as individual commands
        /// </summary>
        public IList<string> Batches
        {
            get
            {
                return batches.AsReadOnly();
            }
        }

        /// <summary>
        /// Executes the script on the given connection
        /// </summary>
        public void Execute(DbConnection con)
        {
            using (DbCommand command = DbCommandProvider.Create(con))
            {
                Execute(command);
            }
        }

        /// <summary>
        /// Executes the script on the given connection with the given transaction
        /// </summary>
        public void Execute(DbCommand cmd)
        {
            log.InfoFormat("Running script {0}", name);

            // Because this is primarily used for schema modifications, set the timeout to a very large amount
            // since not completing execution means an upgrade or install failed.
            cmd.CommandTimeout = (int) TimeSpan.FromHours(12).TotalSeconds;

            // Execute each batch
            for (int i = 0; i < batches.Count; i++)
            {
                string command = batches[i];

                try
                {
                    int tries = 3;

                    // We keep trying for certain errors that mean we need to wait for SQL Server to get done configuring.  This was done for the CreateDatabase script, when
                    // dreating a database in Amazon RDS.
                    while (tries-- > 0)
                    {
                        try
                        {
                            cmd.CommandText = command;
                            DbCommandProvider.ExecuteNonQuery(cmd);

                            break;
                        }
                        catch (SqlException ex)
                        {
                            // "Backup, file manipulation operations (such as ALTER DATABASE ADD FILE) and encryption changes on a database must be serialized.
                            //  Reissue the statement after the current backup or file manipulation operation is completed."
                            if (ex.Number == 3023)
                            {
                                log.Warn(string.Format("Failed executing batch {0} in script {1}, will retry {2} times.", i, name, tries), ex);
                                Thread.Sleep(250);

                                continue;
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }

                    SqlScriptBatchCompletedEventHandler handler = BatchCompleted;
                    if (handler != null)
                    {
                        handler(this, new SqlScriptBatchCompletedEventArgs(i));
                    }
                }
                catch (SqlException ex)
                {
                    throw new SqlScriptException(name, i, ex) { ShowScriptDetails = batches.Count > 1 };
                }
                catch (Exception ex)
                {
                    throw new SqlScriptException(name, i, ex) { ShowScriptDetails = batches.Count > 1 };
                }
            }
        }
    }
}
