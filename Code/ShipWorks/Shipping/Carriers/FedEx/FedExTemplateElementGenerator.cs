using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// A class for generating FedEx shipment template XML.
    /// </summary>
    public static class FedExTemplateElementGenerator
    {
        /// <summary>
        /// Generate the FedEx specific template xml
        /// </summary>
        public static void Generate(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

            Lazy<TemplateLabelData> codReturn = new Lazy<TemplateLabelData>(() => labels.Value.FirstOrDefault(l => l.Name == "COD"));

            // Legacy stuff
            ElementOutline outline = container.AddElement("FedEx", ElementOutline.If(() => shipment().Processed));
            outline.AddAttributeLegacy2x();
            outline.AddElement("Voided", () => shipment().Voided);

            outline.AddElement("LabelCODReturn",
                () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, codReturn.Value.Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Png))),
                ElementOutline.If(() => shipment().ActualLabelFormat == null && codReturn.Value != null));

            outline.AddElement("Package",
                new FedExLegacyPackageTemplateOutline(container.Context),
                () => labels.Value.Where(l => l.Category == TemplateLabelCategory.Primary),
                ElementOutline.If(() => shipment().ActualLabelFormat == null));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // FedEx stores some stuff at the shipmentID level, but we include it as apart of the first package
            List<DataResourceReference> shipmentResources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            // Add labels for each package
            foreach (long packageID in DataProvider.GetRelatedKeys(shipment().ShipmentID, EntityType.FedExPackageEntity))
            {
                // Get the resource list for our shipment
                List<DataResourceReference> packageResources = DataResourceManager.GetConsumerResourceReferences(packageID);

                // Could be none for upgraded 2x shipments
                if (packageResources.Count > 0)
                {
                    // Add our standard label output
                    var labels = packageResources.Where(x => x.Label == "LabelImage")
                        .Select(x => new TemplateLabelData(packageID, "Label", TemplateLabelCategory.Primary, x));
                    labelData.AddRange(labels);

                    // For Ground it will be at the package level,
                    AddCODDocument(packageID, shipmentResources, labelData);

                    // Will be non-null first time through
                    if (shipmentResources != null)
                    {
                        AddBOLDocuments(packageID, shipmentResources, labelData);
                        AddCODDocument(packageID, shipmentResources, labelData);
                        AddDocumentResourceLabelData(packageID, shipmentResources, labelData);

                        // Don't need it anymore
                        shipmentResources = null;
                    }

                    // Add any supporting package documents that may exist
                    AddDocumentResourceLabelData(packageID, packageResources, labelData);
                }
            }

            return labelData;
        }

        /// <summary>
        /// Add bill of lading document
        /// </summary>
        private static void AddBOLDocuments(long packageID, List<DataResourceReference> shipmentResources, List<TemplateLabelData> labelData)
        {
            var bomDocuments = shipmentResources.Where(r => r.Label.StartsWith("BOL"));
            if (bomDocuments.Any())
            {
                labelData.AddRange(bomDocuments.Select(x => new TemplateLabelData(packageID, x.Label, TemplateLabelCategory.Supplemental, x)));
            }
        }

        /// <summary>
        /// Add COD document
        /// </summary>
        private static void AddCODDocument(long packageID, List<DataResourceReference> resources, List<TemplateLabelData> labelData)
        {
            DataResourceReference codPackageResource = resources.SingleOrDefault(r => r.Label == "COD");
            if (codPackageResource != null)
            {
                labelData.Add(new TemplateLabelData(packageID, "COD", TemplateLabelCategory.Supplemental, codPackageResource));
            }
        }

        /// <summary>
        /// Add document resources to the label data collection
        /// </summary>
        private static void AddDocumentResourceLabelData(long packageID, IEnumerable<DataResourceReference> resources, ICollection<TemplateLabelData> labelData)
        {
            IEnumerable<DataResourceReference> documentResources = resources.Where(r => r.Label.StartsWith("Document", StringComparison.OrdinalIgnoreCase));
            foreach (DataResourceReference documentResource in documentResources)
            {
                labelData.Add(new TemplateLabelData(packageID,
                    documentResource.Label.Replace("Document", ""),
                    TemplateLabelCategory.Supplemental,
                    documentResource,
                    documentResource.Label != "DocumentCommercialInvoice"));
            }
        }

        /// <summary>
        /// Outline for the legacy FedEx 'Package' element
        /// </summary>
        private class FedExLegacyPackageTemplateOutline : ElementOutline
        {
            private TemplateLabelData labelData;

            /// <summary>
            /// Constructor
            /// </summary>
            public FedExLegacyPackageTemplateOutline(TemplateTranslationContext context)
                : base(context)
            {
                Lazy<string> labelFile = new Lazy<string>(() => labelData.Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Png)));

                AddAttribute("ID", () => labelData.PackageID);
                AddElement("LabelOnly", () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, labelFile.Value));
            }

            /// <summary>
            /// Create the bound clone
            /// </summary>
            public override ElementOutline CreateDataBoundClone(object data)
            {
                return new FedExLegacyPackageTemplateOutline(Context)
                {
                    labelData = (TemplateLabelData) data
                };
            }
        }
    }
}
