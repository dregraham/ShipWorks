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
    /// Read-only representation of the entity 'WalmartStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyWalmartStoreEntity : ReadOnlyStoreEntity, IWalmartStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyWalmartStoreEntity(IWalmartStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ConsumerID = source.ConsumerID;
            PrivateKey = source.PrivateKey;
            ChannelType = source.ChannelType;
            
            
            

            CopyCustomWalmartStoreData(source);
        }

        
        /// <summary> The ConsumerID property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."ConsumerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ConsumerID { get; }
        /// <summary> The PrivateKey property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."PrivateKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 1000<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PrivateKey { get; }
        /// <summary> The ChannelType property of the Entity WalmartStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "WalmartStore"."ChannelType"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ChannelType { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IWalmartStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomWalmartStoreData(IWalmartStoreEntity source);
    }
}
