using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    public class XmlDeserializer
    {
        private XmlSerializer serializer;
        public object Deserialize(Type dtoType, string xml)
        {
            serializer = new XmlSerializer(dtoType);

            object result;

            using (TextReader reader = new StringReader(xml))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
