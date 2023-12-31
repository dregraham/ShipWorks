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
    /// Read-only representation of the entity 'YahooOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyYahooOrderSearchEntity : IYahooOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyYahooOrderSearchEntity(IYahooOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            YahooOrderSearchID = source.YahooOrderSearchID;
            OrderID = source.OrderID;
            YahooOrderID = source.YahooOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            YahooOrder = (IYahooOrderEntity) source.YahooOrder?.AsReadOnly(objectMap);
            

            CopyCustomYahooOrderSearchData(source);
        }

        
        /// <summary> The YahooOrderSearchID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."YahooOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 YahooOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The YahooOrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."YahooOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String YahooOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity YahooOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IYahooOrderEntity YahooOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IYahooOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomYahooOrderSearchData(IYahooOrderSearchEntity source);
    }
}
