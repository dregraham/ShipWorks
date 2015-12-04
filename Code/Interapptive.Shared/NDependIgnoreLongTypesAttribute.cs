using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Interapptive.Shared
{
    /// <summary>
    /// Attribute that can be applied to indicate the class should be ignored during NDepend metric calculations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class NDependIgnoreLongTypesAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NDependIgnoreLongTypesAttribute()
        {
        }
    }
}
