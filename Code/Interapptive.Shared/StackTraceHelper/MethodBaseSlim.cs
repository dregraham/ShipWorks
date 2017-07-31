using System.Reflection;
using System.Text;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Analog to MethodBase that stores its string representation in a ready-to-consume form for easy extraction
    /// </summary>
    internal struct MethodBaseSlim
    {
        internal MethodBaseSlim(string stringValue, bool isEventInfrastructure, bool isExternalCode)
        {
            StringValue = stringValue;
            IsEventInfrastructure = isEventInfrastructure;
            IsExternalCode = isExternalCode;
        }

        public MethodBaseSlim(MethodBase method)
        {
            StringValue = GetStringValue(method);

            IsEventInfrastructure = ExternalCodeHelper.IsEventInfrastructure(method);
            IsExternalCode = ExternalCodeHelper.IsExternalCode(method);
        }

        public string StringValue { get; }
        public bool IsEventInfrastructure { get; }
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