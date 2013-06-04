﻿using System;
using System.Data;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.WebServices;
using System.IO;

namespace ShipWorks.Shipping.Carriers.iParcel.Net.Ship
{
    /// <summary>
    /// A representation of an i-parcel request used to obtain rates through the i-parcel web service.
    /// </summary>
    public class iParcelRateRequest : iParcelRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelRateRequest" /> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="shipment">The shipment.</param>
        public iParcelRateRequest(iParcelCredentials credentials, ShipmentEntity shipment)
            : base(credentials, "iParcelRateRequest")
        {
            bool isDomestic = shipment.OriginCountryCode.ToUpperInvariant() == shipment.ShipCountryCode.ToUpperInvariant();

            // Default the validation element to domestic for now
            RequestElements.Add(new iParcelShipValidationElement(credentials, isDomestic, true));
            RequestElements.Add(new iParcelVersionElement());
            RequestElements.Add(new iParcelPackageInfoElement(shipment, new iParcelTokenProcessor(), isDomestic, true));
        }

        /// <summary>
        /// Gets the name of the operation being invoked on the i-parcel system.
        /// </summary>
        /// <value>The name of the operation.</value>
        public override string OperationName
        {
            get { return "SubmitPack"; }
        }

        /// <summary>
        /// Gets the name of the root element for the XML sent in UploadXMLFile method of the
        /// i-parcel web service.
        /// </summary>
        /// <value>The name of the root element.</value>
        public override string RootElementName
        {
            get { return "iparcelPackageUpload"; }
        }

        /// <summary>
        /// Executes the logged request. This is overridden here to call the UploadXMLFileString 
        /// due to a serialization error that occurs when calling the normal UploadXMLFile method 
        /// used in the base class.
        /// </summary>
        /// <returns>The raw response from iParcel in the form of a DataSet.</returns>
        public override DataSet Submit()
        {
            try
            {
                using (XMLSOAP iParcelWebService = new XMLSOAP(new ApiLogEntry(ApiLogSource.iParcel, RequestTypeName)))
                {
                    // Use the UploadXMLFileString method and load the XML into a DataSet. This response does
                    // not have all of the schema information like the UploadXMLFile method does, so the 
                    // DataSet loads without any problem
                    string xmlResponse = iParcelWebService.UploadXMLFileString(OperationName, GetRequestXml());
                    using (DataSet dataSet = new DataSet())
                    {
                        using (StringReader reader = new StringReader(xmlResponse))
                        {
                            dataSet.ReadXml(reader, XmlReadMode.Auto);
                            CheckForErrors(dataSet);

                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in ExecuteLoggedRequest", ex);
                throw WebHelper.TranslateWebException(ex, typeof(iParcelException));
            }
        }
    }
}
