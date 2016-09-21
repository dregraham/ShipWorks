///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ShippingProviderRule'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingProviderRuleEntity
    {
        
        /// <summary> The ShippingProviderRuleID property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."ShippingProviderRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingProviderRuleID { get; }
        /// <summary> The RowVersion property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The ShipmentType property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingProviderRuleEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingProviderRuleEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingProviderRule'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingProviderRuleEntity : IShippingProviderRuleEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProviderRuleEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingProviderRuleEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingProviderRuleEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingProviderRuleEntity(this, objectMap);
        }
    }
}
