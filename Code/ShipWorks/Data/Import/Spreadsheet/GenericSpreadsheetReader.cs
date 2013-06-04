using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Base class for reading from a generic spreadsheet
    /// </summary>
    public abstract class GenericSpreadsheetReader : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericSpreadsheetReader));

        GenericSpreadsheetMap map;

        // Suffix to be applied to every fieldIdentifer specfied in any public method
        string fieldIdentifierSuffix = "";

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericSpreadsheetReader(GenericSpreadsheetMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            this.map = map;
        }

        #region Disposable Pattern
        
        /// <summary>
        /// Finalizer
        /// </summary>
        ~GenericSpreadsheetReader()
        {
            Dispose(false);
        }
        

        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Access for derived classes to dispose correctly
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {

        }

        #endregion
        
        /// <summary>
        /// The map that is being used to do the reading
        /// </summary>
        public GenericSpreadsheetMap Map
        {
            get { return map; }
        }

        /// <summary>
        /// Suffix to be applied to every fieldIdentifer specfied in any public method
        /// </summary>
        public string FieldIdentifierSuffix 
        {
            get { return fieldIdentifierSuffix; }
            set { fieldIdentifierSuffix = value ?? ""; }
        }

        /// <summary>
        /// Advance the reader to the next record.  Returns false if there are no more records
        /// </summary>
        public abstract bool NextRecord();

        /// <summary>
        /// Rewind the reader to the previous record.  Returns false if already at the beginning.
        /// </summary>
        public abstract bool PreviousRecord();

        /// <summary>
        /// Get the text value of the given source column.  Should never return null - return string.Empty for missing values.
        /// </summary>
        public abstract string ReadColumnText(string column);

        /// <summary>
        /// Read the CSV value for the current target field
        /// </summary>
        private string InternalReadField(string identifier)
        {
            GenericSpreadsheetFieldMapping mapping = FindFieldMapping(identifier);

            if (mapping == null)
            {
                return null;
            }

            return ReadColumnText(mapping.SourceColumnName);
        }

        /// <summary>
        /// Find the field mapping represented by the given identifier, or null if it is not mapped.  Throws if the identifier is not a valid field in the schema
        /// </summary>
        private GenericSpreadsheetFieldMapping FindFieldMapping(string identifier)
        {
            // Apply the suffix
            identifier += fieldIdentifierSuffix;

            // Verify it's in the schema
            GenericSpreadsheetTargetField targetField = map.TargetSchema.GetField(identifier, map.TargetSettings);
            if (targetField == null)
            {
                throw new GenericSpreadsheetException(string.Format("The target field '{0}' does not exist in the target schema.", identifier));
            }

            // See if its mapped
            GenericSpreadsheetFieldMapping mapping = map.Mappings[identifier];
            if (mapping == null || string.IsNullOrWhiteSpace(mapping.SourceColumnName))
            {
                return null;
            }

            return mapping;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public string ReadField(string identifier, string unmappedDefault)
        {
            return ReadField(identifier, "", unmappedDefault);
        }

        /// <summary>
        /// Read the given field from the reader.  If its mapped, and the content is empty, the mappedBlankDefault is returned.  If unmapped, the unmappedDefault is returned.
        /// </summary>
        public string ReadField(string identifier, string mappedBlankDefault, string unmappedDefault)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return mappedBlankDefault;
            }

            return value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public int ReadField(string identifier, int defaultValue)
        {
            return ReadField(identifier, defaultValue, defaultValue).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public int? ReadField(string identifier, int? mappedDefault, int? unmappedDefault)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return mappedDefault;
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch (FormatException ex)
            {
                throw new GenericSpreadsheetException(string.Format("The value '{0}' in column '{1}' is not a valid integer.",
                    value,
                    FindFieldMapping(identifier).SourceColumnName), ex);
            }
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public long ReadField(string identifier, long defaultValue, bool useDefaultForMapped = true)
        {
            return ReadField(identifier, defaultValue, defaultValue, useDefaultForMapped).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public long? ReadField(string identifier, long? mappedDefault, long? unmappedDefault, bool useDefaultForMapped = true)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                if (useDefaultForMapped)
                {
                    return mappedDefault;
                }
                else
                {
                    throw new GenericSpreadsheetException(string.Format("An empty value was found in column '{0}'.",
                        FindFieldMapping(identifier).SourceColumnName));
                }
            }

            try
            {
                return Convert.ToInt64(value);
            }
            catch (FormatException ex)
            {
                throw new GenericSpreadsheetException(string.Format("The value '{0}' in column '{1}' is not a valid integer.",
                    value,
                    FindFieldMapping(identifier).SourceColumnName), ex);
            }
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public bool ReadField(string identifier, bool defaultValue)
        {
            return ReadField(identifier, defaultValue, defaultValue).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public bool? ReadField(string identifier, bool? mappedDefault, bool? unmappedDefault)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return mappedDefault;
            }

            value = value.Trim().ToLower();

            if (value == "yes" || value == "y" || value == "t" || value == "true" || value == "1")
            {
                return true;
            }

            if (value == "no" || value == "n" || value == "f" || value == "false" || value == "0")
            {
                return false;
            }

            throw new GenericSpreadsheetException(string.Format("The value '{0}' in column '{1}' is not a valid boolean value.",
                value,
                FindFieldMapping(identifier).SourceColumnName));
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public decimal ReadField(string identifier, decimal defaultValue)
        {
            return ReadField(identifier, defaultValue, defaultValue).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public decimal? ReadField(string identifier, decimal? mappedDefault, decimal? unmappedDefault)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return mappedDefault;
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch (FormatException ex)
            {
                throw new GenericSpreadsheetException(string.Format("The value '{0}' in column '{1}' is not a valid number.",
                    value,
                    FindFieldMapping(identifier).SourceColumnName), ex);
            }
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public double ReadField(string identifier, double defaultValue)
        {
            return ReadField(identifier, defaultValue, defaultValue).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public double? ReadField(string identifier, double? mappedDefault, double? unmappedDefault)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return mappedDefault;
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch (FormatException ex)
            {
                throw new GenericSpreadsheetException(string.Format("The value '{0}' in column '{1}' is not a valid number.",
                    value,
                    FindFieldMapping(identifier).SourceColumnName), ex);
            }
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public DateTime ReadField(string identifier, DateTime defaultValue, string dateFormat, bool useDefaultForMapped = true)
        {
            return ReadField(identifier, defaultValue, defaultValue, dateFormat, useDefaultForMapped).Value;
        }

        /// <summary>
        /// Read the given field from the reader.  DefaultValue is returned if its not mapped.
        /// </summary>
        public DateTime? ReadField(string identifier, DateTime? mappedDefault, DateTime? unmappedDefault, string dateFormat, bool useDefaultForMapped = true)
        {
            string value = InternalReadField(identifier);

            if (value == null)
            {
                return unmappedDefault;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                if (useDefaultForMapped)
                {
                    return mappedDefault;
                }
                else
                {
                    throw new GenericSpreadsheetException(string.Format("An empty date value was found in column '{0}'.",
                        FindFieldMapping(identifier).SourceColumnName));
                }
            }

            try
            {
                if (string.IsNullOrWhiteSpace(dateFormat) || dateFormat == "Automatic")
                {
                    return DateTime.Parse(value);
                }
                else
                {
                    return DateTime.ParseExact(value, dateFormat, null);
                }
            }
            catch (FormatException ex)
            {
                throw new GenericSpreadsheetException(string.Format("The date value '{0}' in column '{1}' could not be parsed with date format '{2}'.",
                    value,
                    FindFieldMapping(identifier).SourceColumnName,
                    dateFormat), ex);
            }
        }
    }
}
