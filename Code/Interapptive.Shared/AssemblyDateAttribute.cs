using System;
using System.Globalization;
using System.Reflection;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared
{
    /// <summary>
    /// Attribute that can be applied to indicate the date/time the assembly was built.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class AssemblyDateAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyDateAttribute(string date)
        {
            Date = DateTime.Parse(date, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// The date/time the assembly was built.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Reads the DateTime specified in the attribute applied to the current calling assembly.
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public static DateTime Read() =>
            Read(Assembly.GetCallingAssembly());

        /// <summary>
        /// Reads the DateTime specified in the attribute applied to the specified assembly.
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public static DateTime Read(Assembly assembly) =>
            ReadResult(assembly).Match(x => x, ex => throw ex);

        /// <summary>
        /// Reads the DateTime specified in the attribute applied to the specified assembly.
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public static GenericResult<DateTime> ReadResult(Assembly assembly) =>
            assembly.ToResult(() => new ArgumentNullException("assembly"))
                .Bind(GetAssemblyDateAttribute)
                .Map(x => x.Date);

        /// <summary>
        /// Get the assembly date attribute
        /// </summary>
        private static GenericResult<AssemblyDateAttribute> GetAssemblyDateAttribute(Assembly assembly) =>
            assembly.GetCustomAttribute<AssemblyDateAttribute>()
                .ToResult(() => new InvalidOperationException($"The AssemblyDateAttribute is not applied to assembly '{assembly.FullName}'."));
    }
}
