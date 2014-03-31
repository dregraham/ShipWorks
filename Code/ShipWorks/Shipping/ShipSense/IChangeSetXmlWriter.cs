using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Interface for classes that implement change set XML writers
    /// </summary>
    public interface IChangeSetXmlWriter
    {
        /// <summary>
        /// Writes the XML representation of a knowledge base entry's change set to the given XElement.
        /// </summary>
        /// <param name="element">The XElement being written to.</param>
        void WriteTo(XElement element);
    }
}
