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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'EndiciaProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEndiciaProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> EndiciaAccountID { get; }
        /// <summary> The StealthPostage property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."StealthPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> StealthPostage { get; }
        /// <summary> The ReferenceID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReferenceID { get; }
        /// <summary> The ScanBasedReturn property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ScanBasedReturn"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> ScanBasedReturn { get; }
        
        IPostalProfileEntity PostalProfile { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEndiciaProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EndiciaProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class EndiciaProfileEntity : IEndiciaProfileEntity
    {
        IPostalProfileEntity IEndiciaProfileEntity.PostalProfile => PostalProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEndiciaProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEndiciaProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEndiciaProfileEntity(this, objectMap);
        }
    }
}
