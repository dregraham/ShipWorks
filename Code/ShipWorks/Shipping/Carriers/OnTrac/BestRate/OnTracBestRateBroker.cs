using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;

namespace ShipWorks.Shipping.Carriers.OnTrac.BestRate
{
    class OnTracBestRateBroker : BestRateBroker<OnTracAccountEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracBestRateBroker"/> class.
        /// </summary>
        public OnTracBestRateBroker()
            : this(new OnTracShipmentType(), new OnTracAccountRepository())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        private OnTracBestRateBroker(OnTracShipmentType shipmentType, ICarrierAccountRepository<OnTracAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "OnTrac")
        {

        }

        /// <summary>
        /// Applies OnTrac specific data to the specified shipment
        /// </summary>
        /// <param name="currentShipment">Shipment that will be modified</param>
        /// <param name="originalShipment">Shipment that contains original data for copying</param>
        /// <param name="account">Account that will be attached to the shipment</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, OnTracAccountEntity account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.OnTrac.DimsHeight = originalShipment.BestRate.DimsHeight;
            currentShipment.OnTrac.DimsWidth = originalShipment.BestRate.DimsWidth;
            currentShipment.OnTrac.DimsLength = originalShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.OnTrac.DimsWeight = originalShipment.ContentWeight;
            currentShipment.OnTrac.DimsAddWeight = false;
            currentShipment.OnTrac.Service = (int)OnTracServiceType.Ground;
            currentShipment.OnTrac.OnTracAccountID = account.OnTracAccountID;
        }

        /// <summary>
        /// Checks whether the service type specified in the rate should be excluded from best rate consideration
        /// </summary>
        /// <param name="tag">OnTrac service type from the rate tag</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            return false;
        }

        /// <summary>
        /// Creates and attaches a new instance of a OnTracShipment to the specified shipment
        /// </summary>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.OnTrac = new OnTracShipmentEntity();
        }

        /// <summary>
        /// Sets the service type on the OnTrac shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.OnTrac.Service = (int)tag;
        }
    }
}
