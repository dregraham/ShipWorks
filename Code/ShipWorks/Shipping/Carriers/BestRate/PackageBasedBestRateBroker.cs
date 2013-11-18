using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Best rate broker that performs common functionality for carriers that have packages
    /// </summary>
    /// <typeparam name="TAccount">Type of carrier account</typeparam>
    /// <typeparam name="TPackage">Type of package</typeparam>
    public abstract class PackageBasedBestRateBroker<TAccount, TPackage> : BestRateBroker<TAccount>
        where TAccount : EntityBase2
        where TPackage : EntityBase2
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType">Shipment type that will be used</param>
        /// <param name="accountRepository">Repository that will be used for retrieving accounts</param>
        /// <param name="carrierDescription">Text description of the carrier that will be pre-pended to rate descriptions</param>
        protected PackageBasedBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<TAccount> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        {

        }

        /// <summary>
        /// Select a rate for a package based shipm
        /// </summary>
        /// <param name="shipment">Shipment that should be converted to a package based shipment</param>
        protected override void SelectRate(ShipmentEntity shipment)
        {
            if (ChildShipmentEntityState(shipment) == EntityState.New)
            {
                return;
            }

            IList<TPackage> packages = Packages(shipment);

            // Grab the original ups package so we can get it's UpsPackageID, as we'll need to set it on the
            // cloned package.  There's probably a better way, so need to check with Brian.
            TPackage selectedUpsPackageEntity = packages[0];
            long originalPackageId = PackageId(selectedUpsPackageEntity);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                packages.Where(x => !x.IsNew).ToList().ForEach(x => adapter.DeleteEntity(x));
            }
            packages.Clear();

            // Apply the default profile for the shipment
            ShipmentType.ConfigureNewShipment(shipment);

            packages = Packages(shipment);

            // Update the first package UpsPackgeID to be that of the original persisted package.  If this isn't 
            // done, we get an ORM exception.  There's probably a better way, so need to check with Brian.
            SetPackageId(packages[0], originalPackageId);
        }

        /// <summary>
        /// Gets the current entity state for the specified shipment's child
        /// </summary>
        protected abstract EntityState ChildShipmentEntityState(ShipmentEntity shipment);

        /// <summary>
        /// Gets a collection of packages from the specified shipment
        /// </summary>
        protected abstract IList<TPackage> Packages(ShipmentEntity shipment);

        /// <summary>
        /// Gets the id for the specified package
        /// </summary>
        protected abstract long PackageId(TPackage package);

        /// <summary>
        /// Sets the id on the specified package
        /// </summary>
        protected abstract void SetPackageId(TPackage package, long packageId);
    }
}
