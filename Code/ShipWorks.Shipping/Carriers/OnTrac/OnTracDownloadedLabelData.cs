using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Label data that has been downloaded from a carrier
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OnTracDownloadedLabelData : IDownloadedLabelData
    {
        private readonly IDataResourceManager dataResourceManager;
        private readonly ILog log;
        private readonly IObjectReferenceManager objectReferenceManager;
        private readonly ShipmentEntity shipment;
        private readonly ShipmentResponse shipmentResponse;

        public OnTracDownloadedLabelData(ShipmentEntity shipment,
            ShipmentResponse shipmentResponse,
            IObjectReferenceManager objectReferenceManager,
            IDataResourceManager dataResourceManager,
            Func<Type, ILog> getLogger)
        {
            this.shipment = shipment;
            this.shipmentResponse = shipmentResponse;
            this.objectReferenceManager = objectReferenceManager;
            this.dataResourceManager = dataResourceManager;
            log = getLogger(typeof(OnTracDownloadedLabelData));
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            shipment.TrackingNumber = shipmentResponse.Tracking;
            shipment.ShipmentCost = (decimal) shipmentResponse.TotalChrg;
            shipment.BilledWeight = shipmentResponse.BilledWeight;

            // Interapptive users have an Unprocess button.  If we are reprocessing we need to clear the old images
            objectReferenceManager.ClearReferences(shipment.ShipmentID);

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
                    throw new ShippingException(error, ex);
                }
            }

            dataResourceManager.CreateFromBytes(imageData, shipment.ShipmentID, "LabelPrimary");
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
