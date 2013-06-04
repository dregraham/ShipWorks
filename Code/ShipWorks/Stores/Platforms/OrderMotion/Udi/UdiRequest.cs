using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;

namespace ShipWorks.Stores.Platforms.OrderMotion.Udi
{
    public delegate void WriteRequestDetailsHandler(XmlTextWriter writer);

    /// <summary>
    /// Base class for OrderMotion's UDI system requests 
    /// </summary>
    public class UdiRequest
    {
        // The default api version for requests
        public const string DefaultVersion = "1.00";

        // request version
        string requestVersion = DefaultVersion;

        // request name
        UdiRequestName requestName;

        // request parameters
        NameValueCollection parameters = new NameValueCollection();

        // delegate for writing extra request information
        WriteRequestDetailsHandler detailsWriter;

        /// <summary>
        /// Constructor
        /// </summary>
        public UdiRequest(UdiRequestName requestName, string requestVersion)
        {
            this.requestName = requestName;
            this.requestVersion = requestVersion;

            if (requestName == UdiRequestName.OrderInformation)
            {
                this.requestVersion = "2.00";
            }
        }

        /// <summary>
        /// Name of the request
        /// </summary>
        public string Name
        {
            get
            {
                switch (requestName)
                {
                    case UdiRequestName.AccountStatus: return "AccountStatusRequest";
                    case UdiRequestName.ItemInformation: return "ItemInformationRequest";
                    case UdiRequestName.OrderInformation: return "OrderInformationRequest";
                    case UdiRequestName.ShipmentStatusUpdate: return "ShipmentStatusUpdateRequest";
                }

                throw new InvalidOperationException(string.Format("Value '{0}' is not handled for getting UDI request name.", (int) requestName));
            }
        }

        /// <summary>
        /// UDI request version
        /// </summary>
        public string RequestVersion
        {
            get { return requestVersion; }
            set { requestVersion = value; }
        }

        /// <summary>
        /// The URI to use for the request
        /// </summary>
        public Uri Uri
        {
            get
            {
                switch (requestName)
                {
                    case UdiRequestName.OrderInformation:
                        return new Uri("https://api.omx.ordermotion.com/OM2/udi.ashx");

                    default:
                        return new Uri("https://api.omx.ordermotion.com/hdde/xml/udi.asp");
                }
            }
        }

        /// <summary>
        /// Gets the request parameters
        /// </summary>
        public NameValueCollection Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Output the request xml
        /// </summary>
        public void WriteRequest(XmlTextWriter writer)
        {
            // start the document
            writer.WriteStartDocument();

            // start the request
            writer.WriteStartElement(Name);
            writer.WriteAttributeString("version", requestVersion);


            // parameters collection
            writer.WriteStartElement("UDIParameter");
            foreach (string paramName in parameters)
            {
                string paramValue = parameters[paramName];

                // individual parameter
                writer.WriteStartElement("Parameter");
                writer.WriteAttributeString("key", paramName);
                writer.WriteString(paramValue);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            // write other request contents
            WriteRequestDetails(writer);

            // request
            writer.WriteEndElement();

            // document
            writer.WriteEndDocument();
        }

        /// <summary>
        /// Write request data other than just Parameters.  For derived classes
        /// </summary>
        private void WriteRequestDetails(XmlTextWriter writer)
        {
            if (detailsWriter != null)
            {
                detailsWriter(writer);
            }
        }

        /// <summary>
        /// Assign a custom request detail writer
        /// </summary>
        public void WriteRequestDetails(WriteRequestDetailsHandler writer)
        {
            this.detailsWriter = writer;
        }
    }
}
