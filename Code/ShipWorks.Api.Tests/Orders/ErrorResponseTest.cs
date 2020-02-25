using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ShipWorks.Api.Orders;

namespace ShipWorks.Api.Tests.Orders
{
    public class ErrorResponseTest
    {
        [Fact]
        public void Message_DoesNotContainLineBreaks()
        {
            var testObject = new ErrorResponse("blah\r\nblah");
            Assert.Equal("blahblah", testObject.Message);
        }
    }
}
