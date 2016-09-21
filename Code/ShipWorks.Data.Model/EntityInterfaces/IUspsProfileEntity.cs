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
    /// Entity interface which represents the entity 'UspsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUspsProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> UspsAccountID { get; }
        /// <summary> The HidePostage property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."HidePostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> HidePostage { get; }
        /// <summary> The RequireFullAddressValidation property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."RequireFullAddressValidation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> RequireFullAddressValidation { get; }
        /// <summary> The RateShop property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."RateShop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Boolean> RateShop { get; }
        
        IPostalProfileEntity PostalProfile { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUspsProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UspsProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class UspsProfileEntity : IUspsProfileEntity
    {
        IPostalProfileEntity IUspsProfileEntity.PostalProfile => PostalProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUspsProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUspsProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUspsProfileEntity(this, objectMap);
        }
    }
}
