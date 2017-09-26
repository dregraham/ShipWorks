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
    /// Read-only representation of the entity 'MagentoStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMagentoStoreEntity : ReadOnlyGenericModuleStoreEntity, IMagentoStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMagentoStoreEntity(IMagentoStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MagentoTrackingEmails = source.MagentoTrackingEmails;
            MagentoVersion = source.MagentoVersion;
            
            
            

            CopyCustomMagentoStoreData(source);
        }

        
        /// <summary> The MagentoTrackingEmails property of the Entity MagentoStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoStore"."MagentoTrackingEmails"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean MagentoTrackingEmails { get; }
        /// <summary> The MagentoVersion property of the Entity MagentoStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoStore"."MagentoVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 MagentoVersion { get; }
        
        
        
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
        public IMagentoStoreEntity AsReadOnlyMagentoStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IMagentoStoreEntity AsReadOnlyMagentoStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMagentoStoreData(IMagentoStoreEntity source);
    }
}
