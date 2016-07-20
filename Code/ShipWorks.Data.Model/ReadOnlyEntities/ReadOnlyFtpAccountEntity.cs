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
    /// Entity interface which represents the entity 'FtpAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyFtpAccountEntity : IFtpAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyFtpAccountEntity(IFtpAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            FtpAccountID = source.FtpAccountID;
            Host = source.Host;
            Username = source.Username;
            Password = source.Password;
            Port = source.Port;
            SecurityType = source.SecurityType;
            Passive = source.Passive;
            InternalOwnerID = source.InternalOwnerID;
            ReuseControlConnectionSession = source.ReuseControlConnectionSession;
            
            
            

            CopyCustomFtpAccountData(source);
        }

        
        /// <summary> The FtpAccountID property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."FtpAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 FtpAccountID { get; }
        /// <summary> The Host property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Host"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Host { get; }
        /// <summary> The Username property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The Port property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Port"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Port { get; }
        /// <summary> The SecurityType property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."SecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SecurityType { get; }
        /// <summary> The Passive property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."Passive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Passive { get; }
        /// <summary> The InternalOwnerID property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."InternalOwnerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InternalOwnerID { get; }
        /// <summary> The ReuseControlConnectionSession property of the Entity FtpAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "FtpAccount"."ReuseControlConnectionSession"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ReuseControlConnectionSession { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFtpAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IFtpAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomFtpAccountData(IFtpAccountEntity source);
    }
}
