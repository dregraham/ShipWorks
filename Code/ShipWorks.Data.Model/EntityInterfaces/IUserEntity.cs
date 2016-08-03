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
    /// Entity interface which represents the entity 'User'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUserEntity
    {
        
        /// <summary> The UserID property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 UserID { get; }
        /// <summary> The RowVersion property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Username property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The Email property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Email { get; }
        /// <summary> The IsAdmin property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."IsAdmin"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsAdmin { get; }
        /// <summary> The IsDeleted property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."IsDeleted"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsDeleted { get; }
        
        IUserSettingsEntity Settings { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'User'. <br/><br/>
    /// 
    /// </summary>
    public partial class UserEntity : IUserEntity
    {
        IUserSettingsEntity IUserEntity.Settings => Settings;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUserEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUserEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUserEntity(this, objectMap);
        }
    }
}
