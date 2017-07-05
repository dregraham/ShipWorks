using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.AddressValidation;
using Xunit;
using ShipWorks.Stores.Content;

namespace ShipWorks.Tests.Stores.Content.Combine
{
    public class OrderCombineValidatorTest
    {
        [Fact]
        public void GetValidate_Success()
        {
            var validate = new OrderCombineValidator();
            /*  IF the combination orders are from the same store.
             * AND the combination orders are not processed.
             * AND the combination orders are not prime. 
             */
        }

        [Fact]
        public void GetValidate_Failure_Processed()
        {
            var validate = new OrderCombineValidator();
            /*  IF the combination orders are from the same store.
             * AND the combination orders are processed.
             * AND the combination orders are not prime. 
             */
        }

        [Fact]
        public void GetValidate_Failure_isPrimeShipment()
        {
            var validate = new OrderCombineValidator();
            /*  IF the combination orders are from the same store.
             * AND the combination orders are not processed.
             * AND the combination orders are prime. 
             */
        }
    }
}
