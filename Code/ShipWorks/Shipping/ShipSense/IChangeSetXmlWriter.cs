using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Interface for classes that implement change set xml writers
    /// </summary>
    public interface IChangeSetXmlWriter
    {
        /// <summary>
        /// This method appends the XML representation of the ChangeSet to the given XElement.
        /// </summary>
        /// <param name="changeSetXElement"></param>
        void Write(XElement changeSetXElement);
    }
}
