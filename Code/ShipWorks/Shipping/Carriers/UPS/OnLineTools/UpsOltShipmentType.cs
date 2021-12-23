using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// ShipmentType for UPS OLT
    /// </summary>
    public class UpsOltShipmentType : UpsShipmentType
    {
        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.UpsOnLineTools;

        /// <summary>
        /// Returns are supported for Online Tools
        /// </summary>
        public override bool SupportsReturns => true;

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => true;

        /// <summary>
        /// Create the settings control for UPS
        /// </summary>
        protected override SettingsControlBase CreateSettingsControlInternal(ILifetimeScope scope)
        {
            UpsOltSettingsControl control = new UpsOltSettingsControl(scope);
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

            IEnumerable<int> availableServiceTypes = allServiceTypes.Except(GetExcludedServiceTypes(repository));

            // Filter out non-supported shipengine services when they have ups accounts and
            // all of the accounts are shipengine accounts.
            var upsAccounts = AccountRepository.AccountsReadOnly;
            if (upsAccounts.Any() && upsAccounts.None(a => string.IsNullOrEmpty(a.ShipEngineCarrierId)))
            {
                // All UPS accounts are using ShipEngine, so only show the services supported by it
                IEnumerable<int> seSupportedServices = UpsShipEngineServiceTypeUtility.GetSupportedServices().Cast<int>();

                availableServiceTypes = availableServiceTypes.Intersect(seSupportedServices);
            }

            return availableServiceTypes;
        }

        /// <summary>
        /// Creates the Returns control
        /// </summary>
        public override ReturnsControlBase CreateReturnsControl()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                bool onlyOneBalanceAccounts = scope.Resolve<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>()
                    .AccountsReadOnly.All(a => !string.IsNullOrEmpty(a.ShipEngineCarrierId));

                if (onlyOneBalanceAccounts)
                {
                    return base.CreateReturnsControl();
                }
            }

            return new UpsOltReturnsControl();
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(loaded));

            //add the tax id
            container.AddElement("TIN", () => ($"{loaded().Ups.CustomsRecipientTIN} ({(UpsCustomsRecipientTINType) loaded().Ups.CustomsRecipientTINType})"));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, () => GetStandardImageFormat(loaded)),
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
        /// Labels originating from ShipEngine are png, ups olt are gif
        /// </summary>
        private static ImageFormat GetStandardImageFormat(Func<ShipmentEntity> shipment) =>
            shipment().Ups.ShipEngineLabelID != null ? ImageFormat.Png : ImageFormat.Gif;

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            if (shipment().Ups.ShipEngineLabelID != null)
            {
                return DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID)
                    .Where(x => x.Label.StartsWith("LabelPrimary") || x.Label.StartsWith("LabelPart"))
                    .Select(x => new TemplateLabelData(null, "Label", x.Label.StartsWith("LabelPrimary") ?
                        TemplateLabelCategory.Primary : TemplateLabelCategory.Supplemental, x))
                    .ToList();
            }

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
