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
    /// Entity interface which represents the entity 'AmazonOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The AmazonOrderItemCode property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."AmazonOrderItemCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonOrderItemCode { get; }
        /// <summary> The ASIN property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."ASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ASIN { get; }
        /// <summary> The ConditionNote property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."ConditionNote"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ConditionNote { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonOrderItemEntity : IAmazonOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IAmazonOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonOrderItemEntity(this, objectMap);
        }
    }
}
