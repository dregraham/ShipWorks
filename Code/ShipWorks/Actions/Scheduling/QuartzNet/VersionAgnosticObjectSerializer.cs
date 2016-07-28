using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Quartz.Spi;

namespace ShipWorks.Actions.Scheduling.QuartzNet
{
    public class VersionAgnosticObjectSerializer : IObjectSerializer
    {
        /// <summary>
        /// Serializes given object as bytes
        /// that can be stored to permanent stores.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        public byte[] Serialize<T>(T obj) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Binder = new VersionAgnosticSerializationBinder();
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes object from byte array presentation.
        /// </summary>
        /// <param name="data">Data to deserialize object from.</param>
        public T DeSerialize<T>(byte[] data) where T : class
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Binder = new VersionAgnosticSerializationBinder();
                return (T) formatter.Deserialize(ms);
            }
        }
    }
}
