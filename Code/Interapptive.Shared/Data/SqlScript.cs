using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Supports running a SQLCmd Mode style statement such as the output from a VS 2008 Database Team Edition Database Project
    /// Only a limited subset of the SQLCmd mode syntax is supported. Other gotchas.
    /// Uses a database transaction; if anything fails inside a On Error Exit group then the entire script is rolled back
    /// Supported Commands:
    /// GO (note GO [N] will only be executed once not N times)
    /// :setvar
    /// $(MyVar)
    /// :on error exit
    /// :on error resume (only for SQL Errors)
    /// :on error ignore (only for SQL Errors)
    /// The Following SQLCMD Commands are recognized but ignored. They will not crash your script if encountered but will be skipped
    /// :ED
    /// :Error
    /// :!!
    /// :Perftrace
    /// :Quit
    /// :Exit
    /// :Help
    /// :XML
    /// :r
    /// :ServerList
    /// :Listvar
    /// The following SQLCMD pre-defined variables are pre-defined by this class just like they are by SQLCMD
    /// The only difference is SQLCMD actually used and/or updated these variable. This class simply has them predefined
    /// with much the same values as SQLCMD did. The class allows you to change ALL variables (unlike SQLCMD) where some are
    /// read only.
    /// SQLCMDUSER ""
    /// SQLCMDPASSWORD
    /// SQLCMDSERVER {Server Name}
    /// SQLCMDWORKSTATION {Computer Name}
    /// SQLCMDLOGINTIMEOUT {Connection Timeout}
    /// SQLCMDDBNAME {Database Name}
    /// SQLCMDHEADERS "0"
    /// SQLCMDCOLSEP " "
    /// SQLCMDCOLWIDTH "0"
    /// SQLCMDPACKETSIZE "4096"
    /// SQLCMDERRORLEVEL "0"
    /// SQLCMDMAXVARTYPEWIDTH "256"
    /// SQLCMDMAXFIXEDTYPEWIDTH "0"
    /// SQLCMDEDITOR "edit.com"
    /// SQLCMDINI ""
    /// The following pre-defnined variables ARE used by the class and thier values when set are not ignored
    /// SQLCMDSTATTIMEOUT "0"
    /// One Additional Variable is defined so that scripts could potentially detect they are running in this class instead
    /// of SQLCmd.
    /// SQLCMDREAL "0"
    /// </summary>
    public class SqlScript
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlScript));

        private List<string> ignoredCommands;
        private Dictionary<string, string> variables;

        private List<string> lines;

        public int BatchCount
        {
            get;
            set;
        }

        public string Name { get; private set; }
        public string Content { get; private set; }

        /// <summary>
        /// Raised when a batch has successfully completed
        /// </summary>
        public event SqlScriptBatchCompletedEventHandler BatchCompleted;


        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteSqlCmdMode" /> class.
        /// </summary>
        public SqlScript(string name, string sql)
        {
            Name = name;
            Content = sql;

            BatchCount = Math.Max(Regex.Split(sql, @"^\s*?GO(?!TO)", RegexOptions.Multiline).Count(b => !string.IsNullOrWhiteSpace(b)), 1);

            lines = (sql.Replace(Environment.NewLine, "\n") + "\nGO\n").Split('\n').ToList();

            // Setup the list of commands to be ignored
            ignoredCommands = new List<string>();
            ignoredCommands.Add(":ED");
            ignoredCommands.Add(":ERROR");
            ignoredCommands.Add(":!!");
            ignoredCommands.Add(":PERFTRACE");
            ignoredCommands.Add(":QUIT");
            ignoredCommands.Add(":EXIT");
            ignoredCommands.Add(":HELP");
            ignoredCommands.Add(":XML");
            ignoredCommands.Add(":R");
            ignoredCommands.Add(":SERVERLIST");
            ignoredCommands.Add(":LISTVAR");
            ignoredCommands.Add(":ON");
        }

        /// <summary>
        /// Appends the SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public void AppendSql(string sql)
        {
            Content = string.Format("{0}{1}{2}", Content, Environment.NewLine, sql);

            BatchCount = Math.Max(Regex.Split(Content, @"^\s*?GO(?!TO)", RegexOptions.Multiline).Count(b => !string.IsNullOrWhiteSpace(b)), 1);

            lines = (Content.Replace(Environment.NewLine, "\n") + "\nGO\n").Split('\n').ToList();
        }

        /// <summary>
        /// Executes the specified SQL script with default options
        /// </summary>
        public void Execute(SqlConnection connection)
        {
            Execute(connection, connection.Database, string.Empty, string.Empty);
        }

        /// <summary>
        /// Executes the specified SQL script.
        /// </summary>
        public void Execute(SqlConnection connection, string databaseName, string defaultFilePrefix, string defaultFilePath)
        {
            SetVariables(connection, databaseName, defaultFilePrefix, defaultFilePath);

            int currentBatch = 0;

            StringBuilder commandText = new StringBuilder();

            // Loop each line in the script
            for (int i = 0; i < lines.Count; i++)
            {
                // Prepare a specially modified version of the line for checking for commands
                string ucaseLine = lines[i].Replace("\t", " ").Trim().ToUpper(CultureInfo.InvariantCulture) + " ";

                // See if it's one of the commands to be ignored
                if (!ignoredCommands.Contains((ucaseLine).Split(' ')[0]))
                {
                    // See if we have a GO line (everything after GO on the line is ignored)
                    if (ucaseLine.StartsWith("GO ", StringComparison.OrdinalIgnoreCase))
                    {
                        currentBatch = ExecuteBatch(connection, commandText.ToString(), currentBatch);

                        currentBatch++;

                        // Reset the SQL Command
                        commandText = new StringBuilder();
                    }
                    else
                    {
                        // Handle :SetVar MyVar "MyValue"

                        int begPos;
                        string temp;
                        if (ucaseLine.StartsWith(":SETVAR ", StringComparison.OrdinalIgnoreCase))
                        {
                            temp = lines[i].Trim().Substring(8, lines[i].Trim().Length - 8);
                            begPos = temp.IndexOf(" ", StringComparison.Ordinal);

                            string varName = temp.Substring(0, begPos).Trim().ToUpper();
                            string varValue = temp.Substring(begPos + 1, temp.Length - begPos - 1).Trim();
                            if (varValue.StartsWith("\"", StringComparison.OrdinalIgnoreCase) && varValue.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                            {
                                varValue = varValue.Substring(1, varValue.Length - 2);
                            }
                            else
                            {
                                throw new SqlScriptException(string.Format("Improperly formatted :SetVar on line {0}.", i));
                            }

                            // if variable is passed in from code, don't allow script to overwrite.
                            if (!variables.ContainsKey(varName))
                            {
                                variables.Add(varName, varValue);
                            }
                        }
                        // Regular SQL Line to have variables replaced on then added to SQLCmd for execution
                        else
                        {
                            temp = lines[i];

                            // Quick check to see if there's any possibility of variables in the line of SQL
                            if (temp.Length > 4 && temp.Contains("$("))
                            {
                                // Loop each variable to check the line for
                                foreach (KeyValuePair<string, string> keyPair in variables)
                                {
                                    string SearchFor = string.Format("$({0})", keyPair.Key);
                                    begPos = temp.IndexOf(SearchFor, StringComparison.OrdinalIgnoreCase);
                                    while (begPos >= 0)
                                    {
                                        // Make the variable substitution
                                        int endPos = begPos + SearchFor.Length;
                                        temp = temp.Substring(0, begPos) + keyPair.Value + temp.Substring(endPos, temp.Length - endPos);

                                        // Calculate a new begPos
                                        begPos = temp.IndexOf(SearchFor, StringComparison.OrdinalIgnoreCase);
                                    }
                                }
                            }

                            commandText.AppendLine(temp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the script variables.
        /// </summary>
        private void SetVariables(SqlConnection connection, string databaseName, string defaultFilePrefix, string defaultFilePath)
        {
            variables = new Dictionary<string, string>();
            variables.Add("SQLCMDUSER", "");
            variables.Add("SQLCMDPASSWORD", "");
            variables.Add("SQLCMDSERVER", connection.DataSource);
            variables.Add("SQLCMDWORKSTATION", connection.WorkstationId);
            variables.Add("SQLCMDDBNAME", connection.Database);
            variables.Add("SQLCMDLOGINTIMEOUT", connection.ConnectionTimeout.ToString(CultureInfo.InvariantCulture));
            variables.Add("SQLCMDSTATTIMEOUT", "0");
            variables.Add("SQLCMDHEADERS", "0");
            variables.Add("SQLCMDCOLSEP", "");
            variables.Add("SQLCMDCOLWIDTH", "0");
            variables.Add("SQLCMDPACKETSIZE", "4096");
            variables.Add("SQLCMDERRORLEVEL", "0");
            variables.Add("SQLCMDMAXVARTYPEWIDTH", "256");
            variables.Add("SQLCMDMAXFIXEDTYPEWIDTH", "0");
            variables.Add("SQLCMDEDITOR", "edit.com");
            variables.Add("SQLCMDINI", "");
            variables.Add("DATABASENAME", databaseName);
            variables.Add("DEFAULTFILEPREFIX", defaultFilePrefix);
            variables.Add("DEFAULTDATAPATH", defaultFilePath);
            variables.Add("DEFAULTLOGPATH", defaultFilePath);
        }

        /// <summary>
        /// Executes the batch.
        /// </summary>
        private int ExecuteBatch(SqlConnection connection, string command, int currentBatch)
        {
            int tries = 3;

            if (command.Length > 0)
            {
                try
                {
                    // Attempt the SQL command
                    using (SqlCommand sqlCommand = SqlCommandProvider.Create(connection))
                    {
                        while (tries-- > 0)
                        {
                            try
                            {
                                sqlCommand.CommandText = command;

                                SqlCommandProvider.ExecuteNonQuery(sqlCommand);

                                break;
                            }
                            catch (SqlException ex)
                            {
                                // "Backup, file manipulation operations (such as ALTER DATABASE ADD FILE) and encryption changes on a database must be serialized. 
                                //  Reissue the statement after the current backup or file manipulation operation is completed."
                                if (ex.Number != 3023)
                                {
                                    throw;
                                }

                                log.Warn(string.Format("Failed executing batch {0} in script {1}, will retry {2} times.", currentBatch, Name, tries), ex);
                                Thread.Sleep(250);
                            }
                        }

                        RaiseBatchCompletedEvent(currentBatch);
                    }
                }
                catch (SqlException ex)
                {
                    throw new SqlScriptException(Name, currentBatch, ex) { ShowScriptDetails = BatchCount > 1 };
                }
                catch (Exception ex)
                {
                    throw new SqlScriptException(Name, currentBatch, ex) { ShowScriptDetails = BatchCount > 1 };
                }
            }
            return currentBatch;
        }

        /// <summary>
        /// Raises the batch completed event.
        /// </summary>
        /// <param name="currentBatch">The current batch.</param>
        private void RaiseBatchCompletedEvent(int currentBatch)
        {
            SqlScriptBatchCompletedEventHandler handler = BatchCompleted;
            if (handler != null)
            {
                handler(this, new SqlScriptBatchCompletedEventArgs(currentBatch));
            }
        }
    }
}