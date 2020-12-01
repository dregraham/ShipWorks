using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Installer.Extensions
{
    /// <summary>
    /// Extensions for enums
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the description of the given enumerated value.
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            string description = string.Empty;
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo.Any())
            {
                object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Any())
                {
                    description = ((DescriptionAttribute) attributes[0])?.Description;
                }
            }

            return description;
        }
    }
}
