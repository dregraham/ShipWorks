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
    /// Read-only representation of the entity 'User'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUserEntity : IUserEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUserEntity(IUserEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UserID = source.UserID;
            RowVersion = source.RowVersion;
            Username = source.Username;
            Password = source.Password;
            Email = source.Email;
            IsAdmin = source.IsAdmin;
            IsDeleted = source.IsDeleted;
            
            Settings = (IUserSettingsEntity) source.Settings?.AsReadOnly(objectMap);
            
            

            CopyCustomUserData(source);
        }

        
        /// <summary> The UserID property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The RowVersion property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Username property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The Email property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The IsAdmin property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."IsAdmin"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsAdmin { get; }
        /// <summary> The IsDeleted property of the Entity User<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "User"."IsDeleted"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsDeleted { get; }
        
        public IUserSettingsEntity Settings { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUserData(IUserEntity source);
    }
}
