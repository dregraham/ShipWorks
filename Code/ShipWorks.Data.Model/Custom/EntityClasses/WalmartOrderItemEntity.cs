using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class WalmartOrderItemEntity
    {
        public WalmartOrderItemEntity(OrderEntity order)
            : base(order)
        {
            
        }
    }
}
