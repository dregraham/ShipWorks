using System.Reflection;

namespace ShipWorks.Warehouse.Configuration.DTO
{
    /// <summary>
    /// DTO for importing hub configuration addresses
    /// </summary>
    [Obfuscation]
    public class ConfigurationAddress
    {
        /// <summary>
        /// The first name of the person associated with the address
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name of the person associated with the address
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The last name of the person associated with the address
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The address company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// The address street 1
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// The address street 2
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// The address street 3
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// The address City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The address state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The address zip
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// The address country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The address phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The customer's email
        /// </summary>
        public string Email { get; set; }
    }
}
