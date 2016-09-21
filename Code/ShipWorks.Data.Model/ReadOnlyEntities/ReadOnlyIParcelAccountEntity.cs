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
    /// Read-only representation of the entity 'IParcelAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyIParcelAccountEntity : IIParcelAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyIParcelAccountEntity(IIParcelAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            IParcelAccountID = source.IParcelAccountID;
            RowVersion = source.RowVersion;
            Username = source.Username;
            Password = source.Password;
            Description = source.Description;
            FirstName = source.FirstName;
            MiddleName = source.MiddleName;
            LastName = source.LastName;
            Company = source.Company;
            Street1 = source.Street1;
            Street2 = source.Street2;
            City = source.City;
            StateProvCode = source.StateProvCode;
            PostalCode = source.PostalCode;
            CountryCode = source.CountryCode;
            Phone = source.Phone;
            Email = source.Email;
            Website = source.Website;
            
            
            

            CopyCustomIParcelAccountData(source);
        }

        
        /// <summary> The IParcelAccountID property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."iParcelAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 IParcelAccountID { get; }
        /// <summary> The RowVersion property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Username property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The Description property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The FirstName property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."FirstName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FirstName { get; }
        /// <summary> The MiddleName property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."MiddleName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MiddleName { get; }
        /// <summary> The LastName property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."LastName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LastName { get; }
        /// <summary> The Company property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Company"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Company { get; }
        /// <summary> The Street1 property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Street1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street1 { get; }
        /// <summary> The Street2 property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Street2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Street2 { get; }
        /// <summary> The City property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."City"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String City { get; }
        /// <summary> The StateProvCode property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."StateProvCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StateProvCode { get; }
        /// <summary> The PostalCode property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."PostalCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PostalCode { get; }
        /// <summary> The CountryCode property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."CountryCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CountryCode { get; }
        /// <summary> The Phone property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Phone"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Phone { get; }
        /// <summary> The Email property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Email"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Email { get; }
        /// <summary> The Website property of the Entity IParcelAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "iParcelAccount"."Website"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Website { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IIParcelAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomIParcelAccountData(IIParcelAccountEntity source);
    }
}
