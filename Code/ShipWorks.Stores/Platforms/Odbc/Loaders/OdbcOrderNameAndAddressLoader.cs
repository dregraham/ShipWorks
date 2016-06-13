using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Updates name and address details of an order after it is downloaded
    /// </summary>
    public class OdbcOrderNameAndAddressLoader : IOdbcOrderDetailLoader
    {
        /// <summary>
        /// Updates name and address details for the given order
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            FixNameAndAddress(order, "Ship");
            FixNameAndAddress(order, "Bill");
        }

        /// <summary>
        /// Fixes the name and address
        /// </summary>
        private void FixNameAndAddress(OrderEntity order, string prefix)
        {
            PersonAdapter personAdapter = new PersonAdapter(order, prefix);

            if (string.IsNullOrWhiteSpace(personAdapter.UnparsedName))
            {
                personAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            }
            else
            {
                personAdapter.ParsedName = PersonName.Parse(personAdapter.UnparsedName);
            }

            personAdapter.CountryCode = Geography.GetCountryCode(personAdapter.CountryCode);
            personAdapter.StateProvCode = Geography.GetStateProvCode(personAdapter.StateProvCode);
        }
    }
}
