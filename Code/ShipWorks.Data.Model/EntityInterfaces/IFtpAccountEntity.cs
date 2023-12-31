﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'FtpAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IFtpAccountEntity
    {
        
        /// <summary> The FtpAccountID property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."FtpAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 FtpAccountID { get; }
        /// <summary> The Host property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Host"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Host { get; }
        /// <summary> The Username property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The Port property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Port"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Port { get; }
        /// <summary> The SecurityType property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."SecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SecurityType { get; }
        /// <summary> The Passive property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Passive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Passive { get; }
        /// <summary> The InternalOwnerID property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."InternalOwnerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> InternalOwnerID { get; }
        /// <summary> The ReuseControlConnectionSession property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."ReuseControlConnectionSession"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ReuseControlConnectionSession { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFtpAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IFtpAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'FtpAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class FtpAccountEntity : IFtpAccountEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFtpAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IFtpAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IFtpAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyFtpAccountEntity(this, objectMap);
        }

        
    }
}
