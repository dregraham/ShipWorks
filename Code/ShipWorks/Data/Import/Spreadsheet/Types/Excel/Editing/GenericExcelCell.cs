using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// Represents an excel cell on a sheet
    /// </summary>
    public class GenericExcelCell
    {
        public string Sheet { get; set; }
        public string Address { get; set; }

        #region equality

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            GenericExcelCell other = obj as GenericExcelCell;
            if ((object) other == null)
            {
                return false;
            }

            return
                this.Sheet == other.Sheet &&
                this.Address == other.Address;
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(GenericExcelCell left, GenericExcelCell right)
        {
            return object.Equals(left, right);
        }

        /// <summary>
        /// Operator!=
        /// </summary>
        public static bool operator !=(GenericExcelCell left, GenericExcelCell right)
        {
            return !object.Equals(left, right);
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            string hashBase = Sheet + Address;

            return hashBase.GetHashCode();
        }

        #endregion 

    }
}
