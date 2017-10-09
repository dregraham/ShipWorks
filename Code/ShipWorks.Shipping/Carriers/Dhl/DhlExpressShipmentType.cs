using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;
using ShipWorks.Data.Model.Custom;
using System.Linq;
using System;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Editing;

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
        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.DhlExpress;

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

            dhlExpressShipmentEntity.DeliveredDutyPaid = false;
            dhlExpressShipmentEntity.NonMachinable = false;
            dhlExpressShipmentEntity.SaturdayDelivery = false;
            
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
                DeclaredValue = 0,
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
            DhlExpressAccountEntity account = DhlExpressAccountManager.GetAccount(dhlExpressShipmentEntity.DhlExpressAccountID);

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
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package))
                {
                    TotalWeight = package.Weight + package.DimsWeight
                };
            }

            throw new ArgumentException($"'{parcelIndex}' is out of range for the shipment.", "parcelIndex");
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
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages => true;

        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }
    }
}