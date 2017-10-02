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
    /// Read-only representation of the entity 'AmazonOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonOrderSearchEntity : IAmazonOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonOrderSearchEntity(IAmazonOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonOrderSearchID = source.AmazonOrderSearchID;
            OrderID = source.OrderID;
            AmazonOrderID = source.AmazonOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            AmazonOrder = (IAmazonOrderEntity) source.AmazonOrder?.AsReadOnly(objectMap);
            

            CopyCustomAmazonOrderSearchData(source);
        }

        
        /// <summary> The AmazonOrderSearchID property of the Entity AmazonOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderSearch"."AmazonOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AmazonOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity AmazonOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The AmazonOrderID property of the Entity AmazonOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderSearch"."AmazonOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity AmazonOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IAmazonOrderEntity AmazonOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonOrderSearchData(IAmazonOrderSearchEntity source);
    }
}
