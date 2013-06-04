using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Data;
using System.IO;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Script loader used by the migration process to perform
    /// additional transformations on the scripts when they are loaded.
    /// </summary>
    public class MigrationSqlScriptLoader : SqlScriptLoader
    {
        /// <summary>
        /// Convenience method for accessing the property bag
        /// </summary>
        private MigrationPropertyBag PropertyBag
        {
            get
            {
                if (MigrationContext.Current == null)
                {
                    // just use an empty one
                    return new MigrationPropertyBag();
                }

                return MigrationContext.Current.PropertyBag;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MigrationSqlScriptLoader(string resourcePath)
            : base(resourcePath)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MigrationSqlScriptLoader(DirectoryInfo folder)
            : base(folder)
        {
        }

        /// <summary>
        /// Load a script and perform necessary translations/additions for
        /// migrations
        /// </summary>
        public override SqlScript LoadScript(string name)
        {
            if (MigrationContext.Current != null && MigrationContext.Current.CurrentTask != null)
            {
                MigrationContext.Current.PropertyBag["IsArchive"] = MigrationContext.Current.CurrentTask.IsArchiveDatabase;
            }

            SqlScript script = null;
            try
            {
                script = base.LoadScript(name);
            }
            catch (SqlScriptException)
            {
                return null;
            }
            
            // inject necessary values
            string runtimeVariables = GenerateDynamicContent();

            // return a concatenated script
            return new SqlScript(script.Name, runtimeVariables + TokenReplacement(script.Content));
        }

        /// <summary>
        /// Gets the TSQL data type for the variable to contain this value
        /// </summary>
        private string GetTsqlDataType(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Type valueType = value.GetType();
            if (valueType == typeof(int))
            {
                return "INT";
            }
            else if (valueType == typeof(long))
            {
                return "BIGINT";
            }
            else if (valueType == typeof(DateTime))
            {
                return "DATETIME";
            }
            else if (valueType == typeof(string))
            {
                return "NVARCHAR(255)";
            }
            else if (valueType == typeof(bool))
            {
                return "BIT";
            }
            else
            {
                throw new InvalidOperationException("Cannot generate a TSQL data type for a migration property of type " + valueType.ToString());
            }
        }

        /// <summary>
        /// Gets text used to SET the value of a variable in tsql
        /// </summary>
        private string GetTsqlVariableValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Type valueType = value.GetType();
            if (valueType == typeof(int) ||
                valueType == typeof(long))
            {
                return value.ToString();
            }
            else if (valueType == typeof(string))
            {
                return "'" + value + "'";

            }
            else if (valueType == typeof(DateTime))
            {
                return "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else if (valueType == typeof(bool))
            {
                return (bool)value ? "1" : "0";
            }
            else
            {
                throw new InvalidOperationException("Cannot generate a TSQL data type for a migration property of type " + valueType.ToString());
            }
        }

        /// <summary>
        /// Gets the value of a token's replacement
        /// </summary>
        private string GetTsqlTokenValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Type valueType = value.GetType();
            if (valueType == typeof(int) ||
                valueType == typeof(long) || 
                valueType == typeof(string))
            {
                return value.ToString();
            }
            else if (valueType == typeof(DateTime))
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (valueType == typeof(bool))
            {
                return ((bool)value) ? "1" : "0";
            }
            else
            {
                throw new InvalidOperationException("Cannot generate a TSQL data type for a migration property of type " + valueType.ToString());
            }
        }

        /// <summary>
        /// Performs a search/replace for string tokens in the script.
        /// 
        /// ex.  "{DBNAME}" => "ShipWorks"
        /// </summary>
        private string TokenReplacement(string scriptContents)
        {
           
            // perform a straight search/replace for
            foreach (string propertyName in PropertyBag.PropertyNames)
            {
                string tokenText = "{" + propertyName.ToUpperInvariant() + "}";
                string tokenValue = GetTsqlTokenValue(PropertyBag[propertyName]);

                scriptContents = scriptContents.Replace(tokenText, tokenValue);
            }

            return scriptContents;
        }

        /// <summary>
        /// Create script content
        /// </summary>
        /// <returns></returns>
        private string GenerateDynamicContent()
        {
            StringBuilder prependBlock = new StringBuilder();

            int count = PropertyBag.PropertyNames.Count;
            if (count > 0)
            {
                prependBlock.AppendLine("DECLARE");

                int i = 0;
                foreach (string propertyName in PropertyBag.PropertyNames)
                {
                    i++;
                    
                    // add the DECLARE entry
                    prependBlock.AppendFormat("@{0} {1}", propertyName, GetTsqlDataType(PropertyBag[propertyName]));

                    if (i != count)
                    {
                        prependBlock.AppendLine(",");
                    }
                }
                prependBlock.AppendLine();
                prependBlock.AppendLine();

                // do SET values
                foreach (string propertyName in PropertyBag.PropertyNames)
                {
                    prependBlock.AppendFormat(@"SET @{0} = {1}", propertyName, GetTsqlVariableValue(PropertyBag[propertyName]));
                    prependBlock.AppendLine();
                }

                prependBlock.AppendLine();
                prependBlock.AppendLine();
            }

            return prependBlock.ToString();
        }
    }
}
