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
    /// Entity interface which represents the entity 'UserColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUserColumnSettingsEntity
    {
        
        /// <summary> The UserColumnSettingsID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."UserColumnSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UserColumnSettingsID { get; }
        /// <summary> The SettingsKey property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."SettingsKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid SettingsKey { get; }
        /// <summary> The UserID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The InitialSortType property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."InitialSortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 InitialSortType { get; }
        /// <summary> The GridColumnLayoutID property of the Entity UserColumnSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserColumnSettings"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 GridColumnLayoutID { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserColumnSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UserColumnSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class UserColumnSettingsEntity : IUserColumnSettingsEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserColumnSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUserColumnSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUserColumnSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUserColumnSettingsEntity(this, objectMap);
        }

        
    }
}
