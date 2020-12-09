using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace ShipWorks.Serialization
{
    /// <summary>
    /// Custom implementation of the Json.Net serialization binder that ignores
    /// library version data
    /// </summary>
    public class JsonSerializationBinder : ISerializationBinder
    {
        /// <summary>
        /// Binds a serialized type name to its type
        /// </summary>
        public Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                //Get the name of the assembly, ignoring versions and public keys.
                var assembly = Assembly.Load(assemblyName);
                var type = assembly.GetType(typeName);
                return type;
            }
            catch (Exception)
            {
                //Revert to default binding behaviour.
                return null;
            }
        }

        /// <summary>
        /// Binds a serialized type to its type name
        /// </summary>
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName.Split(',')[0];
            typeName = serializedType.FullName;
        }
    }
}
