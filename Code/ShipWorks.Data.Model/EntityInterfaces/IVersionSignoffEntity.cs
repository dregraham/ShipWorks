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
    /// Entity interface which represents the entity 'VersionSignoff'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IVersionSignoffEntity
    {
        
        /// <summary> The VersionSignoffID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."VersionSignoffID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 VersionSignoffID { get; }
        /// <summary> The Version property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."Version"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Version { get; }
        /// <summary> The UserID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity VersionSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VersionSignoff"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IVersionSignoffEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IVersionSignoffEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'VersionSignoff'. <br/><br/>
    /// 
    /// </summary>
    public partial class VersionSignoffEntity : IVersionSignoffEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IVersionSignoffEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IVersionSignoffEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IVersionSignoffEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyVersionSignoffEntity(this, objectMap);
        }

        
    }
}
