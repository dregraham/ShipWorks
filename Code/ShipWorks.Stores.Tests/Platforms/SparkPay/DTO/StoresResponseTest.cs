using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.SparkPay.DTO
{
    public class StoresResponseTest
    {
        [Fact]
        public void StoresResponse_Deserialize()
        {
            string storesResponse = "{\"total_count\":1,\"stores\":[{\"id\":1,\"name\":\"ShipWorks\",\"domain_name\":\"ShipWorks.americommerce.com\",\"email\":\"t.hughes@ShipWorks.com\",\"keywords\":\"\",\"description\":\"\",\"address_line_1\":\"One Memorial Drive\",\"address_line_2\":\"\",\"city\":\"St. Louis\",\"state\":\"Missouri\",\"country\":\"United States\",\"postal_code\":\"63102\",\"phone\":\"3145551212\",\"fax\":\"\",\"is_micro_store\":false,\"parent_store_id\":0,\"company_name\":\"ShipWorks\",\"billing_first_name\":\"\",\"billing_last_name\":\"\",\"tech_first_name\":\"\",\"tech_last_name\":\"\",\"tech_email\":\"\",\"tech_same_as_billing\":false,\"profile_id\":1}]}";
            StoresResponse response = JsonConvert.DeserializeObject<StoresResponse>(storesResponse);

            foreach (Store store in response.stores)
            {
                Assert.NotNull(store.address_line_1);
            }
        }
    }
}
