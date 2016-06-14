using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.SparkPay.DTO
{
    public class OrderStatusResponseTest
    {
        [Fact]
        public void OrderStatusResponse_Deserialize()
        {
            string orderResponse = "{\"total_count\":9,\"order_statuses\":[{\"id\":1,\"name\":\"Pending Processing\",\"is_open\":true,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":false,\"color\":\"#A4E065\",\"email_template_id\":null,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":2,\"name\":\"Approved, Pending Shipping\",\"is_open\":true,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":false,\"color\":\"#49C880\",\"email_template_id\":null,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":3,\"name\":\"Shipped\",\"is_open\":false,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":true,\"color\":\"#8CA4A5\",\"email_template_id\":3,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":4,\"name\":\"Credit Declined\",\"is_open\":false,\"is_declined\":true,\"is_cancelled\":false,\"is_shipped\":false,\"color\":\"#CD6155\",\"email_template_id\":2,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":5,\"name\":\"Cancel Order\",\"is_open\":false,\"is_declined\":false,\"is_cancelled\":true,\"is_shipped\":false,\"color\":\"#E2814A\",\"email_template_id\":4,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":6,\"name\":\"Awaiting Payment\",\"is_open\":true,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":false,\"color\":\"#F4D03F\",\"email_template_id\":null,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":false},{\"id\":7,\"name\":\"Quote - Open\",\"is_open\":true,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":false,\"color\":\"#7DBDE8\",\"email_template_id\":21,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":true},{\"id\":8,\"name\":\"Quote - Accepted\",\"is_open\":false,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":true,\"color\":\"#80DDAE\",\"email_template_id\":null,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":true},{\"id\":9,\"name\":\"Quote - Declined\",\"is_open\":false,\"is_declined\":false,\"is_cancelled\":false,\"is_shipped\":true,\"color\":\"#F66E6E\",\"email_template_id\":null,\"updated_at\":null,\"created_at\":null,\"is_fully_refunded\":false,\"is_partially_refunded\":false,\"is_quote_status\":true}]}";
            OrderStatusResponse response = JsonConvert.DeserializeObject<OrderStatusResponse>(orderResponse);

            foreach (OrderStatus status in response.Statuses)
            {
                Assert.NotNull(status.Name);
            }
        }
    }
}
