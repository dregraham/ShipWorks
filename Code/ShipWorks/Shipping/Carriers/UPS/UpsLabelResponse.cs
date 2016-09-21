using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// DTO for passing UPS label data around
    /// </summary>
    public class UpsLabelResponse
    {
        /// <summary>
        /// The shipment
        /// </summary>
        public ShipmentEntity Shipment;

        /// <summary>
        /// The ShipConfirm xml navigator
        /// </summary>
        public XPathNavigator ShipConfirmNavigator;

        /// <summary>
        /// The ShipAccept response
        /// </summary>
        public XmlDocument AcceptResponse;

        /// <summary>
        /// The UPS account used
        /// </summary>
        public UpsAccountEntity Account;
    }
}
