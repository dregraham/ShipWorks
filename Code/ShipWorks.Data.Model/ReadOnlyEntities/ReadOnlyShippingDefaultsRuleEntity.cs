///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Read-only representation of the entity 'ShippingDefaultsRule'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShippingDefaultsRuleEntity : IShippingDefaultsRuleEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShippingDefaultsRuleEntity(IShippingDefaultsRuleEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingDefaultsRuleID = source.ShippingDefaultsRuleID;
            RowVersion = source.RowVersion;
            ShipmentType = source.ShipmentType;
            FilterNodeID = source.FilterNodeID;
            ShippingProfileID = source.ShippingProfileID;
            Position = source.Position;
            
            
            

            CopyCustomShippingDefaultsRuleData(source);
        }

        
        /// <summary> The ShippingDefaultsRuleID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShippingDefaultsRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ShippingDefaultsRuleID { get; }
        /// <summary> The RowVersion property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentType property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShipmentType { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterNodeID { get; }
        /// <summary> The ShippingProfileID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The Position property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Position { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingDefaultsRuleEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingDefaultsRuleEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShippingDefaultsRuleData(IShippingDefaultsRuleEntity source);
    }
}
