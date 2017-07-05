using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Content
{
    //Add security check
    public class OrderCombineValidator
    {
        Result Validate(IEnumerable<long> validate)
        {
            return Result.FromSuccess();
        }
    }
}
