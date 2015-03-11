using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels
{
    /// <summary>
    /// A factory for creating instances of a Label from the USPS API.
    /// </summary>
    public class LabelFactory
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelFactory"/> class.
        /// </summary>
        public LabelFactory()
        {
            log = LogManager.GetLogger(typeof (LabelFactory));
        }

        /// <summary>
        /// Creates a collection of labels for a shipment from the list of URLs provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="labelUrls">The label urls.</param>
        /// <returns>A collection of Label instances.</returns>
        public IEnumerable<Label> CreateLabels(ShipmentEntity shipment, List<string> labelUrls)
        {
            List<Label> labels = new List<Label>();

            PostalServiceType serviceType = (PostalServiceType)shipment.Postal.Service;

            // Domestic
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
            {
                // For APO/FPO, the customs docs come in the next two images
                if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode) && PostalUtility.IsMilitaryPostalCode(shipment.ShipPostalCode))
                {
                    // They come down different depending on form type
                    if (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72)
                    {
                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.MilitaryPrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart2", labelUrls[0], CroppingStyles.MilitaryContinuationCrop));
                        
                        // Sometimes we don't get additional label urls (large envelope, etc..), so only try to add 3 and 4 if they exist.
                        if (labelUrls.Count >= 2)
                        {
                            labels.Add(CreateLabel(shipment, "LabelPart3", labelUrls[1], CroppingStyles.MilitaryPrimaryCrop));
                            labels.Add(CreateLabel(shipment, "LabelPart4", labelUrls[1], CroppingStyles.MilitaryContinuationCrop));
                        }
                    }
                    else
                    {
                        // Cropping the envelopes shouldn't occur
                        Rectangle croppingStyle = PostalPackagingType.Envelope == (PostalPackagingType) shipment.Postal.PackagingType ? CroppingStyles.None : CroppingStyles.SingleInternationalCrop;

                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], croppingStyle));
                    }
                }
                else
                {
                    // The bottom half of Guam are instructions we do not need.
                    if (shipment.ShipCountryCode == "GU" || shipment.ShipStateProvCode == "GU")
                    {
                        // Cropping the envelopes shouldn't occur
                        Rectangle croppingStyle = PostalPackagingType.Envelope == (PostalPackagingType)shipment.Postal.PackagingType ? CroppingStyles.None : CroppingStyles.PrimaryCrop;

                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], croppingStyle));
                    }
                    else
                    {
                        // First one is always the primary
                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.None));
                    }
                }
            }
            // International services require some trickdickery
            else
            {
                if (shipment.ActualLabelFormat != null)
                {
                    // If the labels are thermal, just save them all, marking the first as the primary
                    for (int i = 0; i < labelUrls.Count; i++)
                    {
                        string labelName = i == 0 ? "LabelPrimary" : string.Format("LabelPart{0}", i);
                        labels.Add(CreateLabel(shipment, labelName, labelUrls[i], CroppingStyles.None));
                    }
                }
                else if (serviceType == PostalServiceType.InternationalFirst || (serviceType == PostalServiceType.InternationalPriority && labelUrls.Count <= 2))
                {
                    // First-class labels are always a single label. Internatioanl priority flat rate can be the same size as a first-class.  We can tell when this happens
                    // b\c we get only 2 urls (instead of 4).  the 2nd is a duplicate of the first in the cases ive seen, and we dont need it
                    labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.SingleInternationalCrop));
                }
                else if (UspsUtility.IsInternationalConsolidatorServiceType(serviceType))
                {
                    // No cropping needed
                    labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.None));
                }
                else
                {
                    // typical situation not including continuation pages
                    if (labelUrls.Count < 4)
                    {
                        // The first 2 images represent 4 labels that need cropped out. The 3rd url will be the instructions, that we don't need
                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.PrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart2", labelUrls[0], CroppingStyles.ContinuationCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart3", labelUrls[1], CroppingStyles.PrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart4", labelUrls[1], CroppingStyles.ContinuationCrop));
                    }
                    else
                    {
                        // there are Continuation forms to deal with.  We are going to assume there's a single continuation page
                        // last URL is for instructions that aren't needed

                        // primary + continuation
                        labels.Add(CreateLabel(shipment, "LabelPrimary", labelUrls[0], CroppingStyles.PrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart2", labelUrls[0], CroppingStyles.ContinuationCrop));

                        // secondary + continuation
                        labels.Add(CreateLabel(shipment, "LabelPart3", labelUrls[1], CroppingStyles.PrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart4", labelUrls[1], CroppingStyles.ContinuationCrop));

                        // tertiary (Dispatch Note)
                        labels.Add(CreateLabel(shipment, "LabelPart5", labelUrls[2], CroppingStyles.PrimaryCrop));

                        // create a blank png so that the sender's copy and continuation page are separate from the Dispatch Note
                        if (shipment.ActualLabelFormat == null)
                        {
                            labels.Add(CreateBlankLabel(shipment, "LabelPartBlank", CroppingStyles.PrimaryCrop));
                        }

                        // Sender's Copy + continuation
                        labels.Add(CreateLabel(shipment, "LabelPart6", labelUrls[3], CroppingStyles.PrimaryCrop));
                        labels.Add(CreateLabel(shipment, "LabelPart7", labelUrls[3], CroppingStyles.ContinuationCrop));
                    }
                }
            }

            return labels;
        }

        /// <summary>
        /// Creates a single label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="labelUrl">The label URL.</param>
        /// <param name="crop">The crop.</param>
        /// <returns>A Label object that may be either a StandardLabel or ThermalLabel.</returns>
        public Label CreateLabel(ShipmentEntity shipment, string name, string labelUrl, Rectangle crop)
        {
            if (shipment.ActualLabelFormat == null)
            {
                return CreateStandardLabel(shipment, name, labelUrl, crop);
            }

            // Must be a thermal label
            return CreateThermalLabel(shipment, name, labelUrl);

        }

        /// <summary>
        /// Creates a blank label of the size specified by the rectangle. The object creating
        /// the label is responsible for disposing.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Label.</returns>
        private Label CreateBlankLabel(ShipmentEntity shipment, string name, Rectangle rectangle)
        {
            // Create an image and fill it white. Don't wrap this in a using (the object creating
            // the label is responsible for disposing.
            Image image = new Bitmap(rectangle.Width, rectangle.Height);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.FillRectangle(Brushes.White, rectangle);
            }

            return new StandardLabel(shipment, name, image, Rectangle.Empty);
        }

        /// <summary>
        /// Creates a thermal label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="labelUrl">The label URL.</param>
        /// <returns>An instance of a ThermalLabel.</returns>
        private ThermalLabel CreateThermalLabel(ShipmentEntity shipment, string name, string labelUrl)
        {
            byte[] thermalData;
            using (WebClient webClient = new WebClient())
            {
                thermalData = webClient.DownloadData(labelUrl);
            }

            return new ThermalLabel(shipment, name, thermalData);
        }

        /// <summary>
        /// Creates the standard label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="labelUrl">The label URL.</param>
        /// <param name="crop">The crop.</param>
        /// <returns>An instance of a StandardLabel.</returns>
        private StandardLabel CreateStandardLabel(ShipmentEntity shipment, string name, string labelUrl, Rectangle crop)
        {
            Image image = DownloadLabelImage(labelUrl);
            return new StandardLabel(shipment, name, image, crop);
        }

        /// <summary>
        /// Download the USPS label image from the given URL
        /// </summary>
        public Image DownloadLabelImage(string url)
        {
            Image imageLabel;

            try
            {
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        imageLabel = Image.FromStream(stream);
                    }
                }

                return imageLabel;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Failed processing USPS image at URL '{0}'", url), ex);
                throw;
            }
        }
    }
}
