using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Extensions to the shipment class
    /// </summary>
    public static class AdjustedAddressExtensions
    {
        private static readonly Lazy<IDictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>>> adjustmentMethods =
            new Lazy<IDictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>>>(LoadAdjustmentMethods);

        /// <summary>
        /// Get the ship country code that has been adjusted according to carrier specific rules
        /// </summary>
        public static string AdjustedShipCountryCode(this ShipmentEntity shipment)
        {
            return AdjustedCountryCode(shipment, "Ship");
        }

        /// <summary>
        /// Get the origin country code that has been adjusted according to carrier specific rules
        /// </summary>
        public static string AdjustedOriginCountryCode(this ShipmentEntity shipment)
        {
            return AdjustedCountryCode(shipment, "Origin");
        }

        /// <summary>
        /// Get the specified country code that has been adjusted according to carrier specific rules
        /// </summary>
        public static string AdjustedCountryCode(this ShipmentEntity shipment, string addressPrefix)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, "shipment");

            return new PersonAdapter(shipment, addressPrefix)
                .AdjustedCountryCode((ShipmentTypeCode)shipment.ShipmentType);
        }

        /// <summary>
        /// Gets the country code that has been adjusted according to carrier specific rules
        /// </summary>
        public static string AdjustedCountryCode(this IAddressAdapter person, ShipmentTypeCode shipmentType)
        {
            MethodConditions.EnsureArgumentIsNotNull(person, "person");

            IAddressAdapter copiedPerson = CreateAddressCopy(person);

            copiedPerson.CountryCode = copiedPerson.CountryCode.ToUpperInvariant();
            copiedPerson.StateProvCode = copiedPerson.StateProvCode.ToUpperInvariant();

            IEnumerable<Func<IAddressAdapter, string>> methods;
            return adjustmentMethods.Value.TryGetValue(shipmentType, out methods) ? 
                ApplyAdjustments(methods, copiedPerson) : 
                copiedPerson.CountryCode;
        }

        /// <summary>
        /// Apply the adjustment methods to the person adapter
        /// </summary>
        private static string ApplyAdjustments(IEnumerable<Func<IAddressAdapter, string>> methods, IAddressAdapter person)
        {
            foreach (Func<IAddressAdapter, string> method in methods)
            {
                person.CountryCode = method(person);
            }

            return person.CountryCode;
        }

        /// <summary>
        /// Replace country name with country code
        /// </summary>
        private static string ReplaceCountryNameWithCountryCode(IAddressAdapter person)
        {
            return Geography.GetCountryCode(person.CountryCode);
        }

        /// <summary>
        /// Replace UK with GB
        /// </summary>
        private static string ReplaceUnitedKingdomWithGreatBritain(IAddressAdapter person)
        {
            return person.CountryCode == "UK" ? "GB" : person.CountryCode;
        }

        /// <summary>
        /// Replace US territory country code with US
        /// </summary>
        private static string ReplaceInternationalTerritoryCountryCodeWithUnitedStates(IAddressAdapter person)
        {
            return person.IsUSInternationalTerritory() ? "US" : person.CountryCode;
        }

        /// <summary>
        /// Replace US with US territory country code
        /// </summary>
        private static string ReplaceUnitedStatesWithInternationalTerritoryCountryCode(IAddressAdapter person)
        {
            return person.CountryCode == "US" && person.IsUSInternationalTerritory() ? 
                person.StateProvCode : person.CountryCode;
        }

        /// <summary>
        /// Populate the adjustment methods
        /// </summary>
        private static IDictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>> LoadAdjustmentMethods()
        {
            Dictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>> adjustments =
                new Dictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>>();

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.UpsOnLineTools, ShipmentTypeCode.UpsWorldShip, ShipmentTypeCode.FedEx },
                ReplaceUnitedKingdomWithGreatBritain, ReplaceUnitedStatesWithInternationalTerritoryCountryCode);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.Usps, ShipmentTypeCode.Express1Usps, ShipmentTypeCode.None },
                ReplaceUnitedKingdomWithGreatBritain, ReplaceInternationalTerritoryCountryCodeWithUnitedStates);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.Endicia, ShipmentTypeCode.Express1Endicia },
                ReplaceCountryNameWithCountryCode, ReplaceInternationalTerritoryCountryCodeWithUnitedStates, ReplaceUnitedKingdomWithGreatBritain);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.iParcel },
                ReplaceUnitedKingdomWithGreatBritain);

            return adjustments;
        }

        /// <summary>
        /// Add an adjustments for the specified shipment types
        /// </summary>
        private static void AddAdjustment(IDictionary<ShipmentTypeCode, IEnumerable<Func<IAddressAdapter, string>>> adjustments,
            IEnumerable<ShipmentTypeCode> shipmentTypes, params Func<IAddressAdapter, string>[] methods)
        {
            foreach (ShipmentTypeCode shipmentType in shipmentTypes)
            {
                adjustments.Add(shipmentType, methods);
            }
        }

        /// <summary>
        /// Create a copy of an address
        /// </summary>
        private static IAddressAdapter CreateAddressCopy(IAddressAdapter person)
        {
            return new AddressAdapter
            {
                Street1 = person.Street1,
                Street2 = person.Street2,
                Street3 = person.Street3,
                City = person.City,
                StateProvCode = person.StateProvCode,
                CountryCode = person.CountryCode,
                PostalCode = person.PostalCode
            };
        }
    }
}
