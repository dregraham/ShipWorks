using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IShippingAddress
    {
        string AddressType { get; set; }
        string City { get; set; }
        string Company { get; set; }
        string CountryId { get; set; }
        int CustomerAddressId { get; set; }
        string Email { get; set; }
        int EntityId { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string Middlename { get; set; }
        int ParentId { get; set; }
        string Postcode { get; set; }
        string Region { get; set; }
        string RegionCode { get; set; }
        int RegionId { get; set; }
        IList<string> Street { get; set; }
        string Telephone { get; set; }
    }
}