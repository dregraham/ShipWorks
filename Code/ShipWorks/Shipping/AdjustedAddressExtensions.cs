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
        private static readonly Lazy<IDictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>>> adjustmentMethods =
            new Lazy<IDictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>>>(LoadAdjustmentMethods);

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
        public static string AdjustedCountryCode(this PersonAdapter person, ShipmentTypeCode shipmentType)
        {
            MethodConditions.EnsureArgumentIsNotNull(person, "person");

            PersonAdapter copiedPerson = new PersonAdapter();
            person.CopyTo(copiedPerson);

            copiedPerson.CountryCode = copiedPerson.CountryCode.ToUpperInvariant();
            copiedPerson.StateProvCode = copiedPerson.StateProvCode.ToUpperInvariant();

            IEnumerable<Func<PersonAdapter, string>> methods;
            return adjustmentMethods.Value.TryGetValue(shipmentType, out methods) ? 
                ApplyAdjustments(methods, copiedPerson) : 
                copiedPerson.CountryCode;
        }

        /// <summary>
        /// Apply the adjustment methods to the person adapter
        /// </summary>
        private static string ApplyAdjustments(IEnumerable<Func<PersonAdapter, string>> methods, PersonAdapter person)
        {
            foreach (Func<PersonAdapter, string> method in methods)
            {
                person.CountryCode = method(person);
            }

            return person.CountryCode;
        }

        /// <summary>
        /// Replace country name with country code
        /// </summary>
        private static string ReplaceCountryNameWithCountryCode(PersonAdapter person)
        {
            return Geography.GetCountryCode(person.CountryCode);
        }

        /// <summary>
        /// Replace UK with GB
        /// </summary>
        private static string ReplaceUnitedKingdomWithGreatBritain(PersonAdapter person)
        {
            return person.CountryCode == "UK" ? "GB" : person.CountryCode;
        }

        /// <summary>
        /// Replace US territory country code with US
        /// </summary>
        private static string ReplaceInternationalTerritoryCountryCodeWithUnitedStates(PersonAdapter person)
        {
            return Geography.IsUSInternationalTerritory(person.CountryCode) ? "US" : person.CountryCode;
        }

        /// <summary>
        /// Replace US with US territory country code
        /// </summary>
        private static string ReplaceUnitedStatesWithInternationalTerritoryCountryCode(PersonAdapter person)
        {
            return person.CountryCode == "US" && Geography.IsUSInternationalTerritory(person.StateProvCode) ? 
                person.StateProvCode : person.CountryCode;
        }

        /// <summary>
        /// Populate the adjustment methods
        /// </summary>
        private static IDictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>> LoadAdjustmentMethods()
        {
            Dictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>> adjustments =
                new Dictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>>();

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.UpsOnLineTools, ShipmentTypeCode.UpsWorldShip, ShipmentTypeCode.FedEx },
                ReplaceUnitedKingdomWithGreatBritain, ReplaceUnitedStatesWithInternationalTerritoryCountryCode);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.Usps, ShipmentTypeCode.Express1Usps },
                ReplaceUnitedKingdomWithGreatBritain, ReplaceInternationalTerritoryCountryCodeWithUnitedStates);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.Endicia, ShipmentTypeCode.Express1Endicia },
                ReplaceCountryNameWithCountryCode, ReplaceUnitedKingdomWithGreatBritain);

            AddAdjustment(adjustments, new[] { ShipmentTypeCode.iParcel },
                ReplaceUnitedKingdomWithGreatBritain);

            return adjustments;
        }

        /// <summary>
        /// Add an adjustments for the specified shipment types
        /// </summary>
        private static void AddAdjustment(IDictionary<ShipmentTypeCode, IEnumerable<Func<PersonAdapter, string>>> adjustments,
            IEnumerable<ShipmentTypeCode> shipmentTypes, params Func<PersonAdapter, string>[] methods)
        {
            foreach (ShipmentTypeCode shipmentType in shipmentTypes)
            {
                adjustments.Add(shipmentType, methods);
            }
        }
    }
}
