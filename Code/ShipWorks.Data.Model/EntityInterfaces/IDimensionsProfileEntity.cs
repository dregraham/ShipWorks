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
    /// Entity interface which represents the entity 'DimensionsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IDimensionsProfileEntity
    {
        
        /// <summary> The DimensionsProfileID property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."DimensionsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 DimensionsProfileID { get; }
        /// <summary> The Name property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Length property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Length { get; }
        /// <summary> The Width property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Width { get; }
        /// <summary> The Height property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Height { get; }
        /// <summary> The Weight property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double Weight { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDimensionsProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IDimensionsProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'DimensionsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class DimensionsProfileEntity : IDimensionsProfileEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDimensionsProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IDimensionsProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IDimensionsProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyDimensionsProfileEntity(this, objectMap);
        }

        
    }
}
