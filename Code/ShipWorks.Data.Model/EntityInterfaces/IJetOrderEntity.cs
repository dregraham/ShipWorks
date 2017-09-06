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
    /// Entity interface which represents the entity 'JetOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IJetOrderEntity: IOrderEntity
    {
        
        /// <summary> The MerchantOrderId property of the Entity JetOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrder"."MerchantOrderId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantOrderId { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IJetOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IJetOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'JetOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class JetOrderEntity : IJetOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IJetOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IJetOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IJetOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyJetOrderEntity(this, objectMap);
        }
    }
}
