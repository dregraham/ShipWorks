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
    /// Entity interface which represents the entity 'UspsProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUspsProfileEntity : IUspsProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUspsProfileEntity(IUspsProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            UspsAccountID = source.UspsAccountID;
            HidePostage = source.HidePostage;
            RequireFullAddressValidation = source.RequireFullAddressValidation;
            RateShop = source.RateShop;
            
            PostalProfile = source.PostalProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomUspsProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The UspsAccountID property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."UspsAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> UspsAccountID { get; }
        /// <summary> The HidePostage property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."HidePostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> HidePostage { get; }
        /// <summary> The RequireFullAddressValidation property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."RequireFullAddressValidation"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> RequireFullAddressValidation { get; }
        /// <summary> The RateShop property of the Entity UspsProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UspsProfile"."RateShop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> RateShop { get; }
        
        public IPostalProfileEntity PostalProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUspsProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUspsProfileData(IUspsProfileEntity source);
    }
}
