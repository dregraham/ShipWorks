using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Serialization binder that is version agnostic
    /// </summary>
    internal class VersionAgnosticSerializationBinder : SerializationBinder
    {
        /// <summary>
        /// Bind the specified type
        /// </summary>
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (assemblyName.StartsWith("ShipWorks"))
            {
                Assembly assembly = Assembly.Load(assemblyName.Split(',').First());
                return FormatterServices.GetTypeFromAssembly(assembly, typeName);
            }

            return null;
        }
    }
}