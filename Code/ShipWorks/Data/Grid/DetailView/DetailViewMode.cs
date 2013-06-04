using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Data.Grid.DetailView
{
    /// <summary>
    /// Various modes the grid can be displayed as
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DetailViewMode
    {
        [Description("Normal")]
        Normal = 0,

        [Description("Normal with Detail")]
        NormalWithDetail = 1,

        [Description("Detail Only")]
        DetailOnly = 2
    }
}
