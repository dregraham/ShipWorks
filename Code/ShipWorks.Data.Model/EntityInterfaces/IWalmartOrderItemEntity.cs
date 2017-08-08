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
    /// Entity interface which represents the entity 'WalmartOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IWalmartOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The LineNumber property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."LineNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LineNumber { get; }
        /// <summary> The OnlineStatus property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."OnlineStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OnlineStatus { get; }
        /// <summary> The OriginalOrderID property of the Entity WalmartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartOrderItem"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IWalmartOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IWalmartOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'WalmartOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class WalmartOrderItemEntity : IWalmartOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IWalmartOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IWalmartOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyWalmartOrderItemEntity(this, objectMap);
        }
    }
}
