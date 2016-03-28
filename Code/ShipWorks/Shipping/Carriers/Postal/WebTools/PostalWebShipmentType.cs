using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data;
using System.Drawing.Imaging;
using System.Drawing;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Shipment type for USPS WebTools shipments
    /// </summary>
    public class PostalWebShipmentType : PostalShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.PostalWebTools;

        /// <summary>
        /// Create the wizard used to do the initial setup
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard() => new PostalWebSetupWizard();

        /// <summary>
        /// Create the UserControl used to handle USPS WebTools shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl) => new PostalWebServiceControl(rateControl);

        /// <summary>
        /// Create the settings control for defaults & settings
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl() => new PostalWebSettingsControl();

        /// <summary>
        /// Create the control used to edit the profile settings
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl() => new PostalProfileControlBase();

        /// <summary>
        /// USPS supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => true;

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer() => new WebToolsShipmentProcessingSynchronizer();

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

            // Lazily evaluate the full path to the primary label
            Lazy<string> primaryLabelPath = new Lazy<string>(() => labels.Value.Single(l => l.Category == TemplateLabelCategory.Primary).Resource.GetAlternateFilename(TemplateLabelUtility.GetFileExtension(ImageFormat.Png)));

            // Legacy stuff.  Have to check that there are labels, since 2x upgraded shipments won't have them
            ElementOutline outline = container.AddElement("USPS", ElementOutline.If(() => shipment().Processed));
            outline.AddAttributeLegacy2x();
            outline.AddElement("LabelOnly", () => primaryLabelPath.Value, ElementOutline.If(() => labels.Value.Count > 0));
            outline.AddElement("LabelOnlyRot90", () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate90FlipNone, primaryLabelPath.Value), ElementOutline.If(() => labels.Value.Count > 0));
            outline.AddElement("LabelOnlyRot270", () => TemplateLabelUtility.GenerateRotatedLabel(RotateFlipType.Rotate270FlipNone, primaryLabelPath.Value), ElementOutline.If(() => labels.Value.Count > 0));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Get the resource list for our shipment
            List<DataResourceReference> resources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            // Could be none for upgraded 2x shipments
            if (resources.Count > 0)
            {
                // Add our standard label output
                DataResourceReference labelResource = resources.Single(i => i.Label == "LabelPrimary");
                labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));

                // Add all label parts
                var labelResources = resources.Where(r => r.Label.StartsWith("LabelPart"));
                foreach (DataResourceReference documentResource in labelResources)
                {
                    labelData.Add(new TemplateLabelData(null, documentResource.Label, TemplateLabelCategory.Supplemental, documentResource));
                }
            }

            return labelData;
        }

        /// <summary>
        /// Gets all of the confirmation types that are available to a particular implementation of PostalShipmentType.
        /// </summary>
        /// <returns>A collection of all the confirmation types that are available to a USPS Web Tools shipment.</returns>
        public override IEnumerable<PostalConfirmationType> GetAllConfirmationTypes()
        {
            // The adult signature types are not available
            return new List<PostalConfirmationType>
            {
                PostalConfirmationType.None,
                PostalConfirmationType.Delivery,
                PostalConfirmationType.Signature
            };
        }

        /// <summary>
        /// Determines which confirmation types are available for the given service
        /// </summary>
        public override List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            IEnumerable<PostalConfirmationType> baseAvailableConfirmationTypes = base.GetAvailableConfirmationTypes(countryCode, service, packaging);

            // Remove the Adult Sig types since we aren't supporting them.
            return baseAvailableConfirmationTypes.Where(ct => ct != PostalConfirmationType.AdultSignatureRequired &&
                                                              ct != PostalConfirmationType.AdultSignatureRestricted).ToList();
        }

        /// <summary>
        /// Update the dynamic data of the shipment
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // Postal only has the option to use ShipWorks Insurance
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
        }
    }
}
