using System;
using System.Text;
using System.Reflection;
using log4net;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Collections;
using Interapptive.Shared;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Deploys an assembly and all contained CLR objects to a database
    /// </summary>
    public static class SqlAssemblyDeployer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlAssemblyDeployer));

        /// <summary>
        /// Deploy all ShipWorks assemblies to the SQL Server on the given connection. The assemblies are dropped
        /// before they are deployed.
        /// </summary>
        public static void DeployAssemblies(SqlConnection con, SqlTransaction transaction)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            Assembly sqlServer = typeof(StoredProcedures).Assembly;

            DropAssembly(sqlServer, con, transaction);

            DeployAssembly(sqlServer, con, transaction);
        }

        /// <summary>
        /// Drop all ShipWorks assemblies to the SQL Server on the given connection
        /// </summary>
        public static void DropAssemblies(SqlConnection con, SqlTransaction sqlTransaction)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            Assembly sqlServer = typeof(StoredProcedures).Assembly;

            DropAssembly(sqlServer, con, sqlTransaction);
        }

        /// <summary>
        /// Deploy the given assembly to the connection specified by the SQL Session
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void DeployAssembly(Assembly assembly, SqlConnection con, SqlTransaction transaction)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            log.InfoFormat("DeployAssembly: {0}", assembly.FullName);

            // Now create the assembly
            CreateAssembly(assembly, con, transaction);

            UpdateAssemblySchemaVersionProcedure(con, transaction);

            // Look at each type in the assembly
            foreach (Type type in assembly.GetTypes())
            {
                // Look at each custom attribute on the type
                foreach (Attribute attribute in Attribute.GetCustomAttributes(type))
                {
                    // Right now we dont support UDT
                    if (attribute.GetType() == typeof(SqlUserDefinedTypeAttribute))
                    {
                        throw new NotSupportedException("Cannot register UDT within assembly.");
                    }
                    else if (attribute.GetType() == typeof(SqlUserDefinedAggregateAttribute))
                    {
                        throw new NotSupportedException("Cannot register UDA within assembly.");
                    }
                }

                // Go through each method in the type
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.IsPublic && method.IsStatic)
                    {
                        foreach (Attribute attribute in method.GetCustomAttributes(false))
                        {
                            SqlProcedureAttribute procudure = attribute as SqlProcedureAttribute;
                            if (procudure != null)
                            {
                                RegisterProcedure(method, procudure, con, transaction);
                            }

                            SqlTriggerAttribute trigger = attribute as SqlTriggerAttribute;
                            if (trigger != null)
                            {
                                RegisterTrigger(method, trigger, con, transaction);
                            }

                            SqlFunctionAttribute function = attribute as SqlFunctionAttribute;
                            if (function != null)
                            {
                                RegisterFunction(method, function, con, transaction);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the stored procedure that specifies the schema version to which the installed assembly applies
        /// </summary>
        private static void UpdateAssemblySchemaVersionProcedure(SqlConnection con, SqlTransaction transaction)
        {
            using (SqlCommand command = con.CreateCommand())
            {
                command.Transaction = transaction;
                SqlSchemaUpdater.UpdateAssemblyVersionStoredProcedure(command);
            }
        }

        /// <summary>
        /// Add the specified assembly to the database
        /// </summary>
        private static void CreateAssembly(Assembly assembly, SqlConnection con, SqlTransaction transaction)
        {
            log.Info("Create assembly");

            // Get the raw bytes that make up the assembly
            byte[] bytes = File.ReadAllBytes(assembly.Location);

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }

            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.Transaction = transaction;
                cmd.CommandText = string.Format(@"
                CREATE ASSEMBLY [{0}]
                    FROM 0x{1}
                    WITH PERMISSION_SET = SAFE", assembly.GetName().Name, hex);

                SqlCommandProvider.ExecuteNonQuery(cmd);    
            }
        }

        /// <summary>
        /// Drop the specified assembly from the database
        /// </summary>
        private static void DropAssembly(Assembly assembly, SqlConnection con, SqlTransaction transaction)
        {
            string scriptName = "DropAssembly.sql";

            SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Installation");
            StringBuilder sb = new StringBuilder(sqlLoader.LoadScript(scriptName).Content);
            sb.Replace("{AssemblyName}", assembly.GetName().Name);

            log.Info("Dropping assembly.");
            SqlUtility.ExecuteScriptSql(scriptName, sb.ToString(), con, transaction);
        }

        /// <summary>
        /// Register the specified method as a store procedure using metadata from the attribute
        /// </summary>
        private static void RegisterProcedure(MethodInfo method, SqlProcedureAttribute procedure, SqlConnection con, SqlTransaction transaction)
        {
            String procedureName = procedure.Name;
            if (string.IsNullOrEmpty(procedureName))
            {
                procedureName = method.Name;
            }

            log.InfoFormat("Registering {0} as STORED PROCEDURE {1}", method.Name, procedureName);

            StringBuilder command = new StringBuilder();
            command.AppendFormat("CREATE PROCEDURE [{0}] ", procedureName);
            command.AppendFormat(GetParameterDeclaration(method));
            command.AppendFormat("AS EXTERNAL NAME [{0}].[{1}].[{2}]", method.DeclaringType.Assembly.GetName().Name, method.DeclaringType.FullName, method.Name);

            SqlCommandProvider.ExecuteNonQuery(con, command.ToString(), transaction);
        }

        /// <summary>
        /// Register the specified method as a trigger using metadata from the attribute
        /// </summary>
        private static void RegisterTrigger(MethodInfo method, SqlTriggerAttribute trigger, SqlConnection con, SqlTransaction transaction)
        {
            String triggerName = trigger.Name;
            if (string.IsNullOrEmpty(triggerName))
            {
                triggerName = method.Name;
            }

            log.InfoFormat("Registering {0} as TRIGGER {1}", method.Name, triggerName);

            // Check that the target is valid
            if (string.IsNullOrEmpty(trigger.Target))
            {
                throw new InvalidOperationException(string.Format("No Target specified on SqlTriggerAttribute on method {0}.", method.Name));
            }

            // Check that the event is valid
            if (string.IsNullOrEmpty(trigger.Event))
            {
                throw new InvalidOperationException(string.Format("No Event specified on SqlTriggerAttribute on method {0}.", method.Name));
            }

            StringBuilder command = new StringBuilder();
            command.AppendFormat("CREATE TRIGGER [{0}] ON [{1}] {2} ", triggerName, trigger.Target, trigger.Event);
            command.AppendFormat("AS EXTERNAL NAME [{0}].[{1}].[{2}]", method.DeclaringType.Assembly.GetName().Name, method.DeclaringType.FullName, method.Name);

            SqlCommandProvider.ExecuteNonQuery(con, command.ToString(), transaction);
        }

        /// <summary>
        /// Register the specified method as a function using metadata from the attribute
        /// </summary>
        private static void RegisterFunction(MethodInfo method, SqlFunctionAttribute function, SqlConnection con, SqlTransaction transaction)
        {
            String functionName = function.Name;
            if (string.IsNullOrEmpty(functionName))
            {
                functionName = method.Name;
            }

            log.InfoFormat("Registering {0} as FUNCTION {1}", method.Name, functionName);

            string returnType = null;

            // Table-Valued-Function
            if (method.ReturnType == typeof(IEnumerable))
            {
                returnType = "TABLE (" + function.TableDefinition + ")";
            }
            else
            {
                returnType = GetSqlTypeDeclaration(method.ReturnType);
                if (returnType == null)
                {
                    throw new InvalidOperationException(string.Format("Type {0} is not valid as a return type on UDF {1}.", method.ReturnType, functionName));
                }
            }

            StringBuilder command = new StringBuilder();
            command.AppendFormat("CREATE FUNCTION [{0}] ", functionName);
            command.AppendFormat(" ( " + GetParameterDeclaration(method) + " ) ");
            command.AppendFormat(" RETURNS {0} ", returnType);
            command.AppendFormat(" AS EXTERNAL NAME [{0}].[{1}].[{2}]", method.DeclaringType.Assembly.GetName().Name, method.DeclaringType.FullName, method.Name);

            SqlCommandProvider.ExecuteNonQuery(con, command.ToString(), transaction);
        }

        /// <summary>
        /// Get the SQL type declaration for the given .net type
        /// </summary>
        [NDependIgnoreComplexMethodAttribute]
        private static string GetSqlTypeDeclaration(Type type)
        {
            string typeName = type.Name;

            // Remove trailing & for ByRef types.
            typeName = typeName.Replace("&", "");

            switch (typeName)
            {
                case "SqlBoolean": return "bit";
                case "SqlBinary": return "varbinary(8000)";
                case "SqlByte": return "tinyint";
                case "SqlChars": return "nvarchar(max)";
                case "SqlDateTime": return "datetime";
                case "SqlDecimal": return "decimal";
                case "SqlDouble": return "float";
                case "SqlGuid": return "uniqueidentifier";
                case "SqlInt16": return "smallint";
                case "SqlInt32": return "int";
                case "SqlInt64": return "bigint";
                case "SqlMoney": return "money";
                case "SqlSingle": return "real";
                case "SqlString": return "nvarchar(4000)";
                case "SqlXml": return "xml";
                case "Int16": return "smallint";
                case "Int32": return "int";
                case "Int64": return "bigint";
                case "Boolean": return "bit";
                case "Float": return "float";
                case "Decimal": return "decimal";
                case "String": return "nvarchar(4000)";
                case "Double": return "float";
            }

            return null;
        }

        /// <summary>
        /// Get the SQL script parameter declaration for the method
        /// </summary>
        private static string GetParameterDeclaration(MethodInfo method)
        {
            StringBuilder paramString = new StringBuilder();

            // Go through each parameter in the method
            foreach (ParameterInfo parameter in method.GetParameters())
            {
                // See if we are opening it, or continuing
                if (paramString.Length != 0)
                {
                    paramString.Append(", ");
                }

                string paramType = GetSqlTypeDeclaration(parameter.ParameterType);

                if (paramType == null)
                {
                    throw new InvalidOperationException(string.Format("Type {0} can not be used as a SQL Server parameter for method {1}.", parameter.ParameterType, method));
                }

                paramString.AppendFormat("@{0} {1}", parameter.Name, paramType);

                // add OUTPUT if parameter is passed by reference as in (@i INT OUTPUT)
                if (parameter.ParameterType.IsByRef)
                {
                    paramString.Append(" OUTPUT");
                }
            }

            if (paramString.Length > 0)
            {
                paramString.Append(" ");
            }

            return paramString.ToString();
        }
    }
}
