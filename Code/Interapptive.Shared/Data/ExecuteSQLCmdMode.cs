using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

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
    public class ExecuteSqlCmdMode
    {
        private readonly List<string> ignoredCommands; // Hidden
        private readonly List<string> lockedVariables; // Hidden
        private readonly SqlConnection connection; // Hidden
        private readonly Dictionary<string, string> variables; // Hidden
        private bool allowVariableOverwrites;
        private bool exitOnError; // Hidden

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteSqlCmdMode" /> class.
        /// </summary>
        /// <param name="connection">The SQL conn.</param>
        public ExecuteSqlCmdMode(SqlConnection connection)
        {
            // Set connection variable from supplied SQLConnection
            this.connection = connection;

            // Load up the script variables
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
            variables.Add("SQLCMDREAL", "0");

            // Setup pre-locked variables
            lockedVariables = new List<string>();
            lockedVariables.Add("SQLCMDREAL");

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

            // Some other misc values
            exitOnError = true;
            allowVariableOverwrites = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow variable overwrites.
        /// If True then even though a variable is specified externally it may
        /// be overwritten by :SetVar in the script. If False then the reverse
        /// variables specified externally superscede :setvar
        /// Default = false
        /// </summary>
        /// <value>true if allow variable overwrites; otherwise, false.</value>
        public bool AllowVariableOverwrites
        {
            get { return allowVariableOverwrites; }
            set { allowVariableOverwrites = value; }
        }

        /// <summary>
        /// Sets a variable in advance of script execution.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="variableValue">The variable value.</param>
        public void SetVariable(string variableName, string variableValue)
        {
            variableName = variableName.Trim().ToUpper(CultureInfo.InvariantCulture);

            // See if we already have this variable
            if (variables.ContainsKey(variableName))
            {
                variables[variableName] = variableValue;
            }
            else
            {
                variables.Add(variableName, variableValue);

                if (!allowVariableOverwrites)
                {
                    lockedVariables.Add(variableName);
                }
            }
        }

        /// <summary>
        /// Executes the specified SQL script.
        /// </summary>
        /// <param name="scriptToExecute">The SQL script.</param>
        public List<Exception> Execute(string scriptToExecute)
        {
            List<Exception> exceptions = new List<Exception>();
            StringBuilder sqlCmd = new StringBuilder();

            string[] SQLScript = (scriptToExecute.Replace(Environment.NewLine, "\n") + "\nGO\n").Split('\n');

            // Loop each line in the script
            for (int i = 0; i < SQLScript.GetUpperBound(0); i++)
            {
                // Prepare a specially modified version of the line for checking for commands
                string ucaseLine = SQLScript[i].Replace("\t", " ").Trim().ToUpper(CultureInfo.InvariantCulture) + " ";

                // See if it's one of the commands to be ignored
                if (!ignoredCommands.Contains((ucaseLine).Split(' ')[0]))
                {
                    // See if we have a GO line (everything after GO on the line is ignored)
                    if (ucaseLine.StartsWith("GO ", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            if (sqlCmd.Length > 0)
                            {
                                // Attempt the SQL command
                                using (SqlCommand sqlComm = new SqlCommand(sqlCmd.ToString(), connection))
                                {
                                    sqlComm.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (exitOnError)
                            {
                                throw;
                            }

                            exceptions.Add(ex);
                        }

                        // Reset the SQL Command
                        sqlCmd = new StringBuilder();
                    }
                    else
                    {
                        // Handle :SetVar MyVar "MyValue"

                        int begPos;
                        string temp;
                        if (ucaseLine.StartsWith(":SETVAR ", StringComparison.OrdinalIgnoreCase))
                        {
                            temp = SQLScript[i].Trim().Substring(8, SQLScript[i].Trim().Length - 8);
                            begPos = temp.IndexOf(" ", StringComparison.Ordinal);

                            string varName = temp.Substring(0, begPos).Trim().ToUpper();
                            string varValue = temp.Substring(begPos + 1, temp.Length - begPos - 1).Trim();
                            if (varValue.StartsWith("\"", StringComparison.OrdinalIgnoreCase) && varValue.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                            {
                                varValue = varValue.Substring(1, varValue.Length - 2);
                            }
                            else
                            {
                                throw new Exception(string.Format("Improperly formatted :SetVar on line {0}.", i));
                            }

                            if (variables.ContainsKey(varName))
                            {
                                if (!lockedVariables.Contains(varName))
                                {
                                    variables[varName] = varValue;
                                }
                            }
                            else
                            {
                                variables.Add(varName, varValue);
                            }
                        }

                            // Handle :on error
                        else if (ucaseLine.StartsWith(":ON ERROR ", StringComparison.OrdinalIgnoreCase))
                        {
                            temp = ucaseLine.Substring(10, ucaseLine.Length - 10).Trim();
                            switch (temp)
                            {
                                case "EXIT":
                                    exitOnError = true;
                                    break;
                                case "IGNORE":
                                case "RESUME":
                                    exitOnError = false;
                                    break;
                                default:
                                    throw new Exception(string.Format("Unknown On Error mode '{0}' on line {1}", temp, i));
                            }
                        }

                            // Regular SQL Line to have variables replaced on then added to SQLCmd for execution
                        else
                        {
                            temp = SQLScript[i];

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

                            sqlCmd.AppendLine(temp);
                        }
                    }
                }
            }

            return exceptions;
        }
    }
}