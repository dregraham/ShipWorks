using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// UPS authorization data for account registration
    /// </summary>
    public class UpsOltInvoiceAuthorizationData
    {
        public string InvoiceNumber
        {
            get;
            set;
        }

        public DateTime InvoiceDate
        {
            get;
            set;
        }

        public decimal InvoiceAmount
        {
            get;
            set;
        }

        public string ControlID
        {
            get;
            set;
        }
    }
}
