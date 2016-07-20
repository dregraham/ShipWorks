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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ShippingDefaultsRule'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShippingDefaultsRuleEntity
    {
        
        /// <summary> The ShippingDefaultsRuleID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShippingDefaultsRuleID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ShippingDefaultsRuleID { get; }
        /// <summary> The RowVersion property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ShipmentType property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShipmentType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShipmentType { get; }
        /// <summary> The FilterNodeID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."FilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterNodeID { get; }
        /// <summary> The ShippingProfileID property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The Position property of the Entity ShippingDefaultsRule<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShippingDefaultsRule"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Position { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingDefaultsRuleEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShippingDefaultsRuleEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShippingDefaultsRule'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShippingDefaultsRuleEntity : IShippingDefaultsRuleEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShippingDefaultsRuleEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShippingDefaultsRuleEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShippingDefaultsRuleEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShippingDefaultsRuleEntity(this, objectMap);
        }
    }
}
