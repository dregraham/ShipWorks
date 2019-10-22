﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using System.Xml.XPath;
using System.Drawing;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using System.Web;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Imaging;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Class for connecting to the USPS WebTools API
    /// </summary>
    public static class PostalWebClientShipping
    {
        /// <summary>
        /// Process the given shipment and download a USPS label for it.
        /// </summary>
        public static PostalWebToolsLabelResponse ProcessShipment(PostalShipmentEntity postalShipment, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            PostalPackagingType packaging = (PostalPackagingType)postalShipment.PackagingType;

            if (packaging == PostalPackagingType.FlatRatePaddedEnvelope ||
                packaging == PostalPackagingType.FlatRateLegalEnvelope ||
                packaging == PostalPackagingType.RateRegionalBoxA ||
                packaging == PostalPackagingType.RateRegionalBoxB ||
                packaging == PostalPackagingType.RateRegionalBoxC)
            {
                throw new ShippingException(string.Format("{0} is not supported by {1}.", EnumHelper.GetDescription(packaging), ShipmentTypeManager.GetType(ShipmentTypeCode.PostalWebTools).ShipmentTypeName));
            }

            if (packaging == PostalPackagingType.CubicSoftPack && postalShipment.DimsHeight > 0.75)
            {
                throw new ShippingException(string.Format("{0} may only have a Height of 0.75\" or less.", EnumHelper.GetDescription(packaging)));
            }

            // Generate the request data
            string xmlRequest = GenerateXmlRequest(postalShipment);

            // Log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.UspsNoPostage, "Ship");
            logger.LogRequest(xmlRequest);

            // Process the request
            string xmlResponse = ProcessXmlRequest(postalShipment, xmlRequest, telemetricResult);

            // Log the response
            logger.LogResponse(xmlResponse);

            return new PostalWebToolsLabelResponse()
            {
                PostalShipment = postalShipment,
                XmlResponse = xmlResponse
            };
        }

        /// <summary>
        /// Generate the XML request string for the given shipment
        /// </summary>
        private static string GenerateXmlRequest(PostalShipmentEntity postalShipment)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);

                xmlWriter.Formatting = Formatting.Indented;

                if (postalShipment.Shipment.ShipPerson.IsDomesticCountry())
                {
                    if (postalShipment.Service == (int) PostalServiceType.ExpressMail)
                    {
                        GenerateXmlRequestExpress(xmlWriter, postalShipment);
                    }
                    else
                    {
                        GenerateXmlRequestDomestic(xmlWriter, postalShipment);
                    }
                }
                else
                {
                    GenerateXmlRequestInternational(xmlWriter, postalShipment);
                }

                return writer.ToString();
            }
        }

        /// <summary>
        /// Generate the XML request for a domestic express shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void GenerateXmlRequestExpress(XmlTextWriter xmlWriter, PostalShipmentEntity postalShipment)
        {
            ShipmentEntity shipment = postalShipment.Shipment;

            PersonAdapter toAdapter = new PersonAdapter(shipment, "Ship");
            PersonAdapter fromAdapter = new PersonAdapter(shipment, "Origin");

            xmlWriter.WriteStartElement("ExpressMailLabelRequest");

            // Username and password
            xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
            xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

            xmlWriter.WriteElementString("Option", "");
            xmlWriter.WriteElementString("EMCAAccount", "");
            xmlWriter.WriteElementString("EMCAPassword", "");
            xmlWriter.WriteElementString("ImageParameters", "");

            xmlWriter.WriteElementString("FromFirstName", fromAdapter.FirstName);
            xmlWriter.WriteElementString("FromLastName", fromAdapter.LastName);
            xmlWriter.WriteElementString("FromFirm", fromAdapter.Company);

            // This is correct, USPS uses the address lines in opposite world
            xmlWriter.WriteElementString("FromAddress1", fromAdapter.Street2 + ((fromAdapter.Street3.Length > 0) ? " - " + fromAdapter.Street3 : ""));
            xmlWriter.WriteElementString("FromAddress2", fromAdapter.Street1);

            xmlWriter.WriteElementString("FromCity", fromAdapter.City);
            xmlWriter.WriteElementString("FromState", Geography.GetStateProvCode(fromAdapter.StateProvCode));
            xmlWriter.WriteElementString("FromZip5", fromAdapter.PostalCode5);
            xmlWriter.WriteElementString("FromZip4", fromAdapter.PostalCode4);
            xmlWriter.WriteElementString("FromPhone", fromAdapter.Phone10Digits);

            xmlWriter.WriteElementString("ToFirstName", toAdapter.FirstName);
            xmlWriter.WriteElementString("ToLastName", toAdapter.LastName);
            xmlWriter.WriteElementString("ToFirm", toAdapter.Company);

            // This is correct, USPS uses the address lines in opposite world
            xmlWriter.WriteElementString("ToAddress1", toAdapter.Street2 + ((toAdapter.Street3.Length > 0) ? " - " + toAdapter.Street3 : ""));
            xmlWriter.WriteElementString("ToAddress2", toAdapter.Street1);

            xmlWriter.WriteElementString("ToCity", toAdapter.City);
            xmlWriter.WriteElementString("ToState", Geography.GetStateProvCode(toAdapter.StateProvCode));
            xmlWriter.WriteElementString("ToZip5", toAdapter.PostalCode5);
            xmlWriter.WriteElementString("ToZip4", toAdapter.PostalCode4);

            if (toAdapter.Phone10Digits.Length == 10)
            {
                xmlWriter.WriteElementString("ToPhone", toAdapter.Phone10Digits);
            }

            int weightOz = (int) Math.Ceiling(shipment.TotalWeight * 16.0);

            // Since weight doesnt affect the label in any way, if its not valid just set it
            // to some valid number
            if (weightOz <= 0)
            {
                weightOz = 1;
            }

            xmlWriter.WriteElementString("WeightInOunces", weightOz.ToString());
            xmlWriter.WriteElementString("FlatRate", "FALSE");
            xmlWriter.WriteElementString("StandardizeAddress", "TRUE");
            xmlWriter.WriteElementString("WaiverOfSignature", postalShipment.ExpressSignatureWaiver ? "TRUE" : "FALSE");

            xmlWriter.WriteElementString("SeparateReceiptPage", "False");
            xmlWriter.WriteElementString("POZipCode", "");
            xmlWriter.WriteElementString("ImageType", "GIF");
            xmlWriter.WriteElementString("LabelDate", string.Format("{0:MM/dd/yyyy}", shipment.ShipDate.ToLocalTime()));

            GenerateCustomsXml(xmlWriter, postalShipment);

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Generates CustomsXml. This is used APO shipments and other instances where the shipment is kindof international.
        /// </summary>
        private static void GenerateCustomsXml(XmlTextWriter xmlWriter, PostalShipmentEntity postalShipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.PostalWebTools);
            if (shipmentType.IsCustomsRequired(postalShipment.Shipment))
            {
                WriteShippingContents(xmlWriter, postalShipment);

                PostalCustomsContentType contentType = (PostalCustomsContentType) postalShipment.CustomsContentType;
                xmlWriter.WriteElementString("CustomsContentType", GetContentTypeApiValue(contentType));

                if (contentType == PostalCustomsContentType.Other)
                {
                    xmlWriter.WriteElementString("ContentComments", postalShipment.CustomsContentDescription);
                }
            }
        }

        /// <summary>
        /// Generate the XML request for a domestic shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void GenerateXmlRequestDomestic(XmlTextWriter xmlWriter, PostalShipmentEntity postalShipment)
        {
            ShipmentEntity shipment = postalShipment.Shipment;

            PersonAdapter toAdapter = new PersonAdapter(shipment, "Ship");
            PersonAdapter fromAdapter = new PersonAdapter(shipment, "Origin");

            // Signature Confirm
            if (postalShipment.Confirmation == (int)PostalConfirmationType.Signature)
            {
                xmlWriter.WriteStartElement("SignatureConfirmationV4.0Request");
            }

            // Delivery Confirm
            else
            {
                xmlWriter.WriteStartElement("DeliveryConfirmationV4.0Request");
            }

            // Username and password
            xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
            xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

            xmlWriter.WriteElementString("Option", "1");
            xmlWriter.WriteElementString("ImageParameters", "");

            xmlWriter.WriteElementString("FromName", new PersonName(fromAdapter).FullName);
            xmlWriter.WriteElementString("FromFirm", fromAdapter.Company);

            // This is correct, USPS uses the address lines in opposite world
            xmlWriter.WriteElementString("FromAddress1", fromAdapter.Street2 + ((fromAdapter.Street3.Length > 0) ? " - " + fromAdapter.Street3 : ""));
            xmlWriter.WriteElementString("FromAddress2", fromAdapter.Street1);

            xmlWriter.WriteElementString("FromCity", fromAdapter.City);
            xmlWriter.WriteElementString("FromState", Geography.GetStateProvCode(fromAdapter.StateProvCode));
            xmlWriter.WriteElementString("FromZip5", fromAdapter.PostalCode5);
            xmlWriter.WriteElementString("FromZip4", fromAdapter.PostalCode4);

            xmlWriter.WriteElementString("ToName", new PersonName(toAdapter).FullName);
            xmlWriter.WriteElementString("ToFirm", toAdapter.Company);

            // This is correct, USPS uses the address lines in opposite world
            xmlWriter.WriteElementString("ToAddress1", toAdapter.Street2 + ((toAdapter.Street3.Length > 0) ? " - " + toAdapter.Street3 : ""));
            xmlWriter.WriteElementString("ToAddress2", toAdapter.Street1);

            xmlWriter.WriteElementString("ToCity", toAdapter.City);
            xmlWriter.WriteElementString("ToState", Geography.GetStateProvCode(toAdapter.StateProvCode));
            xmlWriter.WriteElementString("ToZip5", toAdapter.PostalCode5);
            xmlWriter.WriteElementString("ToZip4", toAdapter.PostalCode4);

            int weightOz = (int)Math.Ceiling(shipment.TotalWeight * 16.0);

            // Since weight doesnt affect the label in any way, if its not valid just set it
            // to some valid number
            if (weightOz <= 0)
            {
                weightOz = 1;
            }

            xmlWriter.WriteElementString("WeightInOunces", weightOz.ToString());
            xmlWriter.WriteElementString("ServiceType", GetDomesticServiceApiValue((PostalServiceType)postalShipment.Service));

            xmlWriter.WriteElementString("SeparateReceiptPage", "False");
            xmlWriter.WriteElementString("ImageType", "TIF");
            xmlWriter.WriteElementString("LabelDate", string.Format("{0:MM/dd/yyyy}", shipment.ShipDate.ToLocalTime()));
            xmlWriter.WriteElementString("AddressServiceRequested", "False");

            GenerateCustomsXml(xmlWriter, postalShipment);

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Generate the XML request for an international shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void GenerateXmlRequestInternational(XmlTextWriter xmlWriter, PostalShipmentEntity postalShipment)
        {
            ShipmentEntity shipment = postalShipment.Shipment;

            PersonAdapter toAdapter = new PersonAdapter(shipment, "Ship");
            PersonAdapter fromAdapter = new PersonAdapter(shipment, "Origin");

            xmlWriter.WriteStartElement(GetInternationalServiceTagBase(postalShipment) + "Request");

            // Username and password
            xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
            xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

            xmlWriter.WriteElementString("FromFirstName", fromAdapter.FirstName);
            xmlWriter.WriteElementString("FromLastName", fromAdapter.LastName);
            xmlWriter.WriteElementString("FromFirm", fromAdapter.Company);

            // This is correct, USPS uses the address lines in opposite world
            xmlWriter.WriteElementString("FromAddress1", fromAdapter.Street2 + ((fromAdapter.Street3.Length > 0) ? " - " + fromAdapter.Street3 : ""));
            xmlWriter.WriteElementString("FromAddress2", fromAdapter.Street1);

            if (fromAdapter.CountryCode == "PR")
            {
                xmlWriter.WriteElementString("FromUrbanization", fromAdapter.Street3);
            }

            xmlWriter.WriteElementString("FromCity", fromAdapter.City);
            xmlWriter.WriteElementString("FromState", Geography.GetStateProvCode(fromAdapter.StateProvCode));
            xmlWriter.WriteElementString("FromZip5", fromAdapter.PostalCode5);
            xmlWriter.WriteElementString("FromZip4", fromAdapter.PostalCode4);
            xmlWriter.WriteElementString("FromPhone", fromAdapter.Phone10Digits);

            xmlWriter.WriteElementString("ToFirstName", toAdapter.FirstName);
            xmlWriter.WriteElementString("ToLastName", toAdapter.LastName);
            xmlWriter.WriteElementString("ToFirm", toAdapter.Company);

            // The To address for international is apparently not opposite world
            xmlWriter.WriteElementString("ToAddress1", toAdapter.Street1);
            xmlWriter.WriteElementString("ToAddress2", toAdapter.Street2);
            xmlWriter.WriteElementString("ToAddress3", toAdapter.Street3);

            xmlWriter.WriteElementString("ToCity", toAdapter.City);
            xmlWriter.WriteElementString("ToProvince", Geography.GetStateProvCode(toAdapter.StateProvCode));
            xmlWriter.WriteElementString("ToCountry", PostalWebUtility.GetCountryName(toAdapter.CountryCode));
            xmlWriter.WriteElementString("ToPostalCode", toAdapter.PostalCode);
            xmlWriter.WriteElementString("ToPOBoxFlag", "N");
            xmlWriter.WriteElementString("ToPhone", toAdapter.Phone);

            if (postalShipment.Service != (int)PostalServiceType.InternationalFirst)
            {
                xmlWriter.WriteElementString("NonDeliveryOption", "RETURN");
                xmlWriter.WriteElementString("Container", GetContainerApiValue((PostalPackagingType)postalShipment.PackagingType));
            }


            WriteShippingContents(xmlWriter, postalShipment);

            WeightValue weightValue = GetGrossWeightValue(postalShipment.Shipment.TotalWeight, postalShipment.Shipment.CustomsItems);

            xmlWriter.WriteElementString("GrossPounds", weightValue.PoundsOnly.ToString());
            xmlWriter.WriteElementString("GrossOunces", weightValue.OuncesOnly.ToString());

            PostalCustomsContentType contentType = (PostalCustomsContentType)postalShipment.CustomsContentType;
            xmlWriter.WriteElementString("ContentType", GetContentTypeApiValue(contentType));

            if (contentType == PostalCustomsContentType.Other)
            {
                xmlWriter.WriteElementString("ContentTypeOther", postalShipment.CustomsContentDescription);
            }

            xmlWriter.WriteElementString("Agreement", "Y");

            xmlWriter.WriteElementString("ImageType", "TIF");
            xmlWriter.WriteElementString("ImageLayout", "ONEPERFILE");

            // This field is newly required for Canada
            if (toAdapter.CountryCode == "CA" && ((PostalServiceType)postalShipment.Service) != PostalServiceType.InternationalFirst)
            {
                xmlWriter.WriteElementString("POZipCode", fromAdapter.PostalCode5);
            }

            xmlWriter.WriteElementString("LabelDate", string.Format("{0:MM/dd/yyyy}", shipment.ShipDate.ToLocalTime()));

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Write out CustomsContentType object in request
        /// </summary>
        private static void WriteShippingContents(XmlTextWriter xmlWriter, PostalShipmentEntity postalShipment)
        {
            xmlWriter.WriteStartElement("ShippingContents");

            foreach (ShipmentCustomsItemEntity customsItem in postalShipment.Shipment.CustomsItems)
            {
                decimal value = customsItem.UnitValue * (decimal) customsItem.Quantity;

                xmlWriter.WriteStartElement("ItemDetail");

                xmlWriter.WriteElementString("Description", customsItem.Description);
                xmlWriter.WriteElementString("Quantity", customsItem.Quantity.ToString());
                xmlWriter.WriteElementString("Value", value.ToString(CultureInfo.InvariantCulture));

                WeightValue contentWeight = new WeightValue(customsItem.Weight > 0 ? customsItem.Weight : 1.0 / 16);
                xmlWriter.WriteElementString("NetPounds", contentWeight.PoundsOnly.ToString());
                xmlWriter.WriteElementString("NetOunces", contentWeight.OuncesOnly.ToString());

                xmlWriter.WriteElementString("HSTariffNumber", customsItem.HarmonizedCode);
                xmlWriter.WriteElementString("CountryOfOrigin", Geography.GetCountryName(customsItem.CountryOfOrigin));

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Calculate the Gross Weight of a shipment
        /// </summary>
        private static WeightValue GetGrossWeightValue(double shipmentTotalWeight, EntityCollection<ShipmentCustomsItemEntity> shipmentCustomsItems)
        {
            double totalContentWeight = shipmentCustomsItems.Sum(customsItem => customsItem.Weight > 0 ? customsItem.Weight : 1.0 / 16);
            double shipmentWeight = shipmentTotalWeight > 0 ? shipmentTotalWeight : 1.0 / 16;
            return new WeightValue(Math.Max(totalContentWeight, shipmentWeight));
        }
        
        /// <summary>
        /// Get the API string value to use for the given container type
        /// </summary>
        private static string GetContainerApiValue(PostalPackagingType containerType)
        {
            switch (containerType)
            {
                case PostalPackagingType.FlatRateSmallBox: return "SMFLATRATEBOX";
                case PostalPackagingType.FlatRateMediumBox: return "FLATRATEBOX";
                case PostalPackagingType.FlatRateLargeBox: return "LGFLATRATEBOX";
                case PostalPackagingType.FlatRateEnvelope: return "FLATRATEENV";

                default:
                    return "VARIABLE";
            }
        }

        /// <summary>
        /// Get the API string value to use for the given content type
        /// </summary>
        private static string GetContentTypeApiValue(PostalCustomsContentType contentType)
        {
            switch (contentType)
            {
                case PostalCustomsContentType.Merchandise: return "MERCHANDISE";
                case PostalCustomsContentType.Sample: return "SAMPLE";
                case PostalCustomsContentType.Gift: return "GIFT";
                case PostalCustomsContentType.Documents: return "DOCUMENTS";
                case PostalCustomsContentType.ReturnedGoods: return "RETURN";
                case PostalCustomsContentType.Other: return "OTHER";
                case PostalCustomsContentType.DangerousGoods: return "OTHER";
                case PostalCustomsContentType.HumanitarianDonation: return "OTHER";

                default:
                    throw new InvalidOperationException("Invalid contentType " + contentType);
            }
        }

        /// <summary>
        /// Get the XML tag base for the given international shipment
        /// </summary>
        private static string GetInternationalServiceTagBase(PostalShipmentEntity postalShipment)
        {
            switch ((PostalServiceType)postalShipment.Service)
            {
                case PostalServiceType.InternationalExpress:
                    return "ExpressMailIntl";

                case PostalServiceType.InternationalPriority:
                    return "PriorityMailIntl";

                case PostalServiceType.InternationalFirst:
                    return "FirstClassMailIntl";

                default:
                    throw new InvalidOperationException(string.Format("Invalid international USPS service type {0}", postalShipment.Service));
            }
        }

        /// <summary>
        /// Get the API value to submit to USPS for the given service
        /// </summary>
        private static string GetDomesticServiceApiValue(PostalServiceType service)
        {
            switch (service)
            {
                case PostalServiceType.PriorityMail: return "Priority";
                case PostalServiceType.FirstClass: return "First Class";
                case PostalServiceType.StandardPost: return "Parcel Post";
                case PostalServiceType.MediaMail: return "Media Mail";
                case PostalServiceType.LibraryMail: return "Library Mail";
            }

            throw new ShippingException(string.Format("{0} does not support {1}.", EnumHelper.GetDescription(ShipmentTypeCode.PostalWebTools), EnumHelper.GetDescription(service)));
        }

        /// <summary>
        /// Process the XML request for the specified shipment
        /// </summary>
        /// <param name="postalShipment">The postal shipment.</param>
        /// <param name="xmlRequest">The XML request.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.WebException"></exception>
        private static string ProcessXmlRequest(PostalShipmentEntity postalShipment, string xmlRequest, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            // The production server URL
            string serverUrl = PostalWebUtility.ServerUrl;

            if (postalShipment.Shipment.ShipPerson.IsDomesticCountry())
            {
                if (postalShipment.Service == (int)PostalServiceType.ExpressMail)
                {
                    serverUrl += "?API=ExpressMailLabel&XML=";
                }
                else
                {
                    // Signature Confirm
                    if (postalShipment.Confirmation == (int)PostalConfirmationType.Signature)
                    {
                        serverUrl += "?API=SignatureConfirmationV4&XML=";
                    }

                    // Delivery Confirm
                    else
                    {
                        serverUrl += "?API=DeliveryConfirmationV4&XML=";
                    }
                }
            }
            else
            {
                serverUrl += "?API=" + GetInternationalServiceTagBase(postalShipment) + "&XML=";
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl + HttpUtility.UrlEncode(xmlRequest));
                
                Stopwatch sw = new Stopwatch();
                sw.Start();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sw.Stop();

                telemetricResult.AddEntry(TelemetricEventType.GetLabel, sw.ElapsedMilliseconds);

                // See if we got a valid response
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException(response.StatusDescription);
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShippingException));
            }
        }
    }
}
