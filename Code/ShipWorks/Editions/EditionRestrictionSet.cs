using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Stores;
using System.Collections.ObjectModel;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using System.Text.RegularExpressions;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Defines a policy of and can check for violations of edition restrictions
    /// </summary>
    public class EditionRestrictionSet
    {
        List<EditionRestriction> restrictions = new List<EditionRestriction>();

        /// <summary>
        /// Constructor for set of no restrictions
        /// </summary>
        public EditionRestrictionSet()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionRestrictionSet(List<EditionRestriction> restrictions)
        {
            this.restrictions = restrictions ?? new List<EditionRestriction>();
        }

        /// <summary>
        /// Check the restriction level of the given feature. 'data' is relative in context to the specified feature, and must be 
        /// of the type the feature expectes
        /// </summary>
        public EditionRestrictionIssue CheckRestriction(EditionFeature feature)
        {
            return CheckRestriction(feature, null);
        }

        /// <summary>
        /// Check the restriction level of the given feature. 'data' is relative in context to the specified feature, and must be 
        /// of the type the feature expectes
        /// </summary>
        public EditionRestrictionIssue CheckRestriction(EditionFeature feature, object data)
        {
            switch (feature)
            {
                case EditionFeature.ShipmentType:
                case EditionFeature.ShipmentTypeRegistration:
                case EditionFeature.ProcessShipment:
                case EditionFeature.PurchasePostage:
                case EditionFeature.RateDiscountMessaging:
                case EditionFeature.ShippingAccountConversion:
                    return CheckShipmentTypeRestriction(feature, (ShipmentTypeCode) data);

                case EditionFeature.ActionLimit:
                case EditionFeature.FilterLimit:
                    return CheckNumericLimitRestriction(feature, Convert.ToInt32(data) + 1);

                case EditionFeature.SelectionLimit:
                case EditionFeature.EndiciaAccountLimit:
                    return CheckNumericLimitRestriction(feature, Convert.ToInt32(data));

                case EditionFeature.SingleStore:
                    return CheckSingleStoreRestriction();

                case EditionFeature.UpsAccountLimit:
                    return CheckUpsAccountLimitRestriction(Convert.ToInt32(data));

                case EditionFeature.UpsAccountNumbers:
                    return CheckUpsAccountNumbersRestriction((string) data);

                case EditionFeature.EndiciaAccountNumber:
                    return CheckEndiciaAccountNumberRestriction((string) data);

                case EditionFeature.MyFilters:
                case EditionFeature.AddOrderCustomer:
                case EditionFeature.EndiciaScanForm:
                    return CheckExistanceRestriction(feature);

                case EditionFeature.PostalApoFpoPoboxOnly:
                    return CheckPostalApoFpoRestriction((ShipmentEntity) data);

                case EditionFeature.EndiciaInsurance:
                case EditionFeature.EndiciaDhl:
                case EditionFeature.UpsSurePost:
                case EditionFeature.EndiciaConsolidator:
                case EditionFeature.EndiciaScanBasedReturns:
                case EditionFeature.StampsAscendiaConsolidator:
                case EditionFeature.StampsDhlConsolidator:
                case EditionFeature.StampsGlobegisticsConsolidator:
                case EditionFeature.StampsIbcConsolidator:
                case EditionFeature.StampsRrDonnelleyConsolidator:
                    return CheckNonExistanceRestriction(feature);
            }

            throw new InvalidOperationException("Unhandled EditionFeature: " + feature);
        }

        /// <summary>
        /// Check for restrictions that just have to exist - no specific data to compare against
        /// </summary>
        private EditionRestrictionIssue CheckExistanceRestriction(EditionFeature feature)
        {
            var restriction = restrictions.FirstOrDefault(r => r.Feature == feature);

            return (restriction != null) ? 
                new EditionRestrictionIssue(restriction, null) : 
                EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Only returns a restriction if exists for every store
        /// </summary>
        private EditionRestrictionIssue CheckNonExistanceRestriction(EditionFeature feature)
        {
            int count = restrictions.Count(r => r.Feature == feature);

            if (count > 0 && count == StoreManager.GetAllStores().Count)
            {
                return new EditionRestrictionIssue(restrictions.First(r => r.Feature == feature), null);
            }
            else
            {
                return EditionRestrictionIssue.None;
            }
        }

        /// <summary>
        /// Check against restrictions that have a numeric limit
        /// </summary>
        private EditionRestrictionIssue CheckNumericLimitRestriction(EditionFeature feature, int current)
        {
            // Get the most restrictive restriction
            var restriction = restrictions.Where(r => r.Feature == feature).OrderBy(r => (int) r.Data).FirstOrDefault();

            if (restriction != null && current > (int) restriction.Data)
            {
                return new EditionRestrictionIssue(restriction, current);
            }
            else
            {
                return EditionRestrictionIssue.None;
            }
        }

        /// <summary>
        /// Check restriction based on shipment type
        /// </summary>
        private EditionRestrictionIssue CheckShipmentTypeRestriction(EditionFeature feature, ShipmentTypeCode typeCode)
        {
            // Just take the first one that restricts it
            var restriction = restrictions.FirstOrDefault(r => r.Feature == feature && (ShipmentTypeCode)r.Data == typeCode);

            return (restriction != null) ? 
                new EditionRestrictionIssue(restriction, typeCode) :
                EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Check restriction against the given UPS account
        /// </summary>
        private EditionRestrictionIssue CheckUpsAccountNumbersRestriction(string account)
        {
            List<string> allowedAccounts = GetAllowedUpsAccounts();

            if (allowedAccounts != null && !allowedAccounts.Contains(account.ToLower()))
            {
                return new EditionRestrictionIssue(restrictions.First(r => r.Feature == EditionFeature.UpsAccountNumbers), account);
            }

            return EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Check against UPS account quantity
        /// </summary>
        private EditionRestrictionIssue CheckUpsAccountLimitRestriction(int current)
        {
            int? maxAllowed = null;

            List<string> allowedAccounts = GetAllowedUpsAccounts();
            if (allowedAccounts != null)
            {
                maxAllowed = allowedAccounts.Count;
            }
            else
            {
                EditionRestriction countRestriction = restrictions.FirstOrDefault(r => r.Feature == EditionFeature.UpsAccountLimit);
                if (countRestriction != null)
                {
                    maxAllowed = (int) countRestriction.Data;
                }
            }

            if (maxAllowed != null && current > maxAllowed)
            {
                return new EditionRestrictionIssue(restrictions.Where(r => r.Feature == EditionFeature.UpsAccountLimit).First(), current);
            }

            return EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Get the distinct list of allowed UPS accounts
        /// </summary>
        private List<string> GetAllowedUpsAccounts()
        {
            var upsRestrictions = restrictions.Where(r => r.Feature == EditionFeature.UpsAccountNumbers);

            if (upsRestrictions.Any())
            {
                return upsRestrictions.SelectMany(r => (List<string>) r.Data).Select(a => a.ToLower()).Distinct().ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Check to see if the given Encicia account number is allowed
        /// </summary>
        private EditionRestrictionIssue CheckEndiciaAccountNumberRestriction(string account)
        {
            var restriction = restrictions.Where(r => r.Feature == EditionFeature.EndiciaAccountNumber).SingleOrDefault();

            if (restriction != null && (string) restriction.Data != account)
            {
                return new EditionRestrictionIssue(restriction, account);
            }

            return EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Check to see if postal shipments are restricted to being only APO/FPO/POBox
        /// </summary>
        private EditionRestrictionIssue CheckPostalApoFpoRestriction(ShipmentEntity shipment)
        {
            var restriction = restrictions.FirstOrDefault(r => r.Feature == EditionFeature.PostalApoFpoPoboxOnly);

            // No such restriction
            if (restriction == null)
            {
                return EditionRestrictionIssue.None;
            }

            // Not a postal shipment to even check
            if (!PostalUtility.IsPostalShipmentType((ShipmentTypeCode) shipment.ShipmentType))
            {
                return EditionRestrictionIssue.None;
            }

            // APO/FPO is fine
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
            {
                return EditionRestrictionIssue.None;
            }

            // Check for PO box
            if (Regex.Match(shipment.ShipStreet1 + " " + shipment.ShipStreet2 + " " + shipment.ShipStreet3, "P.{0,2}O.{0,2}[ ]?Box", RegexOptions.IgnoreCase).Success)
            {
                return EditionRestrictionIssue.None;
            }

            return new EditionRestrictionIssue(restriction, null);
        }

        /// <summary>
        /// Check for a Single Store restriction
        /// </summary>
        private EditionRestrictionIssue CheckSingleStoreRestriction()
        {
            // Just use the first violator
            var restriction = restrictions.FirstOrDefault(r => r.Feature == EditionFeature.SingleStore);

            if (restriction != null)
            {
                int storeCount = StoreManager.GetAllStores().Count;

                if (storeCount > 0)
                {
                    return new EditionRestrictionIssue(restriction, storeCount);
                }
            }

            return EditionRestrictionIssue.None;
        }

        /// <summary>
        /// Requred since overriding Equals
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();

            foreach (var restriction in restrictions)
            {
                hashCode ^= restriction.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Override for equality of actual restrictions instead of reference
        /// </summary>
        public override bool Equals(object obj)
        {
            EditionRestrictionSet other = obj as EditionRestrictionSet;

            if (other == null)
            {
                return false;
            }

            if (other.restrictions.Count != this.restrictions.Count)
            {
                return false;
            }

            // Go through each restriction we have - and make sure it exists in the other set
            foreach (EditionRestriction restriction in this.restrictions)
            {
                if (!other.HasEquivalentRestriction(restriction))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates if our restriction set has a restriction equivalent to the given restriction
        /// </summary>
        private bool HasEquivalentRestriction(EditionRestriction toCheck)
        {
            foreach (EditionRestriction restriction in restrictions)
            {
                if (restriction.Feature == toCheck.Feature &&
                    restriction.Level == toCheck.Level)
                {
                    if (restriction.Feature == EditionFeature.UpsAccountNumbers)
                    {
                        if (((List<string>) restriction.Data).SequenceEqual((List<string>) toCheck.Data))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (object.Equals(restriction.Data, toCheck.Data))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
