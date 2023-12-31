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
    /// Read-only representation of the entity 'ClickCartProOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyClickCartProOrderSearchEntity : IClickCartProOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyClickCartProOrderSearchEntity(IClickCartProOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ClickCartProOrderSearchID = source.ClickCartProOrderSearchID;
            OrderID = source.OrderID;
            ClickCartProOrderID = source.ClickCartProOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            ClickCartProOrder = (IClickCartProOrderEntity) source.ClickCartProOrder?.AsReadOnly(objectMap);
            

            CopyCustomClickCartProOrderSearchData(source);
        }

        
        /// <summary> The ClickCartProOrderSearchID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."ClickCartProOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ClickCartProOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The ClickCartProOrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."ClickCartProOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ClickCartProOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity ClickCartProOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ClickCartProOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IClickCartProOrderEntity ClickCartProOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IClickCartProOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IClickCartProOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomClickCartProOrderSearchData(IClickCartProOrderSearchEntity source);
    }
}
