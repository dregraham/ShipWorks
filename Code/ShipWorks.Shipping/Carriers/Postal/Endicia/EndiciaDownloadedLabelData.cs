using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class EndiciaDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly LabelRequestResponse response;
        private readonly IObjectReferenceManager objectReferenceManager;
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaDownloadedLabelData(ShipmentEntity shipment,
            LabelRequestResponse response,
            IObjectReferenceManager objectReferenceManager,
            IDataResourceManager dataResourceManager)
        {
            this.shipment = shipment;
            this.response = response;
            this.objectReferenceManager = objectReferenceManager;
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            try
            {
                // Tracking and cost
                shipment.TrackingNumber = response.TrackingNumber;
                shipment.ShipmentCost = shipment.Postal.NoPostage ? 0 : response.FinalPostage;
                shipment.Postal.Endicia.TransactionID = response.TransactionID;

                SaveLabelImages(shipment, response);
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Save the label images generated for the shipment
        /// </summary>
        private void SaveLabelImages(ShipmentEntity shipment, LabelRequestResponse response)
        {
            // If we had saved an image for this shipment previously clear it.
            objectReferenceManager.ClearReferences(shipment.ShipmentID);

            // Primary image
            if (!string.IsNullOrEmpty(response.Base64LabelImage))
            {
                SaveLabelImage(shipment, "LabelPrimary", response.Base64LabelImage, false);
            }

            // Image sets
            if (response.Label != null)
            {
                foreach (ImageData imageData in response.Label.Image)
                {
                    // For international endicia was sending down all 5 copies in the ImageData sets.
                    // In that case we promote the first one to be the "Primary" label.
                    string name = string.IsNullOrEmpty(response.Base64LabelImage) &&
                            response.Label.Image[0] == imageData ?
                        "LabelPrimary" : $"LabelPart{imageData.PartNumber}";

                    SaveLabelImage(shipment, name, imageData.Value, true);
                }
            }

            // Customs
            if (response.CustomsForm != null)
            {
                foreach (ImageData imageData in response.CustomsForm.Image)
                {
                    SaveLabelImage(shipment, string.Format("Customs{0}", imageData.PartNumber), imageData.Value, true);
                }
            }
        }

        /// <summary>
        /// Save the given label image
        /// </summary>
        private void SaveLabelImage(ShipmentEntity shipment, string name, string base64, bool crop)
        {
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                // If not cropping, or if it is thermal, just save it as-is
                if (!crop || shipment.ActualLabelFormat != null)
                {
                    dataResourceManager.CreateFromBytes(stream.ToArray(), shipment.ShipmentID, name);
                }
                else
                {
                    SaveCroppedLabelImage(shipment, name, stream);
                }
            }
        }

        /// <summary>
        /// Save the label image, cropped to a specific size
        /// </summary>
        private void SaveCroppedLabelImage(ShipmentEntity shipment, string name, MemoryStream stream)
        {
            using (Image imageOriginal = Image.FromStream(stream))
            {
                // For endicia we are just cropping off the "Cut here along line", and its at the same spot on every label that needs it
                using (Image imageLabelCrop = DisplayHelper.CropImage(imageOriginal, 0, 0, imageOriginal.Width, Math.Min(imageOriginal.Height, 1580)))
                {
                    using (MemoryStream imageStream = new MemoryStream())
                    {
                        imageLabelCrop.Save(imageStream, ImageFormat.Png);

                        dataResourceManager.CreateFromBytes(imageStream.ToArray(), shipment.ShipmentID, name);
                    }
                }
            }
        }
    }
}
