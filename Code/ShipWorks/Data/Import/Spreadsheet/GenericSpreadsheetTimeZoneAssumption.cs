using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// For dates that don't have a specified timezone, controls what timezone they are assumed to be in
    /// </summary>
    public enum GenericSpreadsheetTimeZoneAssumption
    {
        Local = 0,
        UTC = 1
    }
}
