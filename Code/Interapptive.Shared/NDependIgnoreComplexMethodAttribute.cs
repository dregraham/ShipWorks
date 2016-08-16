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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    public sealed class NDependIgnoreComplexMethodAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NDependIgnoreComplexMethodAttribute()
        {
        }
    }
}
