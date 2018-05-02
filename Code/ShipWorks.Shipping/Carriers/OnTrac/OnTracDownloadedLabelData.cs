using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Imaging;
using Interapptive.Shared.Pdf;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;


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
        private readonly Schemas.ShipmentResponse.Shipment shipmentResponse;
        private const string ErrorMessage = "Error reading OnTrac label.";

        public OnTracDownloadedLabelData(ShipmentEntity shipment,
            Schemas.ShipmentResponse.Shipment shipmentResponse,
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
            shipment.ShipmentCost = shipmentResponse.TotalChrg;
            double billedWeight;
            if (double.TryParse(shipmentResponse.BilledWeight, out billedWeight))
            {
                shipment.BilledWeight = billedWeight;
            }
                
            // Interapptive users have an Unprocess button.  If we are reprocessing we need to clear the old images
            objectReferenceManager.ClearReferences(shipment.ShipmentID);

            // Actual label format will only have a value if thermal
            bool isPdf = !shipment.ActualLabelFormat.HasValue;

            try
            {
                byte[] label = isPdf ?
                    Convert.FromBase64String(shipmentResponse.Label) :
                    Encoding.ASCII.GetBytes(shipmentResponse.Label);

                string labelName = "LabelPrimary";
                using (MemoryStream stream = new MemoryStream(label))
                {
                    if (isPdf)
                    {
                        dataResourceManager.CreateFromPdf(PdfDocumentType.BlackAndWhite, stream, shipment.ShipmentID,
                                                          i => i == 0 ? labelName : $"{labelName}-{i}",
                                                          SaveCroppedLabel);
                    }
                    else
                    {
                        dataResourceManager.CreateFromBytes(stream.ToArray(), shipment.ShipmentID, labelName);
                    }
                }
            }
            catch(FormatException ex)
            {
                log.Error(ErrorMessage, ex);
                throw new ShippingException(ErrorMessage, ex);
            }
        }
        
        /// <summary>
        /// Save the cropped label
        /// </summary>
        private byte[] SaveCroppedLabel(MemoryStream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap labelImage = stream.CropImageStream())
                {
                    Bitmap resized = new Bitmap(labelImage, new Size(576, 384));
                    
                    resized.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    resized.Save(memoryStream, ImageFormat.Png);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
