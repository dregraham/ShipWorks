using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// Strategy used to read multiple line items from the CSV
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum GenericSpreadsheetOrderMultipleItemStrategy
    {
        [Description("All items are on the same line as the order")]
        SingleLine = 0,

        [Description("The order line is repeated for each item")]
        MultipleLine = 1
    }
}
