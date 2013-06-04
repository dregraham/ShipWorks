using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Strongly typed collection of CSV field mappings
    /// </summary>
    public class GenericSpreadsheetFieldMappingCollection : IEnumerable<GenericSpreadsheetFieldMapping>
    {
        List<GenericSpreadsheetFieldMapping> mappings = new List<GenericSpreadsheetFieldMapping>();

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetFieldMappingCollection()
        {

        }

        /// <summary>
        /// Adds (or replaces) the mapping for the target field represented by the given mapping
        /// </summary>
        public void SetMapping(GenericSpreadsheetFieldMapping mapping)
        {
            GenericSpreadsheetFieldMapping existing = this[mapping.TargetField];
            if (existing != null)
            {
                mappings.Remove(existing);
            }

            if (!string.IsNullOrEmpty(mapping.SourceColumnName))
            {
                mappings.Add(mapping);
            }
        }

        /// <summary>
        /// Get or set the mapping for the given target field, or null if not present
        /// </summary>
        public GenericSpreadsheetFieldMapping this[GenericSpreadsheetTargetField field]
        {
            get
            {
                return this[field.Identifier];
            }
        }

        /// <summary>
        /// Get or set the mapping for the given target field identifier, or null if not present.
        /// </summary>
        public GenericSpreadsheetFieldMapping this[string identifier]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    throw new ArgumentException("Cannot be null or empty", "identifier");
                }

                return mappings.FirstOrDefault(m => m.TargetField.Identifier == identifier);
            }
        }

        /// <summary>
        /// The names of all columns from the source schema that are currently used by the mappings
        /// </summary>
        public IEnumerable<string> UtilizedSourceColumns
        {
            get
            {
                return mappings.Select(m => m.SourceColumnName).Distinct().ToList();
            }
        }

        /// <summary>
        /// Enumeration
        /// </summary>
        public IEnumerator<GenericSpreadsheetFieldMapping> GetEnumerator()
        {
            return mappings.GetEnumerator();
        }

        #region IEnumerable Members

        /// <summary>
        /// Non-generic implementation
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }
}
