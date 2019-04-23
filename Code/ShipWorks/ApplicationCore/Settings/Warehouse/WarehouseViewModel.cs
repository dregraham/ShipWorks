using System;
using System.Reflection;
using DTO = ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model that represents a warehouse
    /// </summary>
    public class WarehouseViewModel : IWarehouseViewModel
    {
        private readonly string shipWorksLink;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseViewModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseViewModel(DTO.Warehouse warehouse)
        {
            Id = warehouse.id;
            Name = warehouse.details.name;
            City = warehouse.details.city;
            Code = warehouse.details.code;
            shipWorksLink = warehouse.details.shipWorksLink;
            State = warehouse.details.state;
            Street = warehouse.details.street;
            Zip = warehouse.details.zip;
            IsAlreadyLinked = !string.IsNullOrEmpty(shipWorksLink);
        }

        /// <summary>
        /// Id of the warehouse
        /// </summary>
        [Obfuscation]
        public string Id { get; set; }

        /// <summary>
        /// Name of the warehouse
        /// </summary>
        [Obfuscation]
        public string Name { get; set; }

        /// <summary>
        /// City of the warehouse
        /// </summary>
        [Obfuscation]
        public string City { get; set; }

        /// <summary>
        /// Code of the warehouse
        /// </summary>
        [Obfuscation]
        public string Code { get; set; }

        /// <summary>
        /// State of the warehouse
        /// </summary>
        [Obfuscation]
        public string State { get; set; }

        /// <summary>
        /// Street of the warehouse
        /// </summary>
        [Obfuscation]
        public string Street { get; set; }

        /// <summary>
        /// Zip code of the warehouse
        /// </summary>
        [Obfuscation]
        public string Zip { get; set; }

        /// <summary>
        /// Is the warehouse already linked with a ShipWorks instance
        /// </summary>
        [Obfuscation]
        public bool IsAlreadyLinked { get; set; }

        /// <summary>
        /// Can this warehouse be linked with the given GUID
        /// </summary>
        public bool CanBeLinkedWith(Guid guid) =>
            !IsAlreadyLinked || shipWorksLink == guid.ToString();
    }
}