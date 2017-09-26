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
    /// Entity interface which represents the entity 'JetOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IJetOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The MerchantSku property of the Entity JetOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderItem"."MerchantSku"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MerchantSku { get; }
        /// <summary> The JetOrderItemID property of the Entity JetOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderItem"."JetOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String JetOrderItemID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IJetOrderItemEntity AsReadOnlyJetOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IJetOrderItemEntity AsReadOnlyJetOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'JetOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class JetOrderItemEntity : IJetOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IJetOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyJetOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IJetOrderItemEntity AsReadOnlyJetOrderItem() =>
            (IJetOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IJetOrderItemEntity AsReadOnlyJetOrderItem(IDictionary<object, object> objectMap) =>
            (IJetOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
