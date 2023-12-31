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
    /// Read-only representation of the entity 'JetOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyJetOrderItemEntity : ReadOnlyOrderItemEntity, IJetOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyJetOrderItemEntity(IJetOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MerchantSku = source.MerchantSku;
            JetOrderItemID = source.JetOrderItemID;
            
            
            

            CopyCustomJetOrderItemData(source);
        }

        
        /// <summary> The MerchantSku property of the Entity JetOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderItem"."MerchantSku"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantSku { get; }
        /// <summary> The JetOrderItemID property of the Entity JetOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrderItem"."JetOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String JetOrderItemID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IJetOrderItemEntity AsReadOnlyJetOrderItem() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IJetOrderItemEntity AsReadOnlyJetOrderItem(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomJetOrderItemData(IJetOrderItemEntity source);
    }
}
