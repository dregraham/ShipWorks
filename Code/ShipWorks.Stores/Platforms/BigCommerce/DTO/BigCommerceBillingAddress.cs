using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce.DTO
{
    /// <summary>
    /// Billing address dto object that gets populated by the REST call
    /// </summary>
    public class BigCommerceBillingAddress
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string street_1 { get; set; }
        public string street_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string country_iso2 { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public BigCommerceAddress ToAddress()
        {
            BigCommerceAddress address = new BigCommerceAddress
                {
                    first_name = first_name,
                    last_name = last_name,
                    company = company,
                    street_1 = street_1,
                    street_2 = street_2,
                    city = city,
                    state = state,
                    zip = zip,
                    country = country,
                    country_iso2 = country_iso2,
                    phone = phone,
                    email = email
                };

            return address;
        }
    }

}
