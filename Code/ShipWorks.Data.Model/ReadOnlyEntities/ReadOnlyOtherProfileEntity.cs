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
    /// Read-only representation of the entity 'OtherProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOtherProfileEntity : IOtherProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOtherProfileEntity(IOtherProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            Carrier = source.Carrier;
            Service = source.Service;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomOtherProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The Carrier property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."Carrier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Carrier { get; }
        /// <summary> The Service property of the Entity OtherProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OtherProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Service { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOtherProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IOtherProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOtherProfileData(IOtherProfileEntity source);
    }
}
