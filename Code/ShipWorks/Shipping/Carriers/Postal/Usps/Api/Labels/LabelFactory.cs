using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Creates a collection of labels for a shipment from the list of binary image data provided.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="imageData">The base 64 binary data of each label image.</param>
        /// <returns>A collection of Label instances.</returns>
        [SuppressMessage("SonarLint", "S2368:Public methods should not have multidimensional array parameters",
            Justification = "Image data is two dimensions, which requires the array")]
        public IEnumerable<Label> CreateLabels(ShipmentEntity shipment, byte[][] imageData)
        {
            List<Label> labels = new List<Label>();

            for (int i = 0; i < imageData.Length; i++)
            {
                string labelName = i == 0 ? "LabelPrimary" : string.Format("LabelPart{0}", i);
                labels.Add(CreateLabel(shipment, labelName, imageData[i]));
            }

            return labels;
        }

        /// <summary>
        /// Creates a single label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="imageBytes">The binary byte array of the label image.</param>
        /// <returns>A Label object that may be either a StandardLabel or ThermalLabel.</returns>
        public Label CreateLabel(ShipmentEntity shipment, string name, byte[] imageBytes)
        {
            if (shipment.ActualLabelFormat == null)
            {
                return CreateStandardLabel(shipment, name, imageBytes);
            }

            // Must be a thermal label
            return CreateThermalLabel(shipment, name, imageBytes);
        }

        /// <summary>
        /// Creates a thermal label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="thermalData">The byte array of the label thermal data.</param>
        /// <returns>An instance of a ThermalLabel.</returns>
        private ThermalLabel CreateThermalLabel(ShipmentEntity shipment, string name, byte[] thermalData)
        {
            return new ThermalLabel(shipment, name, thermalData);
        }

        /// <summary>
        /// Creates the standard label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="name">The name.</param>
        /// <param name="imageBytes">The byte array of the label image data.</param>
        /// <returns>An instance of a StandardLabel.</returns>
        private StandardLabel CreateStandardLabel(ShipmentEntity shipment, string name, byte[] imageBytes)
        {
            Image image;

            using (MemoryStream imageBytesStream = new MemoryStream(imageBytes))
            {
                image = Image.FromStream(imageBytesStream);
            }

            return new StandardLabel(shipment, name, image);
        }

        /// <summary>
        /// Creates a single label.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="labelUrl">The label URL.</param>
        /// <returns>A Label object that may be either a StandardLabel or ThermalLabel.</returns>
        public Label CreateLabel(ShipmentEntity shipment, string labelUrl)
        {
            if (shipment.ActualLabelFormat == null)
            {
                return CreateStandardLabel(shipment, "LabelPrimary", labelUrl);
            }

            // Must be a thermal label
            return CreateThermalLabel(shipment, "LabelPrimary", labelUrl);
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
        /// <returns>An instance of a StandardLabel.</returns>
        private StandardLabel CreateStandardLabel(ShipmentEntity shipment, string name, string labelUrl)
        {
            Image image = DownloadLabelImage(labelUrl);
            return new StandardLabel(shipment, name, image);
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
