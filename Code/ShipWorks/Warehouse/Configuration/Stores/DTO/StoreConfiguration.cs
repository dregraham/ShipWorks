using System.Collections.Generic;
using System.Reflection;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// A store configuration downloaded from the Hub
    /// </summary>
    [Obfuscation]
    public class StoreConfiguration
    {
        /// <summary>
        /// Warehouse store Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The type code of this store
        /// </summary>
        public StoreTypeCode StoreType { get; set; }

        /// <summary>
        /// The name of this store
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique identifier of this store
        /// </summary>
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// The JSON serialized store entity
        /// </summary>
        public string SyncPayload { get; set; }

        /// <summary>
        /// The JSON serialized actions list
        /// </summary>
        public string ActionsPayload { get; set; }

        /// <summary>
        /// True if this store should be managed by Hub
        /// </summary>
        public bool ManagedInHub { get; set; }

        /// <summary>
        /// Store's address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Store settings
        /// </summary>
        public StoreSettings Settings { get; set; }

        /// <summary>
        /// True if this store should upload shipment details
        /// </summary>
        public bool UploadShipmentDetails { get; set; }
        
        /// <summary>
        /// The store license, if we know it.
        /// </summary>
        public string StoreLicense { get; set; }
    }

    /// <summary>
    /// The DTO for serializing the ActionsPayload
    /// </summary>
    [Obfuscation]
    public class ActionConfiguration
    {
        public ActionEntity Action { get; set; }

        public IEnumerable<ActionTaskEntity> Tasks { get; set; }
    }
}
