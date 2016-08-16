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
    /// Read-only representation of the entity 'InfopiaStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyInfopiaStoreEntity : ReadOnlyStoreEntity, IInfopiaStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyInfopiaStoreEntity(IInfopiaStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ApiToken = source.ApiToken;
            
            
            

            CopyCustomInfopiaStoreData(source);
        }

        
        /// <summary> The ApiToken property of the Entity InfopiaStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 128<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiToken { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IInfopiaStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IInfopiaStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomInfopiaStoreData(IInfopiaStoreEntity source);
    }
}
