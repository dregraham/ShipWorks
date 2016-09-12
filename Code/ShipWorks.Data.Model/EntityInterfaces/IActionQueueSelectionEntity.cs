﻿///////////////////////////////////////////////////////////////
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'ActionQueueSelection'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IActionQueueSelectionEntity
    {
        
        /// <summary> The ActionQueueSelectionID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ActionQueueSelectionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ActionQueueSelectionID { get; }
        /// <summary> The ActionQueueID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ActionQueueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ActionQueueID { get; }
        /// <summary> The ObjectID property of the Entity ActionQueueSelection<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ActionQueueSelection"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ObjectID { get; }
        
        
        IActionQueueEntity ActionQueue { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueSelectionEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IActionQueueSelectionEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ActionQueueSelection'. <br/><br/>
    /// 
    /// </summary>
    public partial class ActionQueueSelectionEntity : IActionQueueSelectionEntity
    {
        
        IActionQueueEntity IActionQueueSelectionEntity.ActionQueue => ActionQueue;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IActionQueueSelectionEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IActionQueueSelectionEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IActionQueueSelectionEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyActionQueueSelectionEntity(this, objectMap);
        }
    }
}