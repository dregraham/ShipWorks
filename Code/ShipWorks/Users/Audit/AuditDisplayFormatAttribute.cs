using System;
using System.Linq;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Allows an enum type to be registered as a display format
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class AuditDisplayFormatAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuditDisplayFormatAttribute(int format)
        {
            Format = format;
        }

        /// <summary>
        /// Audit display format number
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// Register all loaded AuditDisplayFormats
        /// </summary>
        public static void Register()
        {
            var typeFormats = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.GetName()?.Name?.StartsWith("ShipWorks") ?? false)
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsEnum)
                .Select(x => new { Type = x, Format = GetFormatFromType(x) })
                .Where(x => x.Format.HasValue);
            
            foreach (var typeFormat in typeFormats)
            {
                AuditDisplayFormat.RegisterDisplayFormat(typeFormat.Format.Value, typeFormat.Type);
            }
        }

        /// <summary>
        /// Get audit format from a given type
        /// </summary>
        private static int? GetFormatFromType(Type x) =>
            x.GetCustomAttributes(typeof(AuditDisplayFormatAttribute), false).Cast<AuditDisplayFormatAttribute>().FirstOrDefault()?.Format;
    }
}
