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
    /// Read-only representation of the entity 'AsendiaProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAsendiaProfileEntity : IAsendiaProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAsendiaProfileEntity(IAsendiaProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            AsendiaAccountID = source.AsendiaAccountID;
            Service = source.Service;
            NonMachinable = source.NonMachinable;
            Contents = source.Contents;
            NonDelivery = source.NonDelivery;
            
            ShippingProfile = (IShippingProfileEntity) source.ShippingProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomAsendiaProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The AsendiaAccountID property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."AsendiaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> AsendiaAccountID { get; }
        /// <summary> The Service property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."Service"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<Interapptive.Shared.Enums.AsendiaServiceType> Service { get; }
        /// <summary> The NonMachinable property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."NonMachinable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> NonMachinable { get; }
        /// <summary> The Contents property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> Contents { get; }
        /// <summary> The NonDelivery property of the Entity AsendiaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AsendiaProfile"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> NonDelivery { get; }
        
        public IShippingProfileEntity ShippingProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAsendiaProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAsendiaProfileData(IAsendiaProfileEntity source);
    }
}
