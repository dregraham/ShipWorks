﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'LemonStandOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyLemonStandOrderItemEntity : ReadOnlyOrderItemEntity, ILemonStandOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyLemonStandOrderItemEntity(ILemonStandOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UrlName = source.UrlName;
            ShortDescription = source.ShortDescription;
            Category = source.Category;
            
            
            

            CopyCustomLemonStandOrderItemData(source);
        }

        
        /// <summary> The UrlName property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."UrlName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String UrlName { get; }
        /// <summary> The ShortDescription property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."ShortDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShortDescription { get; }
        /// <summary> The Category property of the Entity LemonStandOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LemonStandOrderItem"."Category"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Category { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new ILemonStandOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomLemonStandOrderItemData(ILemonStandOrderItemEntity source);
    }
}
