using System;
using System.Collections.Generic;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// A list of enumerated values with descriptions.  Suitable for binding, especially to ComboBox.
    /// </summary>
    public class EnumList<T> : List<EnumEntry<T>> where T: struct
    {

    }
}
