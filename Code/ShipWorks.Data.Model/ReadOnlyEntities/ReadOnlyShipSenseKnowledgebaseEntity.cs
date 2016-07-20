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
    /// Entity interface which represents the entity 'ShipSenseKnowledgebase'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyShipSenseKnowledgebaseEntity : IShipSenseKnowledgebaseEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyShipSenseKnowledgebaseEntity(IShipSenseKnowledgebaseEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Hash = source.Hash;
            Entry = source.Entry;
            
            
            

            CopyCustomShipSenseKnowledgebaseData(source);
        }

        
        /// <summary> The Hash property of the Entity ShipSenseKnowledgebase<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipSenseKnowledgeBase"."Hash"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.String Hash { get; }
        /// <summary> The Entry property of the Entity ShipSenseKnowledgebase<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipSenseKnowledgeBase"."Entry"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] Entry { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipSenseKnowledgebaseEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipSenseKnowledgebaseEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomShipSenseKnowledgebaseData(IShipSenseKnowledgebaseEntity source);
    }
}
