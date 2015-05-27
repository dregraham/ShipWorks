using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using System.Web;

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
        public static void ProcessShipment(PostalShipmentEntity postalShipment)
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

            // Generate the request data
            string xmlRequest = GenerateXmlRequest(postalShipment);

            // Log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.UspsNoPostage, "Ship");
            logger.LogRequest(xmlRequest);

            // Process the request
            string xmlResponse = ProcessXmlRequest(postalShipment, xmlRequest);

            // Log the response
            logger.LogResponse(xmlResponse);

            // Process the response
            ProcessXmlResponse(postalShipment, xmlResponse);
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
                    if (postalShipment.Service == (int)PostalServiceType.ExpressMail)
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

            int weightOz = (int)Math.Ceiling(shipment.TotalWeight * 16.0);

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

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Generate the XML request for a domestic shipment
        /// </summary>
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

            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Generate the XML request for an international shipment
        /// </summary>
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

            xmlWriter.WriteStartElement("ShippingContents");
            double totalContentWeight = 0;

            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                xmlWriter.WriteStartElement("ItemDetail");

                xmlWriter.WriteElementString("Description", customsItem.Description);
                xmlWriter.WriteElementString("Quantity", customsItem.Quantity.ToString());
                xmlWriter.WriteElementString("Value", customsItem.UnitValue.ToString());

                WeightValue contentWeight = new WeightValue(customsItem.Weight > 0 ? customsItem.Weight : 1.0 / 16);
                xmlWriter.WriteElementString("NetPounds", contentWeight.PoundsOnly.ToString());
                xmlWriter.WriteElementString("NetOunces", contentWeight.OuncesOnly.ToString());
                totalContentWeight += contentWeight.TotalWeight;

                xmlWriter.WriteElementString("HSTariffNumber", customsItem.HarmonizedCode);
                xmlWriter.WriteElementString("CountryOfOrigin", Geography.GetCountryName(customsItem.CountryOfOrigin));

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            WeightValue weightValue = new WeightValue(Math.Max(totalContentWeight, shipment.TotalWeight > 0 ? shipment.TotalWeight : 1.0 / 16));

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
        /// Get the XML tag base for the given internatino shipment
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
        private static string ProcessXmlRequest(PostalShipmentEntity postalShipment, string xmlRequest)
        {
            // The production server URL
            string serverUrl = PostalWebUtility.UseTestServer ?
                "https://stg-secure.shippingapis.com/ShippingApi.dll?"
                : "http://production.shippingapis.com/ShippingAPI.dll?";

            if (postalShipment.Shipment.ShipPerson.IsDomesticCountry())
            {
                if (postalShipment.Service == (int)PostalServiceType.ExpressMail)
                {
                    serverUrl += "API=ExpressMailLabel&XML=";
                }
                else
                {
                    // Signature Confirm
                    if (postalShipment.Confirmation == (int)PostalConfirmationType.Signature)
                    {
                        serverUrl += "API=SignatureConfirmationV4&XML=";
                    }

                    // Delivery Confirm
                    else
                    {
                        serverUrl += "API=DeliveryConfirmationV4&XML=";
                    }
                }
            }
            else
            {
                serverUrl += "API=" + GetInternationalServiceTagBase(postalShipment) + "&XML=";
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverUrl + HttpUtility.UrlEncode(xmlRequest));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

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

        /// <summary>
        /// Process the response from the USPS server
        /// </summary>
        private static void ProcessXmlResponse(PostalShipmentEntity postalShipment, string xmlResponse)
        {
            // Load the USPS response
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlResponse);

            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // See if there was an error
            XmlNodeList errorNodes = xmlDocument.GetElementsByTagName("Error");
            if (errorNodes.Count > 0)
            {
                string error = XPathUtility.Evaluate(xpath, "//Error/Description", "The USPS server returned an unspecified error.");

                // Throw the exception
                throw new ShippingException("Response from USPS: " + error);
            }

            // Interapptive users have an Unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(postalShipment.ShipmentID);

            if (postalShipment.Shipment.ShipPerson.IsDomesticCountry())
            {
                if (postalShipment.Service == (int)PostalServiceType.ExpressMail)
                {
                    ProcessXmlResponseExpress(postalShipment, xmlDocument);
                }
                else
                {
                    ProcessXmlResponseDomestic(postalShipment, xmlDocument);
                }
            }
            else
            {
                ProcessXmlResponseInternational(postalShipment, xmlDocument);
            }
        }

        /// <summary>
        /// Process the given error=free xmlDocument response from a USPS domestic express shipment
        /// </summary>
        private static void ProcessXmlResponseExpress(PostalShipmentEntity postalShipment, XmlDocument xmlDocument)
        {
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            string barcode = XPathUtility.Evaluate(xpath, "//EMConfirmationNumber", "");
            postalShipment.Shipment.TrackingNumber = barcode;

            string imageBase64 = XPathUtility.Evaluate(xpath, "//EMLabel", "");

            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(imageBase64)))
                {
                    using (Image imageLabel = Image.FromStream(stream))
                    {
                        stream.Position = 0;

                        using (Image imageLabelCrop = DisplayHelper.CropImage(imageLabel, 144, 130, 1207, 807))
                        {
                            imageLabelCrop.RotateFlip(RotateFlipType.Rotate270FlipNone);

                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                imageLabelCrop.Save(imageStream, ImageFormat.Png);

                                DataResourceManager.CreateFromBytes(imageStream.ToArray(), postalShipment.ShipmentID, "LabelPrimary");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the given error-free xmlDocument response from a USPS domestic shipment
        /// </summary>
        private static void ProcessXmlResponseDomestic(PostalShipmentEntity postalShipment, XmlDocument xmlDocument)
        {
            string imageBase64 = string.Empty;
            string tracking = string.Empty;

            // No errors, read out the actual results
            XmlNodeList nodes = xmlDocument.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                switch (node.LocalName)
                {
                    // Label image
                    case "DeliveryConfirmationLabel":
                    case "SignatureConfirmationLabel":
                        {
                            imageBase64 = node.InnerText;
                            break;
                        }

                    // Confirmation number
                    case "DeliveryConfirmationNumber":
                    case "SignatureConfirmationNumber":
                        {
                            tracking = node.InnerText;
                            break;
                        }
                }
            }

            // Update the tracking number
            postalShipment.Shipment.TrackingNumber = tracking;

            // Save the label images
            SaveLabelImagesDomestic(imageBase64, postalShipment.ShipmentID);
        }

        /// <summary>
        /// Create the image from a base 64 string
        /// </summary>
        private static void SaveLabelImagesDomestic(string imageBase64, long shipmentID)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                // Convert the string into an image stream
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(imageBase64)))
                {
                    using (Image imageLabel = Image.FromStream(stream))
                    {
                        imageLabel.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        stream.Position = 0;

                        using (Image imageLabelCrop = DisplayHelper.CropImage(imageLabel, 115, 354, 805, 1205))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                imageLabelCrop.Save(imageStream, ImageFormat.Png);

                                DataResourceManager.CreateFromBytes(imageStream.ToArray(), shipmentID, "LabelPrimary");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the error-free resonse of the given usps shipment
        /// </summary>
        private static void ProcessXmlResponseInternational(PostalShipmentEntity postalShipment, XmlDocument xmlDocument)
        {
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            string barcode = XPathUtility.Evaluate(xpath, "//BarcodeNumber", "");
            postalShipment.Shipment.TrackingNumber = barcode;

            string label = XPathUtility.Evaluate(xpath, "//LabelImage", "");
            string part2 = XPathUtility.Evaluate(xpath, "//Page2Image", "");
            string part3 = XPathUtility.Evaluate(xpath, "//Page3Image", "");
            string part4 = XPathUtility.Evaluate(xpath, "//Page4Image", "");
            string part5 = XPathUtility.Evaluate(xpath, "//Page5Image", "");
            string part6 = XPathUtility.Evaluate(xpath, "//Page6Image", "");

            // If there is no part6 - but there is a part5 - everytime ive seen that it means the part5 is instructions, so remove it
            if (string.IsNullOrEmpty(part6))
            {
                part5 = "";
            }

            List<string> parts = new List<string>();

            if (!string.IsNullOrEmpty(part2)) parts.Add(part2);
            if (!string.IsNullOrEmpty(part3)) parts.Add(part3);
            if (!string.IsNullOrEmpty(part4)) parts.Add(part4);
            if (!string.IsNullOrEmpty(part5)) parts.Add(part5);
            if (!string.IsNullOrEmpty(part6)) parts.Add(part6);

            SaveInternationalLabels(postalShipment, label, parts);
        }

        /// <summary>
        /// Save the given label for the specified shipment
        /// </summary>
        private static void SaveInternationalLabels(PostalShipmentEntity postalShipment, string labelImage, List<string> labelParts)
        {
            Rectangle crop;

            // Crop depends on service type
            switch ((PostalServiceType)postalShipment.Service)
            {
                case PostalServiceType.InternationalPriority:
                    {
                        if (labelParts.Count == 0)
                        {
                            crop = new Rectangle(211, 191, 1287, 792);
                        }
                        else
                        {
                            crop = new Rectangle(49, 54, 1606, 1052);
                        }
                        break;
                    }

                case PostalServiceType.InternationalExpress:
                    {
                        crop = new Rectangle(52, 53, 1602, 1052);
                        break;
                    }

                case PostalServiceType.InternationalFirst:
                    {
                        crop = new Rectangle(211, 191, 1287, 792);
                        break;
                    }

                default:
                    throw new InvalidOperationException("Invalid international service type.");
            }

            SaveInternationalLabel(postalShipment, labelImage, crop, "LabelPrimary");

            for (int i = 0; i < labelParts.Count; i++)
            {
                SaveInternationalLabel(postalShipment, labelParts[i], crop, string.Format("LabelPart{0}", i + 2));
            }
        }

        /// <summary>
        /// Save the given label using the specified cropping
        /// </summary>
        private static void SaveInternationalLabel(PostalShipmentEntity postalShipment, string labelImage, Rectangle crop, string name)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(labelImage)))
                {
                    using (Image imageLabel = Image.FromStream(stream))
                    {
                        stream.Position = 0;

                        using (Image imageLabelCrop = DisplayHelper.CropImage(imageLabel, crop.X, crop.Y, crop.Width, crop.Height))
                        {
                            using (MemoryStream imageStream = new MemoryStream())
                            {
                                imageLabelCrop.Save(imageStream, ImageFormat.Png);

                                DataResourceManager.CreateFromBytes(imageStream.ToArray(), postalShipment.ShipmentID, name);
                            }
                        }
                    }
                }
            }
        }
    }
}
