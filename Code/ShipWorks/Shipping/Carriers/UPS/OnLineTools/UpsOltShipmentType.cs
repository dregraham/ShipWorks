using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Drawing.Imaging;
using ShipWorks.Data;
using System.Drawing;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// ShipmentType for UPS OLT
    /// </summary>
    public class UpsOltShipmentType : UpsShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOltShipmentType"/> class.
        /// </summary>
        public UpsOltShipmentType(IUpsPromoFactory promoFactory) :
            base(promoFactory)
        {
        }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.UpsOnLineTools; }
        }

        /// <summary>
        /// Returns are supported for Online Tools
        /// </summary>
        public override bool SupportsReturns
        {
            get
            {
                return true;
            }
        }
        
        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return true; }
        }

        /// <summary>
        /// Create the settings control for UPS
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            UpsOltSettingsControl control = new UpsOltSettingsControl();
            control.Initialize(ShipmentTypeCode);
            return control;
        }

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not 
        /// been excluded). The integer values are intended to correspond to the appropriate 
        /// enumeration values of the specific shipment type (i.e. the integer values would 
        /// correspond to PostalServiceType values for a UspsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServiceTypes = Enum.GetValues(typeof(UpsServiceType)).Cast<int>().ToList();
            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Creates the Returns control
        /// </summary>
        public override Editing.ReturnsControlBase CreateReturnsControl()
        {
            return new UpsOltReturnsControl();
        }

        /// <summary>
        /// Process the UPS shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            try
            {
                // Call the base class for setting default values as needed based on the service/package type of the shipment
                base.ProcessShipment(shipment);

                UpsOltShipmentValidator upsOltShipmentValidator = new UpsOltShipmentValidator();

                upsOltShipmentValidator.ValidateShipment(shipment);

                UpsServicePackageTypeSetting.Validate(shipment);
                UpsApiShipClient.ProcessShipment(shipment);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" && shipment.ReturnShipment && !String.IsNullOrEmpty(ex.ErrorLocation))
                {
                    if (String.Compare(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        message = "The return shipment's Contents is required.";
                    }
                }

                throw new ShippingException(message, ex);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Gif),
                ElementOutline.If(() => shipment().Processed));

            // Legacy stuff
            ElementOutline outline = container.AddElement("UPS", ElementOutline.If(() => shipment().Processed));
            outline.AddAttributeLegacy2x();
            outline.AddElement("Voided", () => shipment().Voided);
            outline.AddElement("NegotiatedRate", () => loaded().Ups.NegotiatedRate);
            outline.AddElement("PublishedCharges", () => loaded().Ups.PublishedCharges);
            outline.AddElement("Package", new UpsLegacyPackageTemplateOutline(container.Context), () => labels.Value, ElementOutline.If(() => shipment().ActualLabelFormat == null));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Add labels for each package
            foreach (long packageID in DataProvider.GetRelatedKeys(shipment().ShipmentID, EntityType.UpsPackageEntity))
            {
                // Get the resource list for our shipment
                List<DataResourceReference> resources = DataResourceManager.GetConsumerResourceReferences(packageID);

                // Can be missing for 2x upgraded shipments
                if (resources.Count > 0)
                {
                    // Add our standard label output
                    DataResourceReference labelResource = resources.Single(i => i.Label == "LabelImage");
                    labelData.Add(new TemplateLabelData(packageID, "Label", TemplateLabelCategory.Primary, labelResource));
                }

                // Supporting documents
                var customsResources = resources.Where(r => r.Label.StartsWith("Customs"));
                foreach (DataResourceReference customsResource in customsResources)
                {
                    labelData.Add(new TemplateLabelData(null, customsResource.Label, TemplateLabelCategory.Supplemental, customsResource));
                }
            }

            return labelData;
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.UpsShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new UpsShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Outline for the legacy UPS 'Package' element
        /// </summary>
        private class UpsLegacyPackageTemplateOutline : ElementOutline
        {
            TemplateLabelData labelData;

            /// <summary>
            /// Constructor
            /// </summary>
            public UpsLegacyPackageTemplateOutline(TemplateTranslationContext context)
                : base(context)
            {
                Lazy<string> labelFile = new Lazy<string>(() => labelData.Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Gif)));

                AddAttribute("ID", () => labelData.PackageID);
                AddElement("LabelOnly", () => labelFile.Value);
                AddElement("LabelOnlyRot90", () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, labelFile.Value));
            }

            /// <summary>
            /// Create the bound clone
            /// </summary>
            public override ElementOutline CreateDataBoundClone(object data)
            {
                return new UpsLegacyPackageTemplateOutline(Context) { labelData = (TemplateLabelData) data };
            }
        }
    }
}
