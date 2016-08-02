using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using log4net;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    /// <summary>
    /// Serialization binder that is version agnostic
    /// </summary>
    internal class VersionAgnosticSerializationBinder : SerializationBinder
    {
        private static ILog log = LogManager.GetLogger(typeof(VersionAgnosticSerializationBinder));

        /// <summary>
        /// Bind the specified type
        /// </summary>
        /// <remarks>If we return null here, we'll fall back to the default assembly loading behavior</remarks>
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (assemblyName.StartsWith("ShipWorks"))
            {
                Assembly assembly = Assembly.Load(assemblyName.Split(',').First());
                if (assembly != null)
                {
                    return FormatterServices.GetTypeFromAssembly(assembly, typeName);
                }

                log.Warn($"Could not load assembly {assemblyName}");
            }

            return null;
        }
    }
}