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
    /// Entity interface which represents the entity 'JetOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IJetOrderSearchEntity
    {
        
        /// <summary> The JetOrderSearchID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."JetOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 JetOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The MerchantOrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."MerchantOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantOrderID { get; }
        /// <summary> The OriginalOrderID property of the Entity JetOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IJetOrderEntity JetOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IJetOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IJetOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'JetOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class JetOrderSearchEntity : IJetOrderSearchEntity
    {
        
        IJetOrderEntity IJetOrderSearchEntity.JetOrder => JetOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IJetOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IJetOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IJetOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyJetOrderSearchEntity(this, objectMap);
        }

        
    }
}
