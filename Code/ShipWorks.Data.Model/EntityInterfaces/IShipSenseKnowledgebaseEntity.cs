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
    /// Entity interface which represents the entity 'ShipSenseKnowledgebase'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IShipSenseKnowledgebaseEntity
    {
        
        /// <summary> The Hash property of the Entity ShipSenseKnowledgebase<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipSenseKnowledgeBase"."Hash"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.String Hash { get; }
        /// <summary> The Entry property of the Entity ShipSenseKnowledgebase<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ShipSenseKnowledgeBase"."Entry"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] Entry { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipSenseKnowledgebaseEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IShipSenseKnowledgebaseEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ShipSenseKnowledgebase'. <br/><br/>
    /// 
    /// </summary>
    public partial class ShipSenseKnowledgebaseEntity : IShipSenseKnowledgebaseEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IShipSenseKnowledgebaseEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IShipSenseKnowledgebaseEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IShipSenseKnowledgebaseEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyShipSenseKnowledgebaseEntity(this, objectMap);
        }
    }
}
