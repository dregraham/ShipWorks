using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Base class for postal reseller brokers, like Stamps and Endicia
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PostalResellerBestRateBroker<T> : BestRateBroker<T> where T : EntityBase2
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected PostalResellerBestRateBroker(ShipmentType shipmentType, ICarrierAccountRepository<T> accountRepository, string carrierDescription) :
            base(shipmentType, accountRepository, carrierDescription)
        {

        }

        /// <summary>
        /// Gets a list of Postal rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <returns>List of RateResults</returns>
        /// <remarks>This is overridden because Postal rates are returned in a pseudo-nested way and they need </remarks>
        protected override IEnumerable<RateResult> GetRates(ShipmentEntity shipment)
        {
            var rates = base.GetRates(shipment).ToList();
            MergeDescriptionsWithNonSelectableRates(rates);
            return rates;
        }

        /// <summary>
        /// Merge rate descriptions meant as headers with actual rate descriptions
        /// </summary>
        /// <param name="rates">Collection of rates to update</param>
        /// <remarks>It is important that these rates are in the same order that they are returned from
        /// the shipment type's GetRates method or the merging could be incorrect</remarks>
        private static void MergeDescriptionsWithNonSelectableRates(IEnumerable<RateResult> rates)
        {
            Regex beginsWithSpaces = new Regex("^[ ]+");
            
            RateResult lastNonSelectable = null;

            //TODO: There is a bug here where if the next "top-level" rate is selectable, it will have its description
            // Added to the previous non-selectable rate
            foreach (var rate in rates)
            {
                if (rate.Selectable)
                {
                    if (beginsWithSpaces.IsMatch(rate.Description) && lastNonSelectable != null)
                    {
                        rate.Description = lastNonSelectable.Description + beginsWithSpaces.Replace(rate.Description, " ");
                        rate.Days = lastNonSelectable.Days;
                    }
                }
                else
                {
                    lastNonSelectable = rate;
                }
            }
        }

        /// <summary>
        /// Returns whether the specified service type should be excluded from consideration
        /// </summary>
        /// <param name="tag">Service type to check</param>
        /// <returns></returns>
        protected override bool IsExcludedServiceType(object tag)
        {
            PostalServiceType serviceType = ((PostalRateSelection) tag).ServiceType;

            return serviceType == PostalServiceType.MediaMail ||
                   serviceType == PostalServiceType.LibraryMail ||
                   serviceType == PostalServiceType.BoundPrintedMatter ||
                   serviceType == PostalServiceType.DhlBpmExpedited ||
                   serviceType == PostalServiceType.DhlBpmStandard;
        }

        /// <summary>
        /// Updates data on the postal child shipment that is required for checking best rate
        /// </summary>
        /// <param name="currentShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, T account)
        {
            currentShipment.Postal.DimsHeight = currentShipment.BestRate.DimsHeight;
            currentShipment.Postal.DimsWidth = currentShipment.BestRate.DimsWidth;
            currentShipment.Postal.DimsLength = currentShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.Postal.DimsWeight = originalShipment.ContentWeight;
            currentShipment.Postal.DimsAddWeight = false;
            currentShipment.Postal.PackagingType = (int)PostalPackagingType.Package;
            currentShipment.Postal.Service = (int)PostalServiceType.PriorityMail;

            UpdateChildAccountId(currentShipment.Postal, account);
        }

        /// <summary>
        /// Convert the best rate shipment into the specified postal reseller shipment
        /// </summary>
        /// <param name="rateShipment">Postal shipment on which to set reseller shipment data</param>
        /// <param name="selectedShipment">Best rate shipment that is being converted</param>
        protected abstract void SelectChildShipment(PostalShipmentEntity rateShipment, ShipmentEntity selectedShipment);

        /// <summary>
        /// Configures a postal reseller shipment for use in the get rates method
        /// </summary>
        /// <param name="shipment">Test shipment that will be used to get rates</param>
        protected override void CreateShipmentChild(ShipmentEntity shipment)
        {
            shipment.Postal = new PostalShipmentEntity();
        }

        /// <summary>
        /// Gets the service type from the rate tag
        /// </summary>
        /// <param name="tag">Service type specified in the rate tag</param>
        protected override int GetServiceTypeFromTag(object tag)
        {
            return (int)((PostalRateSelection) tag).ServiceType;
        }

        /// <summary>
        /// Sets the service type on the Postal shipment from the value in the rate tag
        /// </summary>
        /// <param name="shipment">Shipment that will be updated</param>
        /// <param name="tag">Rate tag that represents the service type</param>
        protected override void SetServiceTypeFromTag(ShipmentEntity shipment, object tag)
        {
            shipment.Postal.Service = GetServiceTypeFromTag(tag);
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected abstract void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, T account);
    }
}
