using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Imaging;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Label data that has been downloaded from Postal Web Tools
    /// </summary>
    [Component(RegistrationType.Self)]
    public class PostalWebToolsDownloadedLabelData : IDownloadedLabelData
    {
        private readonly PostalWebToolsLabelResponse postalWebToolsLabelResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebToolsDownloadedLabelData(PostalWebToolsLabelResponse postalWebToolsLabelResponse)
        {
            this.postalWebToolsLabelResponse = postalWebToolsLabelResponse;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            ProcessXmlResponse(postalWebToolsLabelResponse.PostalShipment, postalWebToolsLabelResponse.XmlResponse);
        }

        /// <summary>
        /// Process the response from the Postal Web Tools server
        /// </summary>
        private static void ProcessXmlResponse(PostalShipmentEntity postalShipment, string xmlResponse)
        {
            // Load the response
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
        /// Process the error-free response of the given usps shipment
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

            // If there is no part6 - but there is a part5 - every time I've seen that it means the part5 is instructions, so remove it
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
        /// Save the given label using autocrop
        /// </summary>
        private static void SaveInternationalLabels(PostalShipmentEntity postalShipment, string labelImage, List<string> labelParts)
        {
            SaveAutoCropLabel(postalShipment, labelImage, "LabelPrimary");

            for (int i = 0; i < labelParts.Count; i++)
            {
                SaveAutoCropLabel(postalShipment, labelParts[i], string.Format("LabelPart{0}", i + 2));
            }
        }

        /// <summary>
        /// Save the given label using the specified cropping
        /// </summary>
        private static void SaveAutoCropLabel(PostalShipmentEntity postalShipment, string labelImage, string name)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                Debug.Assert(adapter.InSystemTransaction);

                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(labelImage)))
                {
                    using (Bitmap bitmapImage = EdgeDetection.CropImageStream(stream))
                    {
                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            bitmapImage.Save(imageStream, ImageFormat.Png);

                            DataResourceManager.CreateFromBytes(imageStream.ToArray(), postalShipment.ShipmentID, name);
                        }
                    }
                }
            }
        }
    }
}
