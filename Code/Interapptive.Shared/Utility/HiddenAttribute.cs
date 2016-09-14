using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Hide enum values from enumeration
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HiddenAttribute : Attribute
    {
    }
}
