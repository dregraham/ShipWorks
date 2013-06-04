using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email
{
    public enum EmailOutboundRelationType
    {
        /// <summary>
        /// Represents a relation to the object that was processed in the context of the template.
        /// </summary>
        ContextObject = 0,

        /// <summary>
        /// Represents a relation to the object that should show up in the panels.
        /// </summary>
        RelatedObject = 1
    }
}
