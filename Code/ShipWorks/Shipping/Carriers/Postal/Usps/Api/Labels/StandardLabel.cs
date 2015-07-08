using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels
{
    /// <summary>
    /// Represents a USPS standard label and encapsulates the logic for cropping
    /// and persisting the label to the data source.
    /// </summary>
    public class StandardLabel : Label
    {
        private readonly Image originalImage;
        private readonly Rectangle crop;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardLabel"/> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="originalImage">The image.</param>
        /// <param name="name">The name.</param>
        /// <param name="crop">The crop.</param>
        public StandardLabel(ShipmentEntity shipmentEntity, string name, Image originalImage, Rectangle crop)
            : base(shipmentEntity, name)
        {
            this.originalImage = originalImage;
            this.crop = crop;
        }

        /// <summary>
        /// Saves the label to the underlying data source.
        /// </summary>
        public override void Save()
        {
            Image imageCropped = null;
            bool usingOriginalImage = false;

            string fileNameOriginal = GetFileName("ORIGINAL");
            string fileNameFinal = GetFileName("FINAL");

            try
            {
                originalImage.Save(fileNameOriginal, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Debug.Assert(ex != null);
            }

            try
            {
                if (crop == Rectangle.Empty)
                {
                    imageCropped = originalImage;
                    usingOriginalImage = true;
                }
                else
                {
                    imageCropped = DisplayHelper.CropImage(originalImage, crop.X, crop.Y, crop.Width, crop.Height);
                }

                using (MemoryStream imageStream = new MemoryStream())
                {
                    imageCropped.Save(imageStream, ImageFormat.Png);
                    DataResourceManager.CreateFromBytes(imageStream.ToArray(), ShipmentEntity.ShipmentID, Name);
                }


                try
                {
                    imageCropped.Save(fileNameFinal, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    Debug.Assert(ex != null);
                }
            }
            finally
            {
                if (!usingOriginalImage && imageCropped != null)
                {
                    // Make sure cropped get's disposed in case we created it.
                    imageCropped.Dispose();
                }
            }
        }

        private string GetFileName(string postFix)
        {
            int iteration = 0;

            string fileNameFinal = string.Format(@"h:\labels\{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}{12}.jpg",
                ShipmentEntity.ShipmentID,
                (ThermalLanguage)this.ShipmentEntity.RequestedLabelFormat,
                this.ShipmentEntity.ShipMilitaryAddress,
                ShipmentEntity.ShipStateProvCode,
                ShipmentEntity.ShipCountryCode,
                ShipmentEntity.ReturnShipment,
                ShipmentEntity.TotalWeight,
                ShipmentEntity.CustomsValue,
                ShipmentEntity.ShipPostalCode,
                PostalUtility.GetCustomsForm(ShipmentEntity),
                ShipmentEntity.CustomsItems.Count,
                postFix,
                string.Empty
                );

            while (File.Exists(fileNameFinal))
            {
                iteration++;
                fileNameFinal = string.Format(@"h:\labels\{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}{12}.jpg",
                 ShipmentEntity.ShipmentID,
                 (ThermalLanguage)this.ShipmentEntity.RequestedLabelFormat,
                 this.ShipmentEntity.ShipMilitaryAddress,
                 ShipmentEntity.ShipStateProvCode,
                 ShipmentEntity.ShipCountryCode,
                 ShipmentEntity.ReturnShipment,
                 ShipmentEntity.TotalWeight,
                 ShipmentEntity.CustomsValue,
                 ShipmentEntity.ShipPostalCode,
                 PostalUtility.GetCustomsForm(ShipmentEntity),
                 ShipmentEntity.CustomsItems.Count,
                 postFix,
                 iteration == 0 ? string.Empty : iteration.ToString()
                 );
            }

            return fileNameFinal;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                }
            }
        }
    }
}
