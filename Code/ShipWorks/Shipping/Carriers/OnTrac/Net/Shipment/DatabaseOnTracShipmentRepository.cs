using System;
using System.Drawing.Imaging;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
using ShipWorks.UI;
using log4net;
using System.IO;
using System.Drawing;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment
{
    /// <summary>
    /// Repository to save shipment to the database
    /// </summary>
    public class DatabaseOnTracShipmentRepository
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseOnTracShipmentRepository));

        /// <summary>
        /// Saves the label from OnTrac to the database.
        /// </summary>
        /// <exception cref="OnTracException">The label from OnTrac is not readable.</exception>
        public void SaveShipmentFromOnTrac(
            ShipmentResponse shipmentResponse,
            ShipmentEntity shipment)
        {
            shipment.TrackingNumber = shipmentResponse.Tracking;
            shipment.ShipmentCost = (decimal)shipmentResponse.TotalChrg;
            shipment.BilledWeight = shipmentResponse.BilledWeight;

            // Interapptive users have an Unprocess button.  If we are reprocessing we need to clear the old images
            ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

            byte[] imageData;
            if (shipment.ActualLabelFormat.HasValue)
            {
                imageData = Encoding.ASCII.GetBytes(shipmentResponse.Label);
            }
            else
            {
                try
                {
                    imageData = GetCroppedImageData(shipmentResponse.Label);
                }
                catch (FormatException ex)
                {
                    const string error = "Error reading OnTrac label.";
                    log.Error(error, ex);
                    throw new OnTracException(error, ex);
                }
            }

            DataResourceManager.CreateFromBytes(imageData, shipment.ShipmentID, "LabelPrimary");
        }

        /// <summary>
        /// Gets the cropped image data.
        /// </summary>
        private byte[] GetCroppedImageData(string shipmentLabelFromOnTracApi)
        {
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(shipmentLabelFromOnTracApi)))
            {

                using (Image imageOriginal = Image.FromStream(stream))
                {
                    using (Image imageLabelCrop = DisplayHelper.CropImage(imageOriginal, 0, 0, imageOriginal.Width, 396))
                    {
                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            imageLabelCrop.Save(imageStream, ImageFormat.Png);

                            return imageStream.ToArray();
                        }
                    }
                }
            }
        }
    }
}