using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Interapptive.Shared
{
    /// <summary>
    /// Attribute that can be applied to indicate the class or method should be ignored during NDepend metric calculations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class NDependIgnoreLongMethodAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NDependIgnoreLongMethodAttribute()
        {
        }
    }
}
