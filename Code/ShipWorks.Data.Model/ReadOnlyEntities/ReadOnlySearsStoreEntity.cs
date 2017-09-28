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
    /// Read-only representation of the entity 'SearsStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlySearsStoreEntity : ReadOnlyStoreEntity, ISearsStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlySearsStoreEntity(ISearsStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            SearsEmail = source.SearsEmail;
            Password = source.Password;
            SecretKey = source.SecretKey;
            SellerID = source.SellerID;
            
            
            

            CopyCustomSearsStoreData(source);
        }

        
        /// <summary> The SearsEmail property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SearsEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SearsEmail { get; }
        /// <summary> The Password property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The SecretKey property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SecretKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SecretKey { get; }
        /// <summary> The SellerID property of the Entity SearsStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsStore"."SellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SellerID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public ISearsStoreEntity AsReadOnlySearsStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public ISearsStoreEntity AsReadOnlySearsStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomSearsStoreData(ISearsStoreEntity source);
    }
}
