using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    public class OdbcOrderAddressLoader : IOdbcOrderDetailLoader
    {
        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            FixName(order, "Ship");
            FixName(order, "Bill");
        }

        private void FixName(OrderEntity order, string prefix)
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
