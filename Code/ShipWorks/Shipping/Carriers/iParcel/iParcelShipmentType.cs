using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.UI.Wizard;


namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// i-Parcel implementation of the ShipmentType
    /// </summary>
    public class iParcelShipmentType : ShipmentType
    {
        private readonly IiParcelRepository repository;
        private readonly IiParcelServiceGateway serviceGateway;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelShipmentType" /> class.
        /// </summary>
        public iParcelShipmentType()
            : this(new iParcelDatabaseRepository(), new iParcelServiceGateway(), new ExcludedServiceTypeRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelShipmentType" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="serviceGateway">The service gateway.</param>
        public iParcelShipmentType(IiParcelRepository repository, IiParcelServiceGateway serviceGateway, IExcludedServiceTypeRepository excludedServiceTypeRepository)
        {
            this.repository = repository;
            this.serviceGateway = serviceGateway;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.iParcel; }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return iParcelAccountManager.Accounts.Any();
            }
        }

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            IParcelShipmentEntity iParcelShipmentEntity = shipment.IParcel;

            iParcelShipmentEntity.Reference = string.Empty;
            iParcelShipmentEntity.TrackByEmail = false;
            iParcelShipmentEntity.TrackBySMS = false;
            iParcelShipmentEntity.IsDeliveryDutyPaid = true;

            iParcelShipmentEntity.RequestedLabelFormat = (int)ThermalLanguage.None;

            IParcelPackageEntity package = CreateDefaultPackage();
            iParcelShipmentEntity.Packages.Add(package);

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Create a new package entity that has default values
        /// </summary>
        public static IParcelPackageEntity CreateDefaultPackage()
        {
            IParcelPackageEntity package = new IParcelPackageEntity();

            package.Weight = 0;

            package.DimsProfileID = 0;
            package.DimsLength = 0;
            package.DimsWidth = 0;
            package.DimsHeight = 0;
            package.DimsWeight = 0;
            package.DimsAddWeight = true;

            package.Insurance = false;
            package.InsuranceValue = 0;
            package.InsurancePennyOne = false;
            package.DeclaredValue = 0;

            package.TrackingNumber = string.Empty;
            package.ParcelNumber = string.Empty;

            package.SkuAndQuantities = string.Empty;

            return package;
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new iParcelProfileControl();
        }

        /// <summary>
        /// Configures the shipment for ShipSense. This is useful for carriers that support
        /// multiple package shipments, allowing the shipment type a chance to add new packages
        /// to coincide with the ShipSense knowledge base entry.
        /// </summary>
        /// <param name="knowledgebaseEntry">The knowledge base entry.</param>
        /// <param name="shipment">The shipment.</param>
        protected override void SyncNewShipmentWithShipSense(ShipSense.KnowledgebaseEntry knowledgebaseEntry, ShipmentEntity shipment)
        {
            if (shipment.IParcel.Packages.RemovedEntitiesTracker == null)
            {
                shipment.IParcel.Packages.RemovedEntitiesTracker = new IParcelPackageCollection();
            }

            base.SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);

            while (shipment.IParcel.Packages.Count < knowledgebaseEntry.Packages.Count())
            {
                IParcelPackageEntity package = CreateDefaultPackage();
                shipment.IParcel.Packages.Add(package);
            }

            while (shipment.IParcel.Packages.Count > knowledgebaseEntry.Packages.Count())
            {
                // Remove the last package until the packages counts match
                shipment.IParcel.Packages.RemoveAt(shipment.IParcel.Packages.Count - 1);
            }
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.IParcel == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            if (!shipment.IParcel.Packages.Any())
            {
                throw new iParcelException("There must be at least one package to create the i-parcel package adapter.");
            }

            // Return an adapter per package
            List<IPackageAdapter> adapters = new List<IPackageAdapter>();
            foreach (IParcelPackageEntity packageEntity in shipment.IParcel.Packages)
            {
                adapters.Add(new iParcelPackageAdapter(shipment, packageEntity));
            }

            return adapters;
        }

        /// <summary>
        /// Save carrier specific profile data to the database.  Return true if anything was dirty and saved, or was deleted.
        /// </summary>
        public override bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            bool changes = base.SaveProfileData(profile, adapter);

            // First delete out anything that needs deleted
            foreach (IParcelProfilePackageEntity package in profile.IParcel.Packages.ToList())
            {
                // If its new but deleted, just get rid of it
                if (package.Fields.State == EntityState.Deleted)
                {
                    if (package.IsNew)
                    {
                        profile.IParcel.Packages.Remove(package);
                    }

                        // If its deleted, delete it
                    else
                    {
                        package.Fields.State = EntityState.Fetched;
                        profile.IParcel.Packages.Remove(package);

                        adapter.DeleteEntity(package);

                        changes = true;
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = iParcelAccountManager.Accounts.Count > 0
                                 ? iParcelAccountManager.Accounts[0].IParcelAccountID
                                 : (long) 0;

            profile.IParcel.IParcelAccountID = shipperID;
            profile.OriginID = (int) ShipmentOriginSource.Account;

            profile.IParcel.Service = (int) iParcelServiceType.Preferred;
            profile.IParcel.IsDeliveryDutyPaid = false;
            profile.IParcel.TrackByEmail = false;
            profile.IParcel.TrackBySMS = false;
            profile.IParcel.Reference = "Order {//Order/Number}";
            profile.IParcel.SkuAndQuantities = "<xsl:for-each select=\"//Order/Item\"> {SKU}, {Quantity} <xsl:if test=\"position() !=  last()\">|</xsl:if></xsl:for-each>";
            profile.IParcel.IsDeliveryDutyPaid = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            IParcelShipmentEntity iParcel = shipment.IParcel;
            IParcelProfileEntity source = profile.IParcel;

            bool changedPackageWeights = false;

            // Apply all package profiles
            for (int i = 0; i < profile.IParcel.Packages.Count; i++)
            {
                // Get the profile to apply
                IParcelProfilePackageEntity packageProfile = profile.IParcel.Packages[i];

                IParcelPackageEntity package;

                // Get the existing, or create a new package
                if (iParcel.Packages.Count > i)
                {
                    package = iParcel.Packages[i];
                }
                else
                {
                    package = CreateDefaultPackage();
                    iParcel.Packages.Add(package);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, IParcelPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, IParcelPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, IParcelPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, IParcelPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, IParcelPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, IParcelPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, IParcelPackageFields.DimsAddWeight);
                }
            }

            // Remove any packages that are too many for the profile
            if (profile.IParcel.Packages.Count > 0)
            {
                // Go through each package that needs removed
                foreach (IParcelPackageEntity package in iParcel.Packages.Skip(profile.IParcel.Packages.Count).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    iParcel.Packages.Remove(package);

                    // If its saved in the database, we have to delete it
                    if (!package.IsNew)
                    {
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.DeleteEntity(package);
                        }
                    }
                }
            }

            // Apply value stored at shipment level which applies to each package.
            foreach (IParcelPackageEntity package in iParcel.Packages)
            {
                ShippingProfileUtility.ApplyProfileValue(profile.IParcel.SkuAndQuantities, package,
                    IParcelPackageFields.SkuAndQuantities);
            }

            base.ApplyProfile(shipment, profile);

            long? accountID = (source.IParcelAccountID == 0 && iParcelAccountManager.Accounts.Count > 0)
                                  ? (long?) iParcelAccountManager.Accounts[0].IParcelAccountID
                                  : source.IParcelAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, iParcel, IParcelShipmentFields.IParcelAccountID);
            ShippingProfileUtility.ApplyProfileValue(source.Service, iParcel, IParcelShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(source.Reference, iParcel, IParcelShipmentFields.Reference);
            ShippingProfileUtility.ApplyProfileValue(source.TrackByEmail, iParcel, IParcelShipmentFields.TrackByEmail);
            ShippingProfileUtility.ApplyProfileValue(source.TrackBySMS, iParcel, IParcelShipmentFields.TrackBySMS);
            ShippingProfileUtility.ApplyProfileValue(source.IsDeliveryDutyPaid, iParcel, IParcelShipmentFields.IsDeliveryDutyPaid);

            if (changedPackageWeights)
            {
                UpdateTotalWeight(shipment);
            }

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            bool existed = profile.IParcel != null;

            ShipmentTypeDataService.LoadProfileData(profile, "IParcel", typeof (IParcelProfileEntity), refreshIfPresent);

            IParcelProfileEntity iParcelProfileEntityParcel = profile.IParcel;

            // If this is the first time loading it, or we are supposed to refresh, do it now
            if (!existed || refreshIfPresent)
            {
                iParcelProfileEntityParcel.Packages.Clear();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(iParcelProfileEntityParcel.Packages,
                                                  new RelationPredicateBucket(IParcelProfilePackageFields.ShippingProfileID == profile.ShippingProfileID));
                    
                    iParcelProfileEntityParcel.Packages.Sort((int) IParcelProfilePackageFieldIndex.IParcelProfilePackageID, ListSortDirection.Ascending);
                }
            }
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            RedistributeContentWeight(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // Consider the shipment insured of any package is insured
            shipment.Insurance = shipment.IParcel.Packages.Any(p => p.Insurance);

            // Set the provider type based on i-Parcel settings
            shipment.InsuranceProvider = settings.IParcelInsuranceProvider;

            shipment.RequestedLabelFormat = shipment.IParcel.RequestedLabelFormat;

            // Right now ShipWorks Insurance (due to Tango limitation) doesn't support multi-package - so in that case just auto-revert to carrier insurance
            if (shipment.IParcel.Packages.Count > 1)
            {
                shipment.InsuranceProvider = (int) InsuranceProvider.Carrier;
            }

            // Check the IParcel wide PennyOne settings and get them updated
            foreach (var package in shipment.IParcel.Packages)
            {
                package.InsurancePennyOne = settings.IParcelInsurancePennyOne;

                // The only time we send the full insuredvalue as declared value is if insurance is enabled, and they are using carrier insurance.
                if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
                {
                    package.DeclaredValue = package.InsuranceValue;
                }
                else
                {
                    // Otherwise, regardless of if they are insuring or not, penny one or not, etc., send 0, the first $100 is coverred anyway
                    package.DeclaredValue = 0;
                }
            }
        }

        /// <summary>
        /// Get the valid services to the given country
        /// </summary>
        public static List<iParcelServiceType> GetValidServiceTypes()
        {
            // We'll assume all countries are valid, and if one is not, i-Parcel should return us an error upon submission.
            return new List<iParcelServiceType>
                {
                    iParcelServiceType.Immediate,
                    iParcelServiceType.Preferred,
                    iParcelServiceType.Saver,
                    iParcelServiceType.SaverDeferred
                };
        }

        /// <summary>
        /// Update the total weight of the shipment based on the individual package weights
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            double contentWeight = 0;
            double totalWeight = 0;

            foreach (IParcelPackageEntity package in shipment.IParcel.Packages)
            {
                contentWeight += package.Weight;
                totalWeight += package.Weight;

                if (package.DimsAddWeight)
                {
                    totalWeight += package.DimsWeight;
                }
            }

            shipment.ContentWeight = contentWeight;
            shipment.TotalWeight = totalWeight;
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.  Returns true if weight had to be redistributed.
        /// </summary>
        public static bool RedistributeContentWeight(ShipmentEntity shipment)
        {
            // Determine what our content weight should be
            double contentWeight = shipment.IParcel.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (contentWeight != shipment.ContentWeight)
            {
                foreach (IParcelPackageEntity package in shipment.IParcel.Packages)
                {
                    package.Weight = shipment.ContentWeight/shipment.IParcel.Packages.Count;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the i-Parcel shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            IParcelShipmentEntity iParcelShipmentEntity = shipment.IParcel;
            IParcelAccountEntity account = iParcelAccountManager.GetAccount(iParcelShipmentEntity.IParcelAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.Username;
            commonDetail.ServiceType = iParcelShipmentEntity.Service;

            // i-Parcel doesn't have a packaging type concept, so default to 0
            commonDetail.PackagingType = 0;
            commonDetail.PackageLength = iParcelShipmentEntity.Packages[0].DimsLength;
            commonDetail.PackageWidth = iParcelShipmentEntity.Packages[0].DimsWidth;
            commonDetail.PackageHeight = iParcelShipmentEntity.Packages[0].DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Create the setup wizard form that will walk the user through setting up the shipment type.  Can return
        /// null if the shipment type does not require setup
        /// </summary>
        /// <returns>An iParcelSetupWizard object.</returns>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new iParcelSetupWizard();
        }

        /// <summary>
        /// Creates the UserControl that is used to edit the defaults\settings for the service
        /// </summary>
        /// <returns>An iParcelSettingsControl object.</returns>
        public override SettingsControlBase CreateSettingsControl()
        {
            iParcelSettingsControl settingsControl = new iParcelSettingsControl();
            settingsControl.Initialize(ShipmentTypeCode);

            return settingsControl;
        }

        /// <summary>
        /// Creates the UserControl that is used to edit service options for the shipment type
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        /// <returns>An iParcelServiceControl object.</returns>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new iParcelServiceControl(rateControl);
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the service types are fetched.</param>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServices = EnumHelper.GetEnumList<iParcelServiceType>().Select(e => (int)e.Value);
            return allServices.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the IParcel data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "IParcel", typeof (IParcelShipmentEntity), refreshIfPresent);

            IParcelShipmentEntity iParcelShipmentEntity = shipment.IParcel;

            if (refreshIfPresent)
            {
                iParcelShipmentEntity.Packages.Clear();
            }

            // If there are no packages load them now
            if (iParcelShipmentEntity.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(iParcelShipmentEntity.Packages,
                                                  new RelationPredicateBucket(IParcelPackageFields.ShipmentID == shipment.ShipmentID));

                    iParcelShipmentEntity.Packages.Sort((int) IParcelPackageFieldIndex.IParcelPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                iParcelShipmentEntity.Packages.RemovedEntitiesTracker = new IParcelPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (iParcelShipmentEntity.Packages.Count == 0)
            {
                // This was changed to an exception instead of creating the package when the creation was moved to ConfigureNewShipment
                throw new NotFoundException("Primary package not found.");
            }
        }
        
        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new iParcelShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ProcessShipmentAndReturnResponse(shipment);
        }

        /// <summary>
        /// Process the shipment. (This is being used for Fitnesse tests.)
        /// </summary>
        public DataSet ProcessShipmentAndReturnResponse(ShipmentEntity shipment)
        {
            try
            {
                if (shipment == null)
                {
                    throw new ArgumentNullException("shipment");
                }
                
                if (shipment.RequestedLabelFormat != (int) ThermalLanguage.None)
                {
                    shipment.ActualLabelFormat = shipment.RequestedLabelFormat;
                }
                else
                {
                    shipment.ActualLabelFormat = null;
                }

                IParcelAccountEntity iParcelAccount = repository.GetiParcelAccount(shipment);
                iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

                // i-parcel requires that we upload item information, so fetch the order and order items
                repository.PopulateOrderDetails(shipment);

                DataSet response = serviceGateway.SubmitShipment(credentials, shipment);

                repository.SaveLabel(shipment, response);
                repository.SaveTrackingInfoToEntity(shipment, response);

                return response;
            }
            catch (iParcelException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription((iParcelServiceType) shipment.IParcel.Service);
        }

        /// <summary>
        /// Get the total packages contained by the shipment
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return shipment.IParcel.Packages.Count;
        }

        /// <summary>
        /// Get the parcel data that describes details about a particular parcel
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (parcelIndex >= 0 && parcelIndex < shipment.IParcel.Packages.Count)
            {
                IParcelPackageEntity package = shipment.IParcel.Packages[parcelIndex];

                return new ShipmentParcel(shipment, package.IParcelPackageID, package.TrackingNumber,
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package));
            }

            throw new ArgumentException(string.Format("'{0}' is out of range for the shipment.", parcelIndex), "parcelIndex");
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(
            ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels", new LabelsOutline(container.Context, shipment, labels, ImageFormat.Jpeg), ElementOutline.If(() => shipment().Processed));

            // Add xml element for the carrier tracking url
            container.AddElement("CarrierTrackingUrl", () => GetCarrierTrackingUrl(shipment()));
        }

        /// <summary>
        /// Returns a url to the carrier's website for the specified shipment
        /// </summary>
        public override string GetCarrierTrackingUrl(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            ShipmentEntity shipmentEntity = shipment;

            if (!string.IsNullOrWhiteSpace(shipmentEntity.TrackingNumber))
            {
                return string.Format("https://tracking.i-parcel.com/secure/track.aspx?track={0}", shipmentEntity.TrackingNumber);
            }

            return string.Empty;
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // Get the resource list for our shipment
            List<DataResourceReference> resources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            if (resources.Count > 0)
            {
                // Add our standard label output
                foreach (DataResourceReference labelResource in resources)
                {
                    labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));
                }
            }

            return labelData;
        }

        /// <summary>
        /// FedEx supports rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            return GetCachedRates<iParcelException>(shipment, GetRatesFromApi);
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            IParcelAccountEntity iParcelAccount = null;

            try
            {
                iParcelAccount = repository.GetiParcelAccount(shipment);
            }
            catch (iParcelException ex)
            {
                if (ex.Message == "No i-parcel account is selected for the shipment.")
                {
                    // Provide a message with additional context
                    throw new iParcelException("An i-parcel account is required to view rates.", ex);
                }
                else
                {
                    throw;
                }
            }

            // i-parcel requires that we upload item information, so fetch the order and order items
            repository.PopulateOrderDetails(shipment);

            List<RateResult> results = new List<RateResult>();

            iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);
            DataSet ratesResult = serviceGateway.GetRates(credentials, shipment);

            if (ratesResult != null && ratesResult.Tables.Count != 0 && ratesResult.Tables[0].Rows.Count != 0)
            {
                if (ratesResult.Tables.Contains("CostInfo"))
                {
                    // i-parcel will return a negative value if there was some sort of error or the shipment is not eligible for
                    // this service type (e.g. initial testing indicates rates won't come back for any package over 66 pounds)

                    DataTable costInfoTable = ratesResult.Tables["CostInfo"];

                    // Find the service types where a valid rate (shipping cost > 0) was given for each of the packages in the shipment
                    IEnumerable<iParcelServiceType> supportedServiceTypes = costInfoTable.AsEnumerable()
                                                                                            .Where(r => decimal.Parse(r.Field<string>("PackageShipping")) >= 0)
                                                                                            .GroupBy(r => r["Service"])
                                                                                            .Where(grp => grp.Count() == shipment.IParcel.Packages.Count)
                                                                                            .Select(grp => EnumHelper.GetEnumByApiValue<iParcelServiceType>(grp.Key.ToString()));

                    IEnumerable<iParcelServiceType> disabledServices = GetExcludedServiceTypes(excludedServiceTypeRepository)
                        .Select(s => (iParcelServiceType)s);
                    

                    // Filter out the excluded service types before creating rate results
                    foreach (iParcelServiceType serviceType in supportedServiceTypes.Except(disabledServices.Where(s => (iParcelServiceType)s != (iParcelServiceType)shipment.IParcel.Service)))
                    {
                        // Calculate the total shipment cost for all the package rates for the service type
                        List<DataRow> serviceRows = costInfoTable.AsEnumerable()
                                               .Where(row => EnumHelper.GetEnumByApiValue<iParcelServiceType>(row["Service"].ToString()) == serviceType)
                                               .ToList();

                        decimal shippingCost = serviceRows.Sum(row => decimal.Parse(row["PackageShipping"].ToString()) + decimal.Parse(row["PackageInsurance"].ToString()));
                        decimal taxCost = serviceRows.Sum(row => decimal.Parse(row["PackageTax"].ToString()));
                        decimal dutyCost = serviceRows.Sum(row => decimal.Parse(row["PackageDuty"].ToString()));
                        decimal totalCost = shippingCost + taxCost + dutyCost;

                        RateResult serviceRate = new RateResult(EnumHelper.GetDescription(serviceType), string.Empty, totalCost, dutyCost, taxCost, shippingCost, new iParcelRateSelection(serviceType))
                        {
                            ServiceLevel = ServiceLevelType.Anytime,
                            ShipmentType = ShipmentTypeCode.iParcel,
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.iParcel)
                        };

                        results.Add(serviceRate);
                    }

                }
            }

            return new RateGroup(results);
        }

        /// <summary>
        /// Get all the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            IParcelShipmentEntity iParcelShipment = shipment.IParcel;

            if (iParcelShipment.Packages.Count == 1)
            {
                return base.GetTrackingNumbers(shipment);
            }
            
            List<string> trackingList = new List<string>();

            for (int i = 0; i < iParcelShipment.Packages.Count; i++)
            {
                trackingList.Add(string.Format("Package {0}: {1}", i + 1, iParcelShipment.Packages[i].TrackingNumber));
            }

            return trackingList;
        }

        /// <summary>
        /// Must be overridden by derived types to provide tracking details for the given shipment.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">shipment</exception>
        /// <exception cref="ShippingException">ShipWorks cannot track an i-parcel shipment that does not have any packages.</exception>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.IParcel == null || shipment.IParcel.Packages.Count == 0)
            {
                throw new ShippingException("ShipWorks cannot track an i-parcel shipment that does not have any packages.");
            }

            try
            {
                // Send the shipment to the gateway
                IParcelAccountEntity iParcelAccount = repository.GetiParcelAccount(shipment);
                iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

                DataSet response = serviceGateway.TrackShipment(credentials, shipment);

                TrackingResult trackingInfo = new TrackingResult();
                trackingInfo.Summary = GetFormattedTrackingSummary(response);
                BuildTrackingDetails(trackingInfo, response);

                return trackingInfo;
            }
            catch (Exception exception)
            {
                string message = string.Format("ShipWorks was unable to obtain tracking information from i-parcel. {0}", exception.Message);
                throw new ShippingException(message, exception);
            }
        }

        /// <summary>
        /// Gets the formatted tracking summary based on the data set provided.
        /// </summary>
        /// <param name="trackingResponse">The tracking response.</param>
        /// <returns>An HTML formatted string of the tracking summary.</returns>
        private static string GetFormattedTrackingSummary(DataSet trackingResponse)
        {
            const string TrackingEventDetailTableName = "TrackingEventDetail";
            const string PackageTrackingInfoTableName = "PackageTrackingInfo";

            const string EstimatedArrivalDateColumnName = "EstimatedArrivalDate";
            const string EventCodeDescriptionColumnName = "EventCodeDesc";
            const string SignedForByNameColumnName = "SignedForByName";

            if (!trackingResponse.Tables.Contains(PackageTrackingInfoTableName))
            {
                throw new iParcelException("ShipWorks could not find the tracking information in the response from i-parcel.");
            }
            
            StringBuilder summary = new StringBuilder();

            // Event history is in descending order so grab the first row
            DataRow lastTrackedEvent = trackingResponse.Tables[TrackingEventDetailTableName].Rows[0];
            summary.AppendFormat("<b>{0}</b>", lastTrackedEvent[EventCodeDescriptionColumnName]);


            if (lastTrackedEvent[EventCodeDescriptionColumnName].ToString().ToUpperInvariant() == "DELIVERED")
            {
                // The package has been delivered; show the actual date of the delivery
                DateTime deliveryDate = DateTime.Parse(lastTrackedEvent["EventDateTime"].ToString());
                summary.AppendFormat(" on {0} ", deliveryDate.ToString("M/dd/yyy h:mm tt"));
            }
            else
            {
                // Package has not been delivered; show the estimated delivery date (if there is one)
                if (trackingResponse.Tables[PackageTrackingInfoTableName].Columns.Contains(EstimatedArrivalDateColumnName))
                {
                    DateTime estimatedDeliveryDate = DateTime.Parse(trackingResponse.Tables[PackageTrackingInfoTableName].Rows[0][EstimatedArrivalDateColumnName].ToString());
                    summary.AppendFormat("<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {0}</span>", estimatedDeliveryDate.ToString("M/dd/yyy h:mm tt"));
                }
            }

            if (trackingResponse.Tables[TrackingEventDetailTableName].Columns.Contains(SignedForByNameColumnName))
            {
                if (!string.IsNullOrEmpty(lastTrackedEvent[SignedForByNameColumnName].ToString()))
                {
                    // The package has been signed for
                    summary.AppendFormat("<br/><span style='color: rgb(80, 80, 80);'>Signed by: {0}</span>", lastTrackedEvent[SignedForByNameColumnName]);
                }
            }

            return summary.ToString();
        }

        /// <summary>
        /// Gets the tracking details.
        /// </summary>
        /// <param name="trackingResult">The TrackingResult that the details get added to.</param>
        /// <param name="response">The response.</param>
        private static void BuildTrackingDetails(TrackingResult trackingResult, DataSet response)
        {
            const string TrackingEventDetailTableName = "TrackingEventDetail";

            // Build a tracking detail object for each row in the tracking event detail table
            for (int rowIndex = 0; rowIndex < response.Tables[TrackingEventDetailTableName].Rows.Count; rowIndex++)
            {
                DateTime eventDate = DateTime.Parse(response.Tables[TrackingEventDetailTableName].Rows[rowIndex]["EventDateTime"].ToString());

                TrackingResultDetail detail = new TrackingResultDetail
                {
                    Activity = response.Tables[TrackingEventDetailTableName].Rows[rowIndex]["EventCodeDesc"].ToString(),
                    Date = eventDate.ToString("M/dd/yyy"),
                    Time = eventDate.ToString("h:mm tt"),
                    Location = GetTrackingDetailLocation(response, rowIndex)
                };
                
                trackingResult.Details.Add(detail);
            }
        }

        /// <summary>
        /// Gets the tracking detail location for a specific trackable event in the DataSet 
        /// denoted by the given row index.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="rowIndex">Index of the row/event being accessed.</param>
        /// <returns>A string containing the location information for a trackable event.</returns>
        private static string GetTrackingDetailLocation(DataSet response, int rowIndex)
        {
            const string EventLocationTableName = "EventLocation";
            const string CityColumnName = "City";
            const string StateProvinceColumnName = "StateProvince";
            const string PostalCodeColumnName = "PostalCode";
            const string CountryCodeColumnName = "CountryCode";

            StringBuilder location = new StringBuilder();

            // City
            if (response.Tables[EventLocationTableName].Columns.Contains(CityColumnName))
            {
                if (!string.IsNullOrWhiteSpace(response.Tables[EventLocationTableName].Rows[rowIndex][CityColumnName].ToString()))
                {
                    location.Append(response.Tables[EventLocationTableName].Rows[rowIndex][CityColumnName] + ",");
                }
            }

            // State/Province
            if (response.Tables[EventLocationTableName].Columns.Contains(StateProvinceColumnName))
            {
                // State/province is not supplied for all countries, so format the location accordingly
                if (!string.IsNullOrWhiteSpace(response.Tables[EventLocationTableName].Rows[rowIndex][StateProvinceColumnName].ToString()))
                {
                    location.AppendFormat(" {0}", response.Tables[EventLocationTableName].Rows[rowIndex][StateProvinceColumnName]);
                }
            }

            // Postal Code
            if (response.Tables[EventLocationTableName].Columns.Contains(PostalCodeColumnName))
            {
                if (!string.IsNullOrWhiteSpace(response.Tables[EventLocationTableName].Rows[rowIndex][PostalCodeColumnName].ToString()))
                {
                    location.AppendFormat(" {0}", response.Tables[EventLocationTableName].Rows[rowIndex][PostalCodeColumnName]);
                }
            }

            // Country code
            if (response.Tables[EventLocationTableName].Columns.Contains(CountryCodeColumnName))
            {
                // Add the country if it's not the US
                if (response.Tables[EventLocationTableName].Rows[rowIndex][CountryCodeColumnName].ToString().ToUpperInvariant() != "US")
                {
                    if (!string.IsNullOrWhiteSpace(response.Tables[EventLocationTableName].Rows[rowIndex][CountryCodeColumnName].ToString()))
                    {
                        if (location.Length > 0)
                        {
                            location.Append(" ");
                        }

                        location.Append(response.Tables[EventLocationTableName].Rows[rowIndex][CountryCodeColumnName]);
                    }
                }
            }

            return location.ToString();
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the iParcel shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an iParcelBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            // There was a conscious decision made with input from Rich that for now we can assume the origin 
            // country will always be US when the shipment is configured to ship from the account address
            string originCountryCode = (ShipmentOriginSource)shipment.OriginOriginID == ShipmentOriginSource.Account ? 
                                        "US" : shipment.AdjustedOriginCountryCode();

            // We only want to check i-parcel for international shipments originating in the US
            if (originCountryCode != shipment.AdjustedShipCountryCode() && originCountryCode == "US")
            {
                return new iParcelBestRateBroker();
            }

            // This is either a domestic shipment or the shipment does not originate from the US,
            // so we don't want to check i-parcel
            return new NullShippingBroker();
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override bool IsCustomsRequiredByShipment(ShipmentEntity shipment)
        {
            bool requiresCustoms = base.IsCustomsRequiredByShipment(shipment);

            if (shipment.AdjustedOriginCountryCode() == "US")
            {
                // i-Parcel allows customers to upload their SKUs and customs info, so we don't need to enter it in ShipWorks
                // So Customs is never required.
                requiresCustoms = false;
            }

            return requiresCustoms;
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
                    shipment.IParcel.Fields[IParcelShipmentFields.IParcelAccountID.FieldIndex],
                    shipment.IParcel.Fields[IParcelShipmentFields.IsDeliveryDutyPaid.FieldIndex],
                    shipment.Order.Fields[OrderFields.OrderTotal.FieldIndex],
                    shipment.Order.Fields[OrderFields.RollupItemCount.FieldIndex],
                    shipment.IParcel.Fields[IParcelShipmentFields.TrackByEmail.FieldIndex],
                    shipment.IParcel.Fields[IParcelShipmentFields.TrackBySMS.FieldIndex],
                }
            );

            // Grab all the fields for all the package in this shipment
            foreach (IParcelPackageEntity package in shipment.IParcel.Packages)
            {
                fields.Add(package.Fields[IParcelPackageFields.Weight.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.DimsWeight.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.DimsWidth.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.DimsHeight.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.DeclaredValue.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.InsuranceValue.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.Insurance.FieldIndex]);
                fields.Add(package.Fields[IParcelPackageFields.InsurancePennyOne.FieldIndex]);
            }

            return fields;
        }

        /// <summary>
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.IParcel != null)
            {
                shipment.IParcel.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.IParcelShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new IParcelShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }
    }
}
