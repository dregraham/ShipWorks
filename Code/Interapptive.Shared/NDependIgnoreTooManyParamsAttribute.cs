using System;

namespace Interapptive.Shared
{
    /// <summary>
    /// Attribute that can be applied to indicate the method parameter count should be ignored during NDepend metric calculations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class NDependIgnoreTooManyParamsAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NDependIgnoreTooManyParamsAttribute()
        {
        }

        /// <summary>
        /// Justification for ignoring too many parameters check
        /// </summary>
        public string Justification { get; set; }
    }
}
