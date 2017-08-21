using System.Reflection;
using System.Text;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Analog to MethodBase that stores its string representation in a ready-to-consume form for easy extraction
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    internal struct MethodBaseSlim
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal MethodBaseSlim(string stringValue, bool isEventInfrastructure, bool isExternalCode)
        {
            StringValue = stringValue;
            IsEventInfrastructure = isEventInfrastructure;
            IsExternalCode = isExternalCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MethodBaseSlim(MethodBase method)
        {
            StringValue = GetStringValue(method);

            IsEventInfrastructure = ExternalCodeHelper.IsEventInfrastructure(method);
            IsExternalCode = ExternalCodeHelper.IsExternalCode(method);
        }

        /// <summary>
        /// Value as string
        /// </summary>
        public string StringValue { get; }

        /// <summary>
        /// Is this event infrastructure
        /// </summary>
        public bool IsEventInfrastructure { get; }

        /// <summary>
        /// Is this external code
        /// </summary>
        public bool IsExternalCode { get; }

        /// <summary>
        /// Fancy ToString
        /// </summary>
        /// <param name="method">Method</param>
        /// <returns>String value</returns>
        private static string GetStringValue(MethodBase method)
        {
            if (method == null)
            {
                return null;
            }

            var builder = new StringBuilder();

            if (method.DeclaringType != null)
            {
                builder.Append(method.DeclaringType.FullName);
                builder.Append(".");
            }

            builder.Append(method.Name);
            if (method.IsGenericMethod)
            {
                builder.Append("[");

                var genericArguments = method.GetGenericArguments();

                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(",");
                    }

                    builder.Append(genericArguments[i]);
                }

                builder.Append("]");
            }

            builder.Append("(");

            var parameters = method.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(parameters[i] != null ? parameters[i].ParameterType.Name : "<UnknownType>");
                builder.Append(string.IsNullOrEmpty(parameters[i].Name) ? string.Empty : " " + parameters[i].Name);
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}