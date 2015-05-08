using Interapptive.Shared.Business;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Convert API specific addresses into an AddressAdapters
    /// </summary>
    public static class AddressConversions
    {
        /// <summary>
        /// Convert Usps Address into an IAddressAdapter
        /// </summary>
        public static IAddressAdapter AsAddressAdapter(this Carriers.Postal.Usps.WebServices.Address source)
        {
            return source == null ? 
                null :
                new AddressAdapter
            {
                Street1 = source.Address1,
                Street2 = source.Address2,
                Street3 = source.Address3,
                City = source.City,
                StateProvCode = source.State,
                PostalCode = source.ZIPCode,
                CountryCode = source.Country
            };
        }

        /// <summary>
        /// Convert Express1 Usps Address into an IAddressAdapter
        /// </summary>
        public static IAddressAdapter AsAddressAdapter(this Carriers.Postal.Usps.WebServices.v29.Address source)
        {
            return source == null ?
                null :
                new AddressAdapter
            {
                Street1 = source.Address1,
                Street2 = source.Address2,
                Street3 = source.Address3,
                City = source.City,
                StateProvCode = source.State,
                PostalCode = source.ZIPCode,
                CountryCode = source.Country
            };
        }
    }
}
