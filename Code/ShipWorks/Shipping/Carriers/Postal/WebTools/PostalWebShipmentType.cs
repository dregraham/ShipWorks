﻿using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data;
using System.Drawing.Imaging;
using System.Drawing;

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
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.PostalWebTools; }
        }

        /// <summary>
        /// Create the wizard used to do the initial setup
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new PostalWebSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle USPS WebTools shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public override ServiceControlBase CreateServiceControl(RateControl rateControl)
        {
            return new PostalWebServiceControl(rateControl);
        }

        /// <summary>
        /// Create the settings control for defaults & settings
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new PostalWebSettingsControl();
        }

        /// <summary>
        /// Create the control used to edit the profile settings
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new PostalProfileControlBase();
        }

        /// <summary>
        /// Stamps.com supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return true; }
        }

        /// <summary>
        /// Get the rates for the given shipment.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rateGroup = null;
            string rateHash = GetRatingHash(shipment);

            if (RateCache.Instance.Contains(rateHash))
            {
                rateGroup = RateCache.Instance.GetValue(rateHash);
            }
            else
            {
                rateGroup = new RateGroup(PostalWebClientRates.GetRates(shipment));
                RateCache.Instance.Save(rateHash, rateGroup);
            }

            return rateGroup;
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            if (PostalUtility.IsDomesticCountry(shipment.ShipCountryCode) && shipment.Postal.Confirmation == (int) PostalConfirmationType.None)
            {
                PostalPackagingType packaging = (PostalPackagingType) shipment.Postal.PackagingType;

                if ((shipment.Postal.Service != (int) PostalServiceType.ExpressMail) &&
                    !(shipment.Postal.Service == (int) PostalServiceType.FirstClass && (packaging== PostalPackagingType.Envelope || packaging == PostalPackagingType.LargeEnvelope)) )
                {
                    throw new ShippingException(string.Format(
                        "A confirmation option must be selected when shipping {0}.", 
                        EnumHelper.GetDescription((PostalServiceType) shipment.Postal.Service)));
                }
            }

            if (shipment.Postal.Service == (int) PostalServiceType.ExpressMail && shipment.Postal.Confirmation != (int) PostalConfirmationType.None)
            {
                throw new ShippingException("A confirmation option cannot be used with Express mail.");
            }

            // Process the shipment
            PostalWebClientShipping.ProcessShipment(shipment.Postal);
        }

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
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            List<IEntityField2> fields = new List<IEntityField2>(base.GetRatingFields(shipment));

            fields.AddRange
            (
                new List<IEntityField2>()
                {
                    shipment.Postal.Fields[StampsShipmentFields.StampsAccountID.FieldIndex],
                    shipment.Postal.Stamps.Fields[StampsShipmentFields.OriginalStampsAccountID.FieldIndex],
                }
            );

            return fields;
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
    }
}
