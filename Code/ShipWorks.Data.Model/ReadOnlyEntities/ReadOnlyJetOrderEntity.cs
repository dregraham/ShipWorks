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
    /// Read-only representation of the entity 'JetOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyJetOrderEntity : ReadOnlyOrderEntity, IJetOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyJetOrderEntity(IJetOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MerchantOrderId = source.MerchantOrderId;
            
            
            
            JetOrderSearch = source.JetOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IJetOrderSearchEntity>();

            CopyCustomJetOrderData(source);
        }

        
        /// <summary> The MerchantOrderId property of the Entity JetOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "JetOrder"."MerchantOrderId"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MerchantOrderId { get; }
        
        
        
        public IEnumerable<IJetOrderSearchEntity> JetOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IJetOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IJetOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomJetOrderData(IJetOrderEntity source);
    }
}
