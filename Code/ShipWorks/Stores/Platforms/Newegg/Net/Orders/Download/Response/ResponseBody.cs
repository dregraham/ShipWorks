using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response
{
    /// <summary>
    /// A data transport object containing the details of orders download from Newegg.
    /// </summary>
    [Serializable]
    [XmlRoot("ResponseBody")]
    public class ResponseBody
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseBody"/> class.
        /// </summary>
        public ResponseBody()
        {
            this.PageInfo = new PageInfo();
            this.Orders = new List<Order>();
        }

        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        /// <value>The request id.</value>
        [XmlElement("RequestID")]
        public string RequestId { get; set; }


        [XmlElement("RequestDate")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string RequestDateTransportInPacificStandardTime
        {
            // This is a shim for the request date property. Newegg is sending the request date
            // in the format of mm/dd/yyyy hh:mm:ss, but this causes a serialization error
            // because the xml format for dates should be yyyy-mm-dd
            get
            {
                return RequestDateInPacificStandardTime == DateTime.MinValue ? string.Empty : XmlConvert.ToString(RequestDateInPacificStandardTime, XmlDateTimeSerializationMode.RoundtripKind);
            }
            set
            {
                this.RequestDateInPacificStandardTime = string.IsNullOrEmpty(value) ? DateTime.MinValue : DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Gets or sets the request date in pacific standard time (the
        /// timezone that Newegg uses for all dates values.
        /// </summary>
        /// <value>The request date in pacific standard time.</value>
        [XmlIgnore]
        public DateTime RequestDateInPacificStandardTime { get; set; }

        /// <summary>
        /// Converts the request date to UTC time.
        /// </summary>
        /// <returns></returns>
        public DateTime RequestDateToUtcTime()
        {
            TimeZoneInfo pacificStandardTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return System.TimeZoneInfo.ConvertTimeToUtc(this.RequestDateInPacificStandardTime, pacificStandardTimeZone);
        }

        /// <summary>
        /// Gets or sets the page info.
        /// </summary>
        /// <value>The page info.</value>
        [XmlElement("PageInfo")]
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        [XmlArray("OrderInfoList")]
        [XmlArrayItem("OrderInfo")]
        public List<Order> Orders { get; set; }
    }
}
