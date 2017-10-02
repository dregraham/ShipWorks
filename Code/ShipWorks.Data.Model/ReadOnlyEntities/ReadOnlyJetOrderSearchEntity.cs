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
    /// Read-only representation of the entity 'JetOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyJetOrderSearchEntity : IJetOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyJetOrderSearchEntity(IJetOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            JetOrderSearchID = source.JetOrderSearchID;
            OrderID = source.OrderID;
            MerchantOrderID = source.MerchantOrderID;
            OriginalOrderID = source.OriginalOrderID;
            
            
            JetOrder = (IJetOrderEntity) source.JetOrder?.AsReadOnly(objectMap);
            

            CopyCustomJetOrderSearchData(source);
        }

        
        /// <summary> The JetOrderSearchID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."JetOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 JetOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The MerchantOrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."MerchantOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IJetOrderEntity JetOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IJetOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IJetOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomJetOrderSearchData(IJetOrderSearchEntity source);
    }
}
