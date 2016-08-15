using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Linq;

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
            IOdbcFieldMapEntry unparsedNameEntry = map.FindEntriesBy(OrderFields.ShipUnparsedName).FirstOrDefault();
            FixNameAndAddress(order, "Ship", unparsedNameEntry);

            unparsedNameEntry = map.FindEntriesBy(OrderFields.BillUnparsedName).FirstOrDefault();
            FixNameAndAddress(order, "Bill", unparsedNameEntry);
        }

        /// <summary>
        /// Fixes the name and address
        /// </summary>
        private void FixNameAndAddress(OrderEntity order, string prefix, IOdbcFieldMapEntry unparsedNameEntry)
        {
            PersonAdapter personAdapter = new PersonAdapter(order, prefix);

            // If full name is not mapped, set name parse status to simple and unparsed name to empty
            // so that full name will set itself based on the current first, middle, last name.
            if (string.IsNullOrWhiteSpace(unparsedNameEntry?.ExternalField.Value?.ToString()))
            {
                personAdapter.NameParseStatus = PersonNameParseStatus.Simple;

                personAdapter.UnparsedName = string.Empty;
            }
            else
            {
                personAdapter.ParsedName = PersonName.Parse(unparsedNameEntry.ExternalField.Value.ToString());
            }

            // I'd like to move this logic into GetCountryCode, maybe after the demo.
            personAdapter.CountryCode = string.IsNullOrWhiteSpace(personAdapter.CountryCode) ? 
                "US" : 
                Geography.GetCountryCode(personAdapter.CountryCode);

            personAdapter.StateProvCode = Geography.GetStateProvCode(personAdapter.StateProvCode);
        }
    }
}
