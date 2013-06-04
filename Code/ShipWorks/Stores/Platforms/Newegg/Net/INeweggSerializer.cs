
namespace ShipWorks.Stores.Platforms.Newegg.Net
{
    /// <summary>
    /// An interface for serializing Newegg data transport objects.
    /// </summary>
    public interface INeweggSerializer
    {
        /// <summary>
        /// Deserializes the specified XML. If an object is unable to be 
        /// serialized from the XML, a null value is returned.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>A deserialized object.</returns>
        object Deserialize(string xml);

        /// <summary>
        /// Serializes the specified object into XML.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// An XML representation of the given object.
        /// </returns>
        string Serialize(object value);
    }
}
