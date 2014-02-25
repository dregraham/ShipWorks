﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using System.Xml.XPath;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using System.Web;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Class for getting rates from the USPS WebTools API
    /// </summary>
    public static class PostalWebClientRates
    {
        /// <summary>
        /// Get the USPS rates for a shipment of the given information
        /// </summary>
        public static List<RateResult> GetRates(ShipmentEntity shipment, LogEntryFactory logEntryFactory)
        {
            string xmlRequest;

            PostalPackagingType packaging = (PostalPackagingType) shipment.Postal.PackagingType;

            if (packaging == PostalPackagingType.FlatRatePaddedEnvelope ||
                packaging == PostalPackagingType.FlatRateLegalEnvelope ||
                packaging == PostalPackagingType.RateRegionalBoxA ||
                packaging == PostalPackagingType.RateRegionalBoxB || 
                packaging == PostalPackagingType.RateRegionalBoxC)
            {
                throw new ShippingException(string.Format("{0} is not supported by {1}.", EnumHelper.GetDescription(packaging), ShipmentTypeManager.GetType(ShipmentTypeCode.PostalWebTools).ShipmentTypeName));
            }

            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                xmlWriter.Formatting = Formatting.Indented;

                // Default the weight to .1 if it is 0, so we can get a rate without needing the user to provide a value
                double ratedWeight = shipment.TotalWeight > 0 ? shipment.TotalWeight : .1;

                // Domestic
                if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
                {
                    xmlWriter.WriteStartElement("RateV3Request");
                    xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
                    xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

                    xmlWriter.WriteStartElement("Package");
                    xmlWriter.WriteAttributeString("ID", "0");

                    xmlWriter.WriteElementString("Service", "ONLINE");
                    xmlWriter.WriteElementString("FirstClassMailType", GetFirstClassMailType(packaging));
                    xmlWriter.WriteElementString("ZipOrigination", new PersonAdapter(shipment, "Origin").PostalCode5);
                    xmlWriter.WriteElementString("ZipDestination", new PersonAdapter(shipment, "Ship").PostalCode5);
                    
                    WeightValue weightValue = new WeightValue(ratedWeight);
                    xmlWriter.WriteElementString("Pounds", weightValue.PoundsOnly.ToString());
                    xmlWriter.WriteElementString("Ounces", weightValue.OuncesOnly.ToString());
                    xmlWriter.WriteElementString("Container", GetContainerValue(packaging, shipment.Postal.NonRectangular));

                    DimensionsAdapter dimensions = new DimensionsAdapter(shipment.Postal);
                    xmlWriter.WriteElementString("Size", GetSizeValue(dimensions));
                    xmlWriter.WriteElementString("Width", dimensions.Width.ToString());
                    xmlWriter.WriteElementString("Length", dimensions.Length.ToString());
                    xmlWriter.WriteElementString("Height", dimensions.Height.ToString());
                    xmlWriter.WriteElementString("Girth", dimensions.Girth.ToString());
                    xmlWriter.WriteElementString("Machinable", shipment.Postal.NonMachinable ? "false" : "true");
                    xmlWriter.WriteElementString("ReturnLocations", "false");

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                // International
                else
                {
                    xmlWriter.WriteStartElement("IntlRateRequest");
                    xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
                    xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

                    xmlWriter.WriteStartElement("Package");
                    xmlWriter.WriteAttributeString("ID", "0");

                    WeightValue weightValue = new WeightValue(ratedWeight);
                    xmlWriter.WriteElementString("Pounds", weightValue.PoundsOnly.ToString());
                    xmlWriter.WriteElementString("Ounces", weightValue.OuncesOnly.ToString());

                    xmlWriter.WriteElementString("Machinable", shipment.Postal.NonMachinable ? "false" : "true");
                    xmlWriter.WriteElementString("MailType", GetInternationalMailType(packaging));
                    xmlWriter.WriteElementString("Country", Geography.GetCountryName(shipment.ShipCountryCode));

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }

                xmlRequest = writer.ToString();
            }

            
            IApiLogEntry logger = logEntryFactory.GetLogEntry(ApiLogSource.UspsNoPostage, "Rate", LogActionType.GetRates);

            logger.LogRequest(xmlRequest);

            // Process the request
            string xmlResponse = ProcessXmlRequest(xmlRequest, PostalUtility.IsDomesticCountry(shipment.ShipCountryCode) ? "RateV3" : "IntlRate");

            // Log the response
            logger.LogResponse(xmlResponse);

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

            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                return ProcessXmlDomesticResponse(xmlDocument, packaging);
            }
            else
            {
                return ProcessXmlInternationalResponse(xmlDocument, packaging);
            }
        }

        /// <summary>
        /// Process the given XML request and return the response
        /// </summary>
        private static string ProcessXmlRequest(string xmlRequest, string api)
        {
            // The production server URL
            string serverUrl = "http://production.shippingapis.com/ShippingAPI.dll?API=" + api + "&XML=";

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(serverUrl + HttpUtility.UrlEncode(xmlRequest));
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

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
        /// Process the response document and generate the rates from it
        /// </summary>
        private static List<RateResult> ProcessXmlDomesticResponse(XmlDocument xmlDocument, PostalPackagingType packaging)
        {
            List<RateResult> rates = new List<RateResult>();
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            Dictionary<PostalServiceType, List<XPathNavigator>> serviceRates = new Dictionary<PostalServiceType, List<XPathNavigator>>();

            foreach (XPathNavigator rateNode in xpath.Select("//Postage"))
            {
                PostalServiceType serviceType;

                int classID = XPathUtility.Evaluate(rateNode, "@CLASSID", 0);
                switch (classID)
                {
                    case 0:
                        serviceType = PostalServiceType.FirstClass;
                        break;

                    case 1:
                    case 16:
                    case 17:
                    case 22:
                    case 28:
                        serviceType = PostalServiceType.PriorityMail;
                        break;

                    case 3:
                    case 13:
                        serviceType = PostalServiceType.ExpressMail;
                        break;

                    case 4:
                        serviceType = PostalServiceType.StandardPost;
                        break;

                    case 6:
                        serviceType = PostalServiceType.MediaMail;
                        break;

                    default:
                        continue;
                }

                List<XPathNavigator> rateNodes;
                if (!serviceRates.TryGetValue(serviceType, out rateNodes))
                {
                    rateNodes = new List<XPathNavigator>();
                    serviceRates[serviceType] = rateNodes;
                }

                rateNodes.Add(rateNode.Clone());
            }

            // Go through each service type we have rates for, and pick the best one that matches our packaging type
            foreach (KeyValuePair<PostalServiceType, List<XPathNavigator>> pair in serviceRates)
            {
                PostalServiceType serviceType = pair.Key;
                XPathNavigator rateNode = DetermineDomesticRateMatch(pair.Value, serviceType, packaging);

                decimal amount = XPathUtility.Evaluate(rateNode, "Rate", 0m);
            
                if (serviceType == PostalServiceType.ExpressMail)
                {
                    rates.Add(new RateResult(
                        EnumHelper.GetDescription(serviceType), 
                        PostalUtility.GetServiceTransitDays(serviceType),
                        amount,
                        new PostalRateSelection(serviceType, PostalConfirmationType.None)));
                }
                else
                {
                    rates.Add(new RateResult(
                        EnumHelper.GetDescription(serviceType),
                        PostalUtility.GetServiceTransitDays(serviceType))
                    {
                        Tag = new PostalRateSelection(serviceType, PostalConfirmationType.Delivery)
                    });

                    decimal deliveryAmount = (serviceType == PostalServiceType.PriorityMail || serviceType == PostalServiceType.FirstClass) ? 0m : .19m;
                    decimal signatureAmount = 1.80m;

                    // Delivery confirmation
                    rates.Add(new RateResult(
                        string.Format("       Delivery Confirmation ({0:c})", deliveryAmount),
                        "",
                        amount + deliveryAmount,
                        new PostalRateSelection(serviceType, PostalConfirmationType.Delivery)));

                    // Signature confirmation
                    rates.Add(new RateResult(
                        string.Format("       Signature Confirmation ({0:c})", signatureAmount),
                        "",
                        amount + signatureAmount,
                        new PostalRateSelection(serviceType, PostalConfirmationType.Signature)));
                }
            }

            foreach (var rate in rates)
            {
                PostalUtility.SetServiceDetails(rate);
                rate.ShipmentType = ShipmentTypeCode.PostalWebTools;
            }
            
            return rates;
        }

        /// <summary>
        /// Determine the best rate node to use based on the given service type and packaging type
        /// </summary>
        private static XPathNavigator DetermineDomesticRateMatch(List<XPathNavigator> rateNodes, PostalServiceType serviceType, PostalPackagingType packaging)
        {
            XPathNavigator bestNode = rateNodes[0];

            foreach (XPathNavigator rateNode in rateNodes)
            {
                // First class determination
                if (serviceType == PostalServiceType.FirstClass)
                {
                    string description = XPathUtility.Evaluate(rateNode, "MailService", "");

                    if (description.Contains("Flat") && 
                            (packaging == PostalPackagingType.Envelope  ||
                             packaging == PostalPackagingType.FlatRateEnvelope ||
                             packaging == PostalPackagingType.LargeEnvelope))
                    {
                        bestNode = rateNode;
                    }

                    if (description.Contains("Parcel") &&
                            (packaging == PostalPackagingType.FlatRateSmallBox ||
                             packaging == PostalPackagingType.FlatRateMediumBox ||
                             packaging == PostalPackagingType.FlatRateLargeBox ||
                             packaging == PostalPackagingType.Package))
                    {
                        bestNode = rateNode;
                    }
                }

                // Express determination
                if (serviceType == PostalServiceType.ExpressMail)
                {
                    int classID = XPathUtility.Evaluate(rateNode, "@CLASSID", 0);
                    if (classID == 13 && packaging == PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 3 && packaging != PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }
                }

                // Priority determination
                if (serviceType == PostalServiceType.PriorityMail)
                {
                    int classID = XPathUtility.Evaluate(rateNode, "@CLASSID", 0);

                    if (classID == 22 && packaging == PostalPackagingType.FlatRateLargeBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 17 && packaging == PostalPackagingType.FlatRateMediumBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 28 && packaging == PostalPackagingType.FlatRateSmallBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 16 && packaging == PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 1 && 
                            packaging != PostalPackagingType.FlatRateLargeBox && 
                            packaging != PostalPackagingType.FlatRateMediumBox && 
                            packaging != PostalPackagingType.FlatRateSmallBox &&
                            packaging != PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }
                }
            }

            return bestNode;
        }

        /// <summary>
        /// Process the response document for international
        /// </summary>
        private static List<RateResult> ProcessXmlInternationalResponse(XmlDocument xmlDocument, PostalPackagingType packaging)
        {
            List<RateResult> rates = new List<RateResult>();
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            Dictionary<PostalServiceType, List<XPathNavigator>> serviceRates = new Dictionary<PostalServiceType, List<XPathNavigator>>();

            foreach (XPathNavigator rateNode in xpath.Select("//Service"))
            {
                PostalServiceType serviceType;

                int classID = XPathUtility.Evaluate(rateNode, "@ID", 0);
                switch (classID)
                {
                    case 1:
                    case 10:
                        serviceType = PostalServiceType.InternationalExpress;
                        break;

                    case 2:
                    case 8:
                    case 9:
                    case 11:
                    case 16:
                        serviceType = PostalServiceType.InternationalPriority;
                        break;

                    case 14:
                    case 15:
                        serviceType = PostalServiceType.InternationalFirst;
                        break;

                    default:
                        continue;
                }

                List<XPathNavigator> rateNodes;
                if (!serviceRates.TryGetValue(serviceType, out rateNodes))
                {
                    rateNodes = new List<XPathNavigator>();
                    serviceRates[serviceType] = rateNodes;
                }

                rateNodes.Add(rateNode.Clone());
            }

            // Go through each service type we have rates for, and pick the best one that matches our packaging type
            foreach (KeyValuePair<PostalServiceType, List<XPathNavigator>> pair in serviceRates)
            {
                PostalServiceType serviceType = pair.Key;
                XPathNavigator rateNode = DetermineInternationalRateMatch(pair.Value, serviceType, packaging);

                decimal amount = XPathUtility.Evaluate(rateNode, "Postage", 0m);
                string days = XPathUtility.Evaluate(rateNode, "SvcCommitments", "").Replace("Days", "");

                rates.Add(new RateResult(
                    EnumHelper.GetDescription(serviceType), 
                    days, 
                    amount, 
                    new PostalRateSelection(serviceType, PostalConfirmationType.None))
                    {ShipmentType = ShipmentTypeCode.PostalWebTools});
            }

            return rates;
        }

        /// <summary>
        /// Determine the best international rate to use from the given list based on the service type and packaging
        /// </summary>
        private static XPathNavigator DetermineInternationalRateMatch(List<XPathNavigator> rateNodes, PostalServiceType serviceType, PostalPackagingType packaging)
        {
            XPathNavigator bestNode = rateNodes[0];

            foreach (XPathNavigator rateNode in rateNodes)
            {
                int classID = XPathUtility.Evaluate(rateNode, "@ID", 0);

                // First
                if (serviceType == PostalServiceType.InternationalFirst)
                {
                    if (classID == 14 &&
                            (packaging == PostalPackagingType.Envelope ||
                             packaging == PostalPackagingType.FlatRateEnvelope ||
                             packaging == PostalPackagingType.LargeEnvelope))
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 15 &&
                            (packaging == PostalPackagingType.FlatRateSmallBox ||
                             packaging == PostalPackagingType.FlatRateMediumBox ||
                             packaging == PostalPackagingType.FlatRateLargeBox ||
                             packaging == PostalPackagingType.Package))
                    {
                        bestNode = rateNode;
                    }
                }

                // Express
                if (serviceType == PostalServiceType.InternationalExpress)
                {
                    if (classID == 10 && packaging == PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 1 && packaging != PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }
                }

                // Priority
                if (serviceType == PostalServiceType.InternationalPriority)
                {
                    if (classID == 11 && packaging == PostalPackagingType.FlatRateLargeBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 9 && packaging == PostalPackagingType.FlatRateMediumBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 16 && packaging == PostalPackagingType.FlatRateSmallBox)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 8 && packaging == PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }

                    if (classID == 2 && 
                            packaging != PostalPackagingType.FlatRateLargeBox && 
                            packaging != PostalPackagingType.FlatRateMediumBox && 
                            packaging != PostalPackagingType.FlatRateSmallBox &&
                            packaging != PostalPackagingType.FlatRateEnvelope)
                    {
                        bestNode = rateNode;
                    }
                }
            }

            return bestNode;
        }

        /// <summary>
        /// Get the API value to use for internationa mail type
        /// </summary>
        private static string GetInternationalMailType(PostalPackagingType packaging)
        {
            switch (packaging)
            {
                case PostalPackagingType.FlatRateEnvelope:
                    return "Envelope";

                default:
                    return "Package";
            }
        }

        /// <summary>
        /// Get the API size value to use based on the given dimensions
        /// </summary>
        private static string GetSizeValue(DimensionsAdapter dimensions)
        {
            double size = dimensions.Length + 2 * (dimensions.Width + dimensions.Height);

            if (size <= 84)
            {
                return "REGULAR";
            }

            if (size <= 108)
            {
                return "LARGE";
            }

            return "OVERSIZE";
        }

        /// <summary>
        /// Get the API value to use for FirstClassMailType for the given packaging
        /// </summary>
        private static string GetFirstClassMailType(PostalPackagingType packaging)
        {
            switch (packaging)
            {
                case PostalPackagingType.Envelope:
                case PostalPackagingType.LargeEnvelope:
                case PostalPackagingType.FlatRateEnvelope:
                    return "FLAT";

                case PostalPackagingType.Package:
                case PostalPackagingType.FlatRateSmallBox:
                case PostalPackagingType.FlatRateMediumBox:
                case PostalPackagingType.FlatRateLargeBox:
                default:
                    return "PARCEL";
            }
        }

        /// <summary>
        /// Get the API value to use for the container tag
        /// </summary>
        private static string GetContainerValue(PostalPackagingType packaging, bool nonRectangular)
        {
            switch (packaging)
            {
                case PostalPackagingType.FlatRateSmallBox: return "SM FLAT RATE BOX";
                case PostalPackagingType.FlatRateMediumBox: return "FLAT RATE BOX";
                case PostalPackagingType.FlatRateLargeBox: return "LG FLAT RATE BOX";
                case PostalPackagingType.FlatRateEnvelope: return "FLAT RATE ENVELOPE";
                case PostalPackagingType.Package:
                    return nonRectangular ? "NONRECTANGULAR" : "RECTANGULAR";
                default:
                    return "VARIABLE";
            }
        }
    }
}
