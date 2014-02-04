using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXml;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using System.Windows.Forms;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Data;
using System.Drawing.Imaging;
using System.Drawing;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.UI.Wizard;

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
        public override WizardForm CreateSetupWizard()
        {
            return new PostalWebSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle USPS WebTools shipments
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new PostalWebServiceControl();
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
            return new RateGroup(PostalWebClientRates.GetRates(shipment));
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
        /// Gets an instance to the best rate shipping broker for the USPS web tools shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a WebToolsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            // We want to return the null broker if there is already an Endicia or Stamps.com
            // account setup, so postal rates for Web Tools aren't used as well (i.e. just use
            // the provider that has an account instead of rates from web tools).
            IBestRateShippingBroker broker = new NullShippingBroker();

            bool stampsAccountsExist = StampsAccountManager.GetAccounts(false).Any();
            bool endiciaAccountsExist = EndiciaAccountManager.GetAccounts(EndiciaReseller.None).Any();

            if (!stampsAccountsExist && !endiciaAccountsExist)
            {
                // There aren't any postal based accounts setup, so we want to see if we should 
                // show counter rates (depending whether Endicia or Stamps.com have been excluded)
                
                // We need to see which Postal provider to show when signing up for a postal account
                // based on the global shipping settings and best rate settings with preference for Endicia
                ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();

                if (!shippingSettings.ExcludedTypes.Contains((int)ShipmentTypeCode.Endicia) && !shippingSettings.BestRateExcludedTypes.Contains((int)ShipmentTypeCode.Endicia))
                {
                    // Endicia has not been excluded in the global shipping settings, has not been excluded 
                    // from Best Rate, and there aren't any endicia accounts, so set the ShipmentType in the
                    // web tool counter rates broker to be for Endicia
                    broker  = new WebToolsCounterRatesBroker(new EndiciaShipmentType());
                }
                else if (!shippingSettings.ExcludedTypes.Contains((int)ShipmentTypeCode.Stamps) && !shippingSettings.BestRateExcludedTypes.Contains((int)ShipmentTypeCode.Stamps))
                {
                    // Endicia is not being used in best rate (for whatever reason), Stamps.com is has not been excluded 
                    // in the global shipping settings and has not been excluded from Best Rate, so set the ShipmentType 
                    // in the web tool counter rates broker to be for Stamps.com
                    broker = new WebToolsCounterRatesBroker(new StampsShipmentType());
                }

                // If neither of the above conditions were satisfied, Endicia and Stamps have both been excluded from Best Rate, so do nothing
                // and just return the null broker
            }

            return broker;
        }
    }
}
