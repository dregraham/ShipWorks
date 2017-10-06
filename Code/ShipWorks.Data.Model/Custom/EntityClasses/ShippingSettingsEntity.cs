using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Settings;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ShippingSettingsEntity
    /// </summary>
    public partial class ShippingSettingsEntity
    {
        private IDictionary<ShipmentTypeCode, ShipmentDateCutoff> loadedShipmentDateCutoffs;

        /// <summary>
        /// List of shipments types that have been activated to by visible if selected in the shipping window.  This list will be the same as
        /// the Configured list except in the case of upgrading from 2x where they would need to be visible, but maybe not been through configuration yet.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ActivatedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalActivated).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalActivated = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ConfiguredTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalConfigured).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalConfigured = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to have hidden from the ShipWorks UI for selection and configuration.  This list is independent
        /// of the Activated and Configured lists.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalExcluded).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalExcluded = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// List of shipment types that the user has elected to exclude when attempting to get the cheapest rate.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> BestRateExcludedTypes
        {
            get
            {
                return ArrayUtility.ParseCommaSeparatedList<int>(InternalBestRateExcludedShipmentTypes).Select(x => (ShipmentTypeCode) x);
            }
            set
            {
                InternalBestRateExcludedShipmentTypes = ArrayUtility.FormatCommaSeparatedList(value.Cast<int>().ToArray());
            }
        }

        /// <summary>
        /// Strongly typed default shipment type code
        /// </summary>
        public ShipmentTypeCode DefaultShipmentTypeCode
        {
            get { return (ShipmentTypeCode) DefaultType; }
            set { DefaultType = (int) value; }
        }

        /// <summary>
        /// Current list of shipment date cutoffs
        /// </summary>
        public ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> ShipmentDateCutoffList
        {
            get
            {
                LoadShipmentDateCutoffJson();

                return loadedShipmentDateCutoffs.ToReadOnlyDictionary(); 
            }
        }

        /// <summary>
        /// Get the shipment cutoff info for a given shipment type code
        /// </summary>
        public ShipmentDateCutoff GetShipmentDateCutoff(ShipmentTypeCode shipmentTypeCode)
        {
            return ShipmentDateCutoffList.ContainsKey(shipmentTypeCode) ? ShipmentDateCutoffList[shipmentTypeCode] : ShipmentDateCutoff.Default;
        }

        /// <summary>
        /// Load shipment date cutoffs from JSON
        /// </summary>
        private void LoadShipmentDateCutoffJson()
        {
            if (loadedShipmentDateCutoffs != null && loadedShipmentDateCutoffs.Any())
            {
                return;
            }

            try
            {
                loadedShipmentDateCutoffs = JsonConvert.DeserializeObject<Dictionary<ShipmentTypeCode, ShipmentDateCutoff>>(ShipmentDateCutoffJson) ?? 
                                            new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
            }
            catch (Exception ex) when (ex is JsonSerializationException || ex is JsonException || 
                                       ex is ArgumentNullException)
            {
                loadedShipmentDateCutoffs = new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
            }
        }

        /// <summary>
        /// Get the shipment cutoff info for a given shipment type code
        /// </summary>
        public void SetShipmentDateCutoff(ShipmentTypeCode shipmentTypeCode, ShipmentDateCutoff shipmentDateCutoff)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentDateCutoff, nameof(shipmentDateCutoff));

            LoadShipmentDateCutoffJson();

            loadedShipmentDateCutoffs[shipmentTypeCode] = shipmentDateCutoff;

            SetShipmentDateCutoffJson();
        }

        /// <summary>
        /// Load shipment date cutoffs from JSON
        /// </summary>
        private void SetShipmentDateCutoffJson()
        {
            if (loadedShipmentDateCutoffs != null)
            {
                try
                {
                    ShipmentDateCutoffJson = JsonConvert.SerializeObject(loadedShipmentDateCutoffs);
                }
                catch (Exception ex) when (ex is JsonSerializationException || ex is JsonException)
                {
                    loadedShipmentDateCutoffs = new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
                }
            }
        }
    }
}
