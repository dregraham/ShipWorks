using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration to map to the FedEx NAFTA determination code values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExNaftaDeterminationCode
    {
        [Description("Producer of the commodity")]
        ProducerOfCommodity = 0,

        [Description("Not the producer based on knowledge of the commodity")]
        NotProducerKnowledgeOfCommodity = 1,

        [Description("Not the producer based on a written statement from the producer")]
        NotProducerWrittenStatement = 2,

        [Description("Not the producer based on a signed certificate from the producer")]
        NotProducerSignedCertificate = 3
    }
}
