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
    /// Read-only representation of the entity 'DimensionsProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyDimensionsProfileEntity : IDimensionsProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyDimensionsProfileEntity(IDimensionsProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            DimensionsProfileID = source.DimensionsProfileID;
            Name = source.Name;
            Length = source.Length;
            Width = source.Width;
            Height = source.Height;
            Weight = source.Weight;
            
            
            

            CopyCustomDimensionsProfileData(source);
        }

        
        /// <summary> The DimensionsProfileID property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."DimensionsProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 DimensionsProfileID { get; }
        /// <summary> The Name property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Length property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Length"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Length { get; }
        /// <summary> The Width property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Width { get; }
        /// <summary> The Height property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Height"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Height { get; }
        /// <summary> The Weight property of the Entity DimensionsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DimensionsProfile"."Weight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double Weight { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDimensionsProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IDimensionsProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomDimensionsProfileData(IDimensionsProfileEntity source);
    }
}
