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
    /// Read-only representation of the entity 'MivaOrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMivaOrderItemAttributeEntity : ReadOnlyOrderItemAttributeEntity, IMivaOrderItemAttributeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMivaOrderItemAttributeEntity(IMivaOrderItemAttributeEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MivaOptionCode = source.MivaOptionCode;
            MivaAttributeID = source.MivaAttributeID;
            MivaAttributeCode = source.MivaAttributeCode;
            
            
            

            CopyCustomMivaOrderItemAttributeData(source);
        }

        
        /// <summary> The MivaOptionCode property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaOptionCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MivaOptionCode { get; }
        /// <summary> The MivaAttributeID property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 MivaAttributeID { get; }
        /// <summary> The MivaAttributeCode property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaAttributeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MivaAttributeCode { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMivaOrderItemAttributeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMivaOrderItemAttributeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMivaOrderItemAttributeData(IMivaOrderItemAttributeEntity source);
    }
}
