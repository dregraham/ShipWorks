﻿using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource.Schema
{
    /// <summary>
    /// Type of Column Source
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum OdbcColumnSourceType
    {
        [ApiValue("Table")]
        [Description("The column source is a table.")]
        Table = 0,

        [ApiValue("CustomQuery")]
        [Description("The column source is a custom sub query.")]
        CustomQuery = 1,

        [ApiValue("CustomParameterizedQuery")]
        [Description("The column source is a custom parameterized query.")]
        CustomParameterizedQuery = 2
    }
}