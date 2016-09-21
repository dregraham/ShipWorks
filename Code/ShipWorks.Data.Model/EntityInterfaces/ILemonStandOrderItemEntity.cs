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
    /// Entity interface which represents the entity 'LemonStandOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ILemonStandOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The UrlName property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."UrlName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String UrlName { get; }
        /// <summary> The ShortDescription property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."ShortDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShortDescription { get; }
        /// <summary> The Category property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."Category"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Category { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ILemonStandOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new ILemonStandOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'LemonStandOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class LemonStandOrderItemEntity : ILemonStandOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new ILemonStandOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ILemonStandOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyLemonStandOrderItemEntity(this, objectMap);
        }
    }
}
