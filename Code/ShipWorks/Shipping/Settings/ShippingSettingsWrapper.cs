using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    /// <remarks>
    /// Wraps the static ShippingSettings so that static dependencies can be broken
    /// </remarks>
    public class ShippingSettingsWrapper : IShippingSettings, IInitializeForCurrentDatabase, ICheckForChangesNeeded
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSettingsWrapper(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        public bool AutoCreateShipments => FetchReadOnly().AutoCreateShipments;

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> GetConfiguredTypes()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            return settings.ConfiguredTypes.Cast<ShipmentTypeCode>();
        }

        /// <summary>
        /// Sets the default shipping provider to the given shipment type code.
        /// </summary>
        public void SetDefaultProvider(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.DefaultType = (int) shipmentTypeCode;

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingSettings.MarkAsConfigured(shipmentTypeCode);

        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        public ShippingSettingsEntity Fetch() => ShippingSettings.Fetch();

        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        public IShippingSettingsEntity FetchReadOnly() => ShippingSettings.FetchReadOnly();

        /// <summary>
        /// Check the database for the latest SystemData
        /// </summary>
        public void CheckForChangesNeeded() => ShippingSettings.CheckForChangesNeeded();

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ShippingSettings.InitializeForCurrentDatabase();

        /// <summary>
        /// Save the current shipping settings
        /// </summary>
        public void Save(ShippingSettingsEntity shippingSettings)
        {
            bool wasDirty = shippingSettings.IsDirty;

            ShippingSettings.Save(shippingSettings);

            if (wasDirty)
            {
                messenger.Send(new ShippingSettingsChangedMessage(this, shippingSettings));
            }
        }

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public void InitializeForCurrentDatabase()
        {
            ShippingSettings.InitializeForCurrentDatabase();
        }
    }
}