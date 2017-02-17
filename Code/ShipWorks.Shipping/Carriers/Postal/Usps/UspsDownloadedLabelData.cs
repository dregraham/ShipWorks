using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Label data that has been downloaded from USPS
    /// </summary>
    [Component(RegistrationType.Self)]
    public class UspsDownloadedLabelData : IDownloadedLabelData
    {
        private readonly ShipmentEntity shipment;
        private readonly byte[][] imageData;
        private readonly string labelUrl;
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsDownloadedLabelData(UspsLabelResponse uspsLabelResponse, IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
            shipment = uspsLabelResponse.Shipment;
            imageData = uspsLabelResponse.ImageData;
            labelUrl = uspsLabelResponse.LabelUrl;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            try
            {
                // Interapptive users have an unprocess button.  If we are reprocessing we need to clear the old images
                ObjectReferenceManager.ClearReferences(shipment.ShipmentID);

                SaveLabels();
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Uses the label URLs to saves the label(s) for the given shipment.
        /// </summary>
        private void SaveLabels()
        {
            List<Label> labels = new List<Label>();

            try
            {
                LabelFactory labelFactory = new LabelFactory();

                if (imageData != null && imageData.Length > 0)
                {
                    labels.AddRange(labelFactory.CreateLabels(shipment, imageData).ToList());
                }

                if (!string.IsNullOrWhiteSpace(labelUrl))
                {
                    labels.Add(labelFactory.CreateLabel(shipment, labelUrl));
                }

                labels.ForEach(l => l.Save(dataResourceManager));
            }
            finally
            {
                labels.ForEach(l => l.Dispose());
            }
        }
    }
}
