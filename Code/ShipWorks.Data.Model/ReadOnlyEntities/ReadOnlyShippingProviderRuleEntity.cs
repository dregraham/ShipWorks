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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'ShippingProviderRule'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingProviderRuleEntity : IShippingProviderRuleEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingProviderRuleEntity(IShippingProviderRuleEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProviderRuleID = source.ShippingProviderRuleID;
            RowVersion = source.RowVersion;
            FilterNodeID = source.FilterNodeID;
            ShipmentType = source.ShipmentType;
            
            
            

            CopyCustomShippingProviderRuleData(source);
        }

        
        /// <summary> The ShippingProviderRuleID property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."ShippingProviderRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShippingProviderRuleID { get; }
        /// <summary> The RowVersion property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The ShipmentType property of the Entity ShippingProviderRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingProviderRule"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentType { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProviderRuleEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingProviderRuleEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingProviderRuleData(IShippingProviderRuleEntity source);
    }
}
