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
    /// Entity interface which represents the entity 'MivaOrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMivaOrderItemAttributeEntity: IOrderItemAttributeEntity
    {
        
        /// <summary> The MivaOptionCode property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaOptionCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MivaOptionCode { get; }
        /// <summary> The MivaAttributeID property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 MivaAttributeID { get; }
        /// <summary> The MivaAttributeCode property of the Entity MivaOrderItemAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MivaOrderItemAttribute"."MivaAttributeCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MivaAttributeCode { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMivaOrderItemAttributeEntity AsReadOnlyMivaOrderItemAttribute();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMivaOrderItemAttributeEntity AsReadOnlyMivaOrderItemAttribute(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MivaOrderItemAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial class MivaOrderItemAttributeEntity : IMivaOrderItemAttributeEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemAttributeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemAttributeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMivaOrderItemAttributeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMivaOrderItemAttributeEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IMivaOrderItemAttributeEntity AsReadOnlyMivaOrderItemAttribute() =>
            (IMivaOrderItemAttributeEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IMivaOrderItemAttributeEntity AsReadOnlyMivaOrderItemAttribute(IDictionary<object, object> objectMap) =>
            (IMivaOrderItemAttributeEntity) AsReadOnly(objectMap);
        
    }
}
