using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;

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
            
            //DhlExpressPackageEntity package = CreateDefaultPackage();

            //dhlExpressShipmentEntity.Packages.Add(package);
            //shipment.IParcel.Packages.RemovedEntitiesTracker = new IParcelPackageCollection();

            //// Weight of the first package equals the total shipment content weight
            //package.Weight = shipment.ContentWeight;

            //base.ConfigureNewShipment(shipment);
        }

        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            throw new System.NotImplementedException();
        }

        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }

        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            throw new System.NotImplementedException();
        }

        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }
    }
}