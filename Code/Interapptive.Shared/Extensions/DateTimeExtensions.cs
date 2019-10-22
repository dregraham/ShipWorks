using System;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        public static string ToIsoString(this DateTime value)
        {
            return value.ToUniversalTime().ToString("o");
        }
    }
}
