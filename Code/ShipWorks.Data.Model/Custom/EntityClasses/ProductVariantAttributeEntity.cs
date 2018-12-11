using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the ProductVariantAttributeEntity
    /// </summary>
    public partial class ProductVariantAttributeEntity
    {
        /// <summary>
        /// The attributes name
        /// </summary>
        public string AttributeName => ProductAttribute.AttributeName;
    }
}
