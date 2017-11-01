﻿using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;
using ShipWorks.Data.Model.Custom;
using System.Linq;
using System;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Data;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System.Drawing.Imaging;
using ShipWorks.Shipping.Tracking;
using ShipWorks.ApplicationCore.Logging;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// DHL Express implementation of shipment type
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.ShipmentType" />
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ShipmentType), ShipmentTypeCode.DhlExpress, SingleInstance = true)]
    public class DhlExpressShipmentType : ShipmentType
    {
        private readonly ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository"></param>
        public DhlExpressShipmentType(ICarrierAccountRepository<ShipEngineAccountEntity, IShipEngineAccountEntity> accountRepository, IShipEngineWebClient shipEngineWebClient, IShipEngineTrackingResultFactory trackingResultFactory)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.DhlExpress;

        /// <summary>
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages => true;

        /// <summary>
        /// Supports using an origin address from a shipping account
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.DhlExpress == null)
            {
                shipment.DhlExpress = new DhlExpressShipmentEntity(shipment.ShipmentID);
            }

            DhlExpressShipmentEntity dhlExpressShipmentEntity = shipment.DhlExpress;
            dhlExpressShipmentEntity.Service = (int) DhlExpressServiceType.ExpressWorldWide;
            dhlExpressShipmentEntity.DeliveredDutyPaid = false;
            dhlExpressShipmentEntity.NonMachinable = false;
            dhlExpressShipmentEntity.SaturdayDelivery = false;
            dhlExpressShipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;
            dhlExpressShipmentEntity.Contents = (int)ShipEngineContentsType.Merchandise;
            dhlExpressShipmentEntity.NonDelivery = (int)ShipEngineNonDeliveryType.ReturnToSender;
            dhlExpressShipmentEntity.ShipEngineAccountID = 0;
            dhlExpressShipmentEntity.ShipEngineLabelID = string.Empty;

            DhlExpressPackageEntity package = CreateDefaultPackage();

            dhlExpressShipmentEntity.Packages.Add(package);
            shipment.DhlExpress.Packages.RemovedEntitiesTracker = new DhlExpressPackageCollection();

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Create a new package entity that has default values
        /// </summary>
        public static DhlExpressPackageEntity CreateDefaultPackage()
        {
            return new DhlExpressPackageEntity
            {
                Weight = 0,
                DimsProfileID = 0,
                DimsLength = 0,
                DimsWidth = 0,
                DimsHeight = 0,
                DimsWeight = 0,
                DimsAddWeight = true,
                Insurance = false,
                InsuranceValue = 0,
                TrackingNumber = string.Empty,
            };
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
            if (shipment.DhlExpress.Packages.RemovedEntitiesTracker == null)
            {
                shipment.DhlExpress.Packages.RemovedEntitiesTracker = new DhlExpressPackageCollection();
            }

            base.SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);

            while (shipment.DhlExpress.Packages.Count < knowledgebaseEntry.Packages.Count())
            {
                DhlExpressPackageEntity package = CreateDefaultPackage();
                shipment.DhlExpress.Packages.Add(package);
            }

            while (shipment.DhlExpress.Packages.Count > knowledgebaseEntry.Packages.Count())
            {
                // Remove the last package until the packages counts match
                shipment.DhlExpress.Packages.RemoveAt(shipment.DhlExpress.Packages.Count - 1);
            }
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.DhlExpress == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            if (!shipment.DhlExpress.Packages.Any())
            {
                throw new DhlExpressException("There must be at least one package to create the Dhl Express package adapter.");
            }

            // Return an adapter per package
            List<IPackageAdapter> adapters = new List<IPackageAdapter>();
            for (int index = 0; index < shipment.DhlExpress.Packages.Count; index++)
            {
                DhlExpressPackageEntity packageEntity = shipment.DhlExpress.Packages[index];
                adapters.Add(new DhlExpressPackageAdapter(shipment, packageEntity, index + 1));
            }

            return adapters;
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            RedistributeContentWeight(shipment);
            
            shipment.Insurance = shipment.DhlExpress.Packages.Any(p => p.Insurance);
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            shipment.RequestedLabelFormat = shipment.DhlExpress.RequestedLabelFormat;
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.  Returns true if weight had to be redistributed.
        /// </summary>
        public static bool RedistributeContentWeight(ShipmentEntity shipment)
        {
            // Determine what our content weight should be
            double contentWeight = shipment.DhlExpress.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (!contentWeight.IsEquivalentTo(shipment.ContentWeight))
            {
                foreach (DhlExpressPackageEntity package in shipment.DhlExpress.Packages)
                {
                    package.Weight = shipment.ContentWeight / shipment.DhlExpress.Packages.Count;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update the total weight of the shipment based on the individual package weights
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            double contentWeight = 0;
            double totalWeight = 0;

            foreach (DhlExpressPackageEntity package in shipment.DhlExpress.Packages)
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
        /// Get the shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            DhlExpressShipmentEntity dhlExpressShipmentEntity = shipment.DhlExpress;
            ShipEngineAccountEntity account = accountRepository.GetAccount(dhlExpressShipmentEntity.ShipEngineAccountID);
            
            commonDetail.OriginAccount = (account == null) ? "" : account.Description;
            commonDetail.ServiceType = dhlExpressShipmentEntity.Service;

            // i-Parcel doesn't have a packaging type concept, so default to 0
            commonDetail.PackagingType = 0;
            commonDetail.PackageLength = dhlExpressShipmentEntity.Packages[0].DimsLength;
            commonDetail.PackageWidth = dhlExpressShipmentEntity.Packages[0].DimsWidth;
            commonDetail.PackageHeight = dhlExpressShipmentEntity.Packages[0].DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the DhlExpress data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "DhlExpress", typeof(DhlExpressShipmentEntity), refreshIfPresent);

            DhlExpressShipmentEntity dhlExpressShipmentEntity = shipment.DhlExpress;

            if (refreshIfPresent)
            {
                dhlExpressShipmentEntity.Packages.Clear();
            }

            // If there are no packages load them now
            if (dhlExpressShipmentEntity.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(dhlExpressShipmentEntity.Packages,
                                                  new RelationPredicateBucket(DhlExpressPackageFields.ShipmentID == shipment.ShipmentID));

                    dhlExpressShipmentEntity.Packages.Sort((int)DhlExpressPackageFieldIndex.DhlExpressPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                dhlExpressShipmentEntity.Packages.RemovedEntitiesTracker = new DhlExpressPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (dhlExpressShipmentEntity.Packages.Count == 0)
            {
                // This was changed to an exception instead of creating the package when the creation was moved to ConfigureNewShipment
                throw new NotFoundException("Primary package not found.");
            }
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription((DhlExpressServiceType)shipment.DhlExpress.Service);
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

            return shipment.DhlExpress.Packages.Count;
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

            if (parcelIndex >= 0 && parcelIndex < shipment.DhlExpress.Packages.Count)
            {
                DhlExpressPackageEntity package = shipment.DhlExpress.Packages[parcelIndex];

                return new ShipmentParcel(shipment, package.DhlExpressPackageID, package.TrackingNumber,
                    new DhlExpressInsuranceChoice(shipment, package),
                    new DimensionsAdapter(package))
                {
                    TotalWeight = package.Weight + package.DimsWeight
                };
            }

            throw new ArgumentException($"'{parcelIndex}' is out of range for the shipment.", "parcelIndex");
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            bool existed = profile.DhlExpress != null;

            ShipmentTypeDataService.LoadProfileData(profile, "DhlExpress", typeof(DhlExpressProfileEntity), refreshIfPresent);

            DhlExpressProfileEntity dhlExpressProfileEntityParcel = profile.DhlExpress;

            // If this is the first time loading it, or we are supposed to refresh, do it now
            if (!existed || refreshIfPresent)
            {
                dhlExpressProfileEntityParcel.Packages.Clear();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(dhlExpressProfileEntityParcel.Packages,
                                                  new RelationPredicateBucket(DhlExpressProfilePackageFields.ShippingProfileID == profile.ShippingProfileID));

                    dhlExpressProfileEntityParcel.Packages.Sort((int)DhlExpressProfilePackageFieldIndex.DhlExpressProfilePackageID, ListSortDirection.Ascending);
                }
            }
        }

        /// <summary>
        /// Save carrier specific profile data to the database.  Return true if anything was dirty and saved, or was deleted.
        /// </summary>
        public override bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            bool changes = base.SaveProfileData(profile, adapter);

            // First delete out anything that needs deleted
            foreach (DhlExpressProfilePackageEntity package in profile.DhlExpress.Packages.ToList())
            {
                // If its new but deleted, just get rid of it
                if (package.Fields.State == EntityState.Deleted)
                {
                    if (package.IsNew)
                    {
                        profile.DhlExpress.Packages.Remove(package);
                    }

                    // If its deleted, delete it
                    else
                    {
                        package.Fields.State = EntityState.Fetched;
                        profile.DhlExpress.Packages.Remove(package);

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
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = accountRepository.AccountsReadOnly.Select(x => x.ShipEngineAccountID).FirstOrDefault();

            profile.DhlExpress.ShipEngineAccountID = shipperID;
            profile.OriginID = (int)ShipmentOriginSource.Account;

            profile.DhlExpress.Service = (int)DhlExpressServiceType.ExpressWorldWide;
            profile.DhlExpress.DeliveryDutyPaid = false;
            profile.DhlExpress.NonMachinable = false;
            profile.DhlExpress.SaturdayDelivery = false;
            profile.DhlExpress.Contents = (int)ShipEngineContentsType.Merchandise;
            profile.DhlExpress.NonDelivery = (int)ShipEngineNonDeliveryType.ReturnToSender;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, IShippingProfileEntity profile)
        {
            DhlExpressShipmentEntity dhlShipment = shipment.DhlExpress;
            
            bool changedPackageWeights = ApplyDhlExpressPackageProfile(dhlShipment, profile);
            int profilePackageCount = profile.DhlExpress.Packages.Count();

            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (DhlExpressPackageEntity package in dhlShipment.Packages.Skip(profilePackageCount).ToList())
                {
                    if (!package.Weight.IsEquivalentTo(0))
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    dhlShipment.Packages.Remove(package);

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

            base.ApplyProfile(shipment, profile);

            ApplyDhlExpressProfile(dhlShipment, profile);

            if (changedPackageWeights)
            {
                UpdateTotalWeight(shipment);
            }

            UpdateDynamicShipmentData(shipment);
        }

        /// <summary>
        /// Apply the dhl express package profile
        /// </summary>
        /// <returns>bool if the weight of the package has changed</returns>
        private bool ApplyDhlExpressPackageProfile(DhlExpressShipmentEntity dhlShipment, IShippingProfileEntity profile)
        {
            bool changedPackageWeights = false;

            int profilePackageCount = profile.DhlExpress.Packages.Count();

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IDhlExpressProfilePackageEntity packageProfile = profile.DhlExpress.Packages.ElementAt(i);

                DhlExpressPackageEntity package;

                // Get the existing, or create a new package
                if (dhlShipment.Packages.Count > i)
                {
                    package = dhlShipment.Packages[i];
                }
                else
                {
                    package = CreateDefaultPackage();
                    dhlShipment.Packages.Add(package);
                }

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, DhlExpressPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, DhlExpressPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, DhlExpressPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, DhlExpressPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, DhlExpressPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, DhlExpressPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, DhlExpressPackageFields.DimsAddWeight);
                }
            }

            return changedPackageWeights;
        }

        /// <summary>
        /// Apply the DHL Express profile
        /// </summary>
        private void ApplyDhlExpressProfile(DhlExpressShipmentEntity dhlShipment, IShippingProfileEntity profile)
        {
            IDhlExpressProfileEntity source = profile.DhlExpress;
            
            long? accountID = (source.ShipEngineAccountID == 0 && accountRepository.Accounts.Any())
                                  ? (long?)accountRepository.Accounts.First().ShipEngineAccountID
                                  : source.ShipEngineAccountID;

            ShippingProfileUtility.ApplyProfileValue(accountID, dhlShipment, DhlExpressShipmentFields.ShipEngineAccountID);
            ShippingProfileUtility.ApplyProfileValue(source.Service, dhlShipment, DhlExpressShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(source.DeliveryDutyPaid, dhlShipment, DhlExpressShipmentFields.DeliveredDutyPaid);
            ShippingProfileUtility.ApplyProfileValue(source.NonMachinable, dhlShipment, DhlExpressShipmentFields.NonMachinable);
            ShippingProfileUtility.ApplyProfileValue(source.SaturdayDelivery, dhlShipment, DhlExpressShipmentFields.SaturdayDelivery);
            ShippingProfileUtility.ApplyProfileValue(source.NonDelivery, dhlShipment, DhlExpressShipmentFields.NonDelivery);
            ShippingProfileUtility.ApplyProfileValue(source.Contents, dhlShipment, DhlExpressShipmentFields.Contents);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override bool IsCustomsRequiredByShipment(ShipmentEntity shipment) => true;

        /// <summary>
        /// Gets a ShippingBroker
        /// </summary>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new DhlExpressBestRateBroker(this, new DhlExpressAccountRepository(new ShipEngineAccountRepository()));
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.DhlExpress != null)
            {
                shipment.DhlExpress.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Create the XML input to the XSL engine
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            Lazy<List<TemplateLabelData>> labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement(
                "Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));
        }

        /// <summary>
        /// Track the shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                TrackingInformation trackingInfo = Task.Run(() =>
                {
                    return shipEngineWebClient.Track(shipment.DhlExpress.ShipEngineLabelID, ApiLogSource.DHLExpress);
                }).Result;

                return trackingResultFactory.Create(trackingInfo);
            }
            catch (Exception)
            {
                return new TrackingResult { Summary = $"<a href='http://www.dhl.com/en/express/tracking.html?AWB={shipment.TrackingNumber}&brand=DHL' style='color:blue; background-color:white'>Click here to see tracking information</a>" };
            }
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not been excluded).
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {           
            return EnumHelper.GetEnumList<DhlExpressServiceType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipmentFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentFactory, nameof(shipmentFactory));

            return DataResourceManager.GetConsumerResourceReferences(shipmentFactory().ShipmentID)
                .Where(x => x.Label.StartsWith("LabelPrimary") || x.Label.StartsWith("LabelPart"))
                .Select(x => new TemplateLabelData(null, "Label", x.Label.StartsWith("LabelPrimary") ?
                    TemplateLabelCategory.Primary : TemplateLabelCategory.Supplemental, x))
                .ToList();
        }
    }
}