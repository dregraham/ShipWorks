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

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardLabel"/> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="originalImage">The image.</param>
        /// <param name="name">The name.</param>
        public StandardLabel(ShipmentEntity shipmentEntity, string name, Image originalImage)
            : base(shipmentEntity, name)
        {
            this.originalImage = originalImage;
        }

        /// <summary>
        /// Saves the label to the underlying data source.
        /// </summary>
        public override void Save()
        {
            using (MemoryStream imageStream = new MemoryStream())
            {
                originalImage.Save(imageStream, ImageFormat.Png);
                DataResourceManager.CreateFromBytes(imageStream.ToArray(), ShipmentEntity.ShipmentID, Name);
            }
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
