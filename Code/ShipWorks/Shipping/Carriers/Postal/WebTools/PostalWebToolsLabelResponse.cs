using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
     /// <summary>
     /// DTO for passing Postal Web Tools label data around
     /// </summary>
    public class PostalWebToolsLabelResponse
     {
        /// <summary>
        /// The postal shipment for the response
        /// </summary>
         public PostalShipmentEntity PostalShipment;

        /// <summary>
        /// The response from 
        /// </summary>
         public string XmlResponse;
     }
}
