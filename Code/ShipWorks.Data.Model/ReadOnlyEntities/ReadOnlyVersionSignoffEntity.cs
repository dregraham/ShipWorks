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
    /// Entity interface which represents the entity 'VersionSignoff'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyVersionSignoffEntity : IVersionSignoffEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyVersionSignoffEntity(IVersionSignoffEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            VersionSignoffID = source.VersionSignoffID;
            Version = source.Version;
            UserID = source.UserID;
            ComputerID = source.ComputerID;
            
            
            

            CopyCustomVersionSignoffData(source);
        }

        
        /// <summary> The VersionSignoffID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."VersionSignoffID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 VersionSignoffID { get; }
        /// <summary> The Version property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."Version"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Version { get; }
        /// <summary> The UserID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IVersionSignoffEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IVersionSignoffEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomVersionSignoffData(IVersionSignoffEntity source);
    }
}
