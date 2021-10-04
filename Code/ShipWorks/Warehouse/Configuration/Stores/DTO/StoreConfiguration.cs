using System.Collections.Generic;
using System.Reflection;
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
        /// True if this store should upload shipment details
        /// </summary>
        public bool UploadShipmentDetails { get; set; }
    }

    /// <summary>
    /// Store configuration's address
    /// </summary>
    [Obfuscation]
    public class Address 
    {
        public string City { get; set; }
        public string StateProvCode { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Company { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
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
