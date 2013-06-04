using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Interapptive.Shared
{
    /// <summary>
    /// Attribute that can be applied to indicate the date\time the assembly was built.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class AssemblyDateAttribute : Attribute
    {
        DateTime date;

        /// <summary>
        /// Constructor
        /// </summary>
        public AssemblyDateAttribute(string date)
        {
            this.date = DateTime.Parse(date, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// The date\time the assembly was built.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.date;
            }
        }

        /// <summary>
        /// Reads the DateTime specifed in the attribute applied to the current calling assembly.
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public static DateTime Read()
        {
            return Read(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Reads the DateTime specifed in the attribute applied to the specified assembly.
        /// </summary>
        /// <exception cref="System.InvalidOperationException" />
        public static DateTime Read(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            AssemblyDateAttribute attribute = GetCustomAttribute(assembly, typeof(AssemblyDateAttribute)) as AssemblyDateAttribute;

            if (attribute == null)
            {
                throw new InvalidOperationException(string.Format("The AssemblyDateAttribute is not applied to assembly '{0}'.", assembly.FullName));
            }

            return attribute.Date;
        }
    }
}
