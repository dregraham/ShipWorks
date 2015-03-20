using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing;
using System;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Properties;

namespace ShipWorks.Shipping.Carriers.Postal.BestRate
{
    /// <summary>
    /// Base class for postal reseller brokers, like Usps and Endicia
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
        /// Get best rates for the specified shipment
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            brokerExceptions.Add(new BrokerException(new ShippingException("Flat rate and regional boxes were not checked for best rates."), BrokerExceptionSeverityLevel.Information, ShipmentType));

            // Postal services do not ship weights over 70 lbs.  Return no rates if this is the case.
            if (shipment.TotalWeight > 70)
            {
                return new RateGroup(new List<RateResult>());
            }

            return base.GetBestRates(shipment, brokerExceptions);
        }

        /// <summary>
        /// Gets a list of Postal rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <returns>List of RateResults</returns>
        /// <remarks>This is overridden because Postal rates are returned in a pseudo-nested way and they need </remarks>
        protected override RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rates = base.GetRates(shipment);
            
            rates = rates.CopyWithRates(MergeDescriptionsWithNonSelectableRates(rates.Rates));
            // If a postal counter provider, show USPS logo, otherwise show appropriate logo such as endicia:
            rates.Rates.ForEach(f => UseProperUspsLogo(f));

            return rates;
        }

        /// <summary>
        /// Masks the usps logo.
        /// </summary>
        /// <param name="rate">The rate.</param>
        /// <returns></returns>
        private static void UseProperUspsLogo(RateResult rate)
        {
            if (ShipmentTypeManager.IsPostal(rate.ShipmentType))
            {
                rate.ProviderLogo = rate.IsCounterRate ? ShippingIcons.usps : EnumHelper.GetImage(rate.ShipmentType);
            }
        }

        /// <summary>
        /// Merge rate descriptions meant as headers with actual rate descriptions
        /// </summary>
        /// <param name="rates">Collection of rates to update</param>
        /// <remarks>It is important that these rates are in the same order that they are returned from
        /// the shipment type's GetRates method or the merging could be incorrect</remarks>
        private List<RateResult>  MergeDescriptionsWithNonSelectableRates(IEnumerable<RateResult> rates)
        {
            Regex beginsWithSpaces = new Regex("^[ ]+");
            Regex removeDeliveryConfirmation = new Regex(@" Delivery Confirmation \(\$\d*\.\d\d\)");

            RateResult lastNonSelectable = null;

            List<RateResult> RatesToReturn = new List<RateResult>();

            foreach (RateResult originalRate in rates)
            {
                RateResult rate = originalRate.Copy();
                RatesToReturn.Add(rate);

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
                
                rate.Description = removeDeliveryConfirmation.Replace(rate.Description, string.Empty);
            }

            return RatesToReturn;
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
                   serviceType == PostalServiceType.DhlBpmGround;
        }

        /// <summary>
        /// Updates data on the postal child shipment that is required for checking best rate
        /// </summary>
        /// <param name="currentShipment">Shipment that we'll be working with</param>
        /// <param name="originalShipment">The original shipment from which data can be copied.</param>
        /// <param name="account">The Account Entity for this shipment.</param>
        protected override void UpdateChildShipmentSettings(ShipmentEntity currentShipment, ShipmentEntity originalShipment, T account)
        {
            base.UpdateChildShipmentSettings(currentShipment, originalShipment, account);

            currentShipment.Postal.DimsHeight = currentShipment.BestRate.DimsHeight;
            currentShipment.Postal.DimsWidth = currentShipment.BestRate.DimsWidth;
            currentShipment.Postal.DimsLength = currentShipment.BestRate.DimsLength;

            // ConfigureNewShipment sets these fields, but we need to make sure they're what we expect
            currentShipment.Postal.DimsWeight = originalShipment.BestRate.DimsWeight;
            currentShipment.Postal.DimsAddWeight = originalShipment.BestRate.DimsAddWeight;
            currentShipment.Postal.PackagingType = (int)PostalPackagingType.Package;
            currentShipment.Postal.Service = (int)PostalServiceType.PriorityMail;
            currentShipment.Postal.InsuranceValue = originalShipment.BestRate.InsuranceValue;            

            UpdateChildAccountId(currentShipment.Postal, account);
        }

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
        /// Gets the result key for a given rate
        /// </summary>
        /// <param name="rate">Rate result for which to create a result key</param>
        /// <returns>Concatenation of the carrier description and the original rate tag</returns>
        protected override string GetResultKey(RateResult rate)
        {
            // Account for the rate being a previously cached rate where the tag is already a best rate tag; 
            // we need to pass the original tag that is a postal service type
            object originalTag = rate.OriginalTag;
            return "Postal" + EnumHelper.GetDescription((PostalServiceType)GetServiceTypeFromTag(originalTag));
        }

        /// <summary>
        /// Updates the account id on the postal reseller shipment
        /// </summary>
        /// <param name="postalShipmentEntity">Postal shipment on which the account id should be set</param>
        /// <param name="account">Account that should be used for this shipment</param>
        protected abstract void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, T account);
    }
}
