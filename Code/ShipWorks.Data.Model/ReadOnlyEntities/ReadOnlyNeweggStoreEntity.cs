///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'NeweggStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyNeweggStoreEntity : ReadOnlyStoreEntity, INeweggStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyNeweggStoreEntity(INeweggStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SellerID = source.SellerID;
            SecretKey = source.SecretKey;
            ExcludeFulfilledByNewegg = source.ExcludeFulfilledByNewegg;
            Channel = source.Channel;
            
            
            

            CopyCustomNeweggStoreData(source);
        }

        
        /// <summary> The SellerID property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."SellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SellerID { get; }
        /// <summary> The SecretKey property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."SecretKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SecretKey { get; }
        /// <summary> The ExcludeFulfilledByNewegg property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."ExcludeFulfilledByNewegg"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ExcludeFulfilledByNewegg { get; }
        /// <summary> The Channel property of the Entity NeweggStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "NeweggStore"."Channel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Channel { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new INeweggStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomNeweggStoreData(INeweggStoreEntity source);
    }
}
